using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace AnirolacComponent
{
	public class Helpers
	{
		public static bool IsValidUrl(string urlString)
		{
			Uri uri;
			return Uri.TryCreate(urlString, UriKind.Absolute, out uri)
				&& (uri.Scheme == Uri.UriSchemeHttp
					|| uri.Scheme == Uri.UriSchemeHttps
					|| uri.Scheme == Uri.UriSchemeFtp
					|| uri.Scheme == Uri.UriSchemeMailto
					/*...*/);
		}
		public static UIImage LoadFromUrl (string uri)
		{
			using (var url = new NSUrl (uri))
			using (var data = NSData.FromUrl (url))
				return UIImage.LoadFromData (data);
		}
	}
}

