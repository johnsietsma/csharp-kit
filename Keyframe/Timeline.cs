using System;

namespace CSharpKit {

/**
 * A Keyframed Timeline.
 * See the tests for usage.
 */ 
public class Timeline<ParamType,EntityType>
{
    public EntityType timelineEntity;  // the entity the timeline operates on.
    public Keyframe<ParamType>[] keyframes;
    public Func<ParamType,ParamType,float,ParamType> tweenFunction; // returns the result of tweening between two keyframes.
    public Action<EntityType,ParamType> applyKeyframeParameters; // aplies the tween result to the timeline entity.

    public int CurrentKeyframeIndex { 
        get {  return currIndex; }
    }

    private float elapsedTime;
    private float keyframeStartTime;
    private float keyframeLength;
    private int currIndex = -1;
    private int nextIndex = 0;

    public void Update( float dt ) {
        elapsedTime += dt;

        if( currIndex==-1 ) {
            keyframeStartTime = 0;
            keyframeLength = keyframes[1].time; // Prime the time to switch to the next keyframe
            currIndex = 0;
            nextIndex = 1;

            // Start the keyframe off
            DoTween( keyframes[currIndex].value, keyframes[nextIndex].value, 0 );
        }

        if( elapsedTime > keyframeStartTime+keyframeLength ) {
            // End the new keyframe
            DoTween( keyframes[currIndex].value, keyframes[nextIndex].value, 1 );

            // start the next keyframe
            currIndex = (currIndex+1)%keyframes.Length;
            nextIndex = (nextIndex+1)%keyframes.Length;

            // Start the new keyframe off
            DoTween( keyframes[currIndex].value, keyframes[nextIndex].value, 0 );

            keyframeStartTime += keyframes[currIndex].time;
            keyframeLength = keyframes[nextIndex].time;
        }

        Keyframe<ParamType> currKeyframe = keyframes[currIndex];
        Keyframe<ParamType> nextKeyframe = keyframes[nextIndex];

        float elapsedTimeNorm = keyframeLength==0 ? 1 : (elapsedTime-keyframeStartTime)/keyframeLength;
        
        DoTween( currKeyframe.value, nextKeyframe.value, elapsedTimeNorm );
    }

    private void DoTween( ParamType param1, ParamType param2, float t ) {
        // Do the tween
        ParamType tweenedKeyframeParams = tweenFunction( param1, param2, t );
        
        // Apply the result to the timeline entity
        applyKeyframeParameters( timelineEntity, tweenedKeyframeParams );
    }
}

}
