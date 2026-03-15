using FluentAssertions; 
using HabitTracker;
using System.Diagnostics.Metrics;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HABITTRACKER.UnitTesting.UnitTests;

public class ValidatorInputTests
{
    [Theory]
    [InlineData("11-11-11", "11-11-11")]
    [InlineData(" 11-11-11", "11-11-11")]
    [InlineData("11-11-11 ", "11-11-11")]
    [InlineData("01/01/00\n01-01-00", "01-01-00")]
    [InlineData("a\n4\n31-12-99", "31-12-99")]
    public void CorrectDateInput_ReturnCorrectDate_AndWhitInvalidInput(string inputSequence, string expected)
    {
        // Arrange
        var input = new StringReader(inputSequence);

        // Act
        var result = InputInsert.GetDateInput(input);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void CorrectDateInput_WhenInputIs0_ReturnToMainMenu()
    {
        // Arrange
        var input = new StringReader("0");

        // Act
        var result = InputInsert.GetDateInput(input);

    // Assert
    result.Should().Be("0");
    }

    [Fact]
    public void CorrectDateInput_ReturnDateExactFormat_WhenUserAddSpace()
    {
        // Arrange
        var input = new StringReader(" 11-11-11");

        // Act
        var result = InputInsert.GetDateInput(input);

        // Assert
        result.Should().Be("11-11-11");
    }

    [Fact]
    public void CorrectDateInput_ReturnDateExactFormat_WhenUserAddSpaceAfterDate()
    {
        // Arrange
        var input = new StringReader("11-11-11 ");

        // Act
        var result = InputInsert.GetDateInput(input);

        // Assert
        result.Should().Be("11-11-11");
    }

    [Theory]
    [InlineData("-5\n5", 5)]
    [InlineData("-5\na\nA\n11-11-11\n8", 8)]
    [InlineData("a\nA\n11-11-11\n0", 0)]
    [InlineData("5 \n", 5)]
    [InlineData(" 5\n", 5)]
    public void CorrectNumberInput_ReturnNumber_WhitInvalidInput_AndReturnExactNumber(string inputSequence, int exepcted)
    {
        // Arrange
        var inputNumber = new StringReader(inputSequence);

        // Act
        var result = InputInsert.GetNumberInput("ENTER A NUMBER.", inputNumber);

        // Assert
        result.Should().Be(exepcted);
    }

    [Theory]
    [InlineData("TEXT", "TEXT")]
    [InlineData(" TEXT", "TEXT")]
    [InlineData("TEXT ", "TEXT")]
    [InlineData("\n\nHABIT", "HABIT")]
    [InlineData("4\nNEW HABIT", "NEW HABIT")]
    public void CorrectNewHabitInput_ReturnNewHabit_WhitInvalidInput(string inputSequence, string exepcted)
    {
        // Arrange
        var inputHabit = new StringReader(inputSequence);

        // Act
        var result = InputInsert.GetNewHabitInput(inputHabit);

        // Assert
        result.Should().Be(exepcted);
    }

    [Theory]
    [InlineData("LITERS", "LITERS")]
    [InlineData(" LITERS", "LITERS")]
    [InlineData("LITERS ", "LITERS")]
    [InlineData("\n\nKILOGRAMS", "KILOGRAMS")]
    [InlineData("4\nGRAMS", "GRAMS")]
    public void CorrectNewUnitOfMeasureInput_ReturnNewUnitOfMeasure_WhitInvalidInput(string inputSequence, string exepcted)
    {
        // Arrange
        var inputUnitOfMeasure = new StringReader(inputSequence);

        // Act
        var result = InputInsert.GetNewUnitOfMeasureInput(inputUnitOfMeasure);

        // Assert
        result.Should().Be(exepcted);
    }
}
