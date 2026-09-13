using Flashcards.DataAccess;
using Flashcards.Models;
using Dapper;
using FlashcardModels = Flashcards.Models.Flashcard;
using FlashcardDto = Flashcards.Models.FlashcardDto;

namespace Flashcards.Controllers;

public interface IFlashcardsController
{
    List<FlashcardDto> GetAllFlashcards(int stackId);
    void CreateFlashcard();
    void UpdateFlashcard();
    void DeleteFlashcard();
}

internal class FlashcardsController : IFlashcardsController
{
    public List<FlashcardDto> GetAllFlashcards(int stackID)
    {
        using var connection = Database.GetConnection();
        string sql = 
            "SELECT * FROM dbo.Flashcards " +
            "WHERE StackID = @StackID " +
            "ORDER BY FlashcardID ASC";
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

    public void CreateFlashcard()
    {
        return;
    }

    public void UpdateFlashcard()
    {
        return;
    }

    public void DeleteFlashcard()
    {
        return;
    }

}
