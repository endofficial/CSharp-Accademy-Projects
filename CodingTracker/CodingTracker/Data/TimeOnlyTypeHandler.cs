using Dapper;
using System.Data;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace CodingTracker.Data;
public class TimeOnlyTypeHandler : SqlMapper.TypeHandler<TimeOnly>
{
    // This method is called when you send the information to the database
    public override void SetValue(IDbDataParameter parameter, TimeOnly value)
    {
        parameter.Value = value.ToString("HH:mm", CultureInfo.InvariantCulture); // Store as string in the database
    }

    // This method is called when you retrieve the information from the database
    public override TimeOnly Parse(object value)
    {
        return TimeOnly.Parse((string)value); // Parse the string back to TimeOnly
    }
}

