using Generator.Database.Configuration;
using Generator.Database.Services;
using Microsoft.Extensions.Configuration;
using System.Text;

Console.Title = "PlayGround Database Code Generator";
Console.OutputEncoding = Encoding.UTF8;

try
{
    Console.WriteLine("🚀 PlayGround Database Code Generator");
    Console.WriteLine("=====================================");
    Console.WriteLine();

    // Configuration 빌드
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
        .Build();

    Console.WriteLine($"📋 Configuration: appsettings.json");
    var dbConfig = configuration.GetSection("CodeGenerationSettings").Get<CodeGenerationSettings>();
    if (dbConfig == null)
    {
        ShowUsage();
        return 1;
    }

    foreach (var database in dbConfig.Databases)
    {
        var value = database.Value;
        Console.WriteLine($"   Connection: {MaskConnectionString(value.ConnectionString)}");
        Console.WriteLine($"   Output Path Table: {value.Paths.TablePath}");
        Console.WriteLine($"   Output Path Procedure: {value.Paths.ProcedurePath}");
        Console.WriteLine($"   Generate Tables: {value.Options.GenerateTable}");
        Console.WriteLine($"   Generate Procedures: {value.Options.GenerateProcedure}");
        Console.WriteLine();

        Console.WriteLine($"📖 Reading database schema... {database.Key}");
        var schemaReader = new DatabaseSchemaReader(value.ConnectionString);
        var schema = await schemaReader.ReadSchemaAsync();

        Console.WriteLine($"✅ Found {schema.Tables.Count} tables and {schema.Procedures.Count} stored procedures");
        Console.WriteLine();

        // 코드 생성
        Console.WriteLine("⚙️ Generating C# code...");
        var codeGenerator = new CodeGeneratorService(dbConfig.CommonPath, value.Paths);
        var generatedFiles = await codeGenerator.GenerateCodesAsync(database.Key, schema);

        Console.WriteLine();
        Console.WriteLine("📊 Generation Summary:");
        Console.WriteLine("=====================");
        Console.WriteLine(codeGenerator.GenerateSummary(generatedFiles.Item1));
    }

    Console.WriteLine("🎉 Code generation completed successfully!");

    return 0;
}
catch (Exception ex)
{
    Console.WriteLine();
    Console.WriteLine("❌ Error occurred during code generation:");
    Console.WriteLine($"   {ex.Message}");

    if (args.Contains("--verbose"))
    {
        Console.WriteLine();
        Console.WriteLine("Stack Trace:");
        Console.WriteLine(ex.StackTrace);
    }

    return 1;
}

static void ShowUsage()
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  Generator.Database <connection-string> <output-path> <namespace> [options]");
    Console.WriteLine("  Generator.Database <database-name> [options]");
    Console.WriteLine();
    Console.WriteLine("Arguments:");
    //Console.WriteLine("  connection-string  SQL Server connection string");
    //Console.WriteLine("  output-path        Output directory for generated files");
    //Console.WriteLine("  namespace          Root namespace for generated code");
    //Console.WriteLine("  database-name      Database name from appsettings (Accounts, Soccers)");
    Console.WriteLine();
    Console.WriteLine("Options:");
    //Console.WriteLine("  --no-tables        Skip table code generation");
    //Console.WriteLine("  --no-procedures    Skip stored procedure code generation");
    Console.WriteLine("  --verbose          Show detailed error information");
    Console.WriteLine();
    Console.WriteLine("Examples:");
    //Console.WriteLine("  Generator.Database \"Server=.;Database=MyDb;Integrated Security=true\" \"./Generated\" \"MyApp.Database\"");
    Console.WriteLine("  Generator.Database Accounts");
    //Console.WriteLine("  Generator.Database Soccers --no-procedures");
    Console.WriteLine();
}

static string MaskConnectionString(string connectionString)
{
    var masked = connectionString;
    var passwordPatterns = new[] {
        @"Password=([^;]+)",
        @"Pwd=([^;]+)",
        @"Password\s*=\s*([^;]+)",
        @"Pwd\s*=\s*([^;]+)"
    };

    foreach (var pattern in passwordPatterns)
    {
        masked = System.Text.RegularExpressions.Regex.Replace(
            masked,
            pattern,
            m => $"{m.Groups[0].Value.Split('=')[0]}=***",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );
    }

    return masked;
}
