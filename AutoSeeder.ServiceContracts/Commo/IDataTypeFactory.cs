using AutoSeeder.Services.Datatypes;
using AutoSeeder.Services.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.ServiceContracts.Commo
{
    public interface IDataTypeFactory
    {
        public IDataType Create(TokenStream tokens);
    }
}
