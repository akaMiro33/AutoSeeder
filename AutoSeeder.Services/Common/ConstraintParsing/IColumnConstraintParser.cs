using AutoSeeder.Data.Models;
using AutoSeeder.Services.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Services.Common.ConstraintParsing
{
    public interface IColumnConstraintParser
    {
        public bool CanParse(Token token);
        //public ColumnConstraintNode Parse(ParserContext context);
        public ColumnConstraintNode Parse(TokenStream tokens, ParserContext context);
    }
}
