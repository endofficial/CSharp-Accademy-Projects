using Xunit;
using Flashcards.UI;
using Moq;
using Flashcards.Controllers;
using Spectre.Console.Testing;
using System.Text.RegularExpressions;
using Flashcards.Models;

namespace UnitTesting.UnitTest;

public class StacksUITests
{
    [Fact]
    public void ShowsStacksTable_WhenCalled()
    {
        // Arrange
        var mockController = new Mock<IStacksController>();
        var testConsole = new TestConsole();

        mockController.Setup (x => x.GetAllStacks()).Returns(new List<Stack>());
        var sut = new StacksUI(mockController.Object);

        // Act
        sut.ShowStacksTable(waitForKey: false, testConsole);

        // Assert
        mockController.Verify(x => x.GetAllStacks(), Times.Once);
    }

    [Theory]
    [InlineData("MyNewStack", "Stack created successfully!")]
    [InlineData("\"\"", "Invalid input. Please enter a non-empty name that is not a number.")]
    [InlineData("123", "Invalid input. Please enter a non-empty name that is not a number.")]
    public void AddStack_WhenInputIsValid_WhenInputIsInvalid_ShouldReturnExpectedMessage(string input, string expectedMessage)
    {
        // Arrange
        Moq.Mock<IStacksController> mockController = new Mock<IStacksController>(); // To verify that CreateStack is called with the correct parameters
        var testConsole = new TestConsole();
        testConsole.Input.PushText(input);
        testConsole.Input.PushKey(ConsoleKey.Enter);

        // System under test
        var sut = new StacksUI(mockController.Object);

        // Act
        sut.AddStack(testConsole); // Call the method being tested
        var testOutput = testConsole.Output; // Capture the console output

        // Assert 
        Assert.Contains(expectedMessage, testOutput);

        // Assert (to verify behavior)
        if (string.IsNullOrWhiteSpace(input) || double.TryParse(input, out _) || input == "\"\"" || IsOnlySpecialCharacters(input))
        {
            mockController.Verify(x => x.CreateStack(input), Times.Never);
        }
        else
        {
            mockController.Verify(x => x.CreateStack(input), Times.Once);
        }
    }

    public static bool IsOnlySpecialCharacters(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return true;
        return Regex.IsMatch(input, @"^[^a-zA-Z0-9]+$");
    }

    [Theory]
    [InlineData(2, "Math", "Stack updated successfully!")]
    [InlineData(3, "4", "Invalid input. Please enter a non-empty name that is not a number.")]
    public void UpdateStack_WhenInputIsValid_WhenInputIsInvalid(int idStack, string nameStack, string expectedMessage)
    {
        // Arrange
        var mockController = new Mock<IStacksController>();

        mockController.Setup(x => x.CheckIfStackExists(idStack)).Returns(true);

        var testConsole = new TestConsole();

        testConsole.Input.PushText(idStack.ToString());
        testConsole.Input.PushKey(ConsoleKey.Enter);

        testConsole.Input.PushText(nameStack);
        testConsole.Input.PushKey(ConsoleKey.Enter);

        var sut = new StacksUI(mockController.Object);

        // Act
        sut.UpdateStack(waitForKey: false, testConsole);
        var testOutput = testConsole.Output;

        // Assert
        Assert.Contains(expectedMessage, testOutput);

        if (string.IsNullOrWhiteSpace(nameStack) || double.TryParse(nameStack, out _) || nameStack == "\"\"" || IsOnlySpecialCharacters(nameStack))
        {
            mockController.Verify(x => x.UpdateStack(idStack, nameStack), Times.Never);
        }
        else
        {
            mockController.Verify(x => x.UpdateStack(idStack, nameStack), Times.Once);
        }
    }

    [Fact]
    public void UpdateStack_WhenStackDoesNotExist_ReturnExpectedMessage()
    {
        // Arrange
        var mockController = new Mock<IStacksController>();
        var testConsole = new TestConsole();

        mockController.Setup(x => x.CheckIfStackExists(999)).Returns(false);

        testConsole.Input.PushText("999");
        testConsole.Input.PushKey(ConsoleKey.Enter);

        var sut = new StacksUI(mockController.Object);

        // Act
        sut.UpdateStack(waitForKey: false, testConsole);
        var testOutput = testConsole.Output;

        // Assert
        Assert.Contains("Stack not found. Please enter a valid stack ID.", testOutput);

        mockController.Verify(x => x.CheckIfStackExists(999), Times.Once);
        mockController.Verify(x => x.UpdateStack(999, It.IsAny<string>()), Times.Never);

    }

    [Fact]
    public void DeleteStack_WhenStackExists_ShouldDeletestack()
    {
        // Arrange
        var mockController = new Mock<IStacksController>();
        var testConsole = new TestConsole();

        testConsole.Profile.Capabilities.Ansi = true;
        testConsole.Profile.Capabilities.Interactive = true;

        mockController.Setup(x => x.GetAllStacks()).Returns(new List<Stack>());

        testConsole.Input.PushKey(ConsoleKey.Enter);

        testConsole.Input.PushKey(ConsoleKey.DownArrow);
        testConsole.Input.PushKey(ConsoleKey.DownArrow);
        testConsole.Input.PushKey(ConsoleKey.Enter);

        var sut = new StacksUI(mockController.Object);

        // Act
        sut.DeleteStack(testConsole);
        var testOutput = testConsole.Output;

        // Assert
        Assert.Contains("All stacks deleted successfully!", testOutput);

        mockController.Verify(x => x.DeleteAllStacks(), Times.Once);

    }

    [Theory]
    [InlineData(1, "Stack deleted successfully!")]
    [InlineData(0, "Invalid input. Please enter a positive integer for the stack ID.")]
    public void DeleteOneStack_ShouldDeleteOneStack(int stackId, string expectedMessage)
    {
        // Arrange
        var mockController = new Mock<IStacksController>();
        var testConsole = new TestConsole();

        mockController.Setup(x => x.CheckIfStackExists(stackId)).Returns(true);

        testConsole.Input.PushText(stackId.ToString());
        testConsole.Input.PushKey(ConsoleKey.Enter);

        var sut = new StacksUI(mockController.Object);

        // Act
        sut.DeleteOne(testConsole);
        var testOutput = testConsole.Output;

        // Assert
        Assert.Contains(expectedMessage, testOutput);

        if (stackId <= 0)
        {
            mockController.Verify(x => x.DeleteStack(stackId), Times.Never);
        }
        else
        {
            mockController.Verify(x => x.DeleteStack(stackId), Times.Once);
        }
    }
}