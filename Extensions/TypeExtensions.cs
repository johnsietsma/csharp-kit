using System;
using System.Linq;

public static class TypeExtensions
{
    public static bool ImplementsGeneric( this Type type, Type genericType )
    {
        return type.GetInterfaces().Any( i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType );
    }
}