namespace Facebook.Client.Controls
{
    internal class FriendPickerItem : PickerItem<GraphUser>
    {
        internal FriendPickerItem(FriendPicker parent, GraphUser item)
            : base(parent, item)
        {
        }

        #region Properties

        #region DisplayOrder

		public FriendPickerDisplayOrder DisplayOrder {
			get { return ((FriendPicker)Parent).DisplayOrder; }
			set { 
				if (value != this.DisplayOrder) {
					this.NotifyPropertyChanged ("DisplayName");
				}
			}
		}

        #endregion DisplayOrder

        #region DisplayName

        public string DisplayName
        {
            get
            {
                return FriendPicker.FormatDisplayName (this.Item, this.DisplayOrder);
            }
        }

        #endregion DisplayName

        #endregion Properties
    }
}
