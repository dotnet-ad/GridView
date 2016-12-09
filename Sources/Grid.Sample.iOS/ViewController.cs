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
									 .WithColumns(0.75f, 0.25f);
			
			portrait[0, 2][0] = red; 		// (row: 0, rowspan: 2) - (column: 0)
			portrait[2][0, 2] =  blue; 		// (row: 2) - (column: 0, colspan: 2)
			portrait[0][1] = cyan;  		// (row: 0) - (column: 1)
			portrait[1][1] = yellow;        // (row: 1) - (column: 1)


			var landscape = new UIKit.Grid.Layout() 
									 { 
										Spacing = 10, 
										Padding = new UIEdgeInsets(10, 10, 10, 10), 
									 }
									 .WithRows(1.00f)
									 .WithColumns(0.50f, 0.25f, 0.25f);

			landscape[0][0] = red;      	// (row: 0) - (column: 1)
			landscape[0][1] = blue;     	// (row: 0) - (column: 1)
			landscape[0][2] = cyan;        	// (row: 0) - (column: 2)

			var grid = new UIKit.Grid();

			grid.AddLayout(portrait);
			grid.AddLayout(landscape, (width, height) => (width > height));

			this.View = grid;
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}
