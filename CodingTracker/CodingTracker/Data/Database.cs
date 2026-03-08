using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;    
using Dapper;
using CodingTracker.Model;
using System.Runtime.InteropServices.Marshalling;

namespace CodingTracker.Data;

internal class Database
{
    // string.empty is used to initialize the connection string variable with an empty string, ensuring that it has a default value before being assigned the actual connection string from the configuration file.
    private static string _connectionString = string.Empty;

    public Database()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Retrieve the connection string from the configuration
        _connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found in configuration.");
    }

    public static SqliteConnection GetConnection()
    {
        // Create and return a new SQLite connection using the connection string
        return new SqliteConnection(_connectionString);
    }

    public void Initialize()
    {
        using var connection = GetConnection();

        string sql = @"
            CREATE TABLE IF NOT EXISTS CodingSessions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartTime TEXT NOT NULL,
                EndTime TEXT NOT NULL,
                Duration TEXT NOT NULL,
                Date TEXT NOT NULL,
                Description TEXT
            );";

        connection.Execute(sql);
    }
}

