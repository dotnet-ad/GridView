namespace UIKit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CoreGraphics;

	// var grid = new Grid { Spacing = 5, Padding = 5}
	// 				       .WithRows(0.5,0.25,0.25);
	// 				       .WithColumns(0.5,0.25,0.25);
	// grid[1][2] = myview;

	public class Grid : UIView
	{
		public class Layout
		{
			public struct Definition
			{
				public Definition(float size)
				{
					this.Size = size;
				}

				public float Size { get; private set; }
			}

			public struct Position
			{
				public Position(int row, int column, int rowspan = 1, int columnspan = 1)
				{
					this.Row = row;
					this.Column = column;
					this.RowSpan = rowspan;
					this.ColumnSpan = columnspan;
				}

				public int Row { get; set; }

				public int Column { get; set; }

				public int RowSpan { get; set; }

				public int ColumnSpan { get; set; }
			}

			public class RowAccess
			{
				public RowAccess(Layout layout, int index, int span = 1)
				{
					this.Layout = layout;
					this.Index = index;
					this.Span = span;
				}

				public Layout Layout { get; private set; }

				public int Index { get; private set; }

				public int Span { get; private set; }

				public UIView this[int column]
				{
					set { this.Layout.AddCell(value, this.Index, column, this.Span); }
				}

				public UIView this[int column, int columnspan]
				{
					set { this.Layout.AddCell(value, this.Index, column, this.Span, columnspan); }
				}

			}

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

			private Dictionary<UIView, Position> cells = new Dictionary<UIView, Position>();

			public IDictionary<UIView, Position> Cells => new Dictionary<UIView, Position>(cells);

			public Layout AddCell(UIView view, int row, int column, int rowspan = 1, int columnspan = 1)
			{
				this.cells[view] = new Position(row, column, rowspan, columnspan);
				return this;
			}

			public Position GetPosition(UIView cell) => this.cells[cell];

			#endregion

			#region Operators

			public RowAccess this[int i] => new RowAccess(this, i);

			public RowAccess this[int i, int span] => new RowAccess(this, i, span);

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
		}

		public Grid()
		{
			
		}

		#region Triggers

		#endregion

		#region Layout

		private Layout currentLayout;

		private List<Layout> layouts = new List<Layout>();

		public Layout CurrentLayout 
		{ 
			get 
			{
				var layout = this.layouts.FirstOrDefault(l => l.Trigger?.Invoke(this.Frame.Width, this.Frame.Height) ?? false) ?? this.layouts.FirstOrDefault(l => l.Trigger == null);

				if (currentLayout != layout)
				{
					this.currentLayout = layout;

					if (layout != null)
					{
						foreach (var cell in this.Subviews.Where(v => !layout.Cells.ContainsKey(v)))
						{
							cell.RemoveFromSuperview();
						}

						foreach (var cell in layout.Cells.Where(v => !this.Subviews.Contains(v.Key)))
						{
							this.AddSubview(cell.Key);
						}
					}
				}

				return layout; 
			} 
		}

		public void AddLayout(Layout layout, Func<nfloat,nfloat,bool> trigger = null)
		{
			layout.Trigger = trigger;
			this.layouts.Add(layout);
		}

		private CGPoint GetCellAbsolutePosition(nfloat[] absoluteColumnWidth, nfloat[] absoluteRowHeight, Layout.Position pos)
		{
			var position = new CGPoint(pos.Column * this.CurrentLayout.Spacing, pos.Row * this.CurrentLayout.Spacing);

			position.X += this.CurrentLayout.Padding.Left;
			position.Y += this.CurrentLayout.Padding.Top;

			for (int i = 0; i < pos.Column; i++)
				position.X += absoluteColumnWidth[i];

			for (int i = 0; i < pos.Row; i++)
				position.Y += absoluteRowHeight[i];

			return position;
		}

		private CGSize GetCellAbsoluteSize(nfloat[] absoluteColumnWidth, nfloat[] absoluteRowHeight, Layout.Position pos)
		{
			var size = new CGSize((pos.ColumnSpan - 1) * this.CurrentLayout.Spacing, (pos.RowSpan - 1) * this.CurrentLayout.Spacing);

			for (int i = 0; i < pos.ColumnSpan; i++)
				size.Width += absoluteColumnWidth[pos.Column + i];

			for (int i = 0; i < pos.RowSpan; i++)
				size.Height += absoluteRowHeight[pos.Row + i];

			return size;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			// Calculating sizes
			var absoluteRowHeight = this.CurrentLayout.CalculateAbsoluteRowHeight(this.Frame.Height);
			var absoluteColumnWidth = this.CurrentLayout.CalculateAbsoluteColumnWidth(this.Frame.Width);

			// Layout subviews
			foreach (var cell in this.CurrentLayout.Cells)
			{
				var position = GetCellAbsolutePosition(absoluteColumnWidth, absoluteRowHeight, cell.Value);
				var size = GetCellAbsoluteSize(absoluteColumnWidth, absoluteRowHeight, cell.Value);
				cell.Key.Frame = new CGRect(position, size);
			}
		}

		#endregion

	}
}
