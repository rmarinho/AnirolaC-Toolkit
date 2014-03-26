using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;

namespace AnirolacComponent
{
	public class GridView : UICollectionView
	{
		static NSString gridCellId = new NSString ("GridViewCell");
	
		public GridView () : this(default(RectangleF)){}
		//UICollectionViewFlowLayout()
		public GridView (RectangleF frm) : base(frm,  new GridViewLayout(new SizeF(200,100),10) )
		{

			this.AutoresizingMask = UIViewAutoresizing.All;
			this.ContentMode = UIViewContentMode.ScaleToFill;
			BackgroundColor = UIColor.Red;
			RegisterClassForCell (typeof(GridViewItemCell), gridCellId);
		}

	}
}

