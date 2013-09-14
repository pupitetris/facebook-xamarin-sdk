// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace BasicAppiOS
{
	[Register ("BasicApp_iOSViewController")]
	partial class BasicApp_iOSViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIView LoginButtonCont { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (LoginButtonCont != null) {
				LoginButtonCont.Dispose ();
				LoginButtonCont = null;
			}
		}
	}
}
