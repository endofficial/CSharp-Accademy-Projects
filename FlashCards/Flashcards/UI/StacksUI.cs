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

    private static bool IsNotValidStackName(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || double.TryParse(input, out _))
        {
            return true;
        }
        return IsOnlySpecialCharacters(input);
    }

    private static bool IsOnlySpecialCharacters(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return true;

        return Regex.IsMatch(input, @"^[^a-zA-Z0-9]+$"); // This regex checks if the string contains only special characters (no letters or digits)
    }

    private static bool IsNotValidInput(int input)
    {
        return input <= 0;
    }

    private bool DoesStackNotExist(int input)
    {
        return !_stacksController.CheckIfStackExists(input);
    }

    // Method to display all stacks
    public void ShowStacksTable(bool waitForKey = true, IAnsiConsole? console = null) // Method to display stacks in a table format
    {
        IAnsiConsole? _console = console ?? AnsiConsole.Console;

        _console.Clear();
        var stacks = _stacksController.GetAllStacks();

        if (stacks == null || stacks.Count == 0)
        {
            _console.MarkupLine("[yellow]No stacks found. Please create a stack first.[/]");
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
            _console.Write(table);
        }
        

        if (waitForKey)
        {
            WaitForKey(_console);
        }
    }

    // Method to add a new stack with input validation
    public void AddStack(bool waitForKey = true, IAnsiConsole? console = null)
    {
        IAnsiConsole? _console = console ?? AnsiConsole.Console;

        _console.Clear();
        var namePrompt = new TextPrompt<string>("Enter the name of the new stack: ")
            .PromptStyle("yellow")
            .Validate(input =>
            {
                if (IsNotValidStackName(input)) return ValidationResult.Error("[red]Invalid input. Please enter a non-empty name that is not a number.[/]");

                return ValidationResult.Success();
            });
        string name = _console.Prompt(namePrompt); 

        _stacksController.CreateStack(name);
        _console.MarkupLine("[green]Stack created successfully![/]");

        if (waitForKey)
        {
            WaitForKey(_console);
        }
    }  

    // Method to update an existing stack with input validation
    public void UpdateStack(bool waitForKey = false, IAnsiConsole? console = null)
    {
        IAnsiConsole? _console = console ?? AnsiConsole.Console;
        ShowStacksTable(waitForKey, _console);

        var idPrompt = new TextPrompt<int>("\nEnter the ID of the stack you want to update: ")
            .PromptStyle("yellow")
            .Validate(input =>
            {
                if (IsNotValidInput(input))
                {
                    return ValidationResult.Error("[red]Invalid input. Please enter a positive integer for the stack ID.[/]");
                }

                if (DoesStackNotExist(input))
                {
                    return ValidationResult.Error("[red]Stack not found. Please enter a valid stack ID.[/]");
                }

                return ValidationResult.Success();
            });
        int UpStackID = _console.Prompt(idPrompt);

        var namePrompt = new TextPrompt<string>("Enter the name to update the stack: ")
            .PromptStyle("yellow")
            .Validate(input =>
            {
                if (IsNotValidStackName(input)) return ValidationResult.Error("[red]Invalid input. Please enter a non-empty name that is not a number.[/]");

                return ValidationResult.Success();
            });
        string UpStackName = _console.Prompt(namePrompt); 
        _stacksController.UpdateStack(UpStackID, UpStackName);

        _console.MarkupLine("[green]Stack updated successfully![/]");  

        if (waitForKey)
        {
            WaitForKey(_console);
        }
    }

    // Method to delete stacks with options to delete all or a specific stack by ID
    public void DeleteStack(IAnsiConsole? console = null)
    {
        IAnsiConsole? _console = console ?? AnsiConsole.Console;

        try
        {
            _console.Clear();
        }
        catch (IOException)
        {
            // Handle the exception if the console cannot be cleared
        }

        ShowStacksTable(waitForKey: false);

        bool closeApp = false;
        while (!closeApp)
        {
            var actionChoice = _console.Prompt(
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
                    _console.MarkupLine("[green]All stacks deleted successfully![/]");
                    break;
                case ChooseDeleteAction.DeleteOne:
                    DeleteOne(_console);
                    break;
                case ChooseDeleteAction.BackToMainMenu:
                    closeApp = true;
                    break;
            }
        }
    }

    // Method to delete a specific stack by ID with input validation
    public void DeleteOne(IAnsiConsole? console = null, bool waitForKey= true)
    {
        IAnsiConsole? _console = console ?? AnsiConsole.Console;

        var deletePrompt = new TextPrompt<int>("Enter the ID of the stack you want to delete: ")
            .PromptStyle("yellow")
            .Validate(input =>
            {
                if (IsNotValidInput(input))
                {
                    return ValidationResult.Error("[red]Invalid input. Please enter a positive integer for the stack ID.[/]");
                }

                if (DoesStackNotExist(input))
                {
                    return ValidationResult.Error("[red]Stack not found. Please enter a valid stack ID.[/]");
                }

                return ValidationResult.Success();
            });
        int DelStackID = _console.Prompt(deletePrompt);
        _stacksController.DeleteStack(DelStackID);

        _console.MarkupLine("[green]Stack deleted successfully![/]");

        if (waitForKey)
        {
            WaitForKey(_console);
        }
    }
}