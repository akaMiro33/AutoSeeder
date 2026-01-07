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
        public string GenerateSeedSql(IReadOnlyList<CreateTableNode> tables)
        {
            var orderedTables = OrderByForeignKeys(tables);
            string sql;
            var sb = new StringBuilder();

            var generatedIds = new Dictionary<string, Dictionary<string, List<string>>>();

            var insertGen = new InsertGenerator() { RowCount = 100 };

            foreach (var table in orderedTables)
            {
                var insert = insertGen.Generate(table, generatedIds);
                sb.AppendLine(insert);
            }

            sql = sb.ToString();

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
    }
}
