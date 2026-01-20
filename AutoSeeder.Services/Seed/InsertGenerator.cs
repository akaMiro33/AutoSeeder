using AutoSeeder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Services.Seed
{
    public class InsertGenerator
    {
        public required int RowCount { get; set; }
        private static readonly Random random = new Random();

        public string Generate(CreateTableNode table, Dictionary<string, Dictionary<string, List<string>>> generatedIds)
        {
            var columnValues = GenerateValues(table, generatedIds);
            var foreignKeysValues = GenerateForeignKeyValues(table, generatedIds);

            var allColumnValues = columnValues.Concat(foreignKeysValues).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return TransformDataToInsert(table, allColumnValues);           
        }

        private Dictionary<string, List<string>> GenerateValues(CreateTableNode table, Dictionary<string, Dictionary<string, List<string>>> generatedIds)
        {
            var columnValues = new Dictionary<string, List<string>>();
            var noForeignKeyColumns = table.Columns.Where(col => !HasConstraint(col, table, "FOREIGN KEY")).ToList();

            foreach (var column in noForeignKeyColumns)
            {
                // Skip IDENTITY and DEFAULT columns
                if (HasConstraint(column, table, "IDENTITY") || HasConstraint(column, table, "DEFAULT")) continue;

                var values = column.DataType.GenerateValue(true, RowCount);
                if (HasConstraint(column, table, "PRIMARY"))
                {
                    if (!generatedIds.TryGetValue(table.TableName, out var tableIds))
                    {
                        tableIds = new Dictionary<string, List<string>>();
                        generatedIds[table.TableName] = tableIds;
                    }

                    generatedIds[table.TableName].Add(column.Name, values);

                    //generatedIds[(table: table.TableName, column: column.Name)] = values;
                    // I will change this to tuple and check it with benchmarkS
                }

                columnValues[column.Name] = values;
            }

            return columnValues;
        }

        private Dictionary<string, List<string>> GenerateForeignKeyValues(CreateTableNode table, Dictionary<string, Dictionary<string, List<string>>> generatedIds)
        {
            var columnValues = new Dictionary<string, List<string>>();

            foreach (var fk in table.Constraints.Where(c => c.Type == "FOREIGN KEY"))
            {
                // Map referenced columns to their generated IDs
                //var refIdGroups = fk.Columns.Select(col => generatedIds[(fk.ReferenceTable, col)]).ToList();
                var refIdGroups = fk.Columns.Select(col => generatedIds[fk.ReferenceTable][col]).ToList();

                // List of corresponding foreign key target columns
                var fkColumns = fk.ReferenceColumns.ToList();
                var valuesByColumn = fkColumns.ToDictionary(col => col, col => new List<string>());

                for (int row = 0; row < RowCount; row++)
                {
                    int refRow = random.Next(RowCount);

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

            return columnValues;
        }

        private string TransformDataToInsert(CreateTableNode table, Dictionary<string, List<string>> columnValues)
        {
            var rows = new List<string>();
            var columns = new List<string>();


            //creates column for insert statement e.g. "INSERT VALUES INTO (columns)
            foreach (var column in table.Columns)
            {
                if (HasConstraint(column, table, "IDENTITY")) continue;
                if (HasConstraint(column, table, "DEFAULT")) continue;

                columns.Add(column.Name);
            }

            // Assemble rows from column values
            for (int i = 0; i < RowCount; i++)
            {
                var row = new List<string>();
                foreach (var column in table.Columns)
                {
                    if (HasConstraint(column, table, "IDENTITY") || HasConstraint(column, table, "DEFAULT")) continue;

                    row.Add(columnValues[column.Name][i]);
                }

                rows.Add($"({string.Join(", ", row)})");
            }

            //if (table.Constraints.Any(c => c.Type.StartsWith("IDENTITY")))
            //{
            //    var identityColumn = table.Columns.FirstOrDefault(col => HasConstraint(col, table, "IDENTITY"));

            //    if (identityColumn is null)
            //    {
            //        throw new Exception(" bla bla este neviem čo");
            //    }

            //    return
            //        $"DECLARE @{table.TableName} TABLE ( {identityColumn.Name} INT) " +
            //        //$"DECLARE @{table.TableName} TABLE({identityColumn.Name} {identityColumn.DataType})" +
            //        $"INSERT INTO {table.TableName} " +
            //        $"({string.Join(", ", columns)}) " +
            //        $"OUTPUT INSERTED.{identityColumn.Name} INTO @{table.TableName} " +
            //        $"VALUES {string.Join(", ", rows)};";
            //}


            return
                $"INSERT INTO {table.TableName} " +
                $"({string.Join(", ", columns)}) VALUES " +
                $"{string.Join(", ", rows)};";
        }

        private bool HasConstraint(ColumnNode column, CreateTableNode table, string type) => table.Constraints.Any(c => c.Columns.Contains(column.Name) && c.Type.StartsWith(type));





    }
}
