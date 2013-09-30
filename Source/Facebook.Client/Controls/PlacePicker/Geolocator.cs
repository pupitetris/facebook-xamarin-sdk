using System;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Geolocation;

namespace Facebook.Client.Controls
{
	public class PositionChangedEventArgs: PositionEventArgs
	{
		public PositionChangedEventArgs (Position position): base (position)
		{
		}
	}

	public class Geocoordinate: Position
	{
	}

	public class Geoposition
	{
		public Geoposition (Geocoordinate coordinate)
		{
			this.Coordinate = coordinate;
		}

		public Geocoordinate Coordinate { get; set; }
	}

	public enum PositionAccuracy {
		Default = 0,
		High = 1
	}

	public class Geolocator: Xamarin.Geolocation.Geolocator
	{
		public double MovementThreshold { get; set; }
		public new PositionAccuracy DesiredAccuracy { get; set; }
		private bool desiredAccuracyInMetersIsSet = false;
		public Nullable<uint> DesiredAccuracyInMeters {
			get {
				if (this.desiredAccuracyInMetersIsSet) {
					return (uint) base.DesiredAccuracy;
				}
				return null; 
			}
			set {
				if (value != null) {
					this.desiredAccuracyInMetersIsSet = true;
					base.DesiredAccuracy = (double)value;
				} else {
					this.desiredAccuracyInMetersIsSet = false;
				}
			}
		}

		public new event EventHandler<PositionChangedEventArgs> PositionChanged;

		protected virtual void OnPositionChanged (object sender, PositionChangedEventArgs e)
		{
			EventHandler<PositionChangedEventArgs> handler = PositionChanged;
			if (handler != null) {
				handler (sender, e);
			}
		}

		public Geolocator (): base ()
		{
			base.PositionChanged += (object sender, PositionEventArgs e) => 
				this.OnPositionChanged (sender, (PositionChangedEventArgs) e);
		}

		public async Task<Geoposition> GetGeopositionAsync(int timeout, CancellationToken cancelToken) 
		{
			Geocoordinate coordinate = (Geocoordinate) await this.GetPositionAsync (timeout, cancelToken);
			return new Geoposition (coordinate);
		}
	}
}

