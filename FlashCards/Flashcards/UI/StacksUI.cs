using Spectre.Console;
using static Flashcards.Enums.Enums;

namespace Flashcards.UI;

internal class StacksUI
{
    public void stackMenu()
    {
        bool closeApp = false;
        while (!closeApp)
        {
            AnsiConsole.Clear();
            var actionChoice = AnsiConsole.Prompt(
                new SelectionPrompt<StackAction>()
                .Title("What would you like to do?")
                .UseConverter(option => option switch
                {
                    StackAction.ViewStacks => "View Stacks",
                    StackAction.CreateStack => "Create Stack",
                    StackAction.UpdateStack => "Update Stack",
                    StackAction.DeleteStack => "Delete Stack",
                    StackAction.BackToMainMenu => "Back to Main Menu",
                    _ => option.ToString()
                })
                .AddChoices(Enum.GetValues<StackAction>()));

            switch (actionChoice)
            {
                case StackAction.ViewStacks:
                    break;
                case StackAction.CreateStack:
                    break;
                case StackAction.UpdateStack:
                    break;
                case StackAction.DeleteStack:
                    break;
                case StackAction.BackToMainMenu:
                    closeApp = true;
                    break;
            }
        }
    }
}