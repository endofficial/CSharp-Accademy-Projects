using Spectre.Console;
using System.Runtime.InteropServices;
using static System.Collections.Specialized.BitVector32;

namespace CodingTracker.Model;

public class CodingSessions
{
    public int Id { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string Date { get; set; }
    public string? Description { get; set; }

    public TimeSpan Duration => EndTime - StartTime;

    public CodingSessions(int id, TimeOnly startTime, TimeOnly endTime, string date, TimeSpan duration, string? description)
    {
        Id = id;
        StartTime = startTime;
        EndTime = endTime;
        Date = date;
        duration = Duration;
        Description = description;
    }

    public CodingSessions() { }

    /*public void DisplaySession(int id, DateTime startTime, DateTime endTime, string date, TimeSpan duration, string? description)
    {
        AnsiConsole.MarkupLine($"\nSession registered: [green]{startTime:HH:mm} - {endTime:HH:mm}[/] with duration [blue]{duration:hh\\:mm}[/] and description: [yellow]{description}[/]. Date session: [green]{date}[/]");
    }*/

    public void DisplayConfirmRegister()
    {
        AnsiConsole.MarkupLine("\n[green]Session registered![/]");
    }
}

