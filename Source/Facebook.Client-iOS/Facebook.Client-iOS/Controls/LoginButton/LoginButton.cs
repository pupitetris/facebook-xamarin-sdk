using System;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using Facebook.Client.Extensions;

namespace Facebook.Client.Controls
{
	public partial class LoginButton : UIView
	{
		// The design calls for 16 pixels of space on the right edge of the button
		private const float ButtonEndCapWidth = 16.0;
		// The button has a 12 pixel buffer to the right of the f logo
		private const float ButtonPaddingWidth = 12.0;

		private SizeF ButtonSize;
		private UIButton Button;

		public LoginButton () : base ()
		{
			this.AutosizesSubviews = true;
			this.ClipsToBounds = true;

			this.Button = new UIButton (UIButtonType.Custom);
			this.Button.TouchUpInside += ButtonPressed;
			this.Button.HorizontalAlignment = UIControlContentHorizontalAlignment.Fill;
			this.Button.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

			// We want to make sure that when we stretch the image, it includes the curved edges and drop shadow
			// We inset enough pixels to make sure that happens
			UIEdgeInsets imageInsets = new UIEdgeInsets (4.0, 40.0, 4.0, 4.0);

			UIImage image = UIImage.FromBundle ("Images/FBLoginViewButton").CreateResizableImage (imageInsets);
			this.Button.SetBackgroundImage (image, UIControlState.Normal);

			image = UIImage.FromBundle ("Images/FBLoginViewButtonPressed").CreateResizableImage (imageInsets);
			this.Button.SetBackgroundImage (image, UIControlState.Highlighted);

			this.AddSubview (this.Button);

			// Compute the text size to figure out the overall size of the button
			UIFont font = UIFont.FromName ("HelveticaNeue-Bold", 14.0);
			//float textSizeWidth = Math.Max (this.LogInText, 
			//font.Set
		}

		public void ButtonPressed (object sender, EventArgs e) {

		}
		
		private string LogInText () {
			var caption = this.CurrentSession == null? 
		}
	}
}
