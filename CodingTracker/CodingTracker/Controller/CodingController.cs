using Spectre.Console;
using CodingTracker;
using Dapper;
using Microsoft.Data.Sqlite;
using CodingTracker.Data;
using System.Data.SQLite;
using CodingTracker.Model;
using System.Diagnostics;

namespace CodingTracker.Controller;

internal class CodingController : Database
{
    public static bool LiveSession()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[Aquamarine3]Start a live coding session.\n[/]");

        string dateSession = InputInsert.GetDateSessionInput();
        if (dateSession == "0") return false;

        string input = AnsiConsole.Ask<string>("[bold]\nPress 'P' to start the session.[/][yellow]Type 0 to return to main menu.[/]");
        if (input.ToUpper() == "P")
        {
            var durationSession = InputInsert.StopwatchSession(dateSession);

            if (durationSession != null)
            {
                using var connection = GetConnection();

                string sql = @"
                    INSERT INTO CodingSessions (StartTime, EndTime, Date, Duration, Description)
                    VALUES (@StartTime, @EndTime, @Date, @Duration, @Description)";

                connection.Execute(sql, durationSession);
            }
        }
        
        return true;
    }

    public static bool RegisterSession()
    {
        AnsiConsole.Clear();

        AnsiConsole.MarkupLine("[Aquamarine3]Register a new session.[/]\n");

        string dateSession = InputInsert.GetDateSessionInput();
        if (dateSession == "0") return false;

        var durationSession = InputInsert.GetTimeSessionInput(dateSession);
        if (durationSession == null) return false;

        if (durationSession != null)
        {
            using var connection = GetConnection();

            string sql = @"
                INSERT INTO CodingSessions (StartTime, EndTime, Date, Duration, Description)
                VALUES (@StartTime, @EndTime, @Date, @Duration, @Description)";

            connection.Execute(sql, durationSession);
        }

        return true;
    }

    public static bool ViewSessions()
    {
        AnsiConsole.Clear();

        using var connection = GetConnection();

        string sql = @"
            SELECT * FROM CodingSessions";

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
        var action = AnsiConsole.Ask<string>("[yellow]Press 'T' to order records or press '0' to return to main menu.[/]").Trim().ToUpper();

        if (action == "0") return false;
        else InputInsert.OrderSession();

        AnsiConsole.MarkupLine("\n[yellow]Press any key to continue...[/]");
        Console.ReadKey();

        return true;
    }

    public static bool UpdateSession()
    {
        AnsiConsole.Clear();

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
            AnsiConsole.MarkupLine("[red]\nRecord not found![/]\n");

            NumberId = InputInsert.GetId();
            if (NumberId == 0) return false;

            exists = connection.ExecuteScalar<bool>(sqlId, new { Id = NumberId });
        }

        string upInput = AnsiConsole.Ask<string>("\n[bold]Type 1 if you want update the start time.\nType 2 to update the end time.\nType 3 to update the date.\nType 4 to update the description.\n[/][yellow]Type 0 to return to main menu.[/]");
        if (upInput == "0") return false;

        while (!Int32.TryParse(upInput, out _) || Convert.ToInt32(upInput) < 0 || Convert.ToInt32(upInput) > 4)
        {
            AnsiConsole.MarkupLine("[red]\nInvalid input! Please enter a valid number.[/]\n");
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

                AnsiConsole.MarkupLine("[green]\nStart time updated successfully![/]");
                AnsiConsole.MarkupLine("[yellow]Press any key to continue...[/]");
                ReadKey();
                break;
            case "2":
                string endTime = InputInsert.OnlyEndTime();
                if (endTime == "0") return false;

                string sqlUpEndTime = @"
                    UPDATE CodingSessions SET EndTime = @EndTime WHERE Id = @Id";

                connection.ExecuteScalar(sqlUpEndTime, new { Id = NumberId, EndTime = endTime });

                AnsiConsole.MarkupLine("[green]\nEnd time updated successfully![/]");
                AnsiConsole.MarkupLine("[yellow]Press any key to continue...[/]");
                ReadKey();
                break;
            case "3":
                string Date = InputInsert.GetDateSessionInput();
                if (Date == "0") return false;

                string sqlUpDate = @"
                    UPDATE CodingSessions SET Date = @Date WHERE Id = @Id";

                connection.ExecuteScalar(sqlUpDate, new { Id = NumberId, Date = Date });

                AnsiConsole.MarkupLine("[green]\nDate updated successfully![/]");
                AnsiConsole.MarkupLine("[yellow]Press any key to continue...[/]");
                ReadKey();
                break;
            case "4":
                string Description = InputInsert.OnlyDescription();

                string sqlUpDescription = @"
                    UPDATE CodingSessions SET Description = @Description WHERE Id = @Id";

                connection.ExecuteScalar(sqlUpDescription, new { Id = NumberId, Description = Description });

                AnsiConsole.MarkupLine("[green]\nDescription updated successfully![/]");
                AnsiConsole.MarkupLine("[yellow]Press any key to continue...[/]");
                ReadKey();
                break;
        }

        return true;
    }

    public static bool DeleteSession()
    {
        AnsiConsole.Clear();

        AnsiConsole.MarkupLine("[Aquamarine3]Delete a session.[/]\n");

        using var connection = GetConnection();

        string sql = @"
            SELECT * FROM CodingSessions";

        List<CodingSessions> tableData = new List<CodingSessions>();

        var sessions = connection.Query<CodingSessions>(sql).ToList(); // Execute the query and map results to CodingSessions objects

        if (sessions.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No sessions found![/]");
            AnsiConsole.MarkupLine("[yellow]Press any key to continue...[/]");
            ReadKey();
            return false;
        }

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

        string delInput = AnsiConsole.Ask<string>("\n[bold]Type 1 if you want delete all sessions.\nType 2 if you want delete a session.\n[/][yellow]Type 0 to return to main menu.[/]");
        if (delInput == "0") return false;

        while (!Int32.TryParse(delInput, out _) || Convert.ToInt32(delInput) < 0 || Convert.ToInt32(delInput) > 2)
        {
            AnsiConsole.MarkupLine("[red]Invalid input! Please enter a valid number.[/]\n");
            delInput = AnsiConsole.Ask<string>("\n[bold]Type 1 if you want delete all sessions.\nType 2 if you want delete a session.\n[/][yellow]Type 0 to return to main menu.[/]");
            if (delInput == "0") return false;
        }

        switch (delInput)
        {
            case "1":
                string sqlDelAll = @"
                    DELETE FROM CodingSessions";

                connection.Execute(sqlDelAll);

                AnsiConsole.MarkupLine("[red]\nAll sessions deleted![/]");
                AnsiConsole.MarkupLine("[yellow]\nPress any key to continue...[/]");
                ReadKey();

                break;
            case "2":
                int delId = InputInsert.GetId();
                if (delId == 0) return false;

                string sqlDelSession = @"
                    DELETE FROM CodingSessions WHERE Id = @Id";

                connection.Execute(sqlDelSession, new { Id = delId });

                AnsiConsole.MarkupLine("[red]\nSession deleted![/]");
                AnsiConsole.MarkupLine("[yellow]\nPress any key to continue...[/]");
                ReadKey();
                break;
        }

        return true;
    }

}

