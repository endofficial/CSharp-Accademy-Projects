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
                    ShowStacksTable();
                    break;
                case StackAction.CreateStack:
                    AddStack();
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

    private void ShowStacksTable() // Method to display stacks in a table format
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

        AnsiConsole.MarkupLine("\nPress any key to return to the stack menu...");
        ReadKey();
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
}