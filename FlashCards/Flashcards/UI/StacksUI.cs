using Microsoft.IdentityModel.Tokens;
using Spectre.Console;
using static Flashcards.Enums.Enums;
using Flashcards.Controllers;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Flashcards.UI;

public class StacksUI
{
    private readonly IStacksController _stacksController;

    public StacksUI(IStacksController stacksController)
    {
        _stacksController = stacksController;
    }

    public void stackMenu()
    {
        bool closeApp = false;
        while (!closeApp)
        {
            AnsiConsole.Clear();
            var actionChoice = AnsiConsole.Prompt(
                new SelectionPrompt<StackAction>()
                .Title("[blue]Stack Menu[/]")
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
                    ShowStacksTable();
                    break;
                case StackAction.CreateStack:
                    AddStack();
                    break;
                case StackAction.UpdateStack:
                    UpdateStack();
                    break;
                case StackAction.DeleteStack:
                    DeleteStack();
                    break;
                case StackAction.BackToMainMenu:
                    closeApp = true;
                    break;
            }
        }
    }

    // Method to display all stacks
    public void ShowStacksTable(bool waitForKey = true) // Method to display stacks in a table format
    {
        try
        {
            AnsiConsole.Clear();
            var stacks = _stacksController.GetAllStacks();

            if (stacks.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No stacks found. Please create a stack first.[/]");
            }
            else
            {
                var table = new Table();
                table.Border(TableBorder.Rounded)
                    .AddColumn("ID")
                    .AddColumn("Name");

                foreach (var stack in stacks)
                {
                    table.AddRow(
                        stack.StackID.ToString(),
                        stack.NameStack
                    );
                }
                AnsiConsole.Write(table);
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
        }

        if (waitForKey)
        {
            AnsiConsole.MarkupLine("\nPress any key to return to the stack menu...");
            ReadKey(true);
        }
    }

    // Method to add a new stack with input validation
    public void AddStack(IAnsiConsole? console = null)
    {
        IAnsiConsole? _console = console ?? AnsiConsole.Console;
        try
        {
            _console.Clear();

            var namePrompt = new TextPrompt<string>("Enter the name of the new stack: ")
                .PromptStyle("yellow")
                .Validate(input =>
                {
                    if (string.IsNullOrWhiteSpace(input) || double.TryParse(input, out _) || input == "\"\"" || IsOnlySpecialCharacters(input))
                    {
                        return ValidationResult.Error("[red]Invalid input. Please enter a non-empty name that is not a number.[/]");
                    }

                    return ValidationResult.Success();
                });
            string name = _console.Prompt(namePrompt); 

            _stacksController.CreateStack(name);
            _console.MarkupLine("[green]Stack created successfully![/]");
        }
        catch (Exception ex)
        {
            _console.MarkupLine($"[red]Error:[/] {ex.Message}");
        }

        _console.MarkupLine("\nPress any key to return to the stack menu...");

        try
        {
            _console.Input.ReadKey(true);
        }
        catch (InvalidOperationException)
        {
            // during unit testing, the console input may not be available
        }
    }

    public static bool IsOnlySpecialCharacters(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return true;

        return Regex.IsMatch(input, @"^[^a-zA-Z0-9]+$"); // This regex checks if the string contains only special characters (no letters or digits)
    }

    // Method to update an existing stack with input validation
    public void UpdateStack(bool waitForKey = false)
    {
        ShowStacksTable(waitForKey);

        try 
        {
            var idPrompt = new TextPrompt<int>("\nEnter the ID of the stack you want to update: ")
                .PromptStyle("yellow")
                .Validate(input =>
                {
                    if (input <= 0)
                    {
                        return ValidationResult.Error("[red]Invalid input. Please enter a positive integer for the stack ID.[/]");
                    }

                    if (!_stacksController.CheckIfStackExists(input))
                    {
                        return ValidationResult.Error("[red]Stack not found. Please enter a valid stack ID.[/]");
                    }

                    return ValidationResult.Success();
                });
            int UpStackID = AnsiConsole.Prompt(idPrompt);

            var namePrompt = new TextPrompt<string>("Enter the name to update the stack: ")
                .PromptStyle("yellow")
                .Validate(input =>
                {
                    if (string.IsNullOrWhiteSpace(input) || double.TryParse(input, out _))
                    {
                        return ValidationResult.Error("[red]Invalid input. Please enter a non-empty name that is not a number.[/]");
                    }

                    return ValidationResult.Success();
                });
            string UpStackName = AnsiConsole.Prompt(namePrompt); 
            _stacksController.UpdateStack(UpStackName, UpStackID);

            AnsiConsole.MarkupLine("[green]Stack updated successfully![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
        }

        AnsiConsole.MarkupLine("\nPress any key to return to the stack menu...");
        ReadKey(true);
    }

    // Method to delete stacks with options to delete all or a specific stack by ID
    public void DeleteStack()
    {
        AnsiConsole.Clear();

        ShowStacksTable(waitForKey: false);

        bool closeApp = false;
        while (!closeApp)
        {
            var actionChoice = AnsiConsole.Prompt(
                new SelectionPrompt<ChooseDeleteAction>()
                .Title("What would you like to delete?")
                .UseConverter(option => option switch
                {
                    ChooseDeleteAction.DeleteAll => "Delete All Stacks",
                    ChooseDeleteAction.DeleteOne => "Delete One Stack",
                    ChooseDeleteAction.BackToMainMenu => "Back to Stack Menu",
                    _ => option.ToString()
                })
                .AddChoices(Enum.GetValues<ChooseDeleteAction>()));
            switch (actionChoice)
            {
                case ChooseDeleteAction.DeleteAll:
                    _stacksController.DeleteAllStacks();
                    AnsiConsole.MarkupLine("[green]All stacks deleted successfully![/]");
                    break;
                case ChooseDeleteAction.DeleteOne:
                    DeleteOne();
                    break;
                case ChooseDeleteAction.BackToMainMenu:
                    closeApp = true;
                    break;
            }
        }
    }

    // Method to delete a specific stack by ID with input validation
    public void DeleteOne()
    {
        try
        {
            var deletePrompt = new TextPrompt<int>("Enter the ID of the stack you want to delete: ")
                .PromptStyle("yellow")
                .Validate(input =>
                {
                    if (input <= 0)
                    {
                        return ValidationResult.Error("[red]Invalid input. Please enter a positive integer for the stack ID.[/]");
                    }

                    if (!_stacksController.CheckIfStackExists(input))
                    {
                        return ValidationResult.Error("[red]Stack not found. Please enter a valid stack ID.[/]");
                    }

                    return ValidationResult.Success();
                });
            int DelStackID = AnsiConsole.Prompt(deletePrompt);

            bool stackExists = _stacksController.CheckIfStackExists(DelStackID);

            _stacksController.DeleteStack(DelStackID);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
        }

        AnsiConsole.MarkupLine("\nPress any key to return to the stack menu...");
        ReadKey(true);
    }
}