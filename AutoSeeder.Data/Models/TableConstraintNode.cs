namespace AutoSeeder.Data.Models
{
    public class TableConstraintNode
    {
        public string Name { get; set; } // optional
        public string Type { get; set; } // PRIMARY KEY, FOREIGN KEY
        public List<string> Columns { get; set; } = new();
        public string ReferenceTable { get; set; } // FOREIGN KEY
    }
}
