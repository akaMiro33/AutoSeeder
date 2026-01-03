using AutoSeeder.Data.Common.DataTypeFactory;
using AutoSeeder.Data.Common.Datatypes;
using AutoSeeder.Data.Enums;
using AutoSeeder.Data.Models;
using AutoSeeder.Services.Common.ConstraintParsing;
using AutoSeeder.Services.Datatypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Services.Parser
{
    public class ParserService
    {
        public IReadOnlyList<CreateTableNode> ParseTokens(IReadOnlyList<Token> tokens, IEnumerable<IColumnConstraintParser> constraintParsers, IDataTypeFactory dataTypeFactory)
        {
            var context = new ParserContext(tokens, constraintParsers, dataTypeFactory);
            var nodes = context.ParseTokens();

            return nodes;
        }
    }
}