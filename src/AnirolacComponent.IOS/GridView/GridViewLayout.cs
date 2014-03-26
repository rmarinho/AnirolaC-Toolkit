using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;

namespace AnirolacComponent
{
	public class GridViewLayout : UICollectionViewLayout
	{
	
		public int Margin = 10;
		public SizeF ItemSize = new SizeF(0,0);

		SizeF contentFrameSize = new SizeF(0,0);
		int cellCount = 0;
		int nRows =0;
		int nColumns =0;
		float _contentWidth = 0;
		float rowH = 0;
		static NSString myDecorationViewId = new NSString ("MyDecorationView");

		public GridViewLayout (SizeF itemSize, int margn)
		{
			Margin = margn;
			ItemSize = itemSize;

			RegisterClassForDecorationView (typeof(MyDecorationView), myDecorationViewId);
		}
		public override void PrepareLayout ()
		{
			currentRow = 0;
			currentColumn = 0;
		
			if ( contentFrameSize != CollectionView.Frame.Size ) {
				cellCount = CollectionView.NumberOfItemsInSection (0);
				contentFrameSize = CollectionView.Frame.Size;
				nRows = (int)Math.Round (contentFrameSize.Height / (ItemSize.Height + Margin * 2), 0);
				nColumns = (int)Math.Round ((double)(cellCount / nRows));
				//this expands layout
				rowH = contentFrameSize.Height / nRows;
				//this sets row height same as itemheight
				rowH = ItemSize.Height;
				_contentWidth = ItemSize.Width * (nColumns + 1);
		
			}
		}


		public override SizeF CollectionViewContentSize {
			get {
				var rect = CollectionView.Frame;
				rect.Width = _contentWidth;
				return rect.Size;
			}
		}

		public override bool ShouldInvalidateLayoutForBoundsChange (RectangleF newBounds)
		{
			return true;
		}

		int currentRow = 0;
		int currentColumn = 0;

	
		public override UICollectionViewLayoutAttributes LayoutAttributesForItem (NSIndexPath path)
		{
			UICollectionViewLayoutAttributes attributes = UICollectionViewLayoutAttributes.CreateForCell (path);
			var cell = CollectionView.CellForItem (path) as GridViewItemCell;
			var rowspan = 3;
			var colspan = 2;
			if (cell != null) {
			
			}
			var currentSize = new SizeF(ItemSize.Width,ItemSize.Height);
			currentSize.Width *= colspan;
			currentSize.Width += Margin*(colspan-1);;
			currentSize.Height *= rowspan;
			currentSize.Height += Margin*(rowspan-1);

			attributes.Size = currentSize;
		
			currentRow+=rowspan;
			//we need another row
			if(currentRow>= nRows)
			{
				currentColumn++;
				currentRow = rowspan;	
			}
			var x = (currentColumn * currentSize.Width) + (currentSize.Width /2) + (Margin * (currentColumn+1));
			var y = ((currentRow-rowspan) * rowH) + (currentSize.Height /2)  + (Margin * currentRow+1);
		
			attributes.Center = new PointF((float)x,(float)y);
		
		
		
			return attributes;
		}

		public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect (RectangleF rect)
		{
			var attributes = new UICollectionViewLayoutAttributes [cellCount + 1];

			for (int i = 0; i < cellCount; i++) {
				NSIndexPath indexPath = NSIndexPath.FromItemSection (i, 0);
				attributes [i] = LayoutAttributesForItem (indexPath);
			}

			var decorationAttribs = UICollectionViewLayoutAttributes.CreateForDecorationView (myDecorationViewId, NSIndexPath.FromItemSection (0, 0));
		
			decorationAttribs.Size = CollectionView.Frame.Size;
			decorationAttribs.Center = CollectionView.Center;
			decorationAttribs.ZIndex = -1;
			attributes [cellCount] = decorationAttribs;

			return attributes;
		}


	}

	public class MyDecorationView : UICollectionReusableView
	{
		[Export ("initWithFrame:")]
		public MyDecorationView (System.Drawing.RectangleF frame) : base (frame)
		{
			//BackgroundColor = UIColor.Red;
		}
	}
}

