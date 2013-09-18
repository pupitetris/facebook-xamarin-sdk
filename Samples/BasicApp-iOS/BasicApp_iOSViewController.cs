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
			LoginButtonCont.AddSubview (MyLoginButton);

			MyLoginButton.ParentUI = this;
			MyLoginButton.SessionStateChanged += this.OnSessionStateChanged;
			MyLoginButton.ApplicationId = "98206523058";
		}

		private void OnSessionStateChanged(object sender, Facebook.Client.Controls.SessionStateChangedEventArgs e)
		{
			string msg = "Other?";

			switch (e.SessionState) {
			case FacebookSessionState.Closed:
				msg = "Closed.";
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

