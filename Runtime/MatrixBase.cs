// Licensed under GPLv3 license or under special license
// See the LICENSE file in the project root for more information
// -----------------------------------------------------------------------
// Author: Jasur Sadikov <contact@plasticblock.xyz>
// Skype: plasticblock, E-mail: contact@plasticblock.xyz
// Project: Random Walk Caves Generator (RWCG)
// -----------------------------------------------------------------------

using UnityEngine;

namespace vmp1r3.RandomWalkCavesGenerator
{
	/// <summary>
	/// Matrix base.
	/// </summary>
	public class MatrixBase
	{
		/// <summary>
		/// Matrix base.
		/// </summary>
		/// <param name="size">Matrix size.</param>
		public MatrixBase(Vector2Int size)
		{
			Size = size;
			Cells = new bool[size.x, size.y];
		}

		/// <summary>
		/// Cells.
		/// </summary>
		public bool[,] Cells { get; }

		/// <summary>
		/// Matrix size.
		/// </summary>
		public Vector2Int Size { get; }
	}
}