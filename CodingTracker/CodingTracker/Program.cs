using CodingTracker.Data;
using Dapper;

namespace CodingTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SqlMapper.AddTypeHandler(new TimeOnlyTypeHandler()); // Register the TimeOnly type handler with Dapper

            Database database = new();
            database.Initialize();

            UserInterface ui = new();
            ui.MainMenu();
        }
    }
}
