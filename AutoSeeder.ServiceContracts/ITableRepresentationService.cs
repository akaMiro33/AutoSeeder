using AutoSeeder.Data.Models;

namespace AutoSeeder.ServiceContracts
{
    public interface ITableRepresentationService
    {
        IReadOnlyList<CreateTableNode> Create(string schemaText);
    }
}