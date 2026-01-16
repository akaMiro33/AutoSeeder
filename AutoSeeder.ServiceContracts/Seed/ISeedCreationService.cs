using AutoSeeder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.ServiceContracts.Seed
{
    public interface ISeedCreationService
    {
        string GenerateSeedSql(IReadOnlyList<CreateTableNode> tables);
    }
}
