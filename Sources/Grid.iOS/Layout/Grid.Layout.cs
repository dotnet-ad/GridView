namespace UIKit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public partial class Grid : UIView
	{
		public partial class Layout
		{
			#region Margins

			public UIEdgeInsets Padding { get; set; }

			public float Spacing { get; set; }

			#endregion

			#region Trigger

			public Func<nfloat, nfloat, bool> Trigger { get; set; }

			#endregion

			#region Definitions

			public IEnumerable<Definition> ColumnDefinitions { get; private set; } = new[] { new Definition(1) };

			public IEnumerable<Definition> RowDefinitions { get; private set; } = new[] { new Definition(1) };

			public Layout WithRows(params float[] rows)
			{
				this.RowDefinitions = rows.Select(size => new Definition(size));
				return this;
			}

			public Layout WithColumns(params float[] columns)
			{
				this.ColumnDefinitions = columns.Select(size => new Definition(size));
				return this;
			}

			#endregion

			#region Cells

			private List<Cell> cells = new List<Cell>();

			public IEnumerable<Cell> Cells => cells.ToArray();

			public Layout Add(Cell cell)
			{
				this.cells.Add(cell);
				return this;
			}

			public Position GetPosition(UIView cell) => this.cells.First(c => c.View == cell).Position;

			#endregion

			#region Absolute sizes

			public nfloat[] CalculateAbsoluteColumnWidth(nfloat totalWidth)
			{
				var absoluteColumnWidth = new nfloat[this.ColumnDefinitions.Count()];

				var remaining = totalWidth - this.ColumnDefinitions.Where((d) => d.Size > 1).Select(d => d.Size).Sum();
				remaining -= this.Padding.Left + this.Padding.Right;
				remaining -= (this.ColumnDefinitions.Count() - 1) * this.Spacing;
				remaining = (nfloat)Math.Max(0, remaining);

				for (int column = 0; column < this.ColumnDefinitions.Count(); column++)
				{
					var definition = this.ColumnDefinitions.ElementAt(column);
					absoluteColumnWidth[column] = definition.Size > 1 ? definition.Size : definition.Size * remaining;
				}

				return absoluteColumnWidth;
			}

			public nfloat[] CalculateAbsoluteRowHeight(nfloat totalHeight)
			{
				var absoluteRowHeight = new nfloat[this.RowDefinitions.Count()];

				var remaining = totalHeight - this.RowDefinitions.Where((d) => d.Size > 1).Select(d => d.Size).Sum();
				remaining -= this.Padding.Top + this.Padding.Bottom;
				remaining -= (this.RowDefinitions.Count() - 1) * this.Spacing;
				remaining = (nfloat)Math.Max(0, remaining);

				for (int row = 0; row < this.RowDefinitions.Count(); row++)
				{
					var definition = this.RowDefinitions.ElementAt(row);
					absoluteRowHeight[row] = definition.Size > 1 ? definition.Size : definition.Size * remaining;
				}

				return absoluteRowHeight;
			}

			#endregion

			#region Operator

			public static Layout operator +(Layout layout, Cell cell)
			{
				layout.Add(cell);
				return layout;
			}

			#endregion
		}
	}
}
