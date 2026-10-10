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
                            flashcard.DisplayID.ToString(),
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
            _console.Input.ReadKey(true);
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
            _console.Input.ReadKey(true);  
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

                    _console.MarkupLine($"[purple]Current Front:[/]{selectedFlashcard.Front}");
                    _console.MarkupLine($"[purple]Current Back:[/]{selectedFlashcard.Back}");
                    _console.WriteLine();

                    string UpFront = selectedFlashcard.Front;
                    string UpBack = selectedFlashcard.Back;

                    var chooseUpFront = _console.Prompt(
                        new SelectionPrompt<string>()
                        .Title("Do you want update the question of the Flashcard?")
                        .PageSize(3)
                        .AddChoices(new[] { "Yes", "No"})
                        );

                    if (chooseUpFront == "Yes") UpFront = _console.Ask<string>("[blue]New question:[/]");
                    else UpFront = selectedFlashcard.Front;

                    var chooseUpBack = _console.Prompt(
                        new SelectionPrompt<string>()
                        .Title("Do you want update the answer of the Flashcard?")
                        .PageSize(3)
                        .AddChoices(new[] {"Yes", "No"})
                        );

                    if (chooseUpBack == "Yes") UpBack = _console.Ask<string>("[blue]New answer:[/]");
                    else UpBack = selectedFlashcard.Back;

                    _flashcardsController.UpdateFlashcard(selectedFlashcard.FlashcardsID, UpFront, UpBack);

                    if (waitForkey)
                    {
                        _console.MarkupLine("[grey]Press any key to continue...[/]");
                        _console.Input.ReadKey(true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _console.MarkupLine($"[red]Error:[/] {ex.Message}");
            _console.Input.ReadKey(true);
        }
    }

    public void DeleteFlashcard(bool waitForkey = true, IAnsiConsole? console = null)
    {
        IAnsiConsole _console = console ?? AnsiConsole.Console;

        try
        {
            _console.Clear();
            int? SelectedStack = SelectStack(_console);

            if (SelectedStack is null) return;
            else
            {
                var AllFlashcards = _flashcardsController.GetAllFlashcards(SelectedStack);

                if (!AllFlashcards.Any()) _console.MarkupLine($"[red]Flashcards not found in this stack.[/]");
                else
                {
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
                    
                }
            }
        }
        catch (Exception ex)
        {
            _console.MarkupLine($"[red]Error:[/] {ex.Message}");
            _console.Input.ReadKey(true);
        }

        if (waitForkey)
        {
            _console.MarkupLine("[grey]Press any key to continue...[/]");
            _console.Input.ReadKey(true);
        }
    }
}
