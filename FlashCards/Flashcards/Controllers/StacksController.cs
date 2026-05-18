using Spectre.Console;
using Microsoft.Data.SqlClient;
using Dapper;
using Flashcards.Models;
using Flashcards.DataAccess;

namespace Flashcards.Controllers;

internal class StacksController : Database
{
    public static bool ViewStacks()
    {
        return true;
    }

    public static bool CreateStack()
    {
        return true;
    }

    public static bool UpdateStack()
    {
        return true;
    }

    public static bool DeleteStack()
    {
        return true;
    }

}