using System;

namespace CSharpKit {

/**
 * Represents a Keyframe at a particular point in time.
 */
public class Keyframe<T>
{
    public float time;
    public T value;
}

}