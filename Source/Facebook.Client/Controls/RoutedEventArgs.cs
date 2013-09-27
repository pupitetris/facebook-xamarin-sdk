using System;

namespace Facebook.Client.Controls
{
    public class RoutedEventArgs : EventArgs
    {
		private Object _source = null;

		public bool Handled { get; set; }
		public Object OriginalSource { get; private set; }
		public RoutedEvent RoutedEvent { get; set; }
		public Object Source { 
		    get	{ return this._source; }
		    set
			{
			    this._source = value;
			    this.OnSetSource (value);
			}
		}

		public RoutedEventArgs(): base ()
		{
		}

		public RoutedEventArgs(RoutedEvent routedEvent) : base ()
		{
		    this.RoutedEvent = routedEvent;
		}

		public RoutedEventArgs(RoutedEvent routedEvent, Object source): base ()
		{
		    this.RoutedEvent = routedEvent;
		    this.Source = source;
		}
		
		protected virtual void InvokeEventHandler(Delegate genericHandler, Object genericTarget)
		{
		    var deleg = genericHandler as RoutedEventHandler;
		    deleg (genericTarget, this);
		}

		protected virtual void OnSetSource(Object source) {
		}
   	}
}
