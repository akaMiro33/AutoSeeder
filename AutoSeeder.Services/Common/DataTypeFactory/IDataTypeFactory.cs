using AutoSeeder.Data.Models;
using AutoSeeder.Services;
using AutoSeeder.Services.Datatypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Data.Common.DataTypeFactory
{
    public interface IDataTypeFactory
    {
        public IDataType Create(TokenStream tokens);
    }
}
