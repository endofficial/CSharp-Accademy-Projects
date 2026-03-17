using CodingTracker.Controller;
using FluentAssertions;
using Xunit;
using Spectre.Console;
using Spectre.Console.Testing;
using System.Security.Cryptography.X509Certificates;

namespace UnitTesting.UnitTests
{
    public class ValidatorInputTests
    {
        [Theory]
        [InlineData("2020-11-25", "2020-11-25")]
        [InlineData("0", "0")]
        [InlineData(" 2020-11-25", "2020-11-25")]
        [InlineData("2020-11-25 ", "2020-11-25")]
        [InlineData("invalid\r2025-01-01", "2025-01-01")] // Wtih spectre.console don't use => "/n"
        [InlineData("invalid\rinvalid\r2020-11-25", "2020-11-25")]
        public void CorrectDateInput_ReturnCorrectDate(string inputDate, string expected)
        {
            // Arrange
            var console = new TestConsole();
            console.Input.PushTextWithEnter(inputDate); // Simulates the user typing the sequence and pressing ENTER 
            
            // Act
            var result = InputInsert.GetDateSessionInput(console);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("Programming", "21:00", "22:00", 1, "21:00", "22:00")]
        [InlineData("Programming", "20:00", "22:00", 2, "20:00", "22:00")]
        [InlineData("Programming", "invalid\r21:00", "invalid\r22:00", 1, "21:00", "22:00")]
        [InlineData("Programming", " 21:00", " 22:00", 1, "21:00", "22:00")]
        [InlineData("Programming", "21:00 ", "22:00 ", 1, "21:00", "22:00")]
        public void CorrectTimeSessionInput_ReturnCorrectCodingSession(string description, string start, string end, int durationexpected, string startExp, string endExp)
        {
            // Arrange
            var console = new TestConsole();
            console.Input.PushTextWithEnter(description);
            console.Input.PushTextWithEnter(start);
            console.Input.PushTextWithEnter(end);
            console.Input.PushTextWithEnter(" ");

            // Act
            var result = InputInsert.GetTimeSessionInput("2026-12-25", console);

            // Assert
            result.Should().NotBeNull();
            result.Description.Should().Be(description);
            result.StartTime.ToString("HH:mm").Should().Be(startExp);
            result.EndTime.ToString("HH:mm").Should().Be(endExp);
            result.Date.Should().Be("2026-12-25");
            result.Duration.TotalHours.Should().Be(durationexpected);
        }

        [Theory]
        [InlineData("3", 3)]
        [InlineData("invalid\r4", 4)]
        [InlineData(" 4", 4)]
        [InlineData("4 ", 4)]
        [InlineData("0", 0)]
        public void ReturnCorrectId(string inputId, int expected)
        {
            // Arrange
            var console = new TestConsole();
            console.Input.PushTextWithEnter(inputId);

            // Act
            var result = InputInsert.GetId(console);

            // Assert
            result.Should().Be(expected);

        }

        [Theory]
        [InlineData("Working", "Working")]
        public void ReturCorrectOnlyDescription(string inputDescription, string expected)
        {
            // Arrange
            var console = new TestConsole();
            console.Input.PushTextWithEnter(inputDescription);

            // Act
            var result = InputInsert.OnlyDescription(console);

            // Assert
            result.Should().Be(expected);
        }
    }

}
