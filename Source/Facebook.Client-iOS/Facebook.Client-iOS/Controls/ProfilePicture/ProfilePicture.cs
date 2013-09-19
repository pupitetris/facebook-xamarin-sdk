using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace Facebook.Client.Controls
{
	public partial class ProfilePicture: UIView
	{
		private UIImageView ImageView;
		private static float ScreenScaleFactor = 0.0f;
		private HttpHelper connection = null;

		/// <summary>
		/// Initializes a new instance of the ProfilePicture class.
		/// </summary>
		public ProfilePicture()
		{
			this.ImageView = new UIImageView (this.Bounds);
			this.ImageView.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			this.AutosizesSubviews = true;
			this.ClipsToBounds = true;

			this.AddSubview (this.ImageView);

			this.LoadPicture ();
		}

		// Lets us catch resizes of the control, or any outer layout, allowing us to potentially
		// choose a different image.
		public override void LayoutSubviews ()
		{
			this.EnsureImageViewContentMode ();
			base.LayoutSubviews ();
		}

		private float GetWidth () {
			if (ScreenScaleFactor == 0.0) {
				ScreenScaleFactor = UIScreen.MainScreen.Scale;
			}

			// Retina display doesn't increase the bounds that iOS returns.  The size needs
			// to be calculated using the scale factor accessed above.
			return this.Bounds.Width * ScreenScaleFactor;
		}

		private float GetHeight () {
			if (ScreenScaleFactor == 0.0) {
				ScreenScaleFactor = UIScreen.MainScreen.Scale;
			}

			// Retina display doesn't increase the bounds that iOS returns.  The size needs
			// to be calculated using the scale factor accessed above.
			return this.Bounds.Height * ScreenScaleFactor;
		}

		private void CropModeChanged () {
			this.LoadPicture ();
		}

		private void RefreshImage () {
			if (!string.IsNullOrEmpty (this.ProfileId)) {
				if (this.connection != null) {
					this.connection.CancelAsync ();
				}

				this.connection = new HttpHelper (this.ImageSource);
				this.connection.OpenReadTaskAsync ().ContinueWith (t => {
					if (t.IsCompleted) {
						using (var stream = t.Result) {
							using (var data = NSData.FromStream (stream)) {
								this.ImageView.Image = UIImage.LoadFromData (data);
								this.EnsureImageViewContentMode ();
							}
						}
					}
					this.connection = null;
				});

			} else {
				this.ImageView.Image = UIImage.FromBundle (this.ImageSource);
				this.EnsureImageViewContentMode ();
			}
		}

		private void EnsureImageViewContentMode () {
			// Set the image's contentMode such that if the image is larger than the control, we scale it down, preserving aspect 
			// ratio.  Otherwise, we center it.  This ensures that we never scale up, and pixellate, the image.
			SizeF viewSize = this.Bounds.Size;
			SizeF imageSize = this.ImageView.Image.Size;
			UIViewContentMode contentMode;

			// If both of the view dimensions are larger than the image, we'll center the image to prevent scaling up.
			// Note that unlike in choosing the image size, we *don't* use any Retina-display scaling factor to choose centering
			// vs. filling.  If we were to do so, we'd get profile pics shrinking to fill the the view on non-Retina, but getting
			// centered and clipped on Retina.  
			if (viewSize.Width > imageSize.Width && viewSize.Height > imageSize.Height) {
				contentMode = UIViewContentMode.Center;
			} else {
				contentMode = UIViewContentMode.ScaleAspectFit;
			}

			this.ImageView.ContentMode = contentMode;
		}
	}
}

