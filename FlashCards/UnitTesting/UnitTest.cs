using Xunit;
using Flashcards.UI;
using Moq;
using Flashcards.Controllers;
using Spectre.Console.Testing;
using System.Text.RegularExpressions;

namespace UnitTesting.UnitTest;

public class StacksUITests
{
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
}