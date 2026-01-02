using AutoSeeder.Data.Common;
using AutoSeeder.Data.Enums;
using AutoSeeder.Data.Models;
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

        public TableRepresentationService(ParserService parserService, string schemaText)
        {
            this.schemaText = schemaText;
            this.parserService = parserService;
        }

        public List<CreateTableNode> Create()
        {
            var tokens = SqlTokenizer.GetTokens(this.schemaText);
            var tables = parserService.ParseTokens(tokens);


            return tables;
        }
    }
}
