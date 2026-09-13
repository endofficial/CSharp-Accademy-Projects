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
    void UpdateStack(int stackId, string name);
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

    public void CreateStack(string name)
    {
        using var connection = Database.GetConnection();
        string sql = "INSERT INTO Stacks (NameStack) VALUES (@Name)";
        connection.Execute(sql, new { Name = name }); 
    }

    public void UpdateStack(int stackId, string name)
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