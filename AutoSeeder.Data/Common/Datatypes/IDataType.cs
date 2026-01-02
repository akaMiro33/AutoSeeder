using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Services.Datatypes
{
    public interface IDataType
    {
        public List<string> GenerateValue(bool unique, int count);
}
}
