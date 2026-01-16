using AutoSeeder.Data.Enums;
using AutoSeeder.Data.Models;
using AutoSeeder.ServiceContracts.Parser;
using AutoSeeder.Services.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Services.Common.ConstraintParsing
{
    public class PrimaryKeyConstraintParser : IColumnConstraintParser
    {
        public bool CanParse(Token token) => token.Value.Equals("PRIMARY", StringComparison.OrdinalIgnoreCase);


        public ConstraintNode Parse(TokenStream tokens, IParserContext context, string columnName)
        {
            tokens.Consume();
            tokens.Expect(TokenType.Keyword, "KEY");

            return new ConstraintNode
            {
                Type = "PRIMARY KEY",
                Columns = new List<string> { columnName }
            };
        }
    }
}
