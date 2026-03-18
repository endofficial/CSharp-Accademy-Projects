using Dapper;
using CodingTracker.Data;

namespace CodingTracker.Controller;

// To register some records
internal class RandomValues : Database
{
    public void ValueRandom()
    {
        using var connection = GetConnection();

        long countRecord = connection.ExecuteScalar<long>("SELECT COUNT (*) FROM CodingSessions");

        if (countRecord == 0)
        {
            var randomValues = InputInsert.RandomSession();

            string sql = @"
                INSERT INTO CodingSessions (StartTime, EndTime, Date, Duration, Description) 
                VALUES (@StartTime, @EndTime, @Date, @Duration, @Description)";

            connection.Execute(sql, randomValues);
        }
    }
}

