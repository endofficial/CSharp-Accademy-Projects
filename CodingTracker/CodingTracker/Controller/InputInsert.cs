using CodingTracker.Model;
using Spectre.Console;
using System.Globalization;

namespace CodingTracker.Controller;

internal class InputInsert
{
    internal static string GetDateSessionInput()
    {
        AnsiConsole.MarkupLine("[Aquamarine3]Register a new session.[/]\n");
        var date = AnsiConsole.Ask<string>("Please enter date (yyyy-MM-dd). You type [yellow]0 to return to main menu.[/]\n");

        if (date == "0") return "0";

        while (!DateTime.TryParseExact(date, "yyyy-MM-dd", new CultureInfo("en-EN"), DateTimeStyles.None, out _))
        {
            AnsiConsole.MarkupLine("[red]Invalid date format.[/]\n");
            date = AnsiConsole.Ask<string>("Please enter date (yyyy-MM-dd). You type [yellow]0 to return to main menu.[/]");
            if (date == "0") return "0";
        }
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
        Console.ReadKey();

        return session;
    }
}

