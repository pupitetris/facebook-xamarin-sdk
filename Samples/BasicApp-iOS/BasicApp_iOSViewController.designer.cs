// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace BasicAppiOS
{
	[Register ("BasicApp_iOSViewController")]
	partial class BasicApp_iOSViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton CheckInButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView LoginButtonCont { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView ProfilePictureCont { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton SelectFriendsButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel WelcomeLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (SelectFriendsButton != null) {
				SelectFriendsButton.Dispose ();
				SelectFriendsButton = null;
			}

			if (CheckInButton != null) {
				CheckInButton.Dispose ();
				CheckInButton = null;
			}

			if (LoginButtonCont != null) {
				LoginButtonCont.Dispose ();
				LoginButtonCont = null;
			}

			if (ProfilePictureCont != null) {
				ProfilePictureCont.Dispose ();
				ProfilePictureCont = null;
			}

			if (WelcomeLabel != null) {
				WelcomeLabel.Dispose ();
				WelcomeLabel = null;
			}
		}
	}
}
