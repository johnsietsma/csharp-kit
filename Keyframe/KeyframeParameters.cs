using System;

/**
 * The data that represents the state of 
 */ 
public class KeyframeParameters
{
    /**
     * Returns a new KayFrameParameter that tweens between the keyframes.
     */
    public abstract KeyframeParameters TweenFunction( KeyframeParameters p1, KeyframeParameters p2, float t, out KeyframeParameters p3 );

    /**
     * 
     */
    public void Apply( T newData );
    public TimelineData<T> Create( T data );
}

