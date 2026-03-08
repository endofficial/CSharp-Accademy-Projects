using Spectre.Console;
using CodingTracker;

namespace CodingTracker.Controller;

internal class CodingController
{
    public static bool RegisterSession()
    {
        Clear();
        
        bool dateSession = InputInsert.GetDateSessionInput(out string date);
        if (dateSession == false) return false;

        bool durationSession = InputInsert.GetTimeSessionInput(date);
        if (durationSession == false) return false;

        return true;
    }
}

