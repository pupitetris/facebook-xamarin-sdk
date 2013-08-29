using System;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;

namespace Facebook.Client.Controls
{
	public class ShadowLabel: UILabel
	{
		public ShadowLabel () : base ()
		{
		}

		public void DrawTextInRect (RectangleF rect) 
		{
			SizeF myShadowOffset = new SizeF (0, -1);
			float[] myColorValues = {0, 0, 0, 0.3f};

			using (CGContext myContext = UIGraphics.GetCurrentContext ())
			{
				myContext.SaveState ();
				using (CGColorSpace myColorSpace = CGColorSpace.CreateDeviceRGB ())
					using (CGColor myColor = new CGColor (myColorSpace, myColorValues))
					{	
						myContext.SetShadowWithColor (myShadowOffset, 1, myColor);
						base.DrawText (rect);
					}
			}
		}

	} // class ShadowLabel
}

