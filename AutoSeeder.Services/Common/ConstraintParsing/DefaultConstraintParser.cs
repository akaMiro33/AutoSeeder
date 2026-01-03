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

        public ConstraintNode Parse(TokenStream tokens, ParserContext context, string columnName)
        {
            tokens.Consume();
            return new ConstraintNode
            {
                Type = "DEFAULT " + tokens.Consume().Value,
                Columns = new List<string> { columnName } 
                
                
            };
        }
    }
}
