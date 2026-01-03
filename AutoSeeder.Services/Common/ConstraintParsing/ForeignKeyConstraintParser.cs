using AutoSeeder.Data.Enums;
using AutoSeeder.Data.Models;
using AutoSeeder.Services.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Services.Common.ConstraintParsing
{
    public class ForeignKeyConstraintParser : IColumnConstraintParser
    {
        public bool CanParse(Token token) =>  token.Value.Equals("FOREIGN", StringComparison.OrdinalIgnoreCase);
     

        public ConstraintNode Parse(TokenStream tokens, ParserContext context, string columnName)
        {
            tokens.Consume();
            tokens.Expect(TokenType.Keyword, "KEY");
            tokens.Expect(TokenType.Keyword, "REFERENCES");

            var table = context.ParseTableName();
            context.ParseIdentifierList();

            return new ConstraintNode
            {
                Type = "FOREIGN KEY",
                ReferenceTable = table,
                Columns = new List<string> { columnName }
            };
        }
    }
}
