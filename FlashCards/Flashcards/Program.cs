using Flashcards.DataAccess;
using Flashcard.UI;
using System.Net.Http.Headers;
using Flashcards.Controllers;

namespace Flashcard
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Database database = new();
            database.Initialize();

            IStacksController _stacksController = new StacksController();
            IFlashcardsController _flashcardsController = new FlashcardsController();
            UserInterface userInterface = new(_stacksController, _flashcardsController);
            userInterface.MainMenu();
        }
    }
}
