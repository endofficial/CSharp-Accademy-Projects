using CodingTracker.Model;
using Spectre.Console;
using System.Diagnostics;
using System.Globalization;

namespace CodingTracker.Controller;

internal class InputInsert
{
    internal static CodingSessions StopwatchSession(string sessionDate)
    {
        Clear();
        string description = AnsiConsole.Ask<string>("Please enter a description for the session. You can leave it empty if you want.");

        AnsiConsole.MarkupLine("\n[green]Session started![/]");

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

    internal static string GetDateSessionInput()
    {
        var date = AnsiConsole.Ask<string>("Please enter date (yyyy-MM-dd). You type [yellow]0 to return to main menu.[/]\n");

        if (date == "0") return "0";

        while (!DateTime.TryParseExact(date, "yyyy-MM-dd", new CultureInfo("en-EN"), DateTimeStyles.None, out _))
        {
            AnsiConsole.MarkupLine("[red]Invalid date format.[/]\n");
            date = AnsiConsole.Ask<string>("Please enter date (yyyy-MM-dd). You type [yellow]0 to return to main menu.[/]");
            if (date == "0") return "0";
        }

        AnsiConsole.MarkupLine($"[green]Date registered![/]");
        return date;
    }

    internal static CodingSessions? GetTimeSessionInput(string sessionDate)
    {
        string[] formats = { @"h\:mm", @"hh\:mm" };
        string description = AnsiConsole.Ask<string>("Please enter a description for the session. You can leave it empty if you want.");

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

        // Define another because DateTime.TryParseExact doesn't accept TimeSpan formats, it needs to be converted to DateTime
        string[] formatsTime = { "H\\:mm", "HH\\:mm" };
        // convert string to DateTime
        if (!TimeOnly.TryParseExact(startInput, formatsTime, null!, DateTimeStyles.None, out TimeOnly resultStart)) return null!;

        if (!TimeOnly.TryParseExact(endInput, formatsTime, null!, DateTimeStyles.None, out TimeOnly resultEnd)) return null!;

        // calculate duration
        TimeSpan duration = resultEnd - resultStart;

        var session = new CodingSessions(0, resultStart, resultEnd, sessionDate, duration, description);

        session.DisplayConfirmRegister();

        AnsiConsole.MarkupLine("[yellow]Press any key to continue...[/]");
        ReadKey();

        return session;
    }

    internal static string OnlyStartTime()
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

    internal static string OnlyEndTime()
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

    internal static string OnlyDescription()
    {
        string description = AnsiConsole.Ask<string>("\nPlease enter a description for the session. You can leave it empty if you want.\n");
        return description;
    }

    internal static int GetId()
    {
        string numberId = AnsiConsole.Ask<string>("\nPlease enter the ID of the session. You type [yellow]0 to return to main menu.[/]\n");

        if (numberId == "0") return 0;

        // Validate that the input is a positive integer or zero (to return to main menu)
        while (!Int32.TryParse(numberId, out _) || Convert.ToInt32(numberId) < 0)
        {
            AnsiConsole.MarkupLine("[red]Invalid ID. Please enter a positive integer.[/]\n");
            numberId = AnsiConsole.Ask<string>("Please enter the ID of the session. You type [yellow]0 to return to main menu.[/]\n");
            if (numberId == "0") return 0;
        }

        int finalId = Convert.ToInt32(numberId);
        return finalId;
    }
}

