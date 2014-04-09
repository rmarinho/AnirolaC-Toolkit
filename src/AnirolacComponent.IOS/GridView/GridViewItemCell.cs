using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;
using System.Drawing;

namespace AnirolacComponent
{
	public class GridViewItemCell : UICollectionViewCell
	{
	
		public override void Draw (System.Drawing.RectangleF rect)
		{
			base.Draw (rect);
		}
		UILabel txtView;

		[Export ("initWithFrame:")]
		public GridViewItemCell (System.Drawing.RectangleF frame) : base (frame)
		{
			SelectedBackgroundView = new GridItemSelectedViewOverlay(frame);
			this.BringSubviewToFront (SelectedBackgroundView);

			txtView = new UILabel (new RectangleF(10,10,300,30));
			txtView.TextColor = UIColor.White;
			txtView.Font = UIFont.FromName("Helvetica-Bold", 20f);
		

			ContentView.AddSubview (txtView);
		}

		public string Text {
			set {
				txtView.Text = value;
			}
		}

		private int _rowSpan =1; 
		public int RowSpan {
			get {return _rowSpan; }
			set{_rowSpan = value;}
		}


		private int _columnSpan=1; 
		public int ColumnSpan {
			get {return _columnSpan; }
			set{_columnSpan = value;}
		}
	}

	public class GridItemSelectedViewOverlay : UIView
	{

		public GridItemSelectedViewOverlay (RectangleF frame) : base(frame)
		{
			BackgroundColor = UIColor.Clear;
		}

		public override void Draw (RectangleF rect)
		{
			using (var g = UIGraphics.GetCurrentContext())
			{
				g.SetLineWidth(10);
				UIColor.FromRGB(64,30,168).SetStroke ();
				UIColor.Clear.SetFill ();

				//create geometry
				var path = new CGPath ();
				path.AddRect (rect);
				path.CloseSubpath();

				//add geometry to graphics context and draw it
				g.AddPath(path);
				g.DrawPath(CGPathDrawingMode.Stroke);
			}
		}
	}
}

