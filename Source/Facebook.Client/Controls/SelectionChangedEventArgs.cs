using System;
using System.Collections;

namespace Facebook.Client.Controls
{
    public delegate void SelectionChangedEventHandler(Object sender, SelectionChangedEventArgs e);

    public class SelectionChangedEventArgs : RoutedEventArgs
    {
		public IList AddedItems { get; private set; }
		public IList RemovedItems { get; private set; }
	
		protected virtual void InvokeEventHandler(Delegate genericHandler, Object genericTarget)
		{
	    	var deleg = (SelectionChangedEventHandler) genericHandler;
	    	deleg (genericTarget, this);
		}
    }
}