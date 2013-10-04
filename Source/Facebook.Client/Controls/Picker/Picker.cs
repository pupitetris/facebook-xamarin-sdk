namespace Facebook.Client.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// Displays a list of selectable items with optional multi-selection.
    /// </summary>
    public abstract partial class Picker<T>
        where T : class
    {
        #region Default Property Values

        private const PickerSelectionMode DefaultSelectionMode = PickerSelectionMode.Multiple;

        #endregion Default Property Values

        /// <summary>
        /// Initializes a new instance of the Picker class.
        /// </summary>
        public Picker()
        {
			this.InitPicker ();
			this.InitUI ();
        }

		private void InitPicker () {
			Items = new ObservableCollection<T> ();
			SelectedItems = new ObservableCollection<T> ();
			this.SelectionMode = DefaultSelectionMode;
		}

        #region Events

        /// <summary>
        /// Occurs when the current selection changes.
        /// </summary>
        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        /// <summary>
        /// Occurs whenever a new item is about to be added to the list.
        /// </summary>
        public event EventHandler<DataItemRetrievedEventArgs<T>> DataItemRetrieved;

        /// <summary>
        /// Occurs when items in the the list have finished loading.
        /// </summary>
        public event EventHandler<DataReadyEventArgs<T>> LoadCompleted;

        /// <summary>
        /// Occurs whenever an error occurs while loading data.
        /// </summary>
        public event EventHandler<LoadFailedEventArgs> LoadFailed;

        #endregion Events

        #region Properties

        #region SelectionMode

        /// <summary>
        /// Gets or sets the selection behavior of the control. 
        /// </summary>

		private PickerSelectionMode _selectionMode;
		public PickerSelectionMode SelectionMode { 
			get { return this._selectionMode; } 
			set { 
				this._selectionMode = value;
				this.ClearSelection ();
			}
		}

        #endregion SelectionMode

        #region Items

        /// <summary>
        /// Gets the list of currently selected items for the FriendPicker control.
        /// </summary>
		public ObservableCollection<T> Items { get; private set; }

        #endregion Items

        #region SelectedItem

        /// <summary>
        /// Gets the currently selected item for the Picker control.
        /// </summary>
		public T SelectedItem { get; private set; }

        #endregion SelectedItem

        #region SelectedItems

        /// <summary>
        /// Gets the list of currently selected items for the FriendPicker control.
        /// </summary>
		public ObservableCollection<T> SelectedItems { get; private set; }

        #endregion SelectedItems

        #endregion Properties

        #region Implementation

        public void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SelectionMode == PickerSelectionMode.None)
            {
                return;
            }

            if (this.longListSelector == null)
            {
                return;
            }

            if (this.longListSelector.SelectedItem == null)
            {
                return;
            }

            IList<object> removedItems;
            IList<object> addedItems;

            if (this.SelectionMode == PickerSelectionMode.Single)
            {
                // Single Selection mode
				var selectedItem = this.longListSelector.SelectedItem as PickerItem<T>;
				addedItems = selectedItem.IsSelected ? new object[0] : new object[] { selectedItem };
				removedItems = selectedItem.IsSelected ? new object[] { selectedItem } : new object[0];
            }
            else
            {
                // Multiple selection mode
				removedItems = ((IList<object>)e.RemovedItems)
					.Where(item => item != null)
						.ToList();
				addedItems = ((IList<object>)e.AddedItems)
					.Where(item => item != null)
						.ToList();                
            }

			// Reset selected item to null (no selection)
			this.longListSelector.SelectedItem = null;

            foreach (var item in removedItems)
            {
                var pickerItem = (PickerItem<T>)item;
                pickerItem.IsSelected = false;          
                this.SelectedItems.Remove(pickerItem.Item);
            }

            foreach (var item in addedItems)
            {
                var pickerItem = (PickerItem<T>)item;
                pickerItem.IsSelected = true;
                this.SelectedItems.Add(pickerItem.Item);
            }

            this.SelectedItem = addedItems.Select(p => ((PickerItem<T>)p).Item).FirstOrDefault() 
                                    ?? this.SelectedItems.FirstOrDefault();

            this.SelectionChanged.RaiseEvent(
                this, 
                new SelectionChangedEventArgs(removedItems.Select(item => ((PickerItem<T>)item).Item).ToList(), 
			                              addedItems.Select(item => ((PickerItem<T>)item).Item).ToList()));
        }

        protected void ClearSelection()
        {
            this.SelectedItems.Clear();

			if (this.longListSelector != null && this.longListSelector.ItemsSource != null) {
				var source = this.longListSelector.ItemsSource as IEnumerable<PickerItem<T>>;
				if (source == null) {
					source = (this.longListSelector.ItemsSource as IEnumerable<AlphaKeyGroup<PickerItem<T>>>)
                        .SelectMany (i => i);
				}

				source.Where (f => f.IsSelected)
                    .ToList ()
                    .ForEach (i => i.IsSelected = false);

				this.longListSelector.SelectedItem = null;
			}
        }

        protected void SetDataSource(IEnumerable<T> items)
        {
            if (this.longListSelector != null)
            {
                this.longListSelector.ItemsSource = (List<T>) this.GetData(items);
            }
        }

        protected async Task RefreshData()
        {
            this.Items.Clear();
            this.SelectedItems.Clear();
			SelectedItem = null;

            try
            {
                await LoadData();
            }
            catch (Exception ex)
            {
                // TODO: review the types of exception that should be caught here
                this.LoadFailed.RaiseEvent(this, new LoadFailedEventArgs("Error loading data.", ex.Message));
            }

            this.SetDataSource(this.Items);
            this.LoadCompleted.RaiseEvent(this, new DataReadyEventArgs<T>(this.Items.ToList()));
        }

		protected void RefreshDataAsync ()
		{
			Task.Run (async delegate {
				await this.RefreshData ();
			});
		}

        protected void OnLoadCompleted(DataReadyEventArgs<T> args)
        {
            this.LoadCompleted.RaiseEvent(this, args);
        }

        protected void OnLoadFailed(LoadFailedEventArgs args)
        {
            this.LoadFailed.RaiseEvent(this, args);
        }

        protected bool OnDataItemRetrieved(DataItemRetrievedEventArgs<T> args, Func<DataItemRetrievedEventArgs<T>, bool> cancelInvocation)
        {
            return this.DataItemRetrieved.RaiseEvent(this, args, cancelInvocation);
        }

        protected abstract IList GetData(IEnumerable<T> items);

        protected abstract Task LoadData();
        
        #endregion Implementation
    }
}