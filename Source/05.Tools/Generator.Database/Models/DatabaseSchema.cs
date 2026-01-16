namespace Generator.Database.Models
{
    public class DatabaseSchema
    {
        public string DatabaseName { get; set; } = string.Empty;
        public List<TableSchema> Tables { get; set; } = new List<TableSchema>();
        public List<ProcedureSchema> Procedures { get; set; } = new List<ProcedureSchema>();
    }

    public class TableSchema
    {
        public string Schema { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;
        public List<ColumnSchema> Columns { get; set; } = new List<ColumnSchema>();
    }

    public class ColumnSchema
    {
        public string ColumnName { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public string? UserDefinedType { get; set; }
        public bool IsNullable { get; set; }
        public int? CharacterMaximumLength { get; set; }
        public int? NumericPrecision { get; set; }
        public int? NumericScale { get; set; }
    }

    public class ProcedureSchema
    {
        public string Schema { get; set; } = string.Empty;
        public string ProcedureName { get; set; } = string.Empty;
    }

    public class GeneratedFile
    {
        public string FileName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
    }
}
