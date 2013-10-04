using System;
using MonoTouch.UIKit;

namespace Facebook.Client.Controls
{
	public class PlacePickerViewController: PickerViewController
	{
		private string AccessToken;

		public PlacePickerViewController (string access_token): base () {
			this.AccessToken = access_token;
		}

		protected override UIView CreatePickerView ()
		{
			return (UIView)new PlacePicker ();
		}

		protected override string GetTitle ()
		{
			return "Pick a place".t ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var picker = (PlacePicker)this.PickerView;
			picker.AccessToken = this.AccessToken;
		}
	}
}

