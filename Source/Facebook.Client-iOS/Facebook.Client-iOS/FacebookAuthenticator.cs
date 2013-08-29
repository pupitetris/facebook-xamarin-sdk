using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Auth;

namespace Facebook.Client
{
	public partial class FacebookAuthenticator
	{
		public object ParentUI { get; set; }
		public RectangleF PresentRect { get; set; }

		private UIPopoverController Popover = null;

		public void InitUI ()
		{
			PresentRect = default(RectangleF);
		}

		public void PresentUI ()
		{
			var controller = this.ParentUI as UIViewController;
			if (controller != null)
			{
				controller.PresentModalViewController (this.GetUI (), true);
				return;
			}

			UIView v = this.ParentUI as UIView;
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
				controller.DismissModalViewControllerAnimated (true);
				return;
			}

			this.Popover.Dismiss (true);
		}
	}
}

