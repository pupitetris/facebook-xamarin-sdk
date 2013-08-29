using System;

namespace Facebook.Client
{
	public partial class FacebookSessionClient
	{
		public object ParentUI { get; set; }

		public FacebookAuthenticator NewFacebookAuthenticator (Uri startUri, Uri endUri) {
			var auth = new FacebookAuthenticator (this.AppId, startUri, endUri);
			auth.ParentUI = this.ParentUI;
			return auth;
		}
	}
}

