using System;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace Facebook.Client.Controls
{
	public partial class PlacePicker
	{
		private static readonly string CellIdentifier = "FacebookClient.PlacePicker";

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell (CellIdentifier);

			// if there are no cells to reuse, create a new one
			if (cell == null) {
				cell = new UITableViewCell (UITableViewCellStyle.Subtitle, CellIdentifier);
				GraphPlace place = Items [indexPath.Row];
				cell.TextLabel.Text = place.Name;
				//cell.DetailTextLabel.Text = place.
				ProfilePicture pict = new ProfilePicture ();
				pict.PictureLoaded += (object sender, PictureLoadedEventArgs e) => cell.ImageView.Image = (UIImage) e.Image;
				pict.AccessToken = this.AccessToken;
				pict.ProfileId = place.Id;
			}
			return cell;
		}
	}
}

