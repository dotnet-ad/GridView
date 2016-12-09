namespace UIKit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CoreGraphics;

	public partial class Grid : UIView
	{
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
						foreach (var cell in this.Subviews.Where(v => !layout.Cells.Any(c => c.View == v)))
						{
							cell.RemoveFromSuperview();
						}

						foreach (var cell in layout.Cells.Where(v => !this.Subviews.Contains(v.View)))
						{
							this.AddSubview(cell.View);
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
				var position = GetCellAbsolutePosition(absoluteColumnWidth, absoluteRowHeight, cell.Position);
				var size = GetCellAbsoluteSize(absoluteColumnWidth, absoluteRowHeight, cell.Position);
				cell.View.Frame = new CGRect(position, size);
			}
		}

		#endregion

	}
}
