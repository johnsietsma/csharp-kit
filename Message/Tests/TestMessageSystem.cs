using NUnit.Framework;
using System;
using System.Linq;

[TestFixture]
public class TestMessageSystem
{
    [Test]
    public void TestAdd_Generic()
    {
        MessageSystem.Clear();
        Assert.AreEqual( 0, MessageSystem.GetSenders<Message1,string>().Count() );
        MessageSystem.Add<Message1,string>( Listener1, Listener2 );
        Assert.AreEqual( 2, MessageSystem.GetSenders<Message1,string>().Count() );
        Assert.IsTrue( MessageSystem.GetSenders<Message1,string>().Any( l=>l.GetAction<string>()==Listener1 ) );
    }

    [Test]
    public void TestAdd_NonGeneric()
    {
        MessageSystem.Clear();
        Type messageType = typeof(Message1); 
        Assert.AreEqual( 0, MessageSystem.GetSenders<Message1,string>().Count() );
        MessageSystem.Add<string>( messageType, Listener1 );
        Assert.AreEqual( 1, MessageSystem.GetSenders<Message1,string>().Count() );
        Assert.IsTrue( MessageSystem.GetSenders<Message1,string>().Any( l=>l.GetAction<string>()==Listener1 ) );
    }

    [Test]
    public void TestAdd_Multiple()
    {
        MessageSystem.Clear();
        Assert.AreEqual( 0, MessageSystem.GetSenders<Message1,string>().Count() );
        MessageSystem.Add<Message1,string>( Listener1 , Listener2 );
        MessageSystem.Add<Message2,int>( Listener3 );
        Assert.AreEqual( 2, MessageSystem.GetSenders<Message1,string>().Count() );
        Assert.IsTrue( MessageSystem.GetSenders<Message1,string>().Any( l=>l.GetAction<string>()==Listener1 ) );
        Assert.IsTrue( MessageSystem.GetSenders<Message2,int>().Any( l=>l.GetAction<int>()==Listener3 ) );
    }

    [Test]
    public void TestRemove_Generic()
    {
        MessageSystem.Clear();
        MessageSystem.Add<Message1,string>( Listener1 );
        MessageSystem.Add<Message2,int>( Listener3 );
        int numRemoved = MessageSystem.Remove<Message1,string>( Listener1 );
        Assert.AreEqual( 1, numRemoved );
        Assert.AreEqual( 0, MessageSystem.GetSenders<Message1,string>().Count() );
        Assert.AreEqual( 1, MessageSystem.GetSenders<Message2,int>().Count() );
    }

    [Test]
    public void TestRemove_NonGeneric()
    {
        MessageSystem.Clear();
        MessageSystem.Add<Message1,string>( Listener1 );
        int numRemoved = MessageSystem.Remove<string>( typeof(Message1), Listener1 );
        Assert.AreEqual( 1, numRemoved );
        Assert.AreEqual( 0, MessageSystem.GetSenders<Message1,string>().Count() );
    }

    [Test]
    public void TestRemoveAll_Generic()
    {
        MessageSystem.Clear();
        MessageSystem.Add<Message1,string>( Listener1 );
        MessageSystem.Add<Message2,int>( Listener3 );
        bool ret = MessageSystem.RemoveAll<Message1>();
        Assert.IsTrue( ret );
        Assert.AreEqual( 0, MessageSystem.GetSenders<Message1,string>().Count() );
        Assert.AreEqual( 1, MessageSystem.GetSenders<Message2,int>().Count() );
    }    

    [Test]
    public void TestSend()
    {
        bool sent = false;
        MessageSystem.Clear();
        MessageSystem.Add<Message1,string>( o=>sent=true );
        MessageSystem.Send<Message1,string>( null );
        Assert.IsTrue( sent );
    }    


    private class Message1 : IMessage<string> {}
    private class Message2 : IMessage<int> {}

    private void Listener1( string o ) {}
    private void Listener2( string o ) {}
    private void Listener3( int o ) {}
}