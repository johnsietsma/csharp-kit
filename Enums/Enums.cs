using System;
using System.Collections.Generic;
using System.Linq;

public static class Enums {
    public static IEnumerable<T> Get<T>()
    {
        List<T> values = ((T[])Enum.GetValues( typeof(T) )).ToList();
        return values;
    }
}
