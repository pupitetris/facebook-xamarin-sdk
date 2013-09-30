namespace Facebook.Client.Controls
{
    using System;
    using System.Globalization;
    using System.Windows;

	public class PictureLoadedEventArgs: EventArgs
	{
		public object Image { get; private set; }

		public PictureLoadedEventArgs (object image): base () {
			this.Image = image;
		}
	}

    /// <summary>
    /// Shows the profile picture for an object such as a user, place, or event.
    /// </summary>
    public partial class ProfilePicture
    {
        #region Default Property Values

        private const string DefaultAccessToken = "";
        private const string DefaultProfileId = "";
        private const CropMode DefaultCropMode = CropMode.Original;

        #endregion Default Property Values

		#region Member variables

		private string _profileId = DefaultProfileId;
		private CropMode _cropMode = DefaultCropMode;
		private string _imageSource = "";

		#endregion Member variables

		#region Properties

        #region AccessToken
        
        /// <summary>
        /// Gets or sets the access token returned by the Facebook Login service.
        /// </summary>
		public string AccessToken { get; set; }

        #endregion AccessToken
        
        #region ProfileId

        /// <summary>
        /// The Facebook ID of the user, place or object for which a picture should be fetched and displayed.
        /// </summary>
        /// <remarks>
        /// The control displays a blank profile (silhouette) picture if this property is null or empty.
        /// </remarks>
		public string ProfileId { 
			get { return this._profileId; }
			set {
				this._profileId = value;
				this.LoadPicture ();
			} 
		}

		#endregion ProfileId

        #region CropMode
        
        /// <summary>
        /// Gets or sets the cropping treatment of the profile picture.
        /// </summary>
        public CropMode CropMode
        {
			get { return this._cropMode; }
			set {
				this._cropMode = value;
				this.CropModeChanged ();
			}
        }
        
        #endregion CropMode

		#region ImageSource

		private string ImageSource { 
			get { return this._imageSource; }
			set {
				this._imageSource = value;
				this.RefreshImage ();
			}
		}

		#endregion ImageSource

		#region Width

		private float Width
		{ 
			get { return this.GetWidth (); } 
		}

		#endregion Width

		#region Height

		private float Height 
		{ 
			get { return this.GetHeight (); } 
		}

		#endregion Height

		#endregion Properties

        #region Implementation
		
		public event EventHandler<PictureLoadedEventArgs> PictureLoaded;

		protected virtual void OnPictureLoaded (object sender, PictureLoadedEventArgs e)
		{
			EventHandler<PictureLoadedEventArgs> handler = PictureLoaded;
			if (handler != null) {
				handler (sender, e);
			}
		}

        private void LoadPicture()
        {
            string profilePictureUrl;

            if (string.IsNullOrEmpty(this.ProfileId))
            {
                profilePictureUrl = ProfilePicture.GetBlankProfilePictureUrl(this.CropMode == CropMode.Square);
            }
            else
            {
                profilePictureUrl = this.GetFacebookProfilePictureUrl();
            }

			if (this.ImageSource != profilePictureUrl) 
			{
				this.ImageSource = profilePictureUrl;
			}
        }

        private string GetFacebookProfilePictureUrl()
        {
            string profilePictureUrl;
            const string GraphApiUrl = "https://graph.facebook.com";

            if (this.CropMode == CropMode.Square)
            {
                var size = Math.Max(this.Height, this.Width);
                profilePictureUrl = string.Format(
                                        CultureInfo.InvariantCulture,
                                        "{0}/{1}/picture?width={2}&height={3}",
                                        GraphApiUrl,
                                        this.ProfileId,
                                        size,
                                        size);
            }
            else
            {
                profilePictureUrl = string.Format(
                                        CultureInfo.InvariantCulture,
                                        "{0}/{1}/picture?width={2}&height={3}",
                                        GraphApiUrl,
                                        this.ProfileId,
                                        this.Width,
                                        this.Height);
            }

            if (!string.IsNullOrEmpty(this.AccessToken))
            {
                profilePictureUrl += "&access_token=" + this.AccessToken;
            }

            return profilePictureUrl;
        }

        internal static string GetBlankProfilePictureUrl(bool isSquare)
        {
            const string BlankProfilePictureSquare = "fb_blank_profile_square.png";
            const string BlankProfilePicturePortrait = "fb_blank_profile_portrait.png";

            string imageName = isSquare ? BlankProfilePictureSquare : BlankProfilePicturePortrait;
#if NETFX_CORE
            var libraryName = typeof(ProfilePicture).GetTypeInfo().Assembly.GetName().Name;

            return string.Format(CultureInfo.InvariantCulture, "ms-appx:///{0}/Images/{1}", libraryName, imageName);
#endif
#if WINDOWS_PHONE || __MOBILE__
            return string.Format(CultureInfo.InvariantCulture, "/Images/{0}", imageName);
#endif
        }

        #endregion Implementation
    }
}
