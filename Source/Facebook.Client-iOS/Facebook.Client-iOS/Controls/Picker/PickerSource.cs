using System;
using System.Collections;
using System.Collections.Generic;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace Facebook.Client.Controls
{
	public class PickerSource<T> : UITableViewSource
		where T: class
	{
		private List<PickerItem<T>> Items;
		private Dictionary<int, object> Selection;
		private Picker<T> Picker;

		public PickerSource (Picker<T> picker)
		{
			this.Picker = picker;
			this.Selection = new Dictionary<int, object> ();
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
			var added = new List<object> ();
			var removed = new List<object> ();

			foreach (KeyValuePair<int, object> kv in this.Selection) {
				PickerItem<T> item = (PickerItem<T>) kv.Value;
				if (item.IsSelected) {
					removed.Add (kv.Value);
				} else {
					added.Add (kv.Value);
				}
			}
			return new SelectionChangedEventArgs (removed, added);
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			this._selectedItem = this.Items [indexPath.Row];
			PickerItem<T> item = (PickerItem<T>) this._selectedItem;
			item.IsSelected = false;
			this.Selection [indexPath.Row] = this._selectedItem;
			this.Picker.OnSelectionChanged (this.Picker, this.CreateSelectionChangedEvent ());
		}

		public override void RowDeselected (UITableView tableView, NSIndexPath indexPath)
		{
			this._selectedItem = this.Items [indexPath.Row];
			PickerItem<T> item = (PickerItem<T>) this._selectedItem;
			item.IsSelected = true;
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

		public IList ItemsSource { 
			get { return this.Items; }
			set { 
				this.Items = (List<PickerItem<T>>) value; 
				this.SelectedItem = null; 
			}
		}
	}
}

