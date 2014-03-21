using System;
using System.Collections.ObjectModel;
using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using MonoTouch.Foundation;


namespace AnirolacComponent
{
	//TODO: Support orientation change
	//TODO: Autosize 
	public class ImageGallery : UIView
	{
		public bool FadeImages {
			get;
			set;
		}
		private ObservableCollection<string> _images; 
		public ObservableCollection<string> Images {
			get{ 
				return _images;
			}
			set{ 
				_images = value;
			}
		}


		UIPageControl pageControl;
		UIScrollView scroller;

		public ImageGallery (List<string> images) : this(images,default(RectangleF))
		{
		}
		public ImageGallery (List<string> images, RectangleF frame) : base (frame)
		{
			this.AutoresizingMask = UIViewAutoresizing.All;
			this.ContentMode = UIViewContentMode.ScaleToFill;
			FadeImages = false;
			this.BackgroundColor = UIColor.White;
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
			pageControl.AutoresizingMask = UIViewAutoresizing.All;
			pageControl.ContentMode = UIViewContentMode.ScaleToFill;
			pageControl.ValueChanged += HandlePageControlHeadValueChanged;

			scroller = new UIScrollView ();
			scroller.AutoresizingMask = UIViewAutoresizing.All;
			scroller.ShowsHorizontalScrollIndicator = scroller.ShowsVerticalScrollIndicator = false;
			scroller.ContentMode = UIViewContentMode.ScaleToFill;
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

		private RectangleF _initialRecView;
		public  override void Draw (System.Drawing.RectangleF rect)
		{
			_initialRecView = rect;
			NSNotificationCenter.DefaultCenter.AddObserver (UIApplication.DidChangeStatusBarOrientationNotification, not => {
			
				var orientation = UIDevice.CurrentDevice.Orientation;
				if ((UIDeviceOrientation.LandscapeLeft == orientation || UIDeviceOrientation.LandscapeRight == orientation))
				{
					scroller.ContentSize = new System.Drawing.SizeF (Frame.Height * Images.Count-1, Frame.Width);

				}
				else{
					scroller.ContentSize = new System.Drawing.SizeF (rect.Width * Images.Count-1, rect.Height);

				}

			});
			pageControl.Frame = new System.Drawing.RectangleF (rect.Left,rect.Height-40, rect.Width,40);
			scroller.Frame = new System.Drawing.RectangleF (rect.Left,rect.Top, rect.Width, rect.Height);
			var curr = 0;
			foreach (var im in  Images) {
				try {
					var img = new UIImage();
					var isRemote = Helpers.IsValidUrl(im);
					if(isRemote)
						//dont await , fire and forget
						LoadImageAsync(curr,im);
					else
						img = UIImage.FromFile(im);

					var imgView = new UIImageView (img);
					imgView.AutoresizingMask = UIViewAutoresizing.All;
					imgView.ContentMode = UIViewContentMode.ScaleToFill;
					if(FadeImages)
						imgView.Alpha = 0;

					//if first image is local, fade it in
					if(curr == 0 && !isRemote)
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
					if(pageControl.CurrentPage == position && FadeImages)
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

		static void SetImage (UIImageView imgView, UIImage img)
		{
			if (img != null) {
				imgView.Image = img;
			}
			imgView.Alpha = 1;
		}

		 void FadeImageViewIn (UIImageView imgView, UIImage img = null)
		{

			if (FadeImages)
				UIView.Animate (0.3, 0, UIViewAnimationOptions.TransitionCrossDissolve, () => {
					SetImage (imgView, img);
				}, () => {
				});
			else {
				SetImage (imgView, img);
			}
		}
	}
}

