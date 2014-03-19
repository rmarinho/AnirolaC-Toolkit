using System;
using System.Collections.ObjectModel;
using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Drawing;

namespace AnirolacComponent
{
	public class ImageGallery : UIView
	{

		private ObservableCollection<string> images; 
		public ObservableCollection<string> Images {
			get{ 
				return images;
			}
			set{ 
				images = value;
			}
		}

		UIView mainView;
		UIPageControl pageControl;
		UIScrollView scroller;
		public ImageGallery (List<string> imags)
		{
			mainView = new UIView ();
			Images = new ObservableCollection<string> (imags);
			Images.CollectionChanged+= HandleCollectionChanged;

			pageControl = new UIPageControl ();
			pageControl.ValueChanged += HandlePageControlHeadValueChanged;
			scroller = new UIScrollView ();
			scroller.PagingEnabled = true;
			scroller.Bounces = false;
			scroller.Scrolled+= (object sender, EventArgs e) => {
				var pageWidth = double.Parse(scroller.Bounds.Width.ToString());
				var oof = double.Parse(scroller.ContentOffset.X.ToString());
				int pageNumber = int.Parse(( Math.Floor((oof - pageWidth / 2) / pageWidth) + 1).ToString());
				pageControl.CurrentPage = pageNumber;
			};
			this.Add (scroller);
			this.Add (pageControl);
		}

	

		public override void Draw (System.Drawing.RectangleF rect)
		{
			//		mainView.Frame = rect;
			pageControl.Frame = new System.Drawing.RectangleF (rect.Left,rect.Height-40, rect.Width,40);

			scroller.Frame = new System.Drawing.RectangleF (rect.Left,rect.Top, rect.Width, rect.Height);
			var curr = 0;
			foreach (var im in  Images) {
				try {
					var imgView = new UIImageView (UIImage.FromFile(im));
					imgView.Frame = new System.Drawing.RectangleF (rect.Width * curr, rect.Top, rect.Width, rect.Height);
					scroller.AddSubview (imgView);
					curr++;
				} catch (Exception ex) {

				}

			}
			scroller.ContentSize = new System.Drawing.SizeF (scroller.Frame.Width * curr-1, scroller.Frame.Height);
			pageControl.Pages = curr;

			base.Draw (rect);
		}

		private void HandlePageControlHeadValueChanged(object sender, EventArgs e)
		{
			var off = this.pageControl.CurrentPage  * this.scroller.Frame.Width;
			scroller.SetContentOffset(new PointF(off, 0), true);
		}

		void HandleCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{

			if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) {

			}

		}
	}
}

