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
		private IList Items;
		private List<PickerItem<T>> SimpleItems;
		private List<AlphaKeyGroup<PickerItem<T>>> GroupItems;

		private Dictionary<int, object> Selection;
		private Picker<T> Picker;

		public PickerSource (Picker<T> picker)
		{
			this.Picker = picker;
			this.Selection = new Dictionary<int, object> ();
		}

		public override int NumberOfSections (UITableView tableView)
		{
			if (this.Items == null) {
				return 0;
			}

			if (this.SimpleItems != null) {
				return 1;
			}

			return this.GroupItems.Count;
		}

		public override int RowsInSection (UITableView tableView, int section)
		{
			if (this.Items == null) {
				return 0;
			}

			if (this.SimpleItems != null) {
				return this.Items.Count;
			}

			return this.GroupItems [section].Count;
		}

		public override string[] SectionIndexTitles (UITableView tableView)
		{
			if (this.GroupItems == null) {
				return null;
			}
			return this.GroupItems.ConvertAll (x => x.Key).ToArray ();
		}

		internal PickerItem<T> GetItem (NSIndexPath indexPath) {
			if (this.SimpleItems != null) {
				return this.SimpleItems [indexPath.Row];
			}
			return this.GroupItems [indexPath.Section] [indexPath.Row];
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
			PickerItem<T> item = this.GetItem (indexPath);
			this._selectedItem = item;
			item.IsSelected = false;
			this.Selection [indexPath.Row] = this._selectedItem;
			this.Picker.OnSelectionChanged (this.Picker, this.CreateSelectionChangedEvent ());
		}

		public override void RowDeselected (UITableView tableView, NSIndexPath indexPath)
		{
			PickerItem<T> item = this.GetItem (indexPath);
			this._selectedItem = item;
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
				this.SimpleItems = value as List<PickerItem<T>>;
				this.GroupItems = value as List<AlphaKeyGroup<PickerItem<T>>>;
				this.Items = value;
				this.SelectedItem = null; 
			}
		}
	}
}

