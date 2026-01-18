using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Data.Models
{

    public class ConstraintNodeWithTableName
    {
        public string TableName { get; set; }
        public ConstraintNode Constraint { get; set; }
    }

    public class ConstraintNode
    {
        public string Type { get; set; } // PRIMARY KEY, FOREIGN KEY, UNIQUE, NOT NULL, etc.
        public string Name { get; set; } // Optional constraint name
        public List<string> Columns { get; set; } = new(); // Column(s) affected
        public string ReferenceTable { get; set; } // For FOREIGN KEY
        public List<string> ReferenceColumns { get; set; } = new(); // For FOREIGN KEY
    }
}
