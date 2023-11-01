namespace Itmo.Dev.Asap.Google.Common;

public static class CommonMapper
{
    public static Guid ToGuid(this string value)
    {
        return Guid.Parse(value);
    }
}