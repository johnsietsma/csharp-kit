using System;

public static class Message {
    public static string NicifyName( string fullName )
    {
        string name = fullName;

        // strip anything in [[...]] at the end of the string
        int bracketIndex = name.IndexOf("[[");
        if( bracketIndex!=-1 )
            name = name.Remove(bracketIndex);

        // remove the namespace
        name = name.Split( new char[] {'.'} )[1];

        // remove any Message inner classes
        name = name.Replace( "+Message+", "." );

        // Fix up name
        return name.Replace( "+", "." );
    }
}
