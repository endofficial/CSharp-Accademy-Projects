using Flashcards.DataAccess;
using Flashcard.UI;
using System.Net.Http.Headers;

namespace Flashcard
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Database database = new();
            database.Initialize();

            UserInterface userInterface = new();
            userInterface.MainMenu();
        }
    }
}
