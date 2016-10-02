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
	/// Abstract mesh generator. Base of MeshGenerator.
	/// </summary>
	public abstract class AbstractMeshGenerator : MonoBehaviour
	{
		/// <summary>
		/// Mesh.
		/// </summary>
		protected Mesh mesh;

		/// <summary>
		/// Generation Start Point.
		/// </summary>
		protected virtual void Generate()
		{
#if DEBUG
			Debug.Log("Mesh. Generation Started.");
#endif
			Precalculation();
			CalculateVertices();
			CalculateTriangles();
			CalculateNormals();
			CalculateUv();
		}

		/// <summary>
		/// Mesh generation pre-process.
		/// </summary>
		protected virtual void Precalculation()
		{
#if DEBUG
			Debug.Log("Mesh. Precalculating Mesh.");
#endif
			mesh = new Mesh();
			mesh.Optimize();
			mesh.MarkDynamic();
			mesh.name = "Chunk";
		}

		/// <summary>
		/// Generating vertices.
		/// </summary>
		protected abstract void CalculateVertices();

		/// <summary>
		/// Generating triangles.
		/// </summary>
		protected abstract void CalculateTriangles();

		/// <summary>
		/// Generating normals.
		/// </summary>
		protected abstract void CalculateNormals();

		/// <summary>
		/// Generating UVs.
		/// </summary>
		protected abstract void CalculateUv();
	}
}