using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace Flashcards.DataAccess;

// Get connection string from AppSettings.json and create tables if they don't exist
internal class Database
{
	private static string _connectionString = string.Empty;
	private static string _masterConnectionString = string.Empty;

    public Database()
	{
		var config = new ConfigurationBuilder()
			.AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
			.Build();

		_connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found in configuration.");
        _masterConnectionString = config.GetConnectionString("MasterConnection") ?? throw new InvalidOperationException("Master connection string not found in configuration.");
    }

	public static SqlConnection GetConnection()
	{
		return new SqlConnection(_connectionString);
	}

	public void Initialize()
	{
		AnsiConsole.Status()
			.Start("Initializing database...", ctx =>
			{
				try
				{
                    using var masterConnection = new SqlConnection(_masterConnectionString);
                    masterConnection.Open();
                    masterConnection.Execute("IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Flashcards') CREATE DATABASE [Flashcards];");
					masterConnection.Close();

                    using var connection = GetConnection();

					string sql = @"
					IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stacks' AND schema_id = SCHEMA_ID('dbo'))
					BEGIN
						CREATE TABLE dbo.Stacks (
							StackID INT IDENTITY (1, 1) PRIMARY KEY,
							NomeStack VARCHAR(50) UNIQUE NOT NULL 
						);
					END;

					IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Flashcards' AND schema_id = SCHEMA_ID('dbo'))
					BEGIN
						CREATE TABLE dbo.Flashcards (
							FlashcardsID INT IDENTITY (1, 1) PRIMARY KEY,
							StackID INT NOT NULL,
							Front NVARCHAR(255) NOT NULL,
							Back NVARCHAR(255) NOT NULL,
							CONSTRAINT FK_Flashcards_Stacks FOREIGN KEY (StackID) 
								REFERENCES dbo.Stacks (StackID)
								ON DELETE CASCADE
						);
					END; 

					IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudySessions' AND schema_id = SCHEMA_ID('dbo'))
					BEGIN	
						CREATE TABLE dbo.StudySessions (
							SessionID INT PRIMARY KEY IDENTITY (1, 1),
							StackID INT NOT NULL,
							Date DATETIME NOT NULL DEFAULT GETDATE(),
							Score INT NOT NULL,
							CONSTRAINT FK_Studysessions_Stacks
							FOREIGN KEY (StackID) REFERENCES dbo.Stacks (StackID)
							ON DELETE CASCADE
						);
					END;";

					connection.Execute(sql);

					Thread.Sleep(2000);

					ctx.Status("Loading...");
					ctx.Spinner(Spinner.Known.Arc);
					ctx.SpinnerStyle(Style.Parse("green on black"));

                    Thread.Sleep(2000);
					ctx.Status("[green]Database initialized successfully![/]");
					Thread.Sleep(2000);
                }
				catch (Exception ex)
				{
					AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
				}

            });
    }
                
}
