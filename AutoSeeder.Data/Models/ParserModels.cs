using AutoSeeder.Services.Datatypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSeeder.Data.Models
{
    public class CreateTableNode
    {
        public string TableName { get; set; }
        public List<ColumnNode> Columns { get; set; } = new();
        public List<TableConstraintNode> Constraints { get; set; } = new();
    }

    public class ColumnNode
    {
        public string Name { get; set; }
        public IDataType DataType { get; set; }
        public List<ColumnConstraintNode> Constraints { get; set; } = new();
    }

    public class TableConstraintNode
    {
        public string Name { get; set; } // optional
        public string Type { get; set; } // PRIMARY KEY, FOREIGN KEY
        public List<string> Columns { get; set; } = new();
        public string ReferenceTable { get; set; } // FOREIGN KEY
    }

    public class ColumnConstraintNode
    {
        public string Type { get; set; }
        public string ReferenceTable { get; set; }
    }
}
