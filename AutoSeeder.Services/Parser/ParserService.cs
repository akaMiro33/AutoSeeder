using AutoSeeder.Data.Common.DataTypeFactory;
using AutoSeeder.Data.Common.Datatypes;
using AutoSeeder.Data.Enums;
using AutoSeeder.Data.Models;
using AutoSeeder.ServiceContracts.Commo;
using AutoSeeder.ServiceContracts.Parser;
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
    //public interface IParserService
    //{
    //    IReadOnlyList<CreateTableNode> ParseTokens(IReadOnlyList<Token> tokens, IEnumerable<IColumnConstraintParser> constraintParsers, IDataTypeFactory dataTypeFactory);
    //}

    public class ParserService : IParserService
    {
        public IReadOnlyList<CreateTableNode> ParseTokens(IReadOnlyList<Token> tokens, IEnumerable<IColumnConstraintParser> constraintParsers, IDataTypeFactory dataTypeFactory)
        {
            var context = new ParserContext(tokens, constraintParsers, dataTypeFactory);
            var nodes = context.ParseTokens();

            return nodes;
        }
    }
}