using CodingTracker.Model;
using Spectre.Console;
using System.Diagnostics;
using System.Globalization;
using static CodingTracker.Enums;

namespace CodingTracker.Controller;

public class InputInsert
{
    public static CodingSessions StopwatchSession(string sessionDate)
    {
        Clear();
        string description = AnsiConsole.Ask<string>("Please enter a description for the session.");

        AnsiConsole.MarkupLine("\n[green]Session started![/]");
        AnsiConsole.MarkupLine("\n[yellow]Press any key to stop session.[/]");

        var stopwatch = Stopwatch.StartNew(); //initialize to use a stopwatch
        DateTime startTime = DateTime.Now;

        // Display a status message while the session is in progress
        // .status make a status message that can be updated while the session is running,
        // and .spinner adds a spinner animation to indicate that something is happening in the background.
        AnsiConsole.Status()
            .Spinner(Spinner.Known.Clock)
            .Start("Live session in progress...", ctx =>
            {
                stopwatch.Start(); // Start the stopwatch 

                while (!KeyAvailable)
                {
                    var elapsed = stopwatch.Elapsed; // Get the elapsed time since the session started

                    string time = string.Format("{0:00}:{1:00}:{2:00}",
                            elapsed.Hours, elapsed.Minutes, elapsed.Seconds);

                    // to update a message
                    ctx.Status($"[blue]Stopwatch running:[/] {time}");

                    // add a delay to safe CPU
                    // Don't act fot 50ms => 20 FPS
                    Thread.Sleep(50);

                }
                ReadKey(true);
            });

        stopwatch.Stop();

        DateTime endTime = DateTime.Now;
        TimeSpan duration = endTime - startTime;
        string[] formatsTime = { "H\\:mm", "HH\\:mm" };

        TimeOnly StartTime  = TimeOnly.FromDateTime(startTime);
        TimeOnly EndTime = TimeOnly.FromDateTime(endTime);

        var session = new CodingSessions(0, StartTime, EndTime, sessionDate, duration, description);

        Clear();
        AnsiConsole.MarkupLine("[red]Session stopped![/]\n[yellow]Press any key to continue...[/]");
        ReadKey();

        return session;
    }

    // IAnsiConsole is used to test with spectre.console.testing
    public static string GetDateSessionInput(IAnsiConsole? console = null)
    {
        var _console = console ?? AnsiConsole.Console;

        var date = _console.Ask<string>("Please enter date (yyyy-MM-dd). [yellow]You type 0 to return to main menu.[/]\n").Trim();
        if (date == "0") return "0";

        while (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            _console.MarkupLine("[red]Invalid date format.[/]\n");
            date = _console.Ask<string>("Please enter date (yyyy-MM-dd). You type [yellow]0 to return to main menu.\n[/]").Trim();
            if (date == "0") return "0";
        }

        _console.MarkupLine($"[green]Date registered!\n[/]");
        return date;
    }

    public static CodingSessions? GetTimeSessionInput(string sessionDate, IAnsiConsole? console = null)
    {
        var _console = console ?? AnsiConsole.Console;

        string[] formats = { @"h\:mm", @"hh\:mm" };
        string description = _console.Ask<string>("Please enter a description for the session.");

        string startInput = _console.Prompt(
            new TextPrompt<string>("[bold]\nPlease insert the start time (Format: [green]HH:mm[/]) or type [yellow]0[/] to return to main menu.[/]")
            .Validate(input =>
            {
                var cleanInput = input.Trim();
                if (cleanInput == "0") return ValidationResult.Success();

                // if the format is valid, it will be stored in the variable time, otherwise it will return false
                bool isValid = TimeSpan.TryParseExact(cleanInput, formats, CultureInfo.InvariantCulture, out var time);

                // Check if the time is valid and within the range of 0 to 24 hours
                if (!isValid) return ValidationResult.Error("[red]Time invalid! Use the time format '[blue]HH:mm[/]'[/]");

                if (time.Ticks < 0) return ValidationResult.Error("[red]Negative time not allowed.[/]");

                return ValidationResult.Success();
            }));
        if (startInput.Trim() == "0") return null!;

        string endInput = _console.Prompt(
            new TextPrompt<string>("[bold]\nPlease insert the end time (Format: [green]HH:mm[/]) or type [yellow]0[/] to return to main menu.[/]")
            .Validate(input =>
            {
                var cleanInputEnd = input.Trim();
                if (cleanInputEnd == "0") return ValidationResult.Success();

                bool isValid = TimeSpan.TryParseExact(cleanInputEnd, formats, CultureInfo.InvariantCulture, out var time);

                // Check if the time is valid and within the range of 0 to 24 hours
                if (!isValid) return ValidationResult.Error("[red]Time invalid! Use the time format '[blue]hh:mm[/]'[/]");

                if (time.Ticks < 0) return ValidationResult.Error("[red]Negative time not allowed.[/]");

                return ValidationResult.Success();
            }));
        if (endInput.Trim() == "0") return null!;

        // Define another because DateTime.TryParseExact doesn't accept TimeSpan formats, it needs to be converted to DateTime
        string[] formatsTime = { "H\\:mm", "HH\\:mm" };
        // convert string to DateTime
        if (!TimeOnly.TryParseExact(startInput.Trim(), formatsTime, null!, DateTimeStyles.None, out TimeOnly resultStart)) return null!;

        if (!TimeOnly.TryParseExact(endInput.Trim(), formatsTime, null!, DateTimeStyles.None, out TimeOnly resultEnd)) return null!;

        // calculate duration
        TimeSpan duration = resultEnd - resultStart;

        var session = new CodingSessions(0, resultStart, resultEnd, sessionDate, duration, description);

        session.DisplayConfirmRegister();

        _console.MarkupLine("[yellow]Press any key to continue...[/]");
        _console.Input.ReadKey(true);

        return session;
    }

