using AutoSeeder.Services.Datatypes;

namespace AutoSeeder.Data.Models
{
    public class ColumnNode
    {
        public string Name { get; set; }
        public IDataType DataType { get; set; }
        //public List<ColumnConstraintNode> Constraints { get; set; } = new();
    }
}
