using System;

/// <summary>
/// A single property
/// </summary>
[Serializable()]
public class Property : System.Object
{
    #region Member Variables
    private string _propName;
    private System.Object _value;
    private System.Object _owner;

    #endregion

    #region Constructor(s)
    public Property( string name, System.Object owner )
    {
        _propName = name;
        _owner = owner;
        _value = null;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets/Sets the value of this Property
    /// </summary>
    public System.Object Value
    {
        get {
            lock ( this ) {
                System.Reflection.PropertyInfo propInfo = Owner.GetType().GetProperty( Name );
                if ( null != propInfo ) {
                    return propInfo.GetValue( Owner, new object[] { } );
                }
                else {
                    return _value;
                }
            }
        }
        set {
            lock ( this ) {
                System.Reflection.PropertyInfo propInfo = Owner.GetType().GetProperty( Name );

                if ( null != propInfo ) {
                    propInfo.SetValue( Owner, value, new object[] { } );
                }
                else {
                    _value = value;
                }
            }
        }
    }

    /// <summary>
    /// Gets the owner of this property
    /// </summary>
    public System.Object Owner
    {
        get {
            return _owner;
        }
    }

    /// <summary>
    /// Gets the friendly name of the Property
    /// </summary>
    public string Name
    {
        get {
            return _propName;
        }
    }

    #endregion

    public override string ToString() 
    {
        return string.Format("[Property {0}:{1}]", Name, Value);
    }
}
