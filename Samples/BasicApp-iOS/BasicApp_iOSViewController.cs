using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Facebook.Client;
using Facebook.Client.Controls;

namespace BasicAppiOS
{
	public partial class BasicApp_iOSViewController : UIViewController
	{
		protected LoginButton MyLoginButton;
		protected ProfilePicture MyProfilePicture;

		public BasicApp_iOSViewController () : base ("BasicApp_iOSViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			MyLoginButton = new LoginButton ();
			MyLoginButton.ParentUI = this;
			MyLoginButton.SessionStateChanged += this.OnSessionStateChanged;
			MyLoginButton.ApplicationId = "98206523058";
			MyLoginButton.FetchUserInfo = true;
			MyLoginButton.UserInfoChanged += (object sender, UserInfoChangedEventArgs e) => {
				this.CheckInButton.Hidden = false;
				this.SelectFriendsButton.Hidden = false;
				this.WelcomeLabel.Hidden = false;
				this.WelcomeLabel.Text = "Welcome, " + e.User.Name + "!";
			};
			this.LoginButtonCont.AddSubview (MyLoginButton);
			MyLoginButton.Frame = new RectangleF (0, 0, this.LoginButtonCont.Frame.Width, this.LoginButtonCont.Frame.Height);

			MyProfilePicture = new ProfilePicture ();
			this.ProfilePictureCont.AddSubview (MyProfilePicture);
			MyProfilePicture.Frame = new RectangleF (0, 0, this.ProfilePictureCont.Frame.Width, this.ProfilePictureCont.Frame.Height);

			this.CheckInButton.TouchUpInside += (object s, EventArgs e) => {
				var placePickerVC = new PlacePickerViewController (this.MyLoginButton.CurrentSession.AccessToken);
				placePickerVC.PlacePicker.SelectionMode = PickerSelectionMode.Single;
				placePickerVC.PlacePicker.SelectionChanged += (object sender, SelectionChangedEventArgs evt) => {
					if (evt.AddedItems.Count > 0) {
						GraphPlace place = (GraphPlace) evt.AddedItems[0];
						this.WelcomeLabel.Text = string.Format ("You checked in at {0}.", place.Name);
						this.NavigationController.PopViewControllerAnimated (true);
					}
				};
				this.NavigationController.PushViewController (placePickerVC, true);
			};

			this.SelectFriendsButton.TouchUpInside += (object s, EventArgs e) => {
				var friendPickerVC = new FriendPickerViewController (this.MyLoginButton.CurrentSession.AccessToken);
				friendPickerVC.FriendPicker.SelectionChanged += (object sender, SelectionChangedEventArgs evt) => {
					Console.WriteLine ("Selected");
				};
				this.NavigationController.PushViewController (friendPickerVC, true);
			};
		}

		public override void ViewWillAppear (bool animated) {
			base.ViewWillAppear (animated);
			this.NavigationController.SetNavigationBarHidden (true, animated);
		}

		public override void ViewWillDisappear (bool animated) {
			base.ViewWillDisappear (animated);
			this.NavigationController.SetNavigationBarHidden (false, animated);
		}

		private void OnSessionStateChanged(object sender, Facebook.Client.Controls.SessionStateChangedEventArgs e)
		{
			string msg = "Other?";

			switch (e.SessionState) {
			case FacebookSessionState.Closed:
				msg = "Closed.";
				this.MyProfilePicture.AccessToken = null;
				this.MyProfilePicture.ProfileId = null;
				this.WelcomeLabel.Hidden = true;
				this.CheckInButton.Hidden = true;
				this.SelectFriendsButton.Hidden = true;
				break;
			case FacebookSessionState.ClosedLoginFailed:
				msg = "Logout failed.";
				break;
			case FacebookSessionState.Created:
				msg = "Just initialized.";
				break;
			case FacebookSessionState.CreatedTokenLoaded:
				msg = "Just initialized, we have a stored session";
				break;
			case FacebookSessionState.Opened:
				msg = "Opened. Welcome!";
				this.MyProfilePicture.AccessToken = this.MyLoginButton.CurrentSession.AccessToken;
				this.MyProfilePicture.ProfileId = this.MyLoginButton.CurrentSession.FacebookId;
				break;
			case FacebookSessionState.OpenedTokenUpdated:
				msg = "Opened with stored session. Welcome!";
				break;
			case FacebookSessionState.Opening:
				msg = "Session is being opened.";
				break;
			}
			Console.Write ("SessionStateChanged: ");
			Console.WriteLine (msg);
		}
	}
}

