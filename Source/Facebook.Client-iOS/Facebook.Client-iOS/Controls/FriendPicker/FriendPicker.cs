using System;
using System.Drawing;
using System.Globalization;

using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreText;
using MonoTouch.CoreFoundation;

namespace Facebook.Client.Controls
{
	public partial class FriendPicker
	{
		private static readonly string CellIdentifier = "FacebookClient.FriendPicker";

		protected static UIImage DefaultImage = null;

		protected static UIImage GetDefaultImage () {
			if (DefaultImage == null) {
				DefaultImage = UIImage.FromBundle (ProfilePicture.GetBlankProfilePictureUrl (true));
			}
			return DefaultImage;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell (CellIdentifier);

			// if there are no cells to reuse, create a new one
			if (cell == null) {
				cell = new UITableViewCell (UITableViewCellStyle.Default, CellIdentifier);
			}

			GraphUser user = this.longListSelector.GetItem (indexPath).Item;

			var bold = new UIStringAttributes {
				Font = UIFont.BoldSystemFontOfSize (UIFont.LabelFontSize)
			};

			var regular = new UIStringAttributes {
				Font = UIFont.SystemFontOfSize (UIFont.LabelFontSize)
			};

			var str = string.Format ("{0}{1}{2}", 
			                         (user.FirstName == null)? "": user.FirstName + " ", 
			                         (user.MiddleName == null)? "": user.MiddleName + " ",
			                         user.LastName ?? "");


			int namelen = 0;
			if (user.MiddleName != null) {
				namelen += user.MiddleName.Length;
			}
			if (user.FirstName != null) {
				namelen += user.FirstName.Length;
				if (user.MiddleName != null) {
					namelen ++;
				}
			}

			var astr = new NSMutableAttributedString (str);
			if (namelen > 0) {
				astr.SetAttributes (bold.Dictionary, new NSRange (0, namelen));
				astr.SetAttributes (regular.Dictionary, new NSRange (namelen, str.Length - namelen));
			}
			cell.TextLabel.AttributedText = astr;

			string user_picture_url = user.ProfilePictureUrl.ToString ();
			if (this.ImageCache.ContainsKey (user_picture_url)) {
				cell.ImageView.Image = this.ImageCache [user_picture_url];
			} else {
				cell.ImageView.Image = FriendPicker.GetDefaultImage ();
				ProfilePicture pict = new ProfilePicture ();
				pict.PictureLoaded += (object sender, PictureLoadedEventArgs e) => {
					cell.ImageView.Image = (UIImage)e.Image;
					if (!this.ImageCache.ContainsKey (user_picture_url)) {
						this.ImageCache.Add (user_picture_url, (UIImage) e.Image);
					}
				};
				pict.AccessToken = this.AccessToken;
				pict.ProfileId = user.Id;
			}

			return cell;
		}
	}
}

