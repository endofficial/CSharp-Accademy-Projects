using Spectre.Console;
using Microsoft.Data.SqlClient;
using Dapper;
using Flashcards.Models;
using Flashcards.DataAccess;

namespace Flashcards.Controllers;

public interface IStacksController
{
    List<Stack> GetAllStacks();
    void CreateStack(string name);
    void UpdateStack(string name, int stackId);
    void DeleteAllStacks();
    void DeleteStack(int stackId);
    bool CheckIfStackExists(int stackId);
}

public class StacksController : IStacksController
{
    public List<Stack> GetAllStacks()
    {
        using var connection = Database.GetConnection();
        string sql = "SELECT StackID, NameStack FROM dbo.Stacks ORDER BY StackID ASC";
        return connection.Query<Stack>(sql).ToList();
    }

    // Aggiungere il controllo quando non viene inserito un nome stack. Essendo un campo obbligatorio, non dovrebbe essere possibile creare uno stack senza nome.
    public void CreateStack(string name)
    {
        using var connection = Database.GetConnection();
        string sql = "INSERT INTO Stacks (NameStack) VALUES (@Name)";
        connection.Execute(sql, new { Name = name }); 
    }

    // to update the name of a stack, we need to specify the stack id and the new name.
    public void UpdateStack(string name, int stackId)
    {
        using var connection = Database.GetConnection();
        string sql = "UPDATE Stacks SET NameStack = @Name WHERE StackID = @StackID";
        connection.Execute(sql, new { Name = name, StackID = stackId });
    }

    public void DeleteAllStacks()
    {
        using var connection = Database.GetConnection();
        string sql = "DELETE FROM Stacks";
        connection.Execute(sql);
    }

    public void DeleteStack(int stackId)
    {
        using var connection = Database.GetConnection();
        string sql = "DELETE FROM Stacks WHERE StackID = @StackID";
        connection.Execute(sql, new { StackID = stackId });
    }

    // To check if a stack exists
    public bool CheckIfStackExists(int stackId)
    {
        using var connection = Database.GetConnection();
        string sqlCode = "SELECT 1 FROM Stacks WHERE StackID = @StackID";
        bool exists = connection.ExecuteScalar<bool>(sqlCode, new { StackID = stackId });
        if (!exists) return false;
        else return true;
    }
}