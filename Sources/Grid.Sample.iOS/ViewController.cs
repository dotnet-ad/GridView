using System;

using UIKit;

namespace Grid.Sample.iOS
{
	public partial class ViewController : UIViewController
	{
		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var red = new UIView { BackgroundColor = UIColor.Red };
			var blue = new UIView { BackgroundColor = UIColor.Blue };
			var cyan = new UIView { BackgroundColor = UIColor.Cyan };
			var yellow = new UIView { BackgroundColor = UIColor.Yellow };

			var portrait = new UIKit.Grid.Layout() 
									 { 
										Spacing = 10, 
										Padding = new UIEdgeInsets(10,10,10,10) 
									 }
									 .WithRows(0.75f, 0.25f, 200f)
									 .WithColumns(0.75f, 0.25f)
									 + red.At(0, 0).WithSpan(2, 1) 
				  			         + blue.At(2, 0).WithSpan(1, 2) 
									 + cyan.At(0, 1) 
									 + yellow.At(1,1);

			var landscape = new UIKit.Grid.Layout() 
									 { 
										Spacing = 20, 
										Padding = new UIEdgeInsets(20, 20, 20, 20), 
									 }
									 .WithRows(1.00f)
									 .WithColumns(0.50f, 0.25f, 0.25f)
									 + red.At(0, 0)
									 + blue.At(0, 1)
									 + cyan.At(0, 2);

			var grid = new UIKit.Grid();

			grid.AddLayout(portrait);
			grid.AddLayout(landscape, (g) => (g.Frame.Width > g.Frame.Height));

			this.View = grid;
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}
