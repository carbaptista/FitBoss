using System;

namespace Application.Helpers;
public static class Utilities
{
    private static Random Rand = new ();

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Rand.Next(s.Length)]).ToArray());
    }
}
