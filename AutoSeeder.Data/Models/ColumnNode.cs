using AutoSeeder.Services.Datatypes;

namespace AutoSeeder.Data.Models
{
    public class ColumnNode
    {
        public string Name { get; set; }
        public IDataType DataType { get; set; }
    }
}
