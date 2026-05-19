using Spectre.Console;
using Microsoft.Data.SqlClient;
using Dapper;
using Flashcards.Models;
using Flashcards.DataAccess;

namespace Flashcards.Controllers;

internal class StacksController 
{
    public List<Stack> GetAllStacks()
    {
        using var connection = Database.GetConnection();
        string sql = "SELECT StackID, NameStack FROM dbo.Stacks ORDER BY NameStack";
        return connection.Query<Stack>(sql).ToList();
    }

    // Aggiungere il controllo quando non viene inserito un nome stack. Essendo un campo obbligatorio, non dovrebbe essere possibile creare uno stack senza nome.
    public void CreateStack(string name)
    {
        using var connection = Database.GetConnection();
        string sql = "INSERT INTO Stacks (NameStack) VALUES (@Name)";
        connection.Execute(sql, new { Name = name }); 
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