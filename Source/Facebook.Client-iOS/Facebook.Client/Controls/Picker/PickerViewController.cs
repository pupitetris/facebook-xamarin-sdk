using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace Facebook.Client.Controls
{
	public abstract class PickerViewController: UIViewController
	{
		protected UIView PickerView;

		public PickerViewController (): base ()
		{
		}

		protected abstract UIView CreatePickerView ();

		protected virtual string GetTitle ()
		{
			return "Pick an item".t ();
		}

		public override void ViewDidLoad () 
		{
			base.ViewDidLoad ();
			this.Title = this.GetTitle ();

			this.PickerView = this.CreatePickerView ();
			this.PickerView.Frame = this.View.Bounds;

			this.View.AddSubview (this.PickerView);
		}
	}
}
