namespace Generator.Database.Models
{
    // Tables.yaml 모델
    public class TablesMetadata
    {
        public TablesDefaults? Defaults { get; set; }
        public Dictionary<string, TableConfig> Tables { get; set; } = new();
    }

    public class TablesDefaults
    {
        public string Schema { get; set; } = "dbo";
        public GeneratesConfig? Generates { get; set; }
    }

    public class TableConfig
    {
        public string? Description { get; set; }
        public GeneratesConfig? Generates { get; set; }
        public List<string>? Exclude { get; set; }
    }

    public class GeneratesConfig
    {
        public object? Entity { get; set; }
    }

    // Mappings.yaml 모델
    public class MappingsMetadata
    {
        public OutputConfig? Output { get; set; }
        public Dictionary<string, DtoConfig>? Dtos { get; set; }
        public Dictionary<string, MessageConfig>? Messages { get; set; }
        public Dictionary<string, string>? TypeMappings { get; set; }
    }

    public class OutputConfig
    {
        public string Entity { get; set; } = string.Empty;
        public string Dto { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class DtoConfig
    {
        public string? Description { get; set; }
        public string? Source { get; set; }
        public List<string>? Include { get; set; }
        public List<string>? Exclude { get; set; }
        public List<DtoSourceConfig>? Sources { get; set; }
        public bool Custom { get; set; }
        public List<PropertyConfig>? Properties { get; set; }
    }

    public class DtoSourceConfig
    {
        public string Table { get; set; } = string.Empty;
        public string Prefix { get; set; } = string.Empty;
        public List<string>? Include { get; set; }
    }

    public class MessageConfig
    {
        public MessagePartConfig? Request { get; set; }
        public MessagePartConfig? Response { get; set; }
    }

    public class MessagePartConfig
    {
        public string? Description { get; set; }
        public List<PropertyConfig>? Properties { get; set; }
    }

    public class PropertyConfig
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool Nullable { get; set; }
    }
}
