using Generator.Database.Configuration;
using Generator.Database.Generators;
using Generator.Database.Models;
using System.Text;

namespace Generator.Database.Services
{
    public class CodeGeneratorService
    {
        private readonly string CommonPath;
        private readonly PathOptions Paths;
        private readonly MetadataLoader? MetaLoader;

        public CodeGeneratorService(string commonPath, PathOptions path, string? metaPath = null)
        {
            CommonPath = commonPath;
            Paths = path;

            if (!string.IsNullOrEmpty(metaPath) && Directory.Exists(metaPath))
            {
                MetaLoader = new MetadataLoader(metaPath);
            }
        }

        public async Task<(List<string>, List<string>)> GenerateCodesAsync(string database, DatabaseSchema schema)
        {
            var generatedFiles = new List<string>();
            var allFiles = new List<string>();

            Console.WriteLine($"Generating codes for database: {schema.DatabaseName}");

            if (MetaLoader != null)
            {
                var tableCode = await GenerateEntitiesAsync(database, schema.Tables);
                generatedFiles.AddRange(tableCode.Item1);
                allFiles.AddRange(tableCode.Item2);
            }
            else
            {
                Console.WriteLine("⚠️ No metadata loader configured. Skipping generation.");
            }

            Console.WriteLine($"Generated {generatedFiles.Count} files successfully.");

            return (generatedFiles, allFiles);
        }

        private async Task<(List<string>, List<string>)> GenerateEntitiesAsync(string database, List<TableSchema> tables)
        {
            var directoryPath = Path.Combine(CommonPath, Paths.TablePath);
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }

            var generatedFiles = new List<string>();
            var allFiles = new List<string>();

            if (MetaLoader == null)
            {
                return (generatedFiles, allFiles);
            }

            Console.WriteLine("📋 Using YAML metadata for Entity generation...");

            var yamlGenerator = new YamlTableGenerator(MetaLoader);
            var generatedCodes = yamlGenerator.GenerateEntities(database, tables);

            foreach (var generatedCode in generatedCodes)
            {
                var saveResult = await SaveCodeToFileAsync(generatedCode);
                if (saveResult.Generated && !string.IsNullOrEmpty(saveResult.Path))
                {
                    generatedFiles.Add(saveResult.Path);
                    Console.WriteLine($"Generated entity: {generatedCode.FileName}");
                }

                allFiles.Add(saveResult.Path);
            }

            return (generatedFiles, allFiles);
        }

        private async Task<(bool Generated, string Path)> SaveCodeToFileAsync(GeneratedFile generated)
        {
            var directoryPath = Path.Combine(CommonPath, Paths.TablePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, generated.FileName);

            try
            {
                if (File.Exists(filePath))
                {
                    var existingContent = await File.ReadAllTextAsync(filePath);
                    if (NormalizeContent(existingContent) == NormalizeContent(generated.Content))
                    {
                        Console.WriteLine($"Skipped (no changes): {generated.FileName}");
                        return (false, filePath);
                    }
                }

                await File.WriteAllTextAsync(filePath, generated.Content, Encoding.UTF8);

                return (true, filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file {generated.FileName}: {ex.Message}");
                return (false, filePath);
            }
        }

        private static string NormalizeContent(string content)
        {
            return content?.Replace("\r\n", "\n").Replace("\r", "\n").Trim() ?? string.Empty;
        }

        public string GenerateSummary(List<string> generatedFiles)
        {
            if (generatedFiles.Count == 0)
            {
                return "No files were generated.";
            }

            var summary = new StringBuilder();
            summary.AppendLine($"Successfully generated {generatedFiles.Count} files:");
            summary.AppendLine();

            var groupedFiles = generatedFiles
                .GroupBy(f => Path.GetDirectoryName(f))
                .OrderBy(g => g.Key);

            foreach (var group in groupedFiles)
            {
                var relativePath = Path.GetRelativePath(CommonPath, group.Key ?? string.Empty);
                summary.AppendLine($"📁 {relativePath}");

                foreach (var file in group.OrderBy(f => f))
                {
                    var fileName = Path.GetFileName(file);
                    summary.AppendLine($"  📄 {fileName}");
                }
                summary.AppendLine();
            }

            return summary.ToString();
        }
    }
}
