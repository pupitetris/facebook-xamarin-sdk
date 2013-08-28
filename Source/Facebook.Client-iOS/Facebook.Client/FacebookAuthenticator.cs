using System;
using Xamarin.Auth;

namespace Facebook.Client
{
	public partial class FacebookAuthenticator : OAuth2Authenticator
	{
		public FacebookAuthenticator (string appId, Uri startUrl, Uri endUrl): base (appId, "", startUrl, endUrl) 
		{
		}
	}
}

