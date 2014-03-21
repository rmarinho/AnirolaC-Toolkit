﻿using System;
using System.Collections.ObjectModel;
using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;


namespace AnirolacComponent
{
	//TODO: Support orientation change
	//TODO: Autosize 
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


		UIPageControl pageControl;
		UIScrollView scroller;
		public ImageGallery (List<string> images = null, RectangleF frame = default(RectangleF))
		{
			if (frame == default(RectangleF))
				this.Frame = UIScreen.MainScreen.Bounds;
			else
				this.Frame = frame;

			if (images != null)
				Images = new ObservableCollection<string> (images);
			else
				Images = new ObservableCollection<string> ();

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
				var imgView = scroller.Subviews[pageNumber] as UIImageView;
				FadeImageViewIn (imgView);
				pageControl.CurrentPage = pageNumber;
			};
			this.Add (scroller);
			this.Add (pageControl);
		}

	
		public  override void Draw (System.Drawing.RectangleF rect)
		{
			pageControl.Frame = new System.Drawing.RectangleF (rect.Left,rect.Height-40, rect.Width,40);
			scroller.Frame = new System.Drawing.RectangleF (rect.Left,rect.Top, rect.Width, rect.Height);
			var curr = 0;
			foreach (var im in  Images) {
				try {
					var img = new UIImage();

					if(Helpers.IsValidUrl(im))
						//dont await , fire and forget
						LoadImageAsync(curr,im);
					else
						img = UIImage.FromFile(im);

					var imgView = new UIImageView (img);
					imgView.Alpha = 0;

					//if first image is local, fade it in
					if(curr == 0)
						FadeImageViewIn(imgView);

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



		private Task LoadImageAsync(int position, string url)
		{
			return Task.Run (() => {
				var img = Helpers.LoadFromUrl(url);

				InvokeOnMainThread( () => {

					var imgView = scroller.Subviews[position] as UIImageView;
					if(pageControl.CurrentPage == position)
						FadeImageViewIn(imgView,img);
					else
						imgView.Image = img;
				});
			});

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

		static void FadeImageViewIn (UIImageView imgView, UIImage img = null)
		{

			UIView.Animate (0.3, 0, UIViewAnimationOptions.CurveEaseInOut, () =>  {
				if(img!= null)
				{
					imgView.Image	= img;
				}
				imgView.Alpha = 1;
			}, () =>  {
			});
		}
	}
}
