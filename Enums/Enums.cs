using System;
using System.Collections.Generic;
using System.Linq;

public static class Enums {
    public static T[] Get<T>()
    {
        return (T[]) Enum.GetValues( typeof(T) );
    }
}
