using System;
using System.Drawing;
using System.Globalization;

using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace Facebook.Client.Controls
{
	public partial class PlacePicker
	{
		private static readonly string CellIdentifier = "FacebookClient.PlacePicker";

		protected static UIImage DefaultImage = null;

		protected static UIImage GetDefaultImage () {
			if (DefaultImage == null) {
				DefaultImage = UIImage.FromBundle (string.Format (CultureInfo.InvariantCulture, "/Images/{0}", "FBPlacePickerViewGenericPlace.png"));
			}
			return DefaultImage;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell (CellIdentifier);

			// if there are no cells to reuse, create a new one
			if (cell == null) {
				cell = new UITableViewCell (UITableViewCellStyle.Subtitle, CellIdentifier);
			}

			GraphPlace place = this.longListSelector.GetItem (indexPath).Item;
			cell.TextLabel.Text = place.Name;
			cell.DetailTextLabel.Text = string.Format ("{0} • {1} were here".t (), place.Category, place.WereHereCount);

			string place_picture_url = place.ProfilePictureUrl.ToString ();
			if (this.ImageCache.ContainsKey (place_picture_url)) {
				cell.ImageView.Image = this.ImageCache [place_picture_url];
			} else {
				cell.ImageView.Image = PlacePicker.GetDefaultImage ();
				ProfilePicture pict = new ProfilePicture ();
				pict.PictureLoaded += (object sender, PictureLoadedEventArgs e) => {
					cell.ImageView.Image = (UIImage)e.Image;
					if (!this.ImageCache.ContainsKey (place_picture_url)) {
						this.ImageCache.Add (place_picture_url, (UIImage) e.Image);
					}
				};
				pict.AccessToken = this.AccessToken;
				pict.ProfileId = place.Id;
			}

			return cell;
		}
	}
}

