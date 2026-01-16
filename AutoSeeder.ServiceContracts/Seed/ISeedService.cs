using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.ServiceContracts.Seed
{
    public interface ISeedService
    {
        string Create(string schemaText);
    }
}
