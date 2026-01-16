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
    public class IdentityConstraintParser : IColumnConstraintParser
    {
        public bool CanParse(Token token) =>
        token.Value.Equals("IDENTITY", StringComparison.OrdinalIgnoreCase);


        public ConstraintNode Parse(TokenStream tokens, IParserContext context, string columnName)
        {
            tokens.Consume();
            tokens.Expect(TokenType.Symbol, "(");
            var seed = tokens.Expect(TokenType.Number).Value;
            tokens.Expect(TokenType.Symbol, ",");
            var inc = tokens.Expect(TokenType.Number).Value;
            tokens.Expect(TokenType.Symbol, ")");

            return new ConstraintNode
            {
                Type = $"IDENTITY({seed},{inc})",
                Columns = new List<string> { columnName }
            };
        }
    }
}
