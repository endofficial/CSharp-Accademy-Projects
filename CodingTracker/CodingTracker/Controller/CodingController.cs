using Spectre.Console;
using CodingTracker;
using Dapper;
using Microsoft.Data.Sqlite;
using CodingTracker.Data;
using System.Data.SQLite;
using CodingTracker.Model;

namespace CodingTracker.Controller;

internal class CodingController : Database
{
    public static bool RegisterSession()
    {
        Clear();

        string dateSession = InputInsert.GetDateSessionInput();
        if (dateSession == "0") return false;

        var durationSession = InputInsert.GetTimeSessionInput(dateSession);
        if (durationSession == null) return false;

        if (durationSession != null)
        {
            using var connection = GetConnection();

            string sql = @"
                INSERT INTO CodingSessions (StartTime, EndTime, Date, Duration, Description)
                VALUES (@StartTime, @EndTime, @Date, @Duration, @Description);";

            connection.Execute(sql, durationSession);
        }

        return true;
    }

    public static bool ViewSessions()
    {
        Clear();

        using var connection = GetConnection();

        string sql = @"
            SELECT * FROM CodingSessions";

        List<CodingSessions> tableData = new List<CodingSessions>();

        var sessions = connection.Query<CodingSessions>(sql).ToList(); // Execute the query and map results to CodingSessions objects

        var table = new Table();
        table.Border(TableBorder.Rounded);

        table.AddColumn("[yellow]ID[/]");
        table.AddColumn("[yellow]Start Time[/]");
        table.AddColumn("[yellow]End Time[/]");
        table.AddColumn("[yellow]Date[/]");
        table.AddColumn("[yellow]Duration[/]");
        table.AddColumn("[yellow]Description[/]");

        foreach (var session in sessions)
        {
            table.AddRow(
                session.Id.ToString(),
                session.StartTime.ToString("HH:mm"),
                session.EndTime.ToString("HH:mm"),
                session.Date,
                session.Duration.ToString(@"hh\:mm"),
                (session.Description ?? "Empty").ToString()
                );
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("\n[yellow]Press any key to continue...[/]");
        Console.ReadKey();

        return true;
    }
}

