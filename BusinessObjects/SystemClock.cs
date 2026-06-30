namespace BusinessObjects;

public static class SystemClock
{
    private static readonly TimeSpan VietnamOffset = TimeSpan.FromHours(7);

    public static DateTime Now =>
        DateTime.SpecifyKind(DateTime.UtcNow.Add(VietnamOffset), DateTimeKind.Unspecified);

    public static DateTime Today => Now.Date;
}
