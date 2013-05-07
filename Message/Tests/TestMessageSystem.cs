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
        Assert.AreEqual( 0, MessageSystem.GetSenders<Message1,object>().Count() );
        MessageSystem.Add<Message1,object>( Listener1 );
        Assert.AreEqual( 1, MessageSystem.GetSenders<Message1,object>().Count() );
        Assert.IsTrue( MessageSystem.GetSenders<Message1,object>().Any( l=>l.Action==Listener1 ) );
    }

    [Test]
    public void TestAdd_NonGeneric()
    {
        MessageSystem.Clear();
        Type messageType = typeof(Message1); 
        Assert.AreEqual( 0, MessageSystem.GetSenders<Message1,object>().Count() );
        MessageSystem.Add( messageType, Listener1 );
        Assert.AreEqual( 1, MessageSystem.GetSenders<Message1,object>().Count() );
        Assert.IsTrue( MessageSystem.GetSenders<Message1,object>().Any( l=>l.Action==Listener1 ) );
    }

    [Test]
    public void TestAdd_Multiple()
    {
        MessageSystem.Clear();
        Assert.AreEqual( 0, MessageSystem.GetSenders<Message1,object>().Count() );
        MessageSystem.Add<Message1,object>( Listener1 , Listener2 );
        MessageSystem.Add<Message2,object>( Listener1 , Listener2 );
        Assert.AreEqual( 2, MessageSystem.GetSenders<Message1,object>().Count() );
        Assert.IsTrue( MessageSystem.GetSenders<Message1,object>().Any( l=>l.Action==Listener1 ) );
        Assert.IsTrue( MessageSystem.GetSenders<Message1,object>().Any( l=>l.Action==Listener2 ) );
    }

    [Test]
    public void TestRemove_Generic()
    {
        MessageSystem.Clear();
        MessageSystem.Add<Message1,object>( Listener1 );
        MessageSystem.Add<Message2,object>( Listener1 );
        int numRemoved = MessageSystem.Remove<Message1,object>( Listener1 );
        Assert.AreEqual( 1, numRemoved );
        Assert.AreEqual( 0, MessageSystem.GetSenders<Message1,object>().Count() );
        Assert.AreEqual( 1, MessageSystem.GetSenders<Message2,object>().Count() );
    }

    [Test]
    public void TestRemove_NonGeneric()
    {
        MessageSystem.Clear();
        MessageSystem.Add<Message1,object>( Listener1 );
        int numRemoved = MessageSystem.Remove( typeof(Message1), Listener1 );
        Assert.AreEqual( 1, numRemoved );
        Assert.AreEqual( 0, MessageSystem.GetSenders<Message1,object>().Count() );
    }

    [Test]
    public void TestRemoveAll_Generic()
    {
        MessageSystem.Clear();
        MessageSystem.Add<Message1,object>( Listener1 );
        MessageSystem.Add<Message2,object>( Listener1 );
        bool ret = MessageSystem.RemoveAll<Message1,object>();
        Assert.IsTrue( ret );
        Assert.AreEqual( 0, MessageSystem.GetSenders<Message1,object>().Count() );
        Assert.AreEqual( 1, MessageSystem.GetSenders<Message2,object>().Count() );
    }    


    private class Message1 : IMessage<object> {}
    private class Message2 : IMessage<object> {}

    private void Listener1( object o ) {}
    private void Listener2( object o ) {}
}