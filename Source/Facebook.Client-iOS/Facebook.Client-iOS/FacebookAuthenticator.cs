using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Auth;

namespace Facebook.Client
{
	public partial class FacebookAuthenticator
	{
		public object ParentUI { get; set; } // an UIView can be set for iPad. iPhone and iPod require an UIViewController.
		public RectangleF PresentRect { get; set; }

		private UIPopoverController Popover = null;

		public void InitUI ()
		{
			PresentRect = default(RectangleF);
		}

		public void PresentUI ()
		{
			var controller = this.ParentUI as UIViewController; // iPhone and iPod must use this.
			if (controller != null)
			{
				controller.PresentViewController (this.GetUI (), true, null);
				return;
			}

			UIView v = this.ParentUI as UIView; // only for iPad.
			UIBarButtonItem barButton = this.ParentUI as UIBarButtonItem;

			this.Popover = new UIPopoverController (this.GetUI ());

			if (barButton != null)
				this.Popover.PresentFromBarButtonItem (barButton, UIPopoverArrowDirection.Any, true);
			else
				this.Popover.PresentFromRect (this.PresentRect, v, UIPopoverArrowDirection.Any, true);
		}

		public void DismissUI () 
		{
			var controller = this.ParentUI as UIViewController;

			if (controller != null) {
				controller.DismissViewController (true, null);
				return;
			}

			this.Popover.Dismiss (true);
		}
	}
}

