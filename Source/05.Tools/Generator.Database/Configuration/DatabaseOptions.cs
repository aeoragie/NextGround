namespace Generator.Database.Configuration
{
    public class DatabaseOptions
    {
        public string OriginalName { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public GenerateOptions Options { get; set; } = new();
        public PathOptions Paths { get; set; } = new();
    }
}
