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
    public class IdentityConstraintParser : IColumnConstraintParser
    {
        public bool CanParse(Token token) =>
        token.Value.Equals("IDENTITY", StringComparison.OrdinalIgnoreCase);


        public ColumnConstraintNode Parse(TokenStream tokens, ParserContext context)
        {
            tokens.Consume();
            tokens.Expect(TokenType.Symbol, "(");
            var seed = tokens.Expect(TokenType.Number).Value;
            tokens.Expect(TokenType.Symbol, ",");
            var inc = tokens.Expect(TokenType.Number).Value;
            tokens.Expect(TokenType.Symbol, ")");

            return new ColumnConstraintNode
            {
                Type = $"IDENTITY({seed},{inc})"
            };
        }
    }
}
