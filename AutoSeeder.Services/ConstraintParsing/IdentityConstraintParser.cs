using AutoSeeder.Data.Enums;
using AutoSeeder.Data.Models;
using AutoSeeder.Services.ConstraintParsing.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Services.ConstraintParsing
{
    public class IdentityConstraintParser : IColumnConstraintParser
    {
        public bool CanParse(Token token) =>
        token.Value.Equals("IDENTITY", StringComparison.OrdinalIgnoreCase);

        public ColumnConstraintNode Parse(ParserContext ctx)
        {
            ctx.Consume();
            ctx.Expect(TokenType.Symbol, "(");
            var seed = ctx.Expect(TokenType.Number).Value;
            ctx.Expect(TokenType.Symbol, ",");
            var inc = ctx.Expect(TokenType.Number).Value;
            ctx.Expect(TokenType.Symbol, ")");

            return new ColumnConstraintNode
            {
                Type = $"IDENTITY({seed},{inc})"
            };
        }
    }
}
