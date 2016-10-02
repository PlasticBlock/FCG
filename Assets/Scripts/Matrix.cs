// Licensed under GPLv3 license or under special license
// See the LICENSE file in the project root for more information
// -----------------------------------------------------------------------
// Author: Plastic Block <admin@plasticblock.xyz>
// skype: plasticblock, email: support@plasticblock.xyz
// project: Fractal Caves Generator (FCG)
// -----------------------------------------------------------------------
using System;
using UnityEngine;

namespace FCG
{
    /// <summary>
    /// Matrix.
    /// </summary>
    public class Matrix : MatrixBase
    {
	    public Matrix(Vector2Int size, int seed) : base(size)
	    {
		    this.size = size;
		    this.seed = seed;
	    }

		/// <summary>
		/// On Generation End event.
		/// </summary>
		public static event Action OnGenerationEnd = delegate { };

		/// <summary>
		/// Returning result.
		/// </summary>
		public static event Action<bool[,]> ClientsCalling = delegate { };

		/// <summary>
		/// Random seed [Random.seed].
		/// </summary>
		protected int seed;

		public override bool[,] Cells
		{
			get
			{
				return cells;
			}
			set
			{
				new AccessViolationException("Access denied.");
			}
		}

	    /// <summary>
        /// Generation initialization point.
        /// </summary>
        public virtual void Generate()
        {
            Fill();
            OnGenerationEnd();
            ClientsCalling(cells);
#if DEBUG
			Debug.Log(string.Format("\nMatrix statistics.\n Size = {0}.\n Seed = {1}.\n Default Stage = {2}.", Size, seed, defaultStatus));
#endif
        }

		/// <summary>
		/// Matrix filling.
		/// </summary>
        protected virtual void Fill()
        {
            UnityEngine.Random.seed = seed;
            cells = new bool[size.x, size.y];
			for (int y = 0; y < size.y; y++)
				for (int x = 0; x < size.x; x++)
					cells[x, y] = defaultStatus;
		}

        #region Extension methods.

        /// <summary>
        /// Counting alive neighbors.
        /// </summary>
        /// <param name="position">Cell position.</param>
        /// <returns>Alive neighbors count.</returns>
        protected int CountTrueNeighbors(Vector2Int position)
        {
            if (!position.InRange(Vector2Int.zero, size))
                throw new IndexOutOfRangeException(string.Format("Cell is out of range.\n Chord({0}, {1}) Size ({2}, {3})", position.x, position.y, size.x, size.y));

            int neighbors = 0;

            if (cells[position.x - 1, position.y] == defaultStatus)
                neighbors++;
            if (cells[position.x + 1, position.y] == defaultStatus)
                neighbors++;
            if (cells[position.x, position.y - 1] == defaultStatus)
                neighbors++;
            if (cells[position.x, position.y + 1] == defaultStatus)
                neighbors++;

            return neighbors;
        }

        /// <summary>
        /// Activating/Deactivating group.
        /// + - under control.
        /// X - not under control.
        /// X + X
        /// + + +
        /// X + X
        /// </summary>
        /// <param name="position">Center.</param>
        /// <param name="status">Status [activating/deactivating].</param>
        protected void ActiveGroup(Vector2Int position, bool status)
        {
            cells[position.x - 1, position.y] = status;
            cells[position.x + 1, position.y] = status;
            cells[position.x, position.y - 1] = status;
            cells[position.x, position.y + 1] = status;
        }

        #endregion
    }
}