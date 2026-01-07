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
        public CodeGeneratorService(string commonPath, PathOptions path)
        {
            CommonPath = commonPath;
            Paths = path;
        }

        public async Task<(List<string>, List<string>)> GenerateCodesAsync(string database, DatabaseSchema schema)
        {
            var generatedFiles = new List<string>();
            var allFiles = new List<string>();

            Console.WriteLine($"Generating codes for database: {schema.DatabaseName}");

            // 테이블 코드 생성
            var tableCode = await GenerateTableCodesAsync(schema.DatabaseName, schema.Tables);
            generatedFiles.AddRange(tableCode.Item1);
            allFiles.AddRange(tableCode.Item2);

            // 프로시저 코드 생성
            var procedureCodes = await GenerateProcedureCodesAsync(database, schema.DatabaseName, schema.Procedures);
            generatedFiles.AddRange(procedureCodes.Item1);
            allFiles.AddRange(procedureCodes.Item2);

            // Extension 코드 생성
            //var extensionCodes = await GenerateExtensionCodesAsync(database, schema.DatabaseName, schema.Tables, schema.Procedures);
            //generatedFiles.AddRange(extensionCodes.Item1);
            //allFiles.AddRange(extensionCodes.Item2);

            Console.WriteLine($"Generated {generatedFiles.Count} files successfully.");

            return (generatedFiles, allFiles);
        }

        public async Task<List<string>> RemoveFilesAsync(List<string> keepFiles)
        {
            await Task.CompletedTask;
            return new List<string>();
        }

        private async Task<(List<string>, List<string>)> GenerateTableCodesAsync(string database, List<TableSchema> tables)
        {
            var directoryPath = Path.Combine(CommonPath, SubPath(CodeType.Table));
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }

            var generatedFiles = new List<string>();
            var allFiles = new List<string>();
            foreach (var table in tables)
            {
                var generatedCode = CSharpTableGenerator.Generate(database, table);
                if (generatedCode == null)
                {
                    Console.WriteLine($"Skipped table: {table.TableName}");
                    continue;
                }

                var saveResult = await SaveCodeToFileAsync(generatedCode);
                if (saveResult.Generated && !string.IsNullOrEmpty(saveResult.Path))
                {
                    generatedFiles.Add(saveResult.Path);
                    Console.WriteLine($"Generated table: {table.TableName} -> {Path.GetFileName(saveResult.Path)}");
                }

                allFiles.Add(saveResult.Path);
            }

            return (generatedFiles, allFiles);
        }

        private async Task<(List<string>, List<string>)> GenerateProcedureCodesAsync(string database, string fullName, List<ProcedureSchema> procedures)
        {
            var directoryPath = Path.Combine(CommonPath, SubPath(CodeType.StoredProcedure));
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }

            var generatedFiles = new List<string>();
            var allFiles = new List<string>();
            foreach (var procedure in procedures)
            {
                var generatedCode = CSharpProcedureGenerator.Generate(database, fullName, procedure);
                if (generatedCode == null)
                {
                    Console.WriteLine($"Skipped table: {procedure.ProcedureName}");
                    continue;
                }

                var saveResult = await SaveCodeToFileAsync(generatedCode);
                if (saveResult.Generated && !string.IsNullOrEmpty(saveResult.Path))
                {
                    generatedFiles.Add(saveResult.Path);
                    Console.WriteLine($"Generated procedure: {procedure.ProcedureName} -> {Path.GetFileName(saveResult.Path)}");
                }

                allFiles.Add(saveResult.Path);
            }

            return (generatedFiles, allFiles);
        }

        //private async Task<(List<string>, List<string>)> GenerateExtensionCodesAsync(string database, string fullName, List<TableSchema> tables, List<ProcedureSchema> procedures)
        //{
        //    var generatedFiles = new List<string>();
        //    var allFiles = new List<string>();
        //
        //    // 테이블별로 해당 테이블을 사용하는 프로시저들 찾기
        //    foreach (var table in tables)
        //    {
        //        var relatedProcedures = procedures
        //            .Where(p => p.ResultColumns != null &&
        //                       p.ResultColumns.Any(c => c.SourceTableName == table.TableName))
        //            .Select(p => p.ProcedureName)
        //            .ToList();
        //
        //        if (relatedProcedures.Count > 0)
        //        {
        //            var generatedCode = CSharpExtensionGenerator.GenerateForTable(database, fullName, table.TableName, relatedProcedures);
        //
        //            var saveResult = await SaveCodeToFileAsync(generatedCode);
        //            if (saveResult.Generated && !string.IsNullOrEmpty(saveResult.Path))
        //            {
        //                generatedFiles.Add(saveResult.Path);
        //                Console.WriteLine($"Generated extension: {table.TableName}Extensions -> {Path.GetFileName(saveResult.Path)}");
        //            }
        //
        //            allFiles.Add(saveResult.Path);
        //        }
        //    }
        //
        //    return (generatedFiles, allFiles);
        //}

        private string SubPath(CodeType codeType)
        {
            return codeType switch
            {
                CodeType.Table => Paths.TablePath,
                CodeType.StoredProcedure => Paths.ProcedurePath,
                CodeType.Extension => Paths.ExtensionPath,
                CodeType.TableValueParameter => Paths.OtherPath,
                _ => throw new NotImplementedException()
            };

        }

        private async Task<(bool Generated, string Path)> SaveCodeToFileAsync(GeneratedFile generated)
        {
            var directoryPath = Path.Combine(CommonPath, SubPath(generated.CodeType));
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, generated.FileName);

            try
            {
                // 파일이 이미 존재하고 내용이 동일한 경우 스킵
                if (File.Exists(filePath))
                {
                    var existingContent = await File.ReadAllTextAsync(filePath);
                    if (NormalizeContent(existingContent) == NormalizeContent(generated.Content))
                    {
                        Console.WriteLine($"Skipped (no changes): {generated.FileName}");
                        return (false, filePath);
                    }
                }

                // 파일 쓰기
                await File.WriteAllTextAsync(filePath, generated.Content, System.Text.Encoding.UTF8);

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
