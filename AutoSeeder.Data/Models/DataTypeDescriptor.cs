using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Data.Models
{
    public class DataTypeDescriptor
    {
        public string Name { get; set; }          // VARCHAR, DECIMAL, INT
        public int? Length { get; set; }           // VARCHAR(50), CHAR(10)
        public int? Precision { get; set; }        // DECIMAL(10,2)
        public int? Scale { get; set; }             // DECIMAL(10,2)
    }
}
