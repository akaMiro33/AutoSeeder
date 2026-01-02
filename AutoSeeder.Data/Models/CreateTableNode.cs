namespace AutoSeeder.Data.Models
{
    public class CreateTableNode
    {
        public string TableName { get; set; }
        public List<ColumnNode> Columns { get; set; } = new();
        public List<TableConstraintNode> Constraints { get; set; } = new();
    }
}
