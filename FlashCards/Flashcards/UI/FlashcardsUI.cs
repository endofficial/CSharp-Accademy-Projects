using Flashcards.Controllers;
using Spectre.Console;
using static Flashcards.Enums.Enums;
using Flashcards.Models;

namespace Flashcards.UI;

public class FlashcardsUI
{
    private readonly IStacksController _stacksController;
    private readonly IFlashcardsController _flashcardsController;

    public FlashcardsUI(IStacksController stacksController, IFlashcardsController flashcardsController)
    {
        _stacksController = stacksController;
        _flashcardsController = flashcardsController;
    }

    public void flashcardsMenu()
    {
        bool closeApp = false;
        while (!closeApp)
        {
            AnsiConsole.Clear();
            var actionChoice = AnsiConsole.Prompt(
            new SelectionPrompt<FlashcardAction>()
            .Title("What would you like to do?")
            .UseConverter(option => option switch
            {
                FlashcardAction.ViewFlashcards => "View Flashcards",
                FlashcardAction.CreateFlashcard => "Create Flashcard",
                FlashcardAction.UpdateFlashcard => "Update Flashcard",
                FlashcardAction.DeleteFlashcard => "Delete Flashcard",
                FlashcardAction.BackToMainMenu => "Back to Main Menu",
                _ => option.ToString()
            })
            .AddChoices(Enum.GetValues<FlashcardAction>()));

            switch (actionChoice)
            {
                case FlashcardAction.ViewFlashcards:
                    ShowStackToFlashcards();
                    break;
                case FlashcardAction.CreateFlashcard:
                    break;
                case FlashcardAction.UpdateFlashcard:
                    break;
                case FlashcardAction.DeleteFlashcard:
                    break;
                case FlashcardAction.BackToMainMenu:
                    closeApp = true;
                    break;
            }
        }
    }

    // modificare, separare le responsabilità
    public void ShowStackToFlashcards(bool waitForKey = true, IAnsiConsole? console = null)
    {
        IAnsiConsole? _console = console ?? AnsiConsole.Console;

        var stacks = _stacksController.GetAllStacks();

        if (stacks.Count == 0)
        {
            _console.MarkupLine("[yellow]No stacks found.[/]");
        }
        else
        {
            var selectedStack = AnsiConsole.Prompt(
                new SelectionPrompt<Stack>()
                    .Title("Select a stack to view flashcards:")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more stacks)[/]")
                    .UseConverter(stack => stack.NameStack)
                    .AddChoices(stacks)
            );

            int selectedStackId = selectedStack.StackID;
            ShowFlashcards(selectedStackId, waitForKey, console);
        }

        if (waitForKey)
        {
            _console.WriteLine("\nPress any key to continue...");
            _console.Input.ReadKey(true);
        }
    }

    public void ShowFlashcards(int stackId, bool waitForKey = true, IAnsiConsole? console = null)
    {
        IAnsiConsole? _console = console ?? AnsiConsole.Console;

        try
        {
            _console.Clear();
            var flashcards = _flashcardsController.GetAllFlashcards(stackId);

            if (flashcards.Count == 0)
            {
                _console.MarkupLine("[yellow]No flashcards found.[/]");
            }
            else
            {
                var table = new Table();
                table.Border(TableBorder.Rounded)
                    .AddColumn("ID")
                    .AddColumn("Front")
                    .AddColumn("Back");

                foreach (var flashcard in flashcards)
                {
                    table.AddRow(
                        flashcard.FlashcardsID.ToString(),
                        flashcard.Front,
                        flashcard.Back);
                }
                _console.Write(table);  
            }
        }

        catch(Exception ex)
        {
            _console.MarkupLine($"[red]Error:[/] {ex.Message}");
        }
    }
}
