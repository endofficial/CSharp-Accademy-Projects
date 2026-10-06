using Flashcards.DataAccess;
using Flashcards.Models;
using Dapper;
using FlashcardModels = Flashcards.Models.Flashcard;
using FlashcardDto = Flashcards.Models.FlashcardDto;

namespace Flashcards.Controllers;

public interface IFlashcardsController
{
    List<FlashcardDto> GetAllFlashcards(int? stackId);
    void CreateFlashcard(int? stackId, string front, string back);
    void UpdateFlashcard(int flashcardId, string front, string back);
    void DeleteFlashcard(int flashcardId);
}

internal class FlashcardsController : IFlashcardsController
{
    public List<FlashcardDto> GetAllFlashcards(int? stackID)
    {
        using var connection = Database.GetConnection();
        string sql = 
            "SELECT * FROM dbo.Flashcards " +
            "WHERE StackID = @StackID " +
            "ORDER BY FlashcardsID ASC";
        var rawCards = connection.Query<FlashcardModels>(sql, new { StackID = stackID }).ToList();

        List<FlashcardDto> dtoList = new List<FlashcardDto>();

        for (int i = 0; i < rawCards.Count; i++)
        {
            dtoList.Add(new FlashcardDto
            {
                DisplayID = i + 1,
                FlashcardsID = rawCards[i].FlashcardsID,
                Front = rawCards[i].Front,
                Back = rawCards[i].Back
            });
        }
        return dtoList;
    }

    public void CreateFlashcard(int? stackID, string front, string back)
    {
        using var connection = Database.GetConnection();
        string sql = "INSERT INTO Flashcards (StackID, Front, Back) VALUES (@StackID, @Front, @Back)";
        connection.Execute(sql, new { StackID = stackID, Front = front, Back = back });
    }

    public void UpdateFlashcard(int flashcardsID, string front, string back)
    {
        using var connection = Database.GetConnection();
        string sql = "UPDATE Flashcards SET Front = @Front, Back = @Back WHERE FlashcardsID = @FlashcardsID";
        connection.Execute(sql, new { Front = front, Back = back, FlashcardsID = flashcardsID });
    }

    public void DeleteFlashcard(int flashcardID)
    {
        using var connection = Database.GetConnection();
        string sql = "DELETE FROM Flashcards WHERE FlashcardID = @FlashcardID";
        connection.Execute(sql, new { FlashcardID = flashcardID });
    }

}
