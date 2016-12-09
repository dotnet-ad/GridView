namespace UIKit
{
	public partial class Grid : UIView
	{
		public struct Insets
		{
			public Insets(float space) : this(space,space)
			{
				
			}

			public Insets(float horizontal, float vertical) : this(horizontal, vertical, horizontal, vertical)
			{

			}

			public Insets(float left, float top, float right, float bottom)
			{
				this.Left = left;
				this.Right = right;
				this.Top = top;
				this.Bottom = bottom;
			}

			public float Left { get; private set; }

			public float Right { get; private set; }

			public float Top { get; private set; }

			public float Bottom { get; private set; }
		}
	}
}
