using System;
using System.IO;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Facebook.Client
{
	internal partial class ImageLoader<T>
	{
		private static string CacheDir ()
		{
			return Path.Combine (BaseDir, "Library/Caches/Pictures.Facebook.Client/");
		}

		static int sizer (T img)
		{
			UIImage i = img as UIImage;
			var cg = i.CGImage;
			return cg.BytesPerRow * cg.Height;
		}

		static T GetImageFromFile (string picfile)
		{
			var img = UIImage.FromFile (picfile);
			return img as T;
		}
		
		static bool Download (Uri uri)
		{
			try {
				NSUrlResponse response;
				NSError error;

				var target =  PicDir + md5 (uri.AbsoluteUri);
				var req = new NSUrlRequest (new NSUrl (uri.AbsoluteUri.ToString ()), NSUrlRequestCachePolicy.UseProtocolCachePolicy, 120);
				var data = NSUrlConnection.SendSynchronousRequest (req, out response, out error);
				return data.Save (target, true, out error);
			} catch (Exception e) {
				Console.WriteLine ("Problem with {0} {1}", uri, e);
				return false;
			}
		}

		static NSString nsDispatcher = new NSString ("x");

		static void InvokeListeners ()
		{
			nsDispatcher.BeginInvokeOnMainThread (NotifyImageListeners);
		}
	}
}

