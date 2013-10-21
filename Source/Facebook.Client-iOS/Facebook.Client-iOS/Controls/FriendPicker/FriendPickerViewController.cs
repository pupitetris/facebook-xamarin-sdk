using System;
using MonoTouch.UIKit;

namespace Facebook.Client.Controls
{
	public class FriendPickerViewController: PickerViewController
	{
		public FriendPicker FriendPicker { get { return (FriendPicker) this.PickerView; } }

		public FriendPickerViewController (string access_token): base () {
			this.FriendPicker.AccessToken = access_token;
		}

		protected override UIView CreatePickerView ()
		{
			return (UIView)new FriendPicker ();
		}

		protected override string GetTitle ()
		{
			return "Pick a friend".t ();
		}
	}
}

