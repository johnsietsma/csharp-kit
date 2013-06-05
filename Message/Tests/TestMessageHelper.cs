using NUnit.Framework;

[TestFixture]
public class TestMessageHelper
{
    [Test]
    public void TestMultipleFilters()
    {
        string message = "message";

        MessageHelper mh = new MessageHelper();
        mh.AddEqualityFilter( message ).Add<Message1, object>( m => Assert.AreEqual( m, message ) );
        mh.AddInequalityFilter( message ).Add<Message1, object>( m => Assert.AreNotEqual( m, message ) );

        MessageSystem.Send<Message1,object>( message );
        MessageSystem.Send<Message1,object>( "some other message" );
    }

    [Test]
    public void TestAdd()
    {
        MessageSystem.Clear();

        bool decorated = false;
        MessageHelper.DecoratorDelegate dd = ( m )=> {decorated=true; return m;};

        MessageHelper mh = new MessageHelper();
        mh.AddDecorator( dd );
        mh.Add<Message1,object>( ( a )=> {} );

        MessageSystem.Send<Message1,object>( new object() );

        Assert.IsTrue( decorated );
    }

    [Test]
    public void TestDispose()
    {
        MessageSystem.Clear();

        using( MessageHelper mh = new MessageHelper() ) {

            mh.Add<Message1,object>( ( a )=> {} );
            MessageSystem.Add<Message1,object>( a=> {} );

            Assert.AreEqual( 2, MessageSystem.GetSenders<Message1,object>().Count() );
        }

        Assert.AreEqual( 1, MessageSystem.GetSenders<Message1,object>().Count() );

    }

    [Test]
    public void TestFluent()
    {
        object o = new object();
        bool messageSent = false;
        MessageLog log = new MessageLog( 10 );
        new MessageHelper().AddEqualityFilter( o ).AddLogging( log ).Add<Message1,object>( (s)=>messageSent=true );

        Assert.IsFalse( messageSent );
        MessageSystem.Send<Message1,object>( o );
        Assert.IsTrue( messageSent );

        Assert.AreEqual( 1, log.Size );
    }

    class Message1 : IMessage<object> {}
}