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
    public class NotNullConstraintParser : IColumnConstraintParser
    {
       public bool CanParse(Token token) => token.Value.Equals("NOT", StringComparison.OrdinalIgnoreCase);


        public ConstraintNode Parse(TokenStream tokens, IParserContext context, string columnName)
        {
            tokens.Consume();
            tokens.Expect(TokenType.Keyword, "NULL");

            return new ConstraintNode
            {
                Type = "NOT NULL",
                Columns = new List<string> { columnName }
            };
        }
    }
}
