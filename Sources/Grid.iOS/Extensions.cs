using System;
namespace UIKit
{
	public static class Extensions
	{
		public static Grid.Layout.Cell At(this UIView view, int row, int column) => new Grid.Layout.Cell(view, new Grid.Layout.Position(row, column));

		public static Grid.Layout.Cell Span(this Grid.Layout.Cell cell, int rowspan, int columnspan) => new Grid.Layout.Cell(cell.View, new Grid.Layout.Position(cell.Position) { RowSpan = rowspan, ColumnSpan = columnspan });

		public static Grid.Layout.Cell Vertically(this Grid.Layout.Cell cell, Grid.Layout.Alignment alignment) => new Grid.Layout.Cell(cell.View, new Grid.Layout.Position(cell.Position) { Vertical = alignment });

		public static Grid.Layout.Cell Horizontally(this Grid.Layout.Cell cell, Grid.Layout.Alignment alignment) => new Grid.Layout.Cell(cell.View, new Grid.Layout.Position(cell.Position) { Horizontal = alignment });

	}
}
