// This file has been autogenerated from a class added in the UI designer.

using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;

namespace AnirolacComponent.Samples.iOS
{
	public partial class GridViewViewController : UIViewController
	{
		public GridViewViewController (IntPtr handle) : base (handle)
		{
		}

		GridView _gridView;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			_gridView = new GridView (View.Bounds);
			_gridView.DataSource = new TableSource ();
			this.Add (_gridView);
		}
	}

	public class Tile {
		public string Text {
			get;
			set;
		}

		public int RowSpan {
			get;
			set;
		}
		public int ColumnSpan {
			get;
			set;
		}

	}
	public class TableSource : UICollectionViewDataSource
	{
		static NSString CellId = new NSString ("GridViewCell");

		Random rand = new Random (1);
		List<Tile> items = new List<Tile> ();
		public TableSource ()
		{
			for (int i = 0; i < 100; i++) {
				var newTile = new Tile() { Text="hello " +  i, RowSpan=1,ColumnSpan=1 };
				if (i == 5)
					newTile.ColumnSpan = newTile.RowSpan = 2;
				if (i == 10)
					newTile.ColumnSpan = newTile.RowSpan = 2;
//				if (i == 16)
//					newTile.ColumnSpan = newTile.RowSpan = 2;
				items.Add ( newTile);
			}
		}
		#region implemented abstract members of UICollectionViewDataSource
		public override int GetItemsCount (UICollectionView collectionView, int section)
		{
			return items.Count;
		}
		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
		{
			var animalCell = (GridViewItemCell)collectionView.DequeueReusableCell (CellId, indexPath);

			var item = items [indexPath.Row];

			animalCell.Text = item.Text;
			animalCell.RowSpan = item.RowSpan;
			animalCell.ColumnSpan = item.ColumnSpan;
			return animalCell;
		}
		#endregion


	}
}
