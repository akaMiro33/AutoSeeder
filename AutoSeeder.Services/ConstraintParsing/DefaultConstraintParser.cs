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
    public class DefaultConstraintParser : IColumnConstraintParser
    {
        public bool CanParse(Token token) => token.Value.Equals("DEFAULT", StringComparison.OrdinalIgnoreCase);

        public ColumnConstraintNode Parse(ParserContext ctx)
        {
            ctx.Consume();
            return new ColumnConstraintNode
            {
                Type = "DEFAULT " + ctx.Consume().Value
            };
        }
    }
}
