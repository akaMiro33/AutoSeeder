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
    public class DefaultConstraintParser : IColumnConstraintParser
    {
        public bool CanParse(Token token) => token.Value.Equals("DEFAULT", StringComparison.OrdinalIgnoreCase);

        public ColumnConstraintNode Parse(TokenStream tokens, ParserContext context)
        {
            tokens.Consume();
            return new ColumnConstraintNode
            {
                Type = "DEFAULT " + tokens.Consume().Value
            };
        }
    }
}
