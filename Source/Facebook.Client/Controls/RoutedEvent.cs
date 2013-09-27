using System;

namespace Facebook.Client.Controls
{
    public delegate void RoutedEventHandler(Object sender, RoutedEventArgs e);

    public class RoutedEvent : System.Object
    {
		public string Name { get; private set; }
		public RoutingStrategy RoutingStrategy { get; private set; }
		public Type HandlerType	{ get; private set; }
		public Type OwnerType { get; private set; }

		public RoutedEvent (string name, RoutingStrategy routingStrategy, Type handlerType, Type ownerType) : base ()
		{
		    this.Name = name;
		    this.RoutingStrategy = routingStrategy;
		    this.HandlerType = handlerType;
		    this.OwnerType = ownerType;
		}

		public RoutedEvent AddOwner (Type ownerType)
		{
		    return new RoutedEvent (this.Name, this.RoutingStrategy, this.HandlerType, ownerType);
		}

		public override string ToString ()
		{
		    // TODO: code string representation.
			return base.ToString ();
		}
    }
}