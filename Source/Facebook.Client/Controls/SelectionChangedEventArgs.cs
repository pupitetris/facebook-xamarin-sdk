using System;
using System.Collections;

namespace Facebook.Client.Controls
{
    public delegate void SelectionChangedEventHandler(Object sender, SelectionChangedEventArgs e);

    public class SelectionChangedEventArgs : RoutedEventArgs
    {
		public IList AddedItems { get; private set; }
		public IList RemovedItems { get; private set; }

		public SelectionChangedEventArgs(RoutedEvent id, IList removedItems, IList addedItems): base (id)
		{
			this.RemovedItems = removedItems;
			this.AddedItems = addedItems;
		}

		public SelectionChangedEventArgs(IList removedItems, IList addedItems): base ()
		{
			this.RemovedItems = removedItems;
			this.AddedItems = addedItems;
		}

		protected override void InvokeEventHandler(Delegate genericHandler, Object genericTarget)
		{
	    	var deleg = (SelectionChangedEventHandler) genericHandler;
	    	deleg (genericTarget, this);
		}
    }
}