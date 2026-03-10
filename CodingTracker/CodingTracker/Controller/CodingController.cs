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

    public static bool UpdateSession()
    {
        Clear();

        AnsiConsole.MarkupLine("[Aquamarine3]Update a session.[/]\n");

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

        int NumberId = InputInsert.GetId();
        if (NumberId == 0) return false;

        string sqlId = @"
            SELECT EXISTS (SELECT 1 FROM CodingSessions WHERE Id = @Id)";

        bool exists = connection.ExecuteScalar<bool>(sqlId, new { Id = NumberId });

        while (!exists)
        {
            AnsiConsole.MarkupLine("[red]Record not found![/]\n");

            NumberId = InputInsert.GetId();
            if (NumberId == 0) return false;

            exists = connection.ExecuteScalar<bool>(sqlId, new { Id = NumberId });
        }

        string upInput = AnsiConsole.Ask<string>("\n[bold]Type 1 if you want update the start time.\nType 2 to update the end time.\nType 3 to update the date.\nType 4 to update the description.\n[/][yellow]Type 0 to return to main menu.[/]");
        if (upInput == "0") return false;

        while (!Int32.TryParse(upInput, out _) || Convert.ToInt32(upInput) < 0)
        {
            AnsiConsole.MarkupLine("[red]Invalid input! Please enter a valid number.[/]\n");
            upInput = AnsiConsole.Ask<string>("\n[bold]Type 1 if you want update the start time.\nType 2 to update the end time.\nType 3 to update the date.\nType 4 to update the description.\n[/][yellow]Type 0 to return to main menu.[/]");
            if (upInput == "0") return false;
        }

        switch (upInput)
        {
            case "1":
                string startTime = InputInsert.OnlyStartTime();
                if (startTime == "0") return false;

                string sqlUpStartTime = @"
                    UPDATE CodingSessions SET StartTime = @StartTime WHERE Id = @Id";

                connection.ExecuteScalar(sqlUpStartTime, new { Id = NumberId, StartTime = startTime });
                break;
        }

        return true;
    }

}

