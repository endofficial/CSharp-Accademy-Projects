using Flashcards.DataAccess;
using FlashcardMenu.UI;
using System.Net.Http.Headers;
using Flashcards.Controllers;

namespace FlashcardProject
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
