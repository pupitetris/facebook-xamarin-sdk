using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;

namespace Facebook.Client.Controls
{
	public partial class Picker<T> : UIView
		where T: class
	{
		public UITableView TableView { get; private set; }
		private UIActivityIndicatorView Spinner;
		private PickerSource<T> longListSelector;

		public Picker (RectangleF frame): base (frame)
		{
			this.InitPicker ();
			this.InitUI (frame);
		}

		protected void InitUI ()
		{
			this.InitUI (RectangleF.Empty);
		}

		protected void InitUI (RectangleF frame)
		{
			this.longListSelector = new PickerSource<T> (this);
			this.TableView = new UITableView (frame);
			this.TableView.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
			this.TableView.Source = this.longListSelector;
			this.AddSubview (this.TableView);

			this.Spinner = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.Gray);
			this.Spinner.HidesWhenStopped = true;
			// We want user to be able to scroll while we load.
			this.Spinner.UserInteractionEnabled = false;
			this.AddSubview (this.Spinner);
		}

		protected void RefreshUI ()
		{
			InvokeOnMainThread (() => {
				this.TableView.ReloadData ();
			});
		}

		protected void SetSelectionModeUI ()
		{
			switch (this.SelectionMode) {
			case PickerSelectionMode.Multiple:
				this.TableView.AllowsMultipleSelection = true;
				this.TableView.AllowsSelection = true;
				break;
			case PickerSelectionMode.Single:
				this.TableView.AllowsMultipleSelection = false;
				this.TableView.AllowsSelection = true;
				break;
			case PickerSelectionMode.None:
				this.TableView.AllowsSelection = false;
				break;
			default:
				throw new ArgumentOutOfRangeException ("SelectionMode");
			}
		}

		public abstract UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath);
	}
}

