namespace UIKit
{
	public partial class Grid : UIView
	{
		public partial class Layout
		{
			public struct Position
			{
				public Position(int row, int column, int rowspan = 1, int columnspan = 1)
				{
					this.Row = row;
					this.Column = column;
					this.RowSpan = rowspan;
					this.ColumnSpan = columnspan;
				}

				public int Row { get; private set; }

				public int Column { get; private set; }

				public int RowSpan { get; private set; }

				public int ColumnSpan { get; private set; }
			}
		}
	}
}
