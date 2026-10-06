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
                    ShowFlashcards();
                    break;
                case FlashcardAction.CreateFlashcard:
                    CreateFlashcard();
                    break;
                case FlashcardAction.UpdateFlashcard:
                    UpdateFlashcard();
                    break;
                case FlashcardAction.DeleteFlashcard:
                    break;
                case FlashcardAction.BackToMainMenu:
                    closeApp = true;
                    break;
            }
        }
    }

    public int? SelectStack(IAnsiConsole? console = null)
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
            return selectedStackId;
        }
        return null;
    }

    // Method to show flashcards for a selected stack
    public void ShowFlashcards(bool waitForKey = true, IAnsiConsole? console = null)
    {
        IAnsiConsole? _console = console ?? AnsiConsole.Console;

        try
        {
            _console.Clear();

            int? selectedStackId = SelectStack(_console);
            if (selectedStackId is null) return;
            else
            {
                var flashcards = _flashcardsController.GetAllFlashcards(selectedStackId);

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
            
        }

        catch(Exception ex)
        {
            _console.MarkupLine($"[red]Error:[/] {ex.Message}");
        }

        if (waitForKey)
        {
            _console.MarkupLine("[grey]Press any key to continue...[/]");
            _console.Input.ReadKey(true);
        }
    }

    public void CreateFlashcard(bool waitForKey = true, IAnsiConsole? console = null)
    {
        IAnsiConsole? _console = console ?? AnsiConsole.Console;

        try
        {
            _console.Clear();
            int? selectedStackId = SelectStack(_console);

            if (selectedStackId is null) return;
            else
            {
                var front = _console.Ask<string>("Enter the [green]question[/] of the flashcard:");
                var back = _console.Ask<string>("Enter the [green]answer[/] of the flashcard:");

                if (string.IsNullOrWhiteSpace(front) || string.IsNullOrWhiteSpace(back)) return;
                else
                {
                    _flashcardsController.CreateFlashcard(selectedStackId, front, back);
                }
            }
        }
        catch (Exception ex)
        {
            _console.MarkupLine($"[red]Error:[/] {ex.Message}");
        }

        if (waitForKey)
        {
            _console.MarkupLine("\n[grey]Press any key to continue...[/]");
            _console.Input.ReadKey(true);
        }
    }

    public void UpdateFlashcard(bool waitForkey = true, IAnsiConsole? console = null)
    {
        IAnsiConsole _console = console ?? AnsiConsole.Console;

        try
        {
            _console.Clear();
            int? SelectedStacks = SelectStack(_console);

            if (SelectedStacks is null) return;
            else
            {
                var AllFlashcard = _flashcardsController.GetAllFlashcards(SelectedStacks);

                if (!AllFlashcard.Any())
                {
                    _console.MarkupLine("[yellow]Flashcards not found in this stack.[/]");
                }
                else
                {
                    var selectedFlashcard = AnsiConsole.Prompt(
                    new SelectionPrompt<FlashcardDto>()
                    .Title("Selected FLashcard to update")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more flashcards)[/]")
                    .UseConverter(flashCard => flashCard.Front)
                    .AddChoices(AllFlashcard)
                );
                }
            }

            //Passo 3: Scegliere se modificare il front o il back (domanda o risposta), per poi apportare la modifica

        }
        catch (Exception ex)
        {
            _console.MarkupLine($"[red]Error:[/] {ex.Message}");
        }
    }
}
