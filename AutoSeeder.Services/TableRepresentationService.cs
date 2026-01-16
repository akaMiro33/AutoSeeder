using AutoSeeder.Data.Common;
using AutoSeeder.Data.Common.DataTypeFactory;
using AutoSeeder.Data.Enums;
using AutoSeeder.Data.Models;
using AutoSeeder.ServiceContracts;
using AutoSeeder.ServiceContracts.Commo;
using AutoSeeder.ServiceContracts.Parser;
using AutoSeeder.Services.Common.ConstraintParsing;
using AutoSeeder.Services.Parser;
using AutoSeeder.Services.Tokenization;


namespace AutoSeeder.Services
{
    public class TableRepresentationService : ITableRepresentationService
    {
        private readonly IParserService parserService;
        private readonly List<IColumnConstraintParser> parsers;
        private readonly IDataTypeFactory dataTypeFactory;

        public TableRepresentationService(IParserService parserService, IEnumerable<IColumnConstraintParser> parsers, IDataTypeFactory dataTypeFactory)
        {
            this.parserService = parserService;
            this.parsers = parsers.ToList();
            this.dataTypeFactory = dataTypeFactory;
        }

        public IReadOnlyList<CreateTableNode> Create(string schemaText)
        {
            var tokens = SqlTokenizer.GetTokens(schemaText);
            var tables = parserService.ParseTokens(tokens, parsers, dataTypeFactory);


            return tables;
        }
    }
}
