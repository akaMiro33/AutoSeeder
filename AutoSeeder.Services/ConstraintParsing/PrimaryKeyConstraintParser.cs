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
    public class PrimaryKeyConstraintParser : IColumnConstraintParser
    {
        public bool CanParse(Token token) => token.Value.Equals("PRIMARY", StringComparison.OrdinalIgnoreCase);

        public ColumnConstraintNode Parse(ParserContext ctx)
        {
            ctx.Consume();
            ctx.Expect(TokenType.Keyword, "KEY");

            return new ColumnConstraintNode
            {
                Type = "PRIMARY KEY"
            };
        }
    }
}
