using AutoSeeder.Data.Models;
using Microsoft.VisualBasic;
using System;
using System.Text;

namespace AutoSeeder.Services.Seed
{
    public class SeedService
    {
        private readonly TableRepresentationService createTablesService;
        private readonly SeedCreationService seedCreationService;
        public SeedService(TableRepresentationService createTablesService, SeedCreationService seedCreationService)
        {
            this.createTablesService = createTablesService;
            this.seedCreationService = seedCreationService;
        }

        public string Create(string schemaText)
        {
            var tables = createTablesService.Create(schemaText);
            var seed = seedCreationService.GenerateSeedSql(tables);

            return seed;
        }
    }
}
