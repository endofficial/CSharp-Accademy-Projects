using Microsoft.IdentityModel.Tokens;
using Spectre.Console;
using static Flashcards.Enums.Enums;
using Flashcards.Controllers;

namespace Flashcards.UI;

internal class StacksUI
{
    private readonly StacksController _stacksController = new StacksController();

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
                    break;
                case StackAction.BackToMainMenu:
                    closeApp = true;
                    break;
            }
        }
    }

    private void ShowStacksTable(bool waitForKey = true) // Method to display stacks in a table format
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

    private void AddStack()
    {
        try
        {
            AnsiConsole.Clear();
            string name = AnsiConsole.Ask<string>("Enter the name of the new stack: ");
            _stacksController.CreateStack(name);
            AnsiConsole.MarkupLine("[green]Stack created successfully![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
        }

        AnsiConsole.MarkupLine("\nPress any key to return to the stack menu...");
        ReadKey();
    }

    private void UpdateStack(bool waitForKey = false)
    {
        ShowStacksTable(waitForKey);

        try 
        {
            int UpStackID = AnsiConsole.Ask<int>("Enter the ID of the stack you want to update: ");
            if (UpStackID <= 0)
            {
                AnsiConsole.MarkupLine("[red]Invalid input. Please enter a positive integer for the stack ID.[/]\n");
                UpStackID = AnsiConsole.Ask<int>("Enter the ID of the stack you want to update: ");
            }

            bool stackExists = _stacksController.CheckIfStackExists(UpStackID);
            while (!stackExists)
            {
                AnsiConsole.MarkupLine("[red]Stack not found. Please enter a valid stack ID.[/]\n");
                UpStackID = AnsiConsole.Ask<int>("Enter the ID of the stack you want to update: ");
                if (UpStackID < 0)
                {
                    AnsiConsole.MarkupLine("[red]Invalid input. Please enter a positive integer for the stack ID.[/]\n");
                    UpStackID = AnsiConsole.Ask<int>("Enter the ID of the stack you want to update: ");
                }
                else
                {
                    stackExists = _stacksController.CheckIfStackExists(UpStackID);
                }
            }

            string UpStackName = AnsiConsole.Ask<string>("Enter the new name for the stack: ");
            _stacksController.UpdateStack(UpStackName, UpStackID);
            AnsiConsole.MarkupLine("[green]Stack updated successfully![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
        }

        AnsiConsole.MarkupLine("\nPress any key to return to the stack menu...");
        ReadKey();
    }
}