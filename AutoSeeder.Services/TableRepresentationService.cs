using AutoSeeder.Data.Common;
using AutoSeeder.Data.Common.DataTypeFactory;
using AutoSeeder.Data.Enums;
using AutoSeeder.Data.Models;
using AutoSeeder.Services.Common.ConstraintParsing;
using AutoSeeder.Services.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoSeeder.Services
{
    public class TableRepresentationService
    {
        private readonly string schemaText;
        private readonly ParserService parserService;
        private readonly List<IColumnConstraintParser> parsers;
        private readonly IDataTypeFactory dataTypeFactory;

        public TableRepresentationService(ParserService parserService, List<IColumnConstraintParser> parsers, IDataTypeFactory dataTypeFactory,  string schemaText)
        {
            this.schemaText = schemaText;
            this.parserService = parserService;
            this.parsers = parsers;
            this.dataTypeFactory = dataTypeFactory;
        }

        public IReadOnlyList<CreateTableNode> Create()
        {
            var tokens = SqlTokenizer.GetTokens(this.schemaText);
            var tables = parserService.ParseTokens(tokens, parsers, dataTypeFactory);


            return tables;
        }
    }
}
