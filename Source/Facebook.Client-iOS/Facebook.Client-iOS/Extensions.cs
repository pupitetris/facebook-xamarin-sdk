using System;
using MonoTouch.Foundation;

namespace Facebook.Client.Extensions
{
	public static class StringExtensions
	{
		public static string t(this string translate) {
			return NSBundle.MainBundle.LocalizedString (translate, "", "");
		}
	}
}

