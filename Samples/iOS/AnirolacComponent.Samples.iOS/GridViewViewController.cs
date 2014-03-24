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

	public class TableSource : UICollectionViewDataSource
	{
		static NSString animalCellId = new NSString ("GridViewCell");

		List<string> items = new List<string> ();
		public TableSource ()
		{
			for (int i = 0; i < 100; i++) {
				items.Add ("hello " +  i);
			}
		}
		#region implemented abstract members of UICollectionViewDataSource
		public override int GetItemsCount (UICollectionView collectionView, int section)
		{
			return items.Count;
		}
		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
		{
			var animalCell = (GridViewItemCell)collectionView.DequeueReusableCell (animalCellId, indexPath);

			var text = items [indexPath.Row];

			animalCell.Text = text;

			return animalCell;
		}
		#endregion


	}
}
