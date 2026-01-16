using AutoSeeder.Data.Common.DataTypeFactory;
using AutoSeeder.ServiceContracts;
using AutoSeeder.ServiceContracts.Commo;
using AutoSeeder.ServiceContracts.Parser;
using AutoSeeder.ServiceContracts.Seed;
using AutoSeeder.Services;
using AutoSeeder.Services.Common.ConstraintParsing;
using AutoSeeder.Services.Parser;
using AutoSeeder.Services.Seed;

namespace AutoSeeder.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddSingleton<IColumnConstraintParser, NotNullConstraintParser>();
            builder.Services.AddSingleton<IColumnConstraintParser, PrimaryKeyConstraintParser>();
            builder.Services.AddSingleton<IColumnConstraintParser, ForeignKeyConstraintParser>();
            builder.Services.AddSingleton<IColumnConstraintParser, UniqueConstraintParser>();
            builder.Services.AddSingleton<IColumnConstraintParser, DefaultConstraintParser>();
            builder.Services.AddSingleton<IColumnConstraintParser, IdentityConstraintParser>();


            builder.Services.AddSingleton<IDataTypeFactory, SqlTypeFactory>();
            builder.Services.AddSingleton<IParserService, ParserService>();
            builder.Services.AddSingleton<ISeedCreationService, SeedCreationService>();
            builder.Services.AddSingleton<ITableRepresentationService, TableRepresentationService>();
            builder.Services.AddSingleton<ISeedService, SeedService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
