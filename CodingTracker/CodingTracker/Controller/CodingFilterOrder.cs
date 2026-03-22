using CodingTracker.Data;
using CodingTracker.Model;
using Dapper;
using Spectre.Console;
using static CodingTracker.Enums;

namespace CodingTracker.Controller;
internal class CodingFilterOrder : Database
{
    public static void OrderToYears()
    {
        AnsiConsole.Clear();

        using var connection = GetConnection();

        string sql = "SELECT DISTINCT strftime('%Y', Date) AS Year FROM CodingSessions ORDER BY Year ASC";

        // conver to string in int
        var years = connection.Query<int>(sql);
        if (!years.Any())
        {
            AnsiConsole.MarkupLine("[red]Record not found![/]");
            return;
        }

        var selectedYear = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
            .Title("Choice a year to view records:")
            .PageSize(10)
            .AddChoices(years)
            );

        var sqlDetails = "SELECT * FROM CodingSessions WHERE strftime('%Y', Date) = @Year";
        var sessions = connection.Query<CodingSessions>(sqlDetails, new { Year = selectedYear.ToString() }).ToList();

        var table = new Table();
        table.Border(TableBorder.Rounded);

        table.AddColumn("Id");
        table.AddColumn("Start Time");
        table.AddColumn("End Time");
        table.AddColumn("Date");
        table.AddColumn("Duration");
        table.AddColumn("Description");

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
        return;
    }

    public static void OrderToMonths()
    {
        AnsiConsole.Clear();

        var connection = GetConnection();

        string sql = "SELECT DISTINCT strftime('%m', Date) As Month FROM CodingSessions ORDER BY Month ASC";

        var months = connection.Query<int>(sql).ToList();
        if (!months.Any())
        {
            AnsiConsole.MarkupLine("[red]Record not found![/]");
            return;
        }

        var selectedMonth = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
            .Title("Choice a month to view record: ")
            .PageSize(10)
            .AddChoices(months)
            .UseConverter(m =>
            {
                return System.Globalization.DateTimeFormatInfo.InvariantInfo.GetMonthName(m); 
            }));

        // use CAST to specify that valus is an int
        string sqlDetails = "SELECT * FROM CodingSessions WHERE CAST(strftime('%m', Date) AS INT) = @Month";
        var sessions = connection.Query<CodingSessions>(sqlDetails, new { Month = selectedMonth.ToString() }).ToList();

        var table = new Table();
        table.Border(TableBorder.Rounded);

        table.AddColumn("Id");
        table.AddColumn("Start Time");
        table.AddColumn("End Time");
        table.AddColumn("Date");
        table.AddColumn("Duration");
        table.AddColumn("Description");

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
    }

    public static void OrderToDays()
    {
        AnsiConsole.Clear();

        var connection = GetConnection();

        // 'w' => 0: Sunday; 1: Monday...
        string sql = "SELECT DISTINCT strftime ('%w', date) AS Day FROM CodingSessions ORDER BY Day ASC";

        var day = connection.Query<int>(sql).ToList();
        if (!day.Any())
        {
            AnsiConsole.MarkupLine("Record not found!");
            return;
        }

        var dayChoice = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
            .Title("Choice a day to view record: ")
            .PageSize(10)
            .AddChoices(day)
            .UseConverter(m =>
            {
                return System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.GetDayName((DayOfWeek)m);
            }));

        string sqlDetails = "SELECT * FROM CodingSessions WHERE CAST(strfTime('%w', date)AS INT) = @Day";
        var sessions = connection.Query<CodingSessions>(sqlDetails, new { Day = dayChoice.ToString() }).ToList();

        var table = new Table();
        table.Border(TableBorder.Rounded);

        table.AddColumn("Id");
        table.AddColumn("Start Time");
        table.AddColumn("End Time");
        table.AddColumn("Date");
        table.AddColumn("Duration");
        table.AddColumn("Description");

        foreach (var session in sessions)
        {
            table.AddRow(
                session.Id.ToString(),
                session.StartTime.ToString("HH:mm"),
                session.EndTime.ToString("HH:mm"),
                session.Date,
                session.Duration.ToString(@"hh\:mm"),
                (session.Description ?? "Empty").ToString());
        }

        AnsiConsole.Write(table);
        return;
    }

    public static void AscendingOrder()
    {
        AnsiConsole.Clear();

        var connection = GetConnection();

        string sql = "SELECT * FROM CodingSessions ORDER BY Date ASC";
        var sessions = connection.Query<CodingSessions>(sql).ToList();

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
    }

    public static void DescendingOrder()
    {
        AnsiConsole.Clear();

        var connection = GetConnection();

        string sql = "SELECT * FROM CodingSessions ORDER BY Date DESC";
        var sessions = connection.Query<CodingSessions>(sql).ToList();

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
    }
}
