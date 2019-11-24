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
	/// Abstract mesh generator.
	/// </summary>
	public abstract class AbstractMeshGenerator : MonoBehaviour
	{
		/// <summary>
		/// Generated mesh. Null if mesh was not generated.
		/// </summary>
		protected Mesh mesh;

		/// <summary>
		/// Generates mesh.
		/// </summary>
		protected void Generate()
		{
			CreateMesh();
			CalculateVertices();
			CalculateTriangles();
			CalculateNormals();
			CalculateUv();
		}

		/// <summary>
		/// Mesh generation pre-process.
		/// </summary>
		private void CreateMesh()
		{
			mesh = new Mesh();
			mesh.Optimize();
			mesh.MarkDynamic();
			mesh.name = "Chunk";
		}

		/// <summary>
		/// Calculates vertices.
		/// </summary>
		protected abstract void CalculateVertices();

		/// <summary>
		/// Calculates triangles.
		/// </summary>
		protected abstract void CalculateTriangles();

		/// <summary>
		/// Calculates normals.
		/// </summary>
		protected abstract void CalculateNormals();

		/// <summary>
		/// Calculates UVs.
		/// </summary>
		protected abstract void CalculateUv();
	}
}