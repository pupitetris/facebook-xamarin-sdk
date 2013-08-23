namespace Facebook.Client.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a button control that can log in or log out the user when clicked.
    /// </summary>
    /// <remarks>
    /// The LoginButton control keeps track of the authentication status and shows an appropriate label 
    /// that reflects whether the user is currently authenticated. When a user logs in, it can automatically 
    /// retrieve their basic information.
    /// </remarks>
    public partial class LoginButton
    {
        #region Default Property Values

        private const string DefaultApplicationId = "";
        private const Audience DefaultDefaultAudience = Audience.None;
        private const string DefaultPermissions = "";
        private const bool DefaultFetchUserInfo = true;
        private const FacebookSession DefaultCurrentSession = null;
        private const FacebookSession DefaultCurrentUser = null;
        private static readonly float DefaultCornerRadius = 0.0;

        #endregion Default Property Values

        #region Member variables

        private FacebookSessionClient facebookSessionClient;
		private FacebookSession _currentSession;

        #endregion Member variables

        /// <summary>
        /// Initializes a new instance of the LoginButton class. 
        /// </summary>
        public LoginButton()
        {
        }

        #region Events

        /// <summary>
        /// Occurs whenever the status of the session associated with this control changes.
        /// </summary>
        public event EventHandler<SessionStateChangedEventArgs> SessionStateChanged;

        /// <summary>
        /// Occurs whenever a communication or authentication error occurs while logging in.
        /// </summary>
        public event EventHandler<AuthenticationErrorEventArgs> AuthenticationError;

        /// <summary>
        /// Occurs whenever the current user changes.
        /// </summary>
        /// <remarks>
        /// To retrieve the current user information, the FetchUserInfo property must be set to true.
        /// </remarks>
        public event EventHandler<UserInfoChangedEventArgs> UserInfoChanged;

        #endregion Events

        #region Properties

        #region ApplicationId

        /// <summary>
        /// Gets or sets the application ID to be used to open the session.
        /// </summary>
	public string ApplicationId
	{
	    get;
	    set { this.facebookSessionClient = string.IsNullOrWhiteSpace(value) ? null : new FacebookSessionClient(value); }
	}

        #endregion ApplicationId

        #region DefaultAudience

        /// <summary>
        /// Gets or sets the default audience to use, if publish permissions are requested at login time.
        /// </summary>
        /// <remarks>
        /// Certain operations such as publishing a status or publishing a photo require an audience. When the user grants an application 
        /// permission to perform a publish operation, a default audience is selected as the publication ceiling for the application. This 
        /// enumerated value allows the application to select which audience to ask the user to grant publish permission for.
        /// </remarks>
        public Audience DefaultAudience;

        #endregion DefaultAudience

        #region Permissions

        /// <summary>
        /// Gets or sets the permissions to request.
        /// </summary>
        public string Permissions;
        
        #endregion Permissions

        #region FetchUserInfo

        /// <summary>
        /// Controls whether the user information is fetched when the session is opened. Default is true.
        /// </summary>
        public bool FetchUserInfo;
        
        #endregion FetchUserInfo

        #region CurrentSession

        /// <summary>
        /// Gets the current active session.
        /// </summary>
        public FacebookSession CurrentSession
        {
            get { return this._currentSession; }
            private set { this._currentSession = value; this.UpdateSession(); }
        }

        #endregion CurrentSession

        #region CurrentUser

        /// <summary>
        /// Gets the current logged in user.
        /// </summary>
        public GraphUser CurrentUser;

        #endregion CurrentUser

        #region CornerRadius

        /// <summary>
        /// Gets or sets a value that represents the degree to which the corners of a Border are rounded. 
        /// </summary>
        public float CornerRadius;

        #endregion CornerRadius

        #endregion Properties

        #region Methods
        
        /// <summary>
        /// Requests new permissions for the current Facebook session.
        /// </summary>
        /// <param name="permissions">The permissions to request.</param>
        public async Task RequestNewPermissions(string permissions)
        {
            await this.LogIn(permissions);
        }

        #endregion Methods

        #region Implementation

        private async void OnLoginButtonClicked(object sender, RoutedEventArgs e)
        {
            if (this.CurrentSession == null)
            {
                await this.LogIn();
            }
            else
            {
                this.LogOut();
            }
        }

        private async Task LogIn(string permissions = null)
        {
            try
            {
                this.SessionStateChanged.RaiseEvent(this, new SessionStateChangedEventArgs(FacebookSessionState.Opening));

                // TODO: using Permissions for the time being until we decide how 
                // to handle separate ReadPermissions and PublishPermissions
                var session = await this.facebookSessionClient.LoginAsync(permissions ?? this.Permissions);

                // initialize current session
                this.CurrentSession = session;
                this.SessionStateChanged.RaiseEvent(this, new SessionStateChangedEventArgs(FacebookSessionState.Opened));

                // retrieve information about the current user
                if (this.FetchUserInfo)
                {
                    FacebookClient client = new FacebookClient(session.AccessToken);

                    dynamic result = await client.GetTaskAsync("me");
                    this.CurrentUser = new GraphUser(result);
                    var userInfo = new UserInfoChangedEventArgs(this.CurrentUser);
                    this.UserInfoChanged.RaiseEvent(this, userInfo);
                }
            }
            catch (ArgumentNullException error)
            {
                // TODO: remove when bug in SDK is fixed (the bug happens when you cancel the facebook login dialog)
                var authenticationErrorEventArgs =
                    new AuthenticationErrorEventArgs("Login failure.", error.Message);

                this.AuthenticationError.RaiseEvent(this, authenticationErrorEventArgs);
            }
            catch (InvalidOperationException error)
            {
                // TODO: need to obtain richer information than a generic InvalidOperationException
                var authenticationErrorEventArgs =
                    new AuthenticationErrorEventArgs("Login failure.", error.Message);

                this.AuthenticationError.RaiseEvent(this, authenticationErrorEventArgs);
            }
            catch (FacebookOAuthException error)
            {
                var authenticationErrorEventArgs =
                    new AuthenticationErrorEventArgs("Login failure.", error.Message);

                this.AuthenticationError.RaiseEvent(this, authenticationErrorEventArgs);
            }
        }

        private void LogOut()
        {
            this.facebookSessionClient.Logout();
            this.CurrentSession = null;
            this.CurrentUser = null;
            this.SessionStateChanged.RaiseEvent(this, new SessionStateChangedEventArgs(FacebookSessionState.Closed));
        }

        private void UpdateSession()
        {
            this.UpdateButtonCaption();
        }

        #endregion Implementation
    }
}
