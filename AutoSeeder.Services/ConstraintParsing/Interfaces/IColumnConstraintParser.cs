using AutoSeeder.Data.Models;
using AutoSeeder.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Services.ConstraintParsing.Interfaces
{
    public interface IColumnConstraintParser
    {
        public bool CanParse(Token token);
        public ColumnConstraintNode Parse(ParserContext context);
    }
}
