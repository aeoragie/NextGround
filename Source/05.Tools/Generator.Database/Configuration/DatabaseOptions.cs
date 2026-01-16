namespace Generator.Database.Configuration
{
    public class DatabaseOptions
    {
        public string MetaPath { get; set; } = string.Empty;
        public string SqlTablesPath { get; set; } = string.Empty;
        public PathOptions Paths { get; set; } = new();
    }
}
