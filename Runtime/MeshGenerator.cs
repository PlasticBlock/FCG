// Licensed under GPLv3 license or under special license
// See the LICENSE file in the project root for more information
// -----------------------------------------------------------------------
// Author: Jasur Sadikov <contact@plasticblock.xyz>
// Skype: plasticblock, E-mail: contact@plasticblock.xyz
// Project: Random Walk Caves Generator (RWCG)
// -----------------------------------------------------------------------

using UnityEngine;
using System.Linq;

namespace vmp1r3.RandomWalkCavesGenerator
{
	/// <summary>
	/// Mesh generator.
	/// </summary>
	public sealed class MeshGenerator : AbstractMeshGenerator
	{
		/// <summary>
		/// Matrix.
		/// </summary>
		public MatrixBase chunk;

		/// <summary>
		/// Current element scale.
		/// </summary>
		public float scale = 1f;

		/// <summary>
		/// MeshFilter.
		/// </summary>
		public MeshFilter filter;

		/// <summary>
		/// Vertices.
		/// </summary>
		private Vertex[,] _vertices;

		/// <summary>
		/// Initialization point.
		/// </summary>
		public void Begin()
		{
			Generate();
			filter.mesh = mesh;
		}

		/// <summary>
		/// Generating vertices.
		/// </summary>
		protected override void CalculateVertices()
		{
			var unityVertices = new Vector3[chunk.Size.x, chunk.Size.y];
			_vertices = new Vertex[chunk.Size.x, chunk.Size.y];

			for (int y = 0, i = 0; y < chunk.Size.y; y++)
				for (int x = 0; x < chunk.Size.x; x++, i++)
				{
					unityVertices[x, y] = new Vector3(x * scale, y * scale);
					_vertices[x, y] = new Vertex(unityVertices[x, y], new Vector2Int(x, y), i, chunk.Cells[x, y]);
				}

			mesh.vertices = unityVertices.Cast<Vector3>().ToArray();
		}

		/// <summary>
		/// Generating triangles.
		/// </summary>
		protected override void CalculateTriangles()
		{
			int[] triangles = new int[(chunk.Size.x) * (chunk.Size.y) * 6];

			for (int y = 0, i = 0; y < chunk.Size.y - 1; y++)
				for (int x = 0; x < chunk.Size.x - 1; x++)
				{
					bool a = !chunk.Cells[x, y];
					bool b = !chunk.Cells[x, y + 1];
					bool c = !chunk.Cells[x + 1, y + 1];
					bool d = !chunk.Cells[x + 1, y];

					//  b---c
					//  | / |
					//  a---d

					if (a && b && c)
					{
						triangles[i++] = _vertices[x, y].meshId;
						triangles[i++] = _vertices[x, y + 1].meshId;
						triangles[i++] = _vertices[x + 1, y + 1].meshId;
					}

					if (a && d && c)
					{
						triangles[i++] = _vertices[x, y].meshId;
						triangles[i++] = _vertices[x + 1, y + 1].meshId;
						triangles[i++] = _vertices[x + 1, y].meshId;
					}

					if (a && b && c && d)
						continue;


					//  b---c
					//  | \ |
					//  a---d

					if (a && b && d)
					{
						triangles[i++] = _vertices[x, y].meshId;
						triangles[i++] = _vertices[x, y + 1].meshId;
						triangles[i++] = _vertices[x + 1, y].meshId;
					}

					if (d && b && c)
					{
						triangles[i++] = _vertices[x + 1, y].meshId;
						triangles[i++] = _vertices[x, y + 1].meshId;
						triangles[i++] = _vertices[x + 1, y + 1].meshId;
					}
				}

			mesh.triangles = triangles.ToArray();
		}

		/// <summary>
		/// Generating normals.
		/// </summary>
		protected override void CalculateNormals()
		{
			var normals = new Vector3[mesh.vertexCount];

			for (int i = 0; i < normals.Length; i++)
				normals[i] = mesh.vertices[i] + Vector3.back;

			mesh.normals = normals;
		}

		/// <summary>
		/// Generating UVs.
		/// </summary>
		protected override void CalculateUv()
		{
			var uvs = new Vector2[chunk.Size.x, chunk.Size.y];

			for (int y = 0; y < chunk.Size.y; y++)
				for (int x = 0; x < chunk.Size.x; x++)
					uvs[x, y] = new Vector2(1f / chunk.Size.x * x, 1f / chunk.Size.y * y);

			mesh.uv = uvs.Cast<Vector2>().ToArray();
		}

		private void OnDrawGizmosSelected()
		{
			try
			{
				Gizmos.color = Color.gray;
				Gizmos.DrawWireMesh(mesh, transform.position, transform.rotation, transform.localScale);

				for (int y = 0; y < chunk.Size.y; y++)
					for (int x = 0; x < chunk.Size.x; x++)
					{
						Gizmos.color = chunk.Cells[x, y] ? Color.green : Color.black;
						Gizmos.DrawCube(transform.position + new Vector3(x, y) * scale, scale * 0.3f * Vector3.one);
					}

				Gizmos.color = Color.yellow;

				for (int v = 0; v < mesh.vertexCount; v++)
					Gizmos.DrawLine(mesh.vertices[v] + transform.position, mesh.normals[v] + transform.position);
			}
			catch { }
		}
	}
}