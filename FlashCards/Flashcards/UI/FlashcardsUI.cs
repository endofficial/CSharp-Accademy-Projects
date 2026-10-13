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
                    DeleteFlashcard();
                    break;
                case FlashcardAction.BackToMainMenu:
                    closeApp = true;
                    break;
            }
        }
    }

    // Select Stack to view Flashcard
    private int? SelectStack(IAnsiConsole? console = null)
    {
        IAnsiConsole? _console = console ?? AnsiConsole.Console;

        var stacks = _stacksController.GetAllStacks();

        if (stacks == null || stacks.Count == 0)
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

    // This method it's important to wait a key to close method
    private static void WaitForKey(IAnsiConsole console)
    {
        console.MarkupLine("Press any key to return to the stack menu...");
        try
        {
            console.Input.ReadKey(true);
        }
        catch (InvalidOperationException)
        {
            // during unit testing, the console input may not be available
        }
    }

    private static bool CheckTextInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }
        return true;
    }

    private bool CheckIfTheFlashcardIsempty(int? selectedStacks)
    {
        var AllFlashcard = _flashcardsController.GetAllFlashcards(selectedStacks);

        if (!AllFlashcard.Any())
        {
            AnsiConsole.MarkupLine("[yellow]Flashcards not found in this stack.[/]");
            return true;
        }
        return false;
    }

    // Method to show flashcards for a selected stack
    public void ShowFlashcards(bool waitForKey = true, IAnsiConsole? console = null)
    {
        IAnsiConsole? _console = console ?? AnsiConsole.Console;
            _console.Clear();

        int? selectedStackId = SelectStack(_console);
        if (selectedStackId is null) return;
        else
        {
            var flashcards = _flashcardsController.GetAllFlashcards(selectedStackId);

            if (flashcards == null || flashcards.Count == 0)
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
                        flashcard.DisplayID.ToString(),
                        flashcard.Front,
                        flashcard.Back);
                }
                _console.Write(table);
            }
        }
            
        if (waitForKey)
        {
            WaitForKey(_console);
        }
    }

    // Method to create a flashcard
    public void CreateFlashcard(bool waitForKey = true, IAnsiConsole? console = null)
    {
        IAnsiConsole? _console = console ?? AnsiConsole.Console;

        _console.Clear();
        int? selectedStackId = SelectStack(_console);

        if (selectedStackId is null) return;
        else
        {
            var front = _console.Prompt(
                new TextPrompt<string>("Enter the [green]question[/] of the flashcard:")
                .Validate(input =>
                {
                    if (!CheckTextInput(input))
                    {
                        return ValidationResult.Error("[red]The question cannot be empty![/]");
                    }
                    return ValidationResult.Success();
                })
            );

            var back = _console.Prompt(
                new TextPrompt<string>("Enter the [green]answer[/] of the flashcard:")
                .Validate(input =>
                {
                    if (!CheckTextInput(input))
                    {
                        return ValidationResult.Error("[red]The answer cannot be empty![/]");
                    }
                    return ValidationResult.Success();
                })
            );

            _flashcardsController.CreateFlashcard(selectedStackId, front, back);
        }

        if (waitForKey)
        {
            WaitForKey(_console);
        }
    }

    public void UpdateFlashcard(bool waitForkey = true, IAnsiConsole? console = null)
    {
        IAnsiConsole _console = console ?? AnsiConsole.Console;
        _console.Clear();

        int? SelectedStacks = SelectStack(_console);

        if (SelectedStacks is null) return;

        if (CheckIfTheFlashcardIsempty(SelectedStacks))
        {
            WaitForKey(_console);
            return;
        }

        var AllFlashcard = _flashcardsController.GetAllFlashcards(SelectedStacks);

        var selectedFlashcard = AnsiConsole.Prompt(
        new SelectionPrompt<FlashcardDto>()
        .Title("Selected FLashcard to update")
        .PageSize(10)
        .MoreChoicesText("[grey](Move up and down to reveal more flashcards)[/]")
        .UseConverter(flashCard => flashCard.Front)
        .AddChoices(AllFlashcard)
        );

        _console.MarkupLine($"[purple]Current Front:[/]{selectedFlashcard.Front}");
        _console.MarkupLine($"[purple]Current Back:[/]{selectedFlashcard.Back}");
        _console.WriteLine();

        string UpFront = selectedFlashcard.Front;
        string UpBack = selectedFlashcard.Back;

        var chooseUpFront = _console.Prompt(
            new SelectionPrompt<string>()
            .Title("Do you want update the question of the Flashcard?")
            .PageSize(3)
            .AddChoices(new[] { "Yes", "No" })
            );

        if (chooseUpFront == "Yes") UpFront = _console.Ask<string>("[blue]New question:[/]");
        else UpFront = selectedFlashcard.Front;

        var chooseUpBack = _console.Prompt(
            new SelectionPrompt<string>()
            .Title("Do you want update the answer of the Flashcard?")
            .PageSize(3)
            .AddChoices(new[] { "Yes", "No" })
            );

        if (chooseUpBack == "Yes") UpBack = _console.Ask<string>("[blue]New answer:[/]");
        else UpBack = selectedFlashcard.Back;

        _flashcardsController.UpdateFlashcard(selectedFlashcard.FlashcardsID, UpFront, UpBack);

        if (waitForkey)
        {
            WaitForKey(_console);
        }
    }

    public void DeleteFlashcard(bool waitForkey = true, IAnsiConsole? console = null)
    {
        IAnsiConsole _console = console ?? AnsiConsole.Console;
        _console.Clear();

        int? SelectedStacks = SelectStack(_console);

        if (SelectedStacks is null) return;

        if (CheckIfTheFlashcardIsempty(SelectedStacks))
        {
            WaitForKey(_console);
            return;
        }
        var AllFlashcards = _flashcardsController.GetAllFlashcards(SelectedStacks);

        bool returnBack = true;
        while (returnBack)
        {
            var DeletedFlashcard = _console.Prompt(
            new SelectionPrompt<FlashcardDto>()
            .Title("Select the flashcard you want to delete:")
            .PageSize(10)
            .UseConverter(flashcard => flashcard.Front)
            .AddChoices(AllFlashcards)
            );

            var idDel = DeletedFlashcard.FlashcardsID;

            var confirmDel = _console.Prompt(
            new SelectionPrompt<string>()
            .Title("Do you want delete this Flashcard?")
            .PageSize(3)
            .AddChoices(new[] { "Yes", "No" })
            );

            if (confirmDel == "Yes")
            {
                _flashcardsController.DeleteFlashcard(idDel);

                _console.MarkupLine("[green]Flashcard deleted succesfully![/]");
                returnBack = false;
            }
            else
            {
                _console.MarkupLine("[red]You have selected 'No'[/]");
                returnBack = true;
            }
        }

        if (waitForkey)
        {
            WaitForKey(_console);
        }
    }
}
