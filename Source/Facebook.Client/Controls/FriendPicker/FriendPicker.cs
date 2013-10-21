namespace Facebook.Client.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Drawing;
	using System.Globalization;

    /// <summary>
    /// Shows a user interface that can be used to select Facebook friends.
    /// </summary>
    public partial class FriendPicker : Picker<GraphUser>
    {
        #region Default Property Values

        private static readonly string DefaultAccessToken = "";
		private static readonly string DefaultProfileId = "me";
        private static readonly string DefaultDisplayFields = "id,name,first_name,middle_name,last_name,picture";
        private const FriendPickerDisplayOrder DefaultDisplayOrder = FriendPickerDisplayOrder.DisplayLastNameFirst;
        private const FriendPickerSortOrder DefaultSortOrder = FriendPickerSortOrder.SortByFirstName;
        private static readonly bool DefaultDisplayProfilePictures = true;
		private static readonly Size DefaultPictureSize = new Size(50, 50);

        #endregion Default Property Values

        /// <summary>
        /// Initializes a new instance of the FriendPicker class.
        /// </summary>
        public FriendPicker() : base ()
        {
        }

        #region Properties

        #region AccessToken

        /// <summary>
        /// Gets or sets the access token returned by the Facebook Login service.
        /// </summary>
		private string _accessToken = DefaultAccessToken;
		public string AccessToken { 
			get { return _accessToken; }
			set {
				this._accessToken = value;
				this.RefreshDataAsync ();
			}
		}

        #endregion AccessToken

        #region ProfileId

        /// <summary>
        /// The profile ID of the user for which to retrieve a list of friends.
        /// </summary>
		private string _profileId = DefaultProfileId;
		public string ProfileId { 
			get { return _profileId; }
			set {
				this._profileId = value;
				this.RefreshDataAsync ();
			}
		}

        #endregion ProfileId

	#region DisplayOrder

        /// <summary>
        /// Controls whether to display first name or last name first.
        /// </summary>
		public FriendPickerDisplayOrder DisplayOrder { get; set; }

	#endregion DisplayOrder

        #region SortOrder

        /// <summary>
        /// Controls the order in which friends are listed in the friend picker.
        /// </summary>
		public FriendPickerSortOrder SortOrder { get; set; }

        #endregion SortOrder

        #region DisplayFields

        /// <summary>
        /// Gets or sets additional fields to fetch when requesting friend data.
        /// </summary>
        /// <remarks>
        /// By default, the following data is retrieved for each friend: TODO: to be completed.
        /// </remarks>
		private string _displayFields = DefaultDisplayFields;
		public string DisplayFields {
			get { return _displayFields; } 
			set { _displayFields = value; }
		}

        #endregion DisplayFields

        #region DisplayProfilePictures

        /// <summary>
        /// Specifies whether profile pictures are displayed.
        /// </summary>
		private bool _displayProfilePictures = DefaultDisplayProfilePictures;
        public bool DisplayProfilePictures
        {
            get { return _displayProfilePictures; }
            set { _displayProfilePictures = value; }
        }

        #endregion DisplayProfilePictures

        #region PictureSize
        /// <summary>
        /// Gets or sets the size of the profile pictures displayed.
        /// </summary>
		private Size _pictureSize = DefaultPictureSize;
        public Size PictureSize
        {
            get { return _pictureSize; }
            set { _pictureSize = value; }
        }

        #endregion PictureSize

        #endregion Properties

        #region Implementation

        protected override async Task LoadData()
        {
            if (!string.IsNullOrEmpty(this.AccessToken))
            {
                FacebookClient facebookClient = new FacebookClient(this.AccessToken);

                string graphUrl = string.Format(
                                        CultureInfo.InvariantCulture,
                                        "/{0}/friends?fields={1}",
                                        this.ProfileId,
                                        this.DisplayFields);

				IDictionary<string, object> friendsTaskResult = null;
				try {
					friendsTaskResult = (IDictionary<string, object>) await facebookClient.GetTaskAsync(graphUrl);
				} catch (Exception e) {
					throw new Exception (e.Message, e);
				}

				var data = (IEnumerable<object>)friendsTaskResult ["data"];
                foreach (var friend in data)
                {
                    var user = new GraphUser(friend);
                    if (this.OnDataItemRetrieved(new DataItemRetrievedEventArgs<GraphUser>(user), e => e.Exclude))
                    {
                        this.Items.Add(user);
                    }
                }
            }
        }

        internal static string FormatDisplayName(GraphUser user, FriendPickerDisplayOrder displayOrder)
        {
            bool hasFirstName = !string.IsNullOrWhiteSpace(user.FirstName);
            bool hasLastName = !string.IsNullOrWhiteSpace(user.LastName);
            bool hasFirstNameAndLastName = hasFirstName && hasLastName;

            if (hasFirstName || hasLastName)
            {
                switch (displayOrder)
                {
                    case FriendPickerDisplayOrder.DisplayFirstNameFirst:
                        return user.FirstName + (hasFirstNameAndLastName ? " " : null) + user.LastName;
                    case FriendPickerDisplayOrder.DisplayLastNameFirst:
                        return user.LastName + (hasFirstNameAndLastName ? ", " : null) + user.FirstName;
                }
            }

            return user.Name;
        }

        protected override IList GetData(IEnumerable<GraphUser> items)
        {
            return AlphaKeyGroup<PickerItem<GraphUser>>.CreateGroups(
                            items.Select(item => new FriendPickerItem(this, item)),
                            System.Globalization.CultureInfo.CurrentUICulture,
                            u => u.Item.Name,
                            true);
        }

        #endregion Implementation
    }
}
