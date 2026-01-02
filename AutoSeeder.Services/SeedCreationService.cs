using AutoSeeder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Services
{
    public class SeedCreationService
    {
        public List<string> GenerateSeedSql(IReadOnlyList<CreateTableNode> tables)
        {
            var orderedTables = OrderByForeignKeys(tables);
            var sql = new List<string>();

            var generatedIds = new Dictionary<string, List<string>>();

            foreach (var table in orderedTables)
            {
                var insert = GenerateInsert(table, generatedIds, 100);
                sql.Add(insert);

                // simulate identity PK = 1
                //generatedIds[table.TableName] = 1;
            }

            return sql;
        }

        private List<CreateTableNode> OrderByForeignKeys(IReadOnlyList<CreateTableNode> tables)
        {
            var result = new List<CreateTableNode>();
            var visited = new HashSet<string>();

            void Visit(CreateTableNode table)
            {
                if (visited.Contains(table.TableName))
                {
                    return;
                }

                var tablesWithFK = table.Constraints.Where(c => c.Type == "FOREIGN KEY");

                foreach (var fk in tablesWithFK)
                {
                    var parent = tables.First(t => t.TableName == fk.ReferenceTable);
                    Visit(parent);
                }

                visited.Add(table.TableName);
                result.Add(table);
            }

            foreach (var table in tables)
            {
                Visit(table);
            }

            return result;
        }


        private string GenerateInsert(CreateTableNode table, Dictionary<string, List<string>> generatedIds, int rowCount)
        //private string GenerateInsert(CreateTableNode table, int rowCount)
        {
            var columns = new List<string>();
            var rows = new List<string>();

            foreach (var column in table.Columns)
            {
                if (HasConstraint(column, "IDENTITY")) continue;
                if (HasConstraint(column, "DEFAULT")) continue;

                columns.Add(column.Name);
            }

            var columnValues = new Dictionary<string, List<string>>();

            foreach (var column in table.Columns)
            {
                // Skip IDENTITY and DEFAULT columns
                if (HasConstraint(column, "IDENTITY") || HasConstraint(column, "DEFAULT")) continue;

                var values = new List<string>();

                var fk = column.Constraints.FirstOrDefault(c => c.Type == "FOREIGN KEY");

                if (fk != null)
                {
                    var ids = generatedIds[fk.ReferenceTable];
                    var random = new Random();

                    for (int i = 0; i < rowCount; i++)
                    {
                        int index = random.Next(ids.Count);
                        values.Add(ids[index].ToString());
                    }
                }
                else
                {
                    values = column.DataType.GenerateValue(true, rowCount);
                    if (HasConstraint(column, "PRIMARY")) {
                        generatedIds[table.TableName] = values;
                    }
                }

                columnValues[column.Name] = values;
            }

            // Assemble rows from column values
            for (int i = 0; i < rowCount; i++)
            {
                var row = new List<string>();
                foreach (var column in table.Columns)
                {
                    if (HasConstraint(column, "IDENTITY") || HasConstraint(column, "DEFAULT")) continue;

                    row.Add(columnValues[column.Name][i]);
                }

                rows.Add($"({string.Join(", ", row)})");
            }

            return
                $"INSERT INTO {table.TableName} " +
                $"({string.Join(", ", columns)}) VALUES " +
                $"{string.Join(", ", rows)};";
        }

        private bool HasConstraint(ColumnNode column, string type) => column.Constraints.Any(c => c.Type.StartsWith(type));
    }
}
