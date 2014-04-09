using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
using System.Linq;
using System.Collections.Generic;

namespace AnirolacComponent
{
	public class GridViewLayout : UICollectionViewLayout
	{
	
		public int Margin = 10;
		public SizeF ItemSize = new SizeF(0,0);

		Dictionary<KeyValuePair<int,int>,bool> matrix = new 	Dictionary<KeyValuePair<int,int>,bool> ();
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

		void PaintItem (UICollectionViewLayoutAttributes attributes,SizeF currentSize, int rowspan, int colspan)
		{

		
			//we need another column
			if (currentRow+rowspan > nRows) {
				currentColumn++;
				currentRow = 0;
			
			}
			bool keepLooping = true;
			bool canDraw = true;
			for (int i = currentRow; i < currentRow + rowspan && keepLooping; i++) {
				for (int j = currentColumn; j < currentColumn + colspan; j++) {

					var exists = matrix.FirstOrDefault (c => c.Key.Key == i && c.Key.Value == j);
					if(canDraw)
						canDraw = !exists.Value;

				}

			}
			if (canDraw) {
				for (int i = currentRow; i < currentRow + rowspan; i++) {
					for (int j = currentColumn; j < currentColumn + colspan; j++) {
						matrix.Add (new KeyValuePair<int, int> (i, j), true);
					}
				}
				var x = (currentColumn * ItemSize.Width) + (currentSize.Width / 2) + (Margin * (currentColumn + 1));
				var y = ((currentRow) * rowH) + (currentSize.Height / 2) + (Margin * (currentRow));

				attributes.Center = new PointF ((float)x, (float)y);
				currentRow += rowspan;
			} else {
				currentRow++;
				PaintItem (attributes, currentSize, rowspan, colspan);
			}

		
		}
	
		public override UICollectionViewLayoutAttributes LayoutAttributesForItem (NSIndexPath path)
		{
			if (path.Item == 0) {
				matrix.Clear ();
			
			}
			UICollectionViewLayoutAttributes attributes = UICollectionViewLayoutAttributes.CreateForCell (path);
			var cell = CollectionView.CellForItem (path) as GridViewItemCell;
			var rowspan = 1;
			var colspan = 1;
			if (cell != null) {
				rowspan = cell.RowSpan;
				colspan = cell.ColumnSpan;
			}
			var currentSize = new SizeF (ItemSize.Width, ItemSize.Height);
			currentSize.Width *= colspan;
			currentSize.Width += Margin * (colspan - 1);
			currentSize.Height *= rowspan;
			currentSize.Height += Margin * (rowspan - 1);
			attributes.Size = currentSize;
		
			PaintItem (attributes, currentSize,rowspan, colspan);
				return attributes;
		}

		public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect (RectangleF rect)
		{
			var attributes = new UICollectionViewLayoutAttributes [cellCount + 1];

			for (int i = 0; i < cellCount; i++) {
				NSIndexPath indexPath = NSIndexPath.FromItemSection (i, 0);
				attributes [i] = LayoutAttributesForItem (indexPath);
			}

			var decorationAttribs = UICollectionViewLayoutAttributes.CreateForDecorationView (myDecorationViewId, 
				NSIndexPath.FromItemSection (0, 0));
		
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

