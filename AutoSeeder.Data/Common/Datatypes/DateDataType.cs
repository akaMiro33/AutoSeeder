using AutoSeeder.Services.Datatypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Data.Common.Datatypes
{
    public class DateDataType : IDataType
    {
        public List<string> GenerateValue(bool unique, int count)
        {
            var data = new List<string>();

            for (int i = 0; i < count; i++) {
                data.Add("SYSDATETIME()");
            }

            return data;
        }
        
    }
}
