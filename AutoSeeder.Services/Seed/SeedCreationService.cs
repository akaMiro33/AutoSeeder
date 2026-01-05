using AutoSeeder.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Services.Seed
{
    public class SeedCreationService
    {
        public List<string> GenerateSeedSql(IReadOnlyList<CreateTableNode> tables)
        {
            var orderedTables = OrderByForeignKeys(tables);
            var sql = new List<string>();

            var generatedIds = new Dictionary<(string Table, string Column), List<string>>();

            foreach (var table in orderedTables)
            {
                var insert = GenerateInsert(table, generatedIds, 100);
                sql.Add(insert);
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


        private string GenerateInsert(CreateTableNode table, Dictionary<(string Table, string Column), List<string>> generatedIds, int rowCount)
        {
            var columns = new List<string>();
            var rows = new List<string>();

            foreach (var column in table.Columns)
            {
                if (HasConstraint(column, table, "IDENTITY")) continue;
                if (HasConstraint(column, table, "DEFAULT")) continue;

                columns.Add(column.Name);
            }

            var columnValues = new Dictionary<string, List<string>>();

            foreach (var column in table.Columns)
            {
                // Skip IDENTITY and DEFAULT columns
                if (HasConstraint(column, table, "IDENTITY") || HasConstraint(column, table, "DEFAULT")) continue;

                var values = new List<string>();

                var fk = table.Constraints.FirstOrDefault(c => c.Columns.Contains(column.Name) && c.Type == "FOREIGN KEY");

                if (fk != null)
                {
                    ////var ids = generatedIds[fk.ReferenceTable];
                    //var ids = generatedIds[(Table: fk.ReferenceTable, Column: column.Name)];
                    //// I will change this to tuple and check it with benchmark
                    //var random = new Random();

                    //for (int i = 0; i < rowCount; i++)
                    //{
                    //    int index = random.Next(ids.Count);
                    //    values.Add(ids[index].ToString());
                    //}
                }
                else
                {
                    values = column.DataType.GenerateValue(true, rowCount);
                    if (HasConstraint(column, table, "PRIMARY"))
                    {
                        generatedIds[(Table: table.TableName, Column: column.Name)] = values;
                        // I will change this to tuple and check it with benchmarkS
                    }
                }

                columnValues[column.Name] = values;
            }

            foreach (var constraint in table.Constraints.Where(con => con.Type == "FOREIGN KEY"))
            {
                var idsGroup = new List<List<string>>();
                var poradieColuimns = new List<string>();

                for (int i = 0; i < constraint.Columns.Count; i++)
                {
                    idsGroup.Add(generatedIds[(Table: constraint.ReferenceTable, Column: constraint.Columns[i])]);
                    poradieColuimns.Add(constraint.ReferenceColumns[i]);

                }

                var random = new Random();
                var ValuesArray = new Dictionary<string, List<string>>();

                for (int j = 0; j < rowCount; j++)
                {
                    int index = random.Next(100);

                    if (j == 0)
                    {
                        for (int z = 0; z < poradieColuimns.Count; z++)
                        {
                            ValuesArray[poradieColuimns[z]] = new List<string>();
                        }
                    }


                    for (int z = 0; z < poradieColuimns.Count; z++)
                    {
                        ValuesArray[poradieColuimns[z]].Add(idsGroup[z][index]);
                    }

                }


                for (int ii = 0; ii < poradieColuimns.Count; ii++)
                {
                    generatedIds[(Table: table.TableName, Column: poradieColuimns[ii])] = ValuesArray[poradieColuimns[ii]];
                    columnValues[poradieColuimns[ii]] = ValuesArray[poradieColuimns[ii]];
                }



            }



            // Assemble rows from column values
            for (int i = 0; i < rowCount; i++)
            {
                var row = new List<string>();
                foreach (var column in table.Columns)
                {
                    if (HasConstraint(column, table, "IDENTITY") || HasConstraint(column, table, "DEFAULT")) continue;

                    row.Add(columnValues[column.Name][i]);
                }

                rows.Add($"({string.Join(", ", row)})");
            }

            return
                $"INSERT INTO {table.TableName} " +
                $"({string.Join(", ", columns)}) VALUES " +
                $"{string.Join(", ", rows)};";
        }

        private bool HasConstraint(ColumnNode column, CreateTableNode table, string type) => table.Constraints.Any(c => c.Columns.Contains(column.Name) && c.Type.StartsWith(type));
    }
}
