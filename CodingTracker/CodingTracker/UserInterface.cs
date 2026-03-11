using CodingTracker.Controller;
using Spectre.Console;
using static CodingTracker.Enums;

namespace CodingTracker;

internal class UserInterface
{
    internal void MainMenu()
    {
        bool closeApp = false;

        while (!closeApp)
        {
            Clear();
            var actionChoice = AnsiConsole.Prompt(
            new SelectionPrompt<MenuAction>()
            .Title("What do you want to do next?")
            .UseConverter(option => option switch // UseConverter is used to convert the enum values to user-friendly strings in the selection prompt
            {
                MenuAction.RegisterSession => "Register a coding session",
                MenuAction.UpdateSession => "Update a coding session",
                MenuAction.DeleteSession => "Delete a coding session",
                MenuAction.ViewSessions => "View coding sessions",
                MenuAction.ExiSession => "[red]Close App[/]",
                _ => option.ToString() // Fallback to the default enum name if no specific string is provided
            })
            .AddChoices(Enum.GetValues<MenuAction>()));

            switch (actionChoice)
            {
                case MenuAction.RegisterSession:
                    CodingController.RegisterSession();
                    break;
                case MenuAction.UpdateSession:
                    CodingController.UpdateSession();
                    break;
                case MenuAction.DeleteSession:
                    CodingController.DeleteSession();
                    break;
                case MenuAction.ViewSessions:
                    CodingController.ViewSessions();
                    break;
                case MenuAction.ExiSession:
                    closeApp = true;
                    break;
            }
        }
    }
}
