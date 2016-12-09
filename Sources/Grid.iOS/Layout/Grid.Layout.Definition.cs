namespace UIKit
{
	public partial class Grid : UIView
	{
		public partial class Layout
		{
			public struct Definition
			{
				public Definition(float size)
				{
					this.Size = size;
				}

				public float Size { get; private set; }
			}
		}
	}
}
