using Generator.Database.Models;
using System.Diagnostics;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Generator.Database.Services
{
    public class MetadataLoader
    {
        private readonly string MetaPath;

        public MetadataLoader(string metaPath)
        {
            MetaPath = metaPath;
        }

        public TablesMetadata? LoadTablesMetadata()
        {
            var filePath = Path.Combine(MetaPath, "tables.yaml");
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Warning: Tables metadata file not found: {filePath}");
                return null;
            }

            try
            {
                var yaml = File.ReadAllText(filePath);
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(PascalCaseNamingConvention.Instance)
                    .IgnoreUnmatchedProperties()
                    .Build();

                return deserializer.Deserialize<TablesMetadata>(yaml);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tables metadata: {ex.Message}");
                Debug.Assert(false, ex.Message);
                return null;
            }
        }

        public MappingsMetadata? LoadMappingsMetadata()
        {
            var filePath = Path.Combine(MetaPath, "mappings.yaml");
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Warning: Mappings metadata file not found: {filePath}");
                return null;
            }

            try
            {
                var yaml = File.ReadAllText(filePath);
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(PascalCaseNamingConvention.Instance)
                    .IgnoreUnmatchedProperties()
                    .Build();

                return deserializer.Deserialize<MappingsMetadata>(yaml);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading mappings metadata: {ex.Message}");
                Debug.Assert(false, ex.Message);
                return null;
            }
        }

        public string GetEntityClassName(TablesMetadata metadata, string tableName)
        {
            if (metadata.Tables.TryGetValue(tableName, out var tableConfig))
            {
                var entityValue = tableConfig.Generates?.Entity;
                if (entityValue != null)
                {
                    if (entityValue is string entityName && !string.IsNullOrEmpty(entityName))
                    {
                        return entityName;
                    }

                    if (entityValue is bool entityBool && entityBool)
                    {
                        return $"{tableName}Entity";
                    }
                }
            }

            // Default 설정 확인
            if (metadata.Defaults?.Generates?.Entity != null)
            {
                var defaultEntity = metadata.Defaults.Generates.Entity;
                if (defaultEntity is bool defaultBool && defaultBool)
                {
                    return $"{tableName}Entity";
                }
            }

            return string.Empty;
        }

        public bool ShouldGenerateEntity(TablesMetadata metadata, string tableName)
        {
            // 테이블별 설정 확인
            if (metadata.Tables.TryGetValue(tableName, out var tableConfig))
            {
                var entityValue = tableConfig.Generates?.Entity;
                if (entityValue != null)
                {
                    // string 이름이 있으면 생성
                    if (entityValue is string entityName)
                    {
                        return !string.IsNullOrEmpty(entityName);
                    }

                    // bool 값이면 그대로 반환
                    if (entityValue is bool entityBool)
                    {
                        return entityBool;
                    }
                }
            }

            // Default 설정 확인
            if (metadata.Defaults?.Generates?.Entity != null)
            {
                var defaultEntity = metadata.Defaults.Generates.Entity;
                if (defaultEntity is bool defaultBool)
                {
                    return defaultBool;
                }
            }

            return false;
        }

        public List<string>? GetExcludedColumns(TablesMetadata metadata, string tableName)
        {
            if (metadata.Tables.TryGetValue(tableName, out var tableConfig))
            {
                return tableConfig.Exclude;
            }

            return null;
        }
    }
}
