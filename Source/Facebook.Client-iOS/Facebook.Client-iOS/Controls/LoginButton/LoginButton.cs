using System;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using System.Threading.Tasks;

namespace Facebook.Client.Controls
{
	public partial class LoginButton : UIView
	{
		public object ParentUI { get; set; }

		// The design calls for 16 pixels of space on the right edge of the button
		private const float ButtonEndCapWidth = 16.0f;
		// The button has a 12 pixel buffer to the right of the f logo
		private const float ButtonPaddingWidth = 12.0f;

		private SizeF ButtonSize;
		private UILabel Label;
		private UIButton Button;

		public LoginButton () : base ()
		{
			this.ParentUI = null;

			this.AutosizesSubviews = true;
			this.ClipsToBounds = true;

			this.Button = new UIButton (UIButtonType.Custom);
			this.Button.TouchUpInside += OnLoginButtonClicked;
			this.Button.HorizontalAlignment = UIControlContentHorizontalAlignment.Fill;
			this.Button.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

			// We want to make sure that when we stretch the image, it includes the curved edges and drop shadow
			// We inset enough pixels to make sure that happens
			UIEdgeInsets imageInsets = new UIEdgeInsets (4.0f, 40.0f, 4.0f, 4.0f);

			UIImage image = UIImage.FromBundle ("Images/FBLoginViewButton").CreateResizableImage (imageInsets);
			this.Button.SetBackgroundImage (image, UIControlState.Normal);

			image = UIImage.FromBundle ("Images/FBLoginViewButtonPressed").CreateResizableImage (imageInsets);
			this.Button.SetBackgroundImage (image, UIControlState.Highlighted);

			this.AddSubview (this.Button);

			// Compute the text size to figure out the overall size of the button
			UIFont font = UIFont.FromName ("HelveticaNeue-Bold", 14.0f);
			float textSizeWidth = Math.Max (new NSString (this.LogInText ()).StringSize (font).Width,
			                               	new NSString (this.LogOutText ()).StringSize (font).Width);

			// We make the button big enough to hold the image, the text, the padding to the right of the f and the end cap
			this.ButtonSize = new SizeF (image.Size.Width + textSizeWidth + ButtonPaddingWidth + ButtonEndCapWidth, image.Size.Height);

			// add a label that will appear over the button
			this.Label = new ShadowLabel ();
			this.Label.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			this.Label.TextAlignment = UITextAlignment.Center;
			this.Label.BackgroundColor = UIColor.Clear;
			this.Label.Font = font;
			this.Label.TextColor = UIColor.White;
			this.AddSubview (this.Label);

			// We force our height to be the same as the image, but we will let someone make us wider
			// than the default image.
			float width = Math.Max (this.Frame.Size.Width, this.ButtonSize.Width);
			RectangleF frame = new RectangleF (this.Frame.X, this.Frame.Y, width, image.Size.Height);
			this.Frame = frame;

			RectangleF buttonFrame = new RectangleF (0, 0, width, image.Size.Height);
			this.Button.Frame = buttonFrame;

			// This needs to start at an x just to the right of the f in the image, the -1 on both x and y is to account for shadow in the image
			this.Label.Frame = new RectangleF (image.Size.Width - ButtonPaddingWidth - 1, -1, 
			                                   width - (image.Size.Width - ButtonPaddingWidth) - ButtonEndCapWidth, image.Size.Width);
			this.BackgroundColor = UIColor.Clear;

			if (this.CurrentSession != null) {
				if (this.FetchUserInfo) {
					this.LogIn();
				}
			} else {
				this.CurrentUser = null;
			}

			this.UpdateButtonCaption ();
		}

		private SizeF SizeThatFits () {
			return new SizeF (this.ButtonSize);
		}

		public void UpdateButtonWithCaption (string caption) {
			this.Label.Text = caption;
		}

		private Task<FacebookSession> SessionLogInAsync (string permissions) {
			this.facebookSessionClient.ParentUI = this.ParentUI ?? this;
			return this.facebookSessionClient.LoginAsync(permissions);
		}
	}
}
