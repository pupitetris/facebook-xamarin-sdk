using System;
using System.Collections.Generic;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace Facebook.Client.Controls
{
	public class PickerSource<T> : UITableViewSource
		where T: class
	{
		private List<T> Items;
		private Dictionary<int, object> Selection;
		private Picker<T> Picker;

		public PickerSource (Picker<T> picker, IList<T> items = null)
		{
			this.Picker = picker;
			this.Items = (List<T>) items;

			if (items != null && items.Count > 0) {
				this.Selection = new Dictionary<int, object> (items.Count);
			} else {
				this.Selection = new Dictionary<int, object> ();
			}
		}

		public override int RowsInSection (UITableView tableView, int section)
		{
			if (this.Items == null) {
				return 0;
			}
			return this.Items.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			return this.Picker.GetCell (tableView, indexPath);
		}

		private SelectionChangedEventArgs CreateSelectionChangedEvent ()
		{
			var added = new List<PickerItem<T>> ();
			var removed = new List<PickerItem<T>> ();
			var eventArgs = new SelectionChangedEventArgs (removed, added);

			foreach (KeyValuePair<int, object> kv in this.Selection) {
				PickerItem<T> item = (PickerItem<T>) kv.Value;
				if (item.IsSelected) {
					added.Add (item);
				} else {
					removed.Add (item);
				}
			}
			return eventArgs;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			this._selectedItem = this.Items [indexPath.Row];
			PickerItem<T> item = (PickerItem<T>) this._selectedItem;
			item.IsSelected = true;
			this.Selection [indexPath.Row] = this._selectedItem;
			this.Picker.OnSelectionChanged (this.Picker, this.CreateSelectionChangedEvent ());
		}

		public override void RowDeselected (UITableView tableView, NSIndexPath indexPath)
		{
			this._selectedItem = this.Items [indexPath.Row];
			PickerItem<T> item = (PickerItem<T>) this._selectedItem;
			item.IsSelected = false;
			this.Selection [indexPath.Row] = this._selectedItem;
			this.Picker.OnSelectionChanged (this.Picker, this.CreateSelectionChangedEvent ());
		}

		// Interfaces required by Picker

		private object _selectedItem;
		// TODO: the real thing.
		public object SelectedItem { 
			get { return this._selectedItem; }
			set {
				if (value == null) {
					this.Selection.Clear ();
				}
				this._selectedItem = value; 
			} 
		}

		public List<T> ItemsSource { 
			get { return this.Items; }
			set { this.Items = value; this.SelectedItem = null; }
		}
	}
}