    public static string OnlyStartTime()
    {
        string[] formats = { @"h\:mm", @"hh\:mm" };
        string startInput = AnsiConsole.Prompt(
            new TextPrompt<string>("[bold]\nPlease insert the start time (Format: [green]HH:mm[/]) or type [yellow]0[/] to return to main menu.[/]")
            .Validate(input =>
            {
                if (input == "0") return ValidationResult.Success();

                // if the format is valid, it will be stored in the variable time, otherwise it will return false
                bool isValid = TimeSpan.TryParseExact(input, formats, CultureInfo.InvariantCulture, out var time);

                // Check if the time is valid and within the range of 0 to 24 hours
                if (!isValid) return ValidationResult.Error("[red]Time invalid! Use the time format '[blue]HH:mm[/]'[/]");

                if (time.Ticks < 0) return ValidationResult.Error("[red]Negative time not allowed.[/]");

                return ValidationResult.Success();
            }));

        if (startInput == "0") return null!;

        return startInput;
    }

    public static string OnlyEndTime()
    {
        string[] formats = { @"h\:mm", @"hh\:mm" };
        string endInput = AnsiConsole.Prompt(
            new TextPrompt<string>("[bold]\nPlease insert the end time (Format: [green]HH:mm[/]) or type [yellow]0[/] to return to main menu.[/]")
            .Validate(input =>
            {
                if (input == "0") return ValidationResult.Success();

                bool isValid = TimeSpan.TryParseExact(input, formats, CultureInfo.InvariantCulture, out var time);

                // Check if the time is valid and within the range of 0 to 24 hours
                if (!isValid) return ValidationResult.Error("[red]Time invalid! Use the time format '[blue]hh:mm[/]'[/]");

                if (time.Ticks < 0) return ValidationResult.Error("[red]Negative time not allowed.[/]");

                return ValidationResult.Success();
            }));
        if (endInput == "0") return null!;

        return endInput;
    }

    public static string OnlyDescription(IAnsiConsole? console = null)
    {
        var _console = console ?? AnsiConsole.Console;

        string description = _console.Ask<string>("\nPlease enter a description for the session.\n");
        return description;
    }

    public static int GetId(IAnsiConsole? console = null)
    {
        var _console = console ?? AnsiConsole.Console;

        string numberId = _console.Ask<string>("\nPlease enter the ID of the session. You type [yellow]0 to return to main menu.\n[/]").Trim();

        if (numberId == "0") return 0;

        // Validate that the input is a positive integer or zero (to return to main menu)
        while (!Int32.TryParse(numberId, out _) || Convert.ToInt32(numberId) < 0)
        {
            _console.MarkupLine("[red]Invalid ID. Please enter a positive integer.[/]\n");
            numberId = _console.Ask<string>("Please enter the ID of the session. You type [yellow]0 to return to main menu.\n[/]").Trim();
            if (numberId == "0") return 0;
        }

        int finalId = Convert.ToInt32(numberId);
        return finalId;
    }

    public static List<CodingSessions> RandomSession()
    {
        Random random = new Random();

        List<CodingSessions> codSession = new List<CodingSessions>();
        string[] Description = { "Working", "Programming", "Playing" };

        DateTime start = new DateTime(2020, 1, 1);
        int range = (DateTime.Today - start).Days;

        for(int i = 0; i < 100; i++)
        {
            var randomStart = new TimeOnly(random.Next(0, 24), random.Next(0, 60), random.Next(0, 59));
            var randomEnd = new TimeOnly(random.Next(0, 24), random.Next(0, 60), random.Next(0, 59));
            string randomDate = start.AddDays(random.Next(range)).ToString("yyyy-MM-dd");
            
            TimeSpan duration = randomEnd - randomStart;

            int desRandom = random.Next(0, Description.Length);
            string description = Description[desRandom];

            var session = new CodingSessions(0, randomStart, randomEnd, randomDate, duration, description);
            codSession.Add(session);
        }
        
        return codSession;
    }

    public static FilterAction OrderSession()
    {
        Clear();
 
        var filterChoice = AnsiConsole.Prompt(
        new SelectionPrompt<FilterAction>()
        .Title("How do you want to visualize the data?")
        .UseConverter(option => option switch
        {
            FilterAction.orderToYears => "Order to year",
            FilterAction.orderToMonths => "Order to month",
            FilterAction.orderToDays => "Order to day",
            FilterAction.ascendingOrder => "Ascending order",
            FilterAction.descendingOrder => "Descending order",
            FilterAction.Exit => "[red]Close App[/]",
            _ => option.ToString()
        })
        .AddChoices(Enum.GetValues<FilterAction>()));

        switch (filterChoice)
        {
            case FilterAction.orderToYears:
                CodingFilterOrder.OrderToYears();
                break;
            case FilterAction.orderToMonths:
                CodingFilterOrder.OrderToMonths();
                break;
            case FilterAction.orderToDays:
                CodingFilterOrder.OrderToDays();
                break;
            case FilterAction.ascendingOrder:
                CodingFilterOrder.AscendingOrder();
                break;
            case FilterAction.descendingOrder:
                CodingFilterOrder.DescendingOrder();
                break;
            case FilterAction.Exit:
                AnsiConsole.Status()
                    .Spinner(Spinner.Known.Dots)
                    .Start("Data rescue...", ctx =>
                    {
                        Thread.Sleep(1000);
                    });

                Environment.Exit(0);
                break;
        }

        return filterChoice;
    }
}

