using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;

namespace AnirolacComponent
{
	public class GridViewItemCell : UICollectionViewCell
	{
	

		UIImageView imageView;

		[Export ("initWithFrame:")]
		public GridViewItemCell (System.Drawing.RectangleF frame) : base (frame)
		{
			BackgroundView = new UIView{BackgroundColor = UIColor.Orange};

			SelectedBackgroundView = new UIView{BackgroundColor = UIColor.Green};

			ContentView.Layer.BorderColor = UIColor.LightGray.CGColor;
			ContentView.Layer.BorderWidth = 2.0f;
			ContentView.BackgroundColor = UIColor.White;
			ContentView.Transform = CGAffineTransform.MakeScale (0.8f, 0.8f);

			imageView = new UIImageView (frame);
			imageView.Center = ContentView.Center;
			imageView.Transform = CGAffineTransform.MakeScale (0.7f, 0.7f);

			ContentView.AddSubview (imageView);
		}

		public UIImage Image {
			set {
				imageView.Image = value;
			}
		}


	}
}

