using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;

namespace AnirolacComponent
{
	public class GridView : UICollectionView
	{
		static NSString animalCellId = new NSString ("GridViewCell");
	
		public GridView () : this(default(RectangleF)){}
		//UICollectionViewFlowLayout()
		public GridView (RectangleF frm) : base(frm,  new GridViewLayout(new SizeF(200,100),10) )
		{

			this.AutoresizingMask = UIViewAutoresizing.All;
			this.ContentMode = UIViewContentMode.ScaleToFill;
			BackgroundColor = UIColor.Red;
				RegisterClassForCell (typeof(GridViewItemCell), animalCellId);
			//RegisterClassForSupplementaryView (typeof(Header), UICollectionElementKindSection.Header, headerId);

			// add a custom menu item
//			UIMenuController.SharedMenuController.MenuItems = new UIMenuItem[] { 
//				new UIMenuItem ("Custom", new Selector ("custom:")) 
//			};
		
		}
	
	}
}

