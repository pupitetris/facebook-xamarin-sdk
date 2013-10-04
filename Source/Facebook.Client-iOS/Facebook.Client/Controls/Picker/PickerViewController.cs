using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace Facebook.Client.Controls
{
	public abstract class PickerViewController: UIViewController
	{
		public UIView PickerView { get; private set; }

		public PickerViewController (): base ()
		{
			this.PickerView = this.CreatePickerView ();
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

			this.PickerView.Frame = this.View.Bounds;

			this.View.AddSubview (this.PickerView);
		}
	}
}
