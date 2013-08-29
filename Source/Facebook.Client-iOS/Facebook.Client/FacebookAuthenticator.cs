using System;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace Facebook.Client
{
	public partial class FacebookAuthenticator : OAuth2Authenticator
	{
		public FacebookAuthenticator (string appId, Uri startUri, Uri endUri) : base (appId, "", startUri, endUri)
		{
			this.AllowCancel = true;
			this.ClearCookiesBeforeLogin = false;

			InitUI ();
		}

		public async Task<Account> AuthenticateAsync ()
		{
			TaskCompletionSource<Account> tcs = new TaskCompletionSource<Account>();

			this.Error += (object sender, AuthenticatorErrorEventArgs e) => {
				// Platform-specific UI routine.
				this.DismissUI ();

				tcs.TrySetException (new InvalidOperationException (e.Message));
			};

			this.Completed += (object sender, AuthenticatorCompletedEventArgs e) => {
				// Platform-specific UI routine.
				this.DismissUI ();

				if (!e.IsAuthenticated) {
					tcs.TrySetException (new InvalidOperationException ());
					return;
				}

				tcs.TrySetResult (e.Account);
			};

			// Platform-specific UI routine.
			this.PresentUI ();

			return tcs.Task;
		}
	}
}
