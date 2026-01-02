using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Data.Models
{

    public class ColumnConstraintNode
    {
        public string Type { get; set; }
        public string ReferenceTable { get; set; }
    }
}
