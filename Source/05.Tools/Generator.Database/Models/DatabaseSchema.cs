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
        //public int OrdinalPosition { get; set; }
        //public bool IsPrimaryKey { get; set; }
        //public bool IsIdentity { get; set; }
        //public string? DefaultValue { get; set; }
    }

    public class ProcedureSchema
    {
        public string Schema { get; set; } = string.Empty;
        public string ProcedureName { get; set; } = string.Empty;
        public List<ParameterSchema> Parameters { get; set; } = new List<ParameterSchema>();
        public bool HasOutParameter { get; set; } = false;
        public bool HasReturn { get; set; } = false;
        public List<ResultColumnSchema> ResultColumns { get; set; } = new List<ResultColumnSchema>();
    }

    public class ResultColumnSchema
    {
        public string ColumnName { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public bool IsNullable { get; set; }
        public int? MaxLength { get; set; }
        public string? SourceTableName { get; set; }
    }

    public class ParameterSchema
    {
        public string ParameterName { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public string? DefinedType { get; set; }
        public string ParameterMode { get; set; } = string.Empty;
        public int? CharacterMaximumLength { get; set; }
        //public int OrdinalPosition { get; set; }
        //public string? DefaultValue { get; set; }
    }

    public class GeneratedFile
    {
        public string FileName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public CodeType CodeType { get; set; }
        public string Database { get; set; } = string.Empty;
    }

    public enum CodeType
    {
        Table,
        StoredProcedure,
        TableValueParameter,
        Extension,
    }
}
