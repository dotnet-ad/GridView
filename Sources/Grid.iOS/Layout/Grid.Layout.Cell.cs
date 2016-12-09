namespace UIKit
{
	public partial class Grid : UIView
	{
		public partial class Layout
		{
			public class Cell
			{
				public Cell(UIView view, Position position)
				{
					this.View = view;
					this.Position = position;
				}

				public UIView View { get; private set; }

				public Position Position { get; private set; }
			}
		}
	}
}
