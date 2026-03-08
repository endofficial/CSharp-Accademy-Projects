using Spectre.Console;
using CodingTracker;
using Dapper;
using Microsoft.Data.Sqlite;
using CodingTracker.Data;

namespace CodingTracker.Controller;

internal class CodingController : Database
{
    public static bool RegisterSession()
    {
        Clear();

        string dateSession = InputInsert.GetDateSessionInput();
        if (dateSession == "0") return false;

        var durationSession = InputInsert.GetTimeSessionInput(dateSession);
        if (durationSession == null) return false;

        if (durationSession != null)
        {
            using var connection = GetConnection();

            // FERMO QUI: PROBLEMA CON LA REGISTRAZIONE DELLA TABELLA. NON TROVA I VALORI.
            string sql = @"
                INSERT INTO CodingSessions (StartTime, EndTime, Date, Duration, Description)
                VALUES (@startTime, @endTime, @date, @duration, @description);";

            connection.Execute(sql, durationSession);
        }

        return true;
    }
}

