using Spectre.Console;
using Flashcards.Controllers;
using static Flashcards.Enums.Enums;
using Flashcards.UI;

namespace Flashcard.UI;

internal class  UserInterface
{
    internal void MainMenu()
    {
        bool closeApp = false;
        while (!closeApp)
        {
            AnsiConsole.Clear();
            var actionChoice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuAction>()
                .Title("[blue]Main Menu[/]")
                .UseConverter(option => option switch
                {
                    MenuAction.ManageStacks => "Manage Stacks",
                    MenuAction.ManageFlashcards => "Manage Flashcards",
                    MenuAction.StudySession => "Start Study Session",
                    MenuAction.ChronoSession => "Start Chrono Session",
                    MenuAction.Exit => "[red]Exit Application[/]",
                    _ => option.ToString()
                })
                .AddChoices(Enum.GetValues<MenuAction>()));

            switch (actionChoice)
            {
                case MenuAction.ManageStacks:
                    StacksUI stacksUI = new StacksUI();
                    stacksUI.stackMenu();
                    break;
                case MenuAction.ManageFlashcards:
                    FlashcardsUI flashcardsUI = new FlashcardsUI();
                    flashcardsUI.flashcardsMenu();
                    break;
                case MenuAction.StudySession:
                    break;
                case MenuAction.ChronoSession:
                    break;
                case MenuAction.Exit:
                    AnsiConsole.Status()
                    .Spinner(Spinner.Known.Dots)
                    .Start("Data rescue...", ctx =>
                    {
                        Thread.Sleep(1000);
                    });
                    closeApp = true;
                    break;
            }
        }
    }
}