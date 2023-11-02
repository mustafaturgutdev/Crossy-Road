using System;


public static class Utils
{
    private static Random random = new Random();

    public static T GetRandomEnumValue<T>()
    {
        T[] values = (T[])Enum.GetValues(typeof(T));
        return values[random.Next(0, values.Length)];
    }
}
