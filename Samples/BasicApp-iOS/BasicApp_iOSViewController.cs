using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
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
				this.WelcomeLabel.Hidden = false;
				this.WelcomeLabel.Text = "Welcome, " + e.User.Name + "!";
			};
			this.LoginButtonCont.AddSubview (MyLoginButton);
			MyLoginButton.Frame = new RectangleF (0, 0, this.LoginButtonCont.Frame.Width, this.LoginButtonCont.Frame.Height);

			MyProfilePicture = new ProfilePicture ();
			this.ProfilePictureCont.AddSubview (MyProfilePicture);
			MyProfilePicture.Frame = new RectangleF (0, 0, this.ProfilePictureCont.Frame.Width, this.ProfilePictureCont.Frame.Height);

			this.CheckInButton.TouchUpInside += (object sender, EventArgs e) => {
				var vc = new PlacePickerViewController (this.MyLoginButton.CurrentSession.AccessToken);
				this.PresentViewController (vc, true, null);
			};
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

