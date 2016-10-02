// Licensed under GPLv3 license or under special license
// See the LICENSE file in the project root for more information
// -----------------------------------------------------------------------
// Author: Plastic Block <admin@plasticblock.xyz>
// skype: plasticblock, email: support@plasticblock.xyz
// project: Fractal Caves Generator (FCG)
// -----------------------------------------------------------------------
namespace FCG
{
	/// <summary>
	/// Matrix base.
	/// </summary>
	public class MatrixBase
	{
		/// <summary>
		/// Matrix base.
		/// </summary>
		/// <param name="size">Size.</param>
		public MatrixBase(Vector2Int size)
		{
			this.size = size;
		}

		/// <summary>
		/// Cells.
		/// </summary>
		public virtual bool[,] Cells
		{
			get { return cells; }
			set { cells = value; }
		}
		/// <summary>
		/// Matrix size.
		/// </summary>
		public Vector2Int Size
		{
			get { return size; }
		}

		/// <summary>
		/// Matrix element default status.
		/// </summary>
		public bool DefaultStatus
		{
			get { return defaultStatus; }
		}

        /// <summary>
        /// Matrix cells.
        /// </summary>
        protected bool[,] cells;

		/// <summary>
		/// Matrix size.
		/// </summary>
		protected Vector2Int size;

        /// <summary>
        /// Matrix element default status.
        /// </summary>
        protected bool defaultStatus;
	}
}
