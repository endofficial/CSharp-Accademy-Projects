using CodingTracker.Data;
using CodingTracker.Controller;
using Dapper;
using Spectre.Console;

namespace CodingTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SqlMapper.AddTypeHandler(new TimeOnlyTypeHandler()); // Register the TimeOnly type handler with Dapper

            Database database = new();
            database.Initialize();

            RandomValues values = new RandomValues();
            values.ValueRandom();

            UserInterface ui = new();
            ui.MainMenu();
        }
    }
}
