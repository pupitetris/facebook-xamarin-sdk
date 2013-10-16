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

    /// <summary>
    /// Shows a user interface that can be used to select Facebook places.
    /// </summary>
    public partial class PlacePicker : Picker<GraphPlace>
    {
        #region Default Property Values

        private static readonly string DefaultAccessToken = "";
        private static readonly string DefaultDisplayFields = "id,name,location,category,picture,were_here_count";
        private static readonly bool DefaultDisplayProfilePictures = true;
        private static readonly string DefaultSearchText = "";
        private static readonly int DefaultRadiusInMeters = 1000;
        //private static readonly int DefaultResultsLimit = 100;
        private static readonly bool DefaultTrackLocation = false;
        private static readonly double DefaultLatitude = 51.494338;
        private static readonly double DefaultLongitude = -0.176759;
        private static readonly double DefaultMovementThreshold = 100.0;
		private static readonly Size DefaultPictureSize = new Size(50, 50);

        #endregion Default Property Values

        #region Member variables

        private Geolocator geoLocator;
        private CancellationTokenSource cancelGeopositionOperation;

        #endregion Member variables

        /// <summary>
        /// Initializes a new instance of the PlacePicker class.
        /// </summary>
        public PlacePicker() : base ()
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

        #region DisplayFields

        /// <summary>
        /// Gets or sets additional fields to fetch when requesting place data.
        /// </summary>
        /// <remarks>
        /// By default, the following data is retrieved for each place: TODO: to be completed.
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

        #region SearchText

        /// <summary>
        /// Gets or sets the search text to filter place data (e.g. Restaurant, Supermarket, Sports, etc...)
        /// </summary>
		private string _searchText = DefaultSearchText;
		public string  SearchText { 
			get { return _searchText; }
			set {
				this._searchText = value;
				this.RefreshDataAsync ();
			}
		}

        #endregion SearchText

        #region RadiusInMeters

        /// <summary>
        /// Gets or sets the distance in meters from the search location for which results are returned.
        /// </summary>
		private int _radiusInMeters = DefaultRadiusInMeters;
		public int RadiusInMeters { 
			get { return _radiusInMeters; }
			set {
				this._radiusInMeters = value;
				this.RefreshDataAsync ();
			}
		}

        #endregion RadiusInMeters

        #region Latitude

        /// <summary>
        /// Gets or sets the latitude of the location around which to retrieve place data.
        /// </summary>
		private double _latitude = DefaultLatitude;
        public double Latitude {
			get { return this._latitude; }
			set {
				this._latitude = value;
				this.RefreshDataAsync ();
			}
		}
        
        #endregion Latitude

        #region Longitude

        /// <summary>
        /// Gets or sets the longitude of the location around which to retrieve place data.
        /// </summary>
		private double _longitude = DefaultLongitude;
		public double Longitude {
			get { return this._longitude; }
			set {
				this._longitude = value;
				this.RefreshDataAsync ();
			}
		}
        
        #endregion Longitude

        #region TrackLocation

        /// <summary>
        /// Specifies whether to track the current location for searches.
        /// </summary>
		private bool _trackLocation = DefaultTrackLocation;
        public bool TrackLocation
        {
			get { return this._trackLocation; }
            set {
				this._trackLocation = value;

	            if ((this.cancelGeopositionOperation != null) && (!this.cancelGeopositionOperation.IsCancellationRequested))
            	{
                	this.cancelGeopositionOperation.Cancel();
            	}

	            if (value)
	            {
	                if (this.geoLocator == null)
	                {
	                    this.geoLocator = new Geolocator();
	                    this.geoLocator.MovementThreshold = DefaultMovementThreshold;
	                    this.geoLocator.DesiredAccuracy = PositionAccuracy.High;
	                }
				
	                this.geoLocator.PositionChanged += this.OnPositionChanged;
	            }
	            else if (this.geoLocator != null)
	            {
	                this.geoLocator.PositionChanged -= this.OnPositionChanged;
	            }

				this.RefreshDataAsync ();
	        }
		}

        #endregion TrackLocation

        #endregion Properties

        #region Implementation

        private async void OnPositionChanged(object sender, PositionChangedEventArgs args)
        {
            await this.RefreshData();
        }

        protected override async Task LoadData()
        {
            if (!string.IsNullOrEmpty(this.AccessToken))
            {
                var currentLocation = this.TrackLocation ? await this.GetCurrentLocation() : new LocationCoordinate(this.Latitude, this.Longitude);
                FacebookClient facebookClient = new FacebookClient(this.AccessToken);

				var parameters = new Dictionary<string, object> ();
				parameters ["type"] = "place";
				parameters ["center"] = currentLocation.ToString ();
				parameters ["distance"] = this.RadiusInMeters;
				parameters ["fields"] = this.DisplayFields;
				if (!string.IsNullOrWhiteSpace (this.SearchText)) {
					parameters ["q"] = this.SearchText;
				}

				IDictionary<string, object> placesTaskResult = null;
				try {
					placesTaskResult = (IDictionary<string, object>) await facebookClient.GetTaskAsync("/search", parameters);
				} catch (Exception e) {
					throw new Exception (e.Message, e);
				}

				var data = (IEnumerable<object>)placesTaskResult ["data"];
                foreach (var item in data)
                {
                    var place = new GraphPlace(item);
                    if (this.OnDataItemRetrieved(new DataItemRetrievedEventArgs<GraphPlace>(place), e => e.Exclude))
                    {
						Console.Write (place.ToString ());
						Console.Write (" ");
						Console.WriteLine (place.Name);
                        this.Items.Add(place);
                    }
                }
            }
        }

        private async Task<LocationCoordinate> GetCurrentLocation()
        {
            try
            {
                if ((this.cancelGeopositionOperation != null) && (!this.cancelGeopositionOperation.IsCancellationRequested))
                {
                    this.cancelGeopositionOperation.Cancel();
                }

                this.cancelGeopositionOperation = new CancellationTokenSource(3000);
                var position = await this.geoLocator.GetGeopositionAsync(10000, this.cancelGeopositionOperation.Token);
                return new LocationCoordinate(position.Coordinate.Latitude, position.Coordinate.Longitude);
            }
            catch (System.UnauthorizedAccessException)
            {
                this.OnLoadFailed(new LoadFailedEventArgs("Error retrieving current location.", "Location is disabled."));
            }
            catch (TaskCanceledException)
            {
                this.OnLoadFailed(new LoadFailedEventArgs("Error retrieving current location.", "Task was cancelled."));
            }
            catch (Exception ex)
            {
                this.OnLoadFailed(new LoadFailedEventArgs("Error retrieving current location.", ex.Message));
            }
            finally
            {
                this.cancelGeopositionOperation = null;
            }

            // default location
            return new LocationCoordinate(DefaultLatitude, DefaultLongitude);
        }

        protected override IList GetData(IEnumerable<GraphPlace> items)
        {
			return items.Select(item => new PickerItem<GraphPlace>(this, item)).ToList();
        }

        #endregion Implementation
    }
}
