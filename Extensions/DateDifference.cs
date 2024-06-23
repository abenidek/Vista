using System;

namespace Vista;

public static class DateDifference
{
    public static string FormatDateDifference(this TimeSpan d)
    {
        return true switch
        {
            var _ when d.Minutes < 1 => $"{(int)d.TotalSeconds}s ago",
            var _ when d.Hours < 1 => $"{(int)d.TotalMinutes}m ago",
            var _ when d.Days < 1 => $"{(int)d.TotalHours}h ago",
            var _ when d.Days < 30 => $"{(int)d.TotalDays}d ago",
            var _ when d.Days < 365 => $"{(int)(d.TotalDays / 12)}m ago",
            _ => $"{(int)(d.TotalDays / 365)}y ago"
        };
    }
}
