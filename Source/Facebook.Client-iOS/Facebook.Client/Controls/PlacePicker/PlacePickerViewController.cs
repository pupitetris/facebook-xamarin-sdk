using System;
using MonoTouch.UIKit;

namespace Facebook.Client.Controls
{
	public class PlacePickerViewController: PickerViewController
	{
		public PlacePicker PlacePicker { get { return (PlacePicker) this.PickerView; } }

		public PlacePickerViewController (string access_token): base () {
			this.PlacePicker.AccessToken = access_token;
		}

		protected override UIView CreatePickerView ()
		{
			return (UIView)new PlacePicker ();
		}

		protected override string GetTitle ()
		{
			return "Pick a place".t ();
		}
	}
}

