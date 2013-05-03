using System;
using NUnit.Framework;

using CSharpKit;

[TestFixture]
public class TimelineTests
{
    private class KeyframeParams {
        public int a;
        public float b;
    }

    private class KeyframeObject {
        public string msg;
    }

    [Test]
    public void TestTimelineData() {
        Keyframe<KeyframeParams>[] keyframes = new Keyframe<KeyframeParams>[] {
            new Keyframe<KeyframeParams>() { time=0, value=new KeyframeParams() { a=0,b=10 } },
            new Keyframe<KeyframeParams>() { time=0.5f, value=new KeyframeParams() { a=1,b=5 } },
            new Keyframe<KeyframeParams>() { time=0.1f, value=new KeyframeParams() { a=3,b=1 } }
        };

        KeyframeObject keyframeObject = new KeyframeObject();

        Func<int,float,string> formatMsg = (a,b)=>string.Format( "{0}-{1}", a, b );

        Timeline<KeyframeParams,KeyframeObject> timeline = new Timeline<KeyframeParams, KeyframeObject>() {
            timelineEntity=keyframeObject,
            keyframes=keyframes,
            tweenFunction=(p1,p2,t)=>{
                return new KeyframeParams() {
                    a=(int)Lerp(p1.a,p2.a,t),
                    b=Lerp(p1.b,p2.b,t)
                };
            },
            applyKeyframeParameters=(kfo,p)=>kfo.msg = formatMsg(p.a,p.b)
        };

        Assert.AreEqual( -1, timeline.CurrentKeyframeIndex );

        timeline.Update(0);
        Assert.AreEqual( 0, timeline.CurrentKeyframeIndex );
        Assert.AreEqual( formatMsg(0,10), keyframeObject.msg );

        timeline.Update(0.1f);
        Assert.AreEqual( 0, timeline.CurrentKeyframeIndex );
        Assert.AreEqual( formatMsg(0,9f), keyframeObject.msg );

        timeline.Update(0.4f);
        Assert.AreEqual( 0, timeline.CurrentKeyframeIndex );
        Assert.AreEqual( formatMsg(1,5f), keyframeObject.msg );

        timeline.Update(0.1f);
        Assert.AreEqual( 1, timeline.CurrentKeyframeIndex );
        Assert.AreEqual( formatMsg(3,0.999999f), keyframeObject.msg );

        timeline.Update(0.1f);
        Assert.AreEqual( 2, timeline.CurrentKeyframeIndex );
        Assert.AreEqual( formatMsg(0,10f), keyframeObject.msg ); // this frame has no length

        timeline.Update(0.1f);
        Assert.AreEqual( 0, timeline.CurrentKeyframeIndex ); // loop around
        Assert.AreEqual( formatMsg(0,8f), keyframeObject.msg );
    }

    private static float Lerp( float f1, float f2, float t ) {
        return f1+(f2-f1)*t;
    }
}

