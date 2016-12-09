namespace UIKit
{
	public partial class Grid : UIView
	{
		public partial class Layout
		{
			public struct Position
			{
				public Position(Position other)
				{
					this.Row = other.Row;
					this.Column = other.Column;
					this.RowSpan = other.RowSpan;
					this.ColumnSpan = other.ColumnSpan;
					this.Horizontal = other.Horizontal;
					this.Vertical = other.Vertical;
				}

				public Position(int row, int column)
				{
					this.Row = row;
					this.Column = column;
					this.RowSpan = 1;
					this.ColumnSpan = 1;
					this.Horizontal = default(Alignment);
					this.Vertical = default(Alignment);
				}

				public int Row { get; set; }

				public int Column { get; set; }

				public int RowSpan { get; set; }

				public int ColumnSpan { get; set; }

				public Alignment Vertical { get; set; }

				public Alignment Horizontal { get; set; }
			}
		}
	}
}
