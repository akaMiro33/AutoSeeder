using AutoSeeder.Data.Models;
using Microsoft.VisualBasic;
using System;

namespace AutoSeeder.Services
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

        public string Create()
        {
            var tables = createTablesService.Create();
            var seed = seedCreationService.GenerateSeedSql(tables);
            return "";
        }
    }
}
