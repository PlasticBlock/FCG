// Licensed under GPLv3 license or under special license
// See the LICENSE file in the project root for more information
// -----------------------------------------------------------------------
// Author: Plastic Block <admin@plasticblock.xyz>
// skype: plasticblock, email: support@plasticblock.xyz
// project: Fractal Caves Generator (FCG)
// -----------------------------------------------------------------------
using UnityEngine;

namespace FCG
{
	/// <summary>
	/// Vertex.
	/// </summary>
	public struct Vertex
	{
		/// <summary>
		/// Vertex.
		/// </summary>
		/// <param name="spacePosition">Position in space.</param>
		/// <param name="position">Position in matrix.</param>
		/// <param name="meshId">Mesh index.</param>
		/// <param name="status">Current vertex status.</param>
		public Vertex(Vector3 spacePosition, Vector2Int position, int meshId, bool status)
		{
			this.spacePosition = spacePosition;
			this.position = position;
			this.meshId = meshId;
			this.status = status;
		}

		/// <summary>
		/// Position in space.
		/// </summary>
		public Vector3 spacePosition;

		/// <summary>
		/// Position in matrix.
		/// </summary>
		public Vector2Int position;

		/// <summary>
		/// Mesh index.
		/// </summary>
		public int meshId;

		/// <summary>
		/// Current vertex status.
		/// </summary>
		public bool status;
	}
}
