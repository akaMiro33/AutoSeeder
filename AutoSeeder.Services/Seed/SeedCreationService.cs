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
        private static readonly Random random = new Random();

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
            var rows = new List<string>();
            var columnValues = new Dictionary<string, List<string>>();
            var columns = new List<string>();


            //creates column for insert statement e.g. "INSERT VALUES INTO (columns)
            foreach (var column in table.Columns)
            {
                if (HasConstraint(column, table, "IDENTITY")) continue;
                if (HasConstraint(column, table, "DEFAULT")) continue;

                columns.Add(column.Name);
            }

            var noForeignKeyColumns = table.Columns.Where(col => !HasConstraint(col, table, "FOREIGN KEY")).ToList();

            foreach (var column in noForeignKeyColumns)
            {
                // Skip IDENTITY and DEFAULT columns
                if (HasConstraint(column, table, "IDENTITY") || HasConstraint(column, table, "DEFAULT")) continue;

                var values = column.DataType.GenerateValue(true, rowCount);
                if (HasConstraint(column, table, "PRIMARY"))
                {
                    generatedIds[(Table: table.TableName, Column: column.Name)] = values;
                    // I will change this to tuple and check it with benchmarkS
                }

                columnValues[column.Name] = values;
            }

            foreach (var fk in table.Constraints.Where(c => c.Type == "FOREIGN KEY"))
            {
                // Map referenced columns to their generated IDs
                var refIdGroups = fk.Columns.Select(col => generatedIds[(fk.ReferenceTable, col)]).ToList();

                // List of corresponding foreign key target columns
                var fkColumns = fk.ReferenceColumns.ToList();
                var valuesByColumn = fkColumns.ToDictionary(col => col, col => new List<string>());

                for (int row = 0; row < rowCount; row++)
                {
                    int refRow = random.Next(rowCount);

                    for (int col = 0; col < fkColumns.Count; col++)
                    {
                        valuesByColumn[fkColumns[col]].Add(refIdGroups[col][refRow]);
                    }
                }

                foreach (var col in fkColumns)
                {
                    columnValues[col] = valuesByColumn[col];
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

        //private string TransformDataToInsert(CreateTableNode table, Dictionary<string, List<string>> columnValues, int rowCount)
        //{
        //    var rows = new List<string>();
        //    var columns = new List<string>();


        //    //creates column for insert statement e.g. "INSERT VALUES INTO (columns)
        //    foreach (var column in table.Columns)
        //    {
        //        if (HasConstraint(column, table, "IDENTITY")) continue;
        //        if (HasConstraint(column, table, "DEFAULT")) continue;

        //        columns.Add(column.Name);
        //    }

        //    // Assemble rows from column values
        //    for (int i = 0; i < rowCount; i++)
        //    {
        //        var row = new List<string>();
        //        foreach (var column in table.Columns)
        //        {
        //            if (HasConstraint(column, table, "IDENTITY") || HasConstraint(column, table, "DEFAULT")) continue;

        //            row.Add(columnValues[column.Name][i]);
        //        }

        //        rows.Add($"({string.Join(", ", row)})");
        //    }

        //    return
        //        $"INSERT INTO {table.TableName} " +
        //        $"({string.Join(", ", columns)}) VALUES " +
        //        $"{string.Join(", ", rows)};";
        //}

        private bool HasConstraint(ColumnNode column, CreateTableNode table, string type) => table.Constraints.Any(c => c.Columns.Contains(column.Name) && c.Type.StartsWith(type));
    }
}
