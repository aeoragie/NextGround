using Generator.Database.Models;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Generator.Database.Services
{
    public class DatabaseSchemaReader
    {
        private readonly string ConnectionString;
        private readonly List<string> ExcludeTables = new List<string>() { "sysdiagrams" };
        //private readonly List<string> ExcludeProcedures = new List<string>() { };

        public DatabaseSchemaReader(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async Task<DatabaseSchema> ReadSchemaAsync()
        {
            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var schema = new DatabaseSchema
            {
                DatabaseName = connection.Database,
                Tables = await ReadTablesAsync(connection),
                Procedures = await ReadProceduresAsync(connection)
            };

            return schema;
        }

        private async Task<List<TableSchema>> ReadTablesAsync(SqlConnection connection)
        {
            var tableQuery = @"
                SELECT
                    TABLE_NAME,
                    TABLE_SCHEMA
                FROM
                    INFORMATION_SCHEMA.TABLES 
                WHERE
                    TABLE_TYPE = 'BASE TABLE'
                ORDER BY TABLE_NAME
            ";

            using var tableCommand = new SqlCommand(tableQuery, connection);
            using var tableReader = await tableCommand.ExecuteReaderAsync();

            var tableNames = new List<(string TableName, string Schema)>();
            while (await tableReader.ReadAsync())
            {
                tableNames.Add(
                    (tableReader.GetString(0), tableReader.GetString(1))
                );
            }
            tableReader.Close();

            try
            {
                var tables = new List<TableSchema>();
                tableNames = tableNames.Where(x => (
                    (x.TableName.Contains('_') == false) &&
                    (x.Schema.Equals("nested", StringComparison.OrdinalIgnoreCase) == false) &&
                    (ExcludeTables.Contains(x.TableName) == false)
                )).ToList();
                foreach (var pair in tableNames)
                {
                    var columns = await ReadTableColumnsAsync(connection, pair.TableName);
                    tables.Add(new TableSchema
                    {
                        Schema = pair.Schema,
                        TableName = pair.TableName,
                        Columns = columns
                    });
                }
                return tables;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Debug.Assert(false, ex.Message);
                return new List<TableSchema>();
            }
        }

        private async Task<List<ColumnSchema>> ReadTableColumnsAsync(SqlConnection connection, string tableName)
        {
            var columnQuery = @"
                SELECT
                    c.COLUMN_NAME,
                    c.DATA_TYPE,
                    c.IS_NULLABLE,
                    c.CHARACTER_MAXIMUM_LENGTH,
                    c.NUMERIC_PRECISION,
	                c.NUMERIC_SCALE,
                    t.name as USER_DEFINED_TYPE_NAME
                FROM
                    INFORMATION_SCHEMA.COLUMNS c LEFT JOIN sys.types t ON c.DATA_TYPE = t.name
                WHERE
                    c.TABLE_NAME = @TableName
                ORDER BY c.ORDINAL_POSITION
            ";

            using var command = new SqlCommand(columnQuery, connection);
            command.Parameters.AddWithValue("@TableName", tableName);

            try
            {
                var columns = new List<ColumnSchema>();
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    columns.Add(new ColumnSchema
                    {
                        ColumnName = reader.GetString(0),
                        DataType = reader.GetString(1),
                        IsNullable = reader.GetString(2).Equals("yes", StringComparison.OrdinalIgnoreCase),
                        CharacterMaximumLength = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                        NumericPrecision = reader.IsDBNull(4) ? null : Convert.ToInt32(reader.GetByte(4)),
                        NumericScale = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                        UserDefinedType = reader.IsDBNull(6) ? null : reader.GetString(6)
                    });
                }
                return columns;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Debug.Assert(false, ex.Message);
                return new List<ColumnSchema>();
            }
        }

        private async Task<List<ProcedureSchema>> ReadProceduresAsync(SqlConnection connection)
        {
            var procedureQuery = @"
                SELECT
                    ROUTINE_SCHEMA,
                    ROUTINE_NAME
                FROM
                    INFORMATION_SCHEMA.ROUTINES 
                WHERE
                    ROUTINE_TYPE = 'PROCEDURE' ORDER BY ROUTINE_NAME
            ";

            using var procedureCommand = new SqlCommand(procedureQuery, connection);
            using var procedureReader = await procedureCommand.ExecuteReaderAsync();

            var procedureNames = new List<(string Schema, string Procedure)>();
            while (await procedureReader.ReadAsync())
            {
                procedureNames.Add(
                    (procedureReader.GetString(0), procedureReader.GetString(1))
                );
            }
            procedureReader.Close();

            try
            {
                // 리턴 스케마가 있는 Nested 프로시저는 먼저 처리
                var nesteds = new Dictionary<string, ProcedureSchema>();
                var nestedsProcedureNames = procedureNames.Where(x => (
                    x.Procedure.Contains('_') == false &&
                    x.Schema.Equals("nested", StringComparison.OrdinalIgnoreCase))
                ).ToList();
                foreach (var pair in nestedsProcedureNames)
                {
                    var parameters = await ReadProcedureParametersAsync(connection, pair.Schema, pair.Procedure);
                    var outParameter = await HasOutParameterAsync(connection, pair.Schema, pair.Procedure);
                    (var resultColumns, var resultReturn) = await ReadProcedureResultColumnsAsync(connection, pair.Schema, pair.Procedure, new());
                    nesteds.Add(pair.Procedure, new ProcedureSchema
                    {
                        Schema = pair.Schema,
                        ProcedureName = pair.Procedure,
                        Parameters = parameters,
                        HasOutParameter = outParameter,
                        HasReturn = resultReturn,
                        ResultColumns = resultColumns
                    });
                }

                var procedures = new List<ProcedureSchema>();
                procedureNames = procedureNames.Where(x => (
                    x.Procedure.Contains('_') == false &&
                    x.Schema.Equals("nested", StringComparison.OrdinalIgnoreCase) == false)
                ).ToList();
                foreach (var pair in procedureNames)
                {
                    var parameters = await ReadProcedureParametersAsync(connection, pair.Schema, pair.Procedure);
                    var outParameter = await HasOutParameterAsync(connection, pair.Schema, pair.Procedure);
                    (var resultColumns, var resultReturn) = await ReadProcedureResultColumnsAsync(connection, pair.Schema, pair.Procedure, nesteds);
                    procedures.Add(new ProcedureSchema
                    {
                        Schema = pair.Schema,
                        ProcedureName = pair.Procedure,
                        Parameters = parameters,
                        HasOutParameter = outParameter,
                        HasReturn = resultReturn,
                        ResultColumns = resultColumns
                    });
                }

                return procedures;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Debug.Assert(false, ex.Message);
                return new List<ProcedureSchema>();
            }
        }

        private async Task<List<ParameterSchema>> ReadProcedureParametersAsync(SqlConnection connection, string procedureSchema, string procedureName)
        {
            var parameterQuery = @"
                SELECT 
                    p.PARAMETER_NAME,
                    p.DATA_TYPE,
                    p.PARAMETER_MODE,
                    p.CHARACTER_MAXIMUM_LENGTH,
                    t.name as USER_DEFINED_TYPE_NAME
                FROM
                    INFORMATION_SCHEMA.PARAMETERS p LEFT JOIN sys.types t ON p.DATA_TYPE = t.name
                WHERE
                    p.SPECIFIC_NAME = @ProcedureName AND p.SPECIFIC_SCHEMA = @ProcedureSchema AND p.PARAMETER_NAME IS NOT NULL
                ORDER BY p.ORDINAL_POSITION
            ";

            using var command = new SqlCommand(parameterQuery, connection);
            command.Parameters.AddWithValue("@ProcedureName", procedureName);
            command.Parameters.AddWithValue("@ProcedureSchema", procedureSchema);

            try
            {
                var parameters = new List<ParameterSchema>();
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    parameters.Add(new ParameterSchema
                    {
                        ParameterName = reader.GetString(0),
                        DataType = reader.GetString(1),
                        ParameterMode = reader.GetString(2),
                        CharacterMaximumLength = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                        DefinedType = reader.IsDBNull(4) ? null : reader.GetString(4)
                    });
                }
                return parameters;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Debug.Assert(false, ex.Message);
                return new List<ParameterSchema>();
            }
        }

        private async Task<bool> HasOutParameterAsync(SqlConnection connection, string procedureSchema, string procedureName)
        {
            var parameterQuery = @"
                SELECT
                    COUNT(*)
                FROM
                    INFORMATION_SCHEMA.PARAMETERS
                WHERE
                    SPECIFIC_NAME = @ProcedureName AND SPECIFIC_SCHEMA = @ProcedureSchema AND PARAMETER_MODE = 'INOUT' AND PARAMETER_NAME = '@RETURN_VALUE'
            ";

            using var command = new SqlCommand(parameterQuery, connection);
            command.Parameters.AddWithValue("@ProcedureName", procedureName);
            command.Parameters.AddWithValue("@ProcedureSchema", procedureSchema);

            var count = (int)(await command.ExecuteScalarAsync() ?? 0);
            return count > 0;
        }

        private async Task<(List<ResultColumnSchema> Columns, bool HasReturn)> ReadProcedureResultColumnsAsync(SqlConnection connection, string procedureSchema, string procedureName, Dictionary<string, ProcedureSchema> nesteds)
        {
            bool returnValue = false;
            var returnColumns = new List<ResultColumnSchema>();

            // 프로시저 내용을 읽어서 RESULT_TYPE 주석 분석
            var definitionQuery = @"
                SELECT OBJECT_DEFINITION(OBJECT_ID(@FullProcedureName))
            ";

            using var command = new SqlCommand(definitionQuery, connection);
            command.Parameters.AddWithValue("@FullProcedureName", $"{procedureSchema}.{procedureName}");

            try
            {
                var definition = await command.ExecuteScalarAsync() as string;
                if (string.IsNullOrEmpty(definition))
                {
                    return (returnColumns, returnValue);
                }

                // RETURN 0 찾기
                var returnPattern = @"RETURN\s+-?\d+";
                var returnTypeMatch = Regex.Match(definition, returnPattern, RegexOptions.IgnoreCase);
                if (returnTypeMatch.Success)
                {
                    returnValue = true;
                }

                // Tables 주석 찾기
                var resultTablePattern = @"--\s*Results:\s*(.+)";
                var resultTableMatch = Regex.Match(definition, resultTablePattern, RegexOptions.IgnoreCase);
                if (resultTableMatch.Success)
                {
                    var matchedStr = resultTableMatch.Groups[1].Value.Trim();
                    if (string.IsNullOrEmpty(matchedStr))
                    {
                        Debug.Assert(false, "Tables is empty");
                        return (returnColumns, returnValue);
                    }

                    // Nested Procedure (예: "Procedure:NspGetUserWithSocialAccount")
                    if (matchedStr.StartsWith("Procedure:", StringComparison.OrdinalIgnoreCase))
                    {
                        Debug.Assert(nesteds.Count > 0, "No nested procedures available");
                        var nestedProcedureName = matchedStr.Substring(10).Trim();
                        if (nesteds.TryGetValue(nestedProcedureName, out var nestedProcedure))
                        {
                            returnColumns.AddRange(nestedProcedure.ResultColumns);
                        }
                        else
                        {
                            Console.WriteLine($"Error: Nested procedure '{nestedProcedureName}' not found.");
                            Debug.Assert(false, $"Nested procedure '{nestedProcedureName}' not found.");
                        }
                    }
                    // 조인 테이블인 경우 (예: "User s, SocialAccount sa")
                    else if (matchedStr.Contains(",", StringComparison.OrdinalIgnoreCase))
                    {
                        var tableMappings = ParseTableMappings(matchedStr);

                        // SELECT 절 파싱해서 실제 선택된 컬럼 확인
                        var selectColumns = ParseSelectColumns(definition, resultTableMatch.Index);

                        // 각 컬럼에 대해 테이블 스키마 정보 매칭
                        foreach (var columnInfo in selectColumns)
                        {
                            var tableAlias = columnInfo.TableAlias;
                            var columnName = columnInfo.ColumnName;
                            var outputName = columnInfo.OutputName;

                            if (tableMappings.TryGetValue(tableAlias, out var tableInfo))
                            {
                                var columnSchema = await GetColumnSchemaFromTableAsync(connection, tableInfo.TableName, columnName);
                                if (columnSchema != null)
                                {
                                    // AS 별칭이 있으면 출력 이름 변경
                                    if (!string.IsNullOrEmpty(outputName))
                                    {
                                        columnSchema.ColumnName = outputName;
                                    }
                                    returnColumns.Add(columnSchema);
                                }
                                else
                                {
                                    // 컬럼이 존재하지 않는 경우 에러
                                    Console.WriteLine($"Error: Column '{columnName}' not found in table '{tableInfo.TableName}'.");
                                    Debug.Assert(false, $"Column '{columnName}' not found in table '{tableInfo.TableName}'.");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Error: Table alias '{tableAlias}' not found in ResultType mapping.");
                                Debug.Assert(false, $"Table alias '{tableAlias}' not found in ResultType mapping.");
                            }
                        }
                    }
                    // 단일 테이블인 경우 (예: "Table:User")
                    else if (matchedStr.StartsWith("Table:", StringComparison.OrdinalIgnoreCase))
                    {
                        var tableName = matchedStr.Substring(6).Trim();
                        var tableColumns = await ReadTableColumnsAsync(connection, tableName);
                        foreach (var col in tableColumns)
                        {
                            returnColumns.Add(new ResultColumnSchema
                            {
                                ColumnName = col.ColumnName,
                                DataType = col.DataType,
                                IsNullable = col.IsNullable,
                                MaxLength = col.CharacterMaximumLength,
                                SourceTableName = tableName
                            });
                        }
                    }
                    // 커스텀 결과인 경우 (예: "Custom:TokenId:BIGINT,TokenId:BIGINT,TokenId:BIGINT...")
                    else if (matchedStr.StartsWith("Custom:", StringComparison.OrdinalIgnoreCase))
                    {
                        var customColumns = matchedStr.Substring(7).Trim().Split(',');
                        foreach (var columnInfo in customColumns)
                        {
                            var nameAndTypes = columnInfo.Split(':');
                            Debug.Assert(nameAndTypes.Length == 2, $"Invalid custom column definition: {columnInfo}");

                            returnColumns.Add(new ResultColumnSchema
                            {
                                ColumnName = nameAndTypes[0].Trim(),
                                DataType = nameAndTypes[1].Trim(),
                                IsNullable = true,
                                MaxLength = null
                            });
                        }
                    }
                    else
                    {
                        Debug.Assert(false, $"Invalid Result Tables {matchedStr}");
                    }
                }

                return (returnColumns, returnValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading procedure result columns: {ex.Message}");
                Debug.Assert(false, ex.Message);
                return (returnColumns, returnValue);
            }
        }

        private Dictionary<string, (string TableName, bool IsLeftJoin)> ParseTableMappings(string resultTypeInfo)
        {
            var mappings = new Dictionary<string, (string, bool)>();

            // "Users u, SocialAccounts sa" 형식 파싱
            var parts = resultTypeInfo.Split(',');
            foreach (var part in parts)
            {
                var trimmed = part.Trim();
                var tokens = trimmed.Split(' ');
                if (tokens.Length >= 2)
                {
                    var tableName = tokens[0];
                    var alias = tokens[1];
                    mappings[alias] = (tableName, false);
                }
            }

            return mappings;
        }

        private class ColumnInfo
        {
            public string TableAlias { get; set; } = string.Empty;
            public string ColumnName { get; set; } = string.Empty;
            public string? OutputName { get; set; }
        }

        private List<ColumnInfo> ParseSelectColumns(string definition, int startIndex)
        {
            var columns = new List<ColumnInfo>();
        
            // SELECT ~ FROM 절 추출
            var selectPattern = @"SELECT\s+(.+?)\s+FROM";
            var selectMatch = Regex.Match(definition.Substring(startIndex), selectPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
        
            if (!selectMatch.Success)
                return columns;
        
            var selectClause = selectMatch.Groups[1].Value;
        
            // 컬럼 표현식 파싱 정규식
            // 매칭 패턴: alias.[column] AS [name] 또는 alias.[column]
            var columnPattern = @"(\w+)\.\[?(\w+)\]?(?:\s+AS\s+\[?(\w+)\]?)?";
            var matches = Regex.Matches(selectClause, columnPattern, RegexOptions.IgnoreCase);
        
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    var tableAlias = match.Groups[1].Value;
                    var columnName = match.Groups[2].Value;
                    var outputName = match.Groups[3].Success ? match.Groups[3].Value : columnName;
        
                    columns.Add(new ColumnInfo
                    {
                        TableAlias = tableAlias,
                        ColumnName = columnName,
                        OutputName = outputName
                    });
                }
            }
        
            return columns;
        }

        private async Task<ResultColumnSchema?> GetColumnSchemaFromTableAsync(SqlConnection connection, string tableName, string columnName)
        {
            var query = @"
                SELECT
                    c.COLUMN_NAME,
                    c.DATA_TYPE,
                    c.IS_NULLABLE,
                    c.CHARACTER_MAXIMUM_LENGTH
                FROM
                    INFORMATION_SCHEMA.COLUMNS c
                WHERE
                    c.TABLE_NAME = @TableName AND c.COLUMN_NAME = @ColumnName
            ";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TableName", tableName);
            command.Parameters.AddWithValue("@ColumnName", columnName);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ResultColumnSchema
                {
                    ColumnName = reader.GetString(0),
                    DataType = reader.GetString(1),
                    IsNullable = reader.GetString(2).Equals("YES", StringComparison.OrdinalIgnoreCase),
                    MaxLength = reader.IsDBNull(3) ? null : reader.GetInt32(3)
                };
            }

            return null;
        }
    }
}

