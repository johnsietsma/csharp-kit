// originally from http://rogers-neighborhood.com/wordpress/?p=92
using System;
using System.Collections.Generic;

/// <summary>
/// Class that represents the "bag" o' properties
/// </summary>
[Serializable()]
public class PropertyBag : System.Object
{
    #region Member Variables
    private Dictionary<string,Property> _propCollection = new Dictionary<string,Property>();
    #endregion

    #region Constructor(s)
    public PropertyBag() 
    {
        Owner = this;
    }

    public PropertyBag( System.Object owner )
    {
        Owner = owner;
    }
    #endregion

    /// <summary>
    /// Gets the owner of this property bag.
    /// </summary>
    public System.Object Owner { get;  private set; }

    /// <summary>
    /// The 'this' operator that reuturn a Property type.
    /// </summary>
    ///
    ///
    public Property this[string Name]
    {
        get {
            Property prop;
            if ( _propCollection.ContainsKey( Name ) ) {
                prop = ( Property )_propCollection[Name];
            }
            else {
                prop = new Property( Name, Owner );
                _propCollection.Add( Name, prop );
            }
            return prop;
        }
    }

    public T GetValue<T>() {
        return GetValue<T>( typeof(T).Name );
    }

    public T GetValue<T>( string name ) {
        if( !_propCollection.ContainsKey(name) ) {
            return default(T);
        }
        
        Property p = this[name];
        return (T)p.Value;
    }

    public override string ToString()
    {
        return string.Format("[PropertyBag: {0}]", _propCollection.Values.ToStringJoin() );
    }
}