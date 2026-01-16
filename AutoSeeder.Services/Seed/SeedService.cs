using AutoSeeder.Data.Models;
using AutoSeeder.ServiceContracts;
using AutoSeeder.ServiceContracts.Seed;
using Microsoft.VisualBasic;
using System;
using System.Text;

namespace AutoSeeder.Services.Seed
{
    public class SeedService : ISeedService
    {
        private readonly ITableRepresentationService createTablesService;
        private readonly ISeedCreationService seedCreationService;
        public SeedService(ITableRepresentationService createTablesService, ISeedCreationService seedCreationService)
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
