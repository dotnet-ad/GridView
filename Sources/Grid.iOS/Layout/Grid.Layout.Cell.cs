namespace UIKit
{
	using CoreGraphics;

	public partial class Grid : UIView
	{
		public partial class Layout
		{
			public class Cell
			{
				public Cell(UIView view, Position position)
				{
					this.View = view;
					this.InitialSize = this.View.Bounds.Size;
					this.Position = position;
				}

				public CGSize InitialSize { get; private set; }

				public UIView View { get; private set; }

				public Position Position { get; private set; }
			}
		}
	}
}
