using Generator.Database.Configuration;
using Generator.Database.Services;
using Microsoft.Extensions.Configuration;
using System.Text;

Console.Title = "Database Code Generator";
Console.OutputEncoding = Encoding.UTF8;

try
{
    Console.WriteLine("🚀 Database Code Generator");
    Console.WriteLine("==========================");
    Console.WriteLine();

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
        Console.WriteLine($"📦 Database: {database.Key}");
        Console.WriteLine($"   Output Path: {value.Paths.TablePath}");
        Console.WriteLine($"   Meta Path: {value.MetaPath}");
        Console.WriteLine($"   SQL Tables Path: {value.SqlTablesPath}");
        Console.WriteLine();

        if (string.IsNullOrEmpty(value.SqlTablesPath))
        {
            Console.WriteLine($"⚠️ Skipping {database.Key}: SqlTablesPath not configured");
            continue;
        }

        Console.WriteLine($"📖 Reading schema from SQL files...");
        var sqlFileReader = new SqlFileSchemaReader(value.SqlTablesPath);
        var tables = sqlFileReader.ReadTablesFromSqlFiles();

        var schema = new Generator.Database.Models.DatabaseSchema
        {
            DatabaseName = database.Key,
            Tables = tables,
            Procedures = new List<Generator.Database.Models.ProcedureSchema>()
        };

        Console.WriteLine($"✅ Found {schema.Tables.Count} tables from SQL files");
        Console.WriteLine();

        Console.WriteLine("⚙️ Generating C# code...");
        var codeGenerator = new CodeGeneratorService(dbConfig.CommonPath, value.Paths, value.MetaPath);
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
    Console.WriteLine("Usage: Generator.Database [options]");
    Console.WriteLine();
    Console.WriteLine("Options:");
    Console.WriteLine("  --verbose    Show detailed error information");
    Console.WriteLine();
    Console.WriteLine("Configuration is read from appsettings.json");
}
