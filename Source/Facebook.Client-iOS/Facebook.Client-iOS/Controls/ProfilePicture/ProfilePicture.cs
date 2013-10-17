using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace Facebook.Client.Controls
{
	public partial class ProfilePicture: UIView, IImageUpdated
	{
		private UIImageView ImageView;
		private static float ScreenScaleFactor = 0.0f;

		private static ImageLoader<UIImage> Loader = new ImageLoader<UIImage> (250, 4*1024*1024);

		protected static UIImage DefaultImage = null;
		protected static UIImage DefaultSquareImage = null;

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

		protected static UIImage GetDefaultImage (CropMode crop_mode) {
			if (crop_mode == CropMode.Square) {
				if (DefaultSquareImage == null) {
					DefaultSquareImage = UIImage.FromBundle (ProfilePicture.GetBlankProfilePictureUrl (true));
				}
				return DefaultSquareImage;
			} else {
				if (DefaultImage == null) {
					DefaultImage = UIImage.FromBundle (ProfilePicture.GetBlankProfilePictureUrl (false));
				}
				return DefaultImage;
			}
		}

		// Required by IImageUpdated
		void IImageUpdated.UpdatedImage (Uri uri)
		{
			if (uri != null)
				this.SetImage (uri);
		}

		private void SetImage (Uri uri) {
			UIImage img = ProfilePicture.Loader.RequestImage (uri, this);
			if (img != null) {
				this.ImageView.Image = img;
				this.EnsureImageViewContentMode ();
				this.OnPictureLoaded (this, new PictureLoadedEventArgs (this.ImageView.Image));
			}
		}

		private void RefreshImage () {
			if (!string.IsNullOrEmpty (this.ProfileId)) {
				this.SetImage (new Uri (this.ImageSource));
			} else {
				this.ImageView.Image = ProfilePicture.GetDefaultImage (this.CropMode);
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

