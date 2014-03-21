// This file has been autogenerated from a class added in the UI designer.

using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;

namespace AnirolacComponent.Samples.iOS
{
	public partial class MainViewController : UIViewController
	{
		public MainViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			var images = new List<string> ();
			images.Add ("algarvesurfschool2.jpg");
			images.Add ("algarvesurfschool3.jpg");
			images.Add ("algarvesurfschool4.jpg");

			images.Add ("https://dl.dropboxusercontent.com/u/1966569/algarvesurfschool2.jpg");
			images.Add ("https://dl.dropboxusercontent.com/u/1966569/algarvesurfschool2.jpg");
			images.Add ("https://dl.dropboxusercontent.com/u/1966569/algarvesurfschool4.jpg");
			images.Add ("algarvesurfschool2.jpg");
			images.Add ("algarvesurfschool3.jpg");
			images.Add ("algarvesurfschool4.jpg");


			var imageGallery = new ImageGallery (images, new System.Drawing.RectangleF(0,0,320,260));

			this.View.Add (imageGallery);
			imageGallery.Images.Add ("algarvesurfschool4.jpg");
		}
	}
}