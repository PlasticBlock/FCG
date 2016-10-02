// Licensed under GPLv3 license or under special license
// See the LICENSE file in the project root for more information
// -----------------------------------------------------------------------
// Author: Plastic Block <admin@plasticblock.xyz>
// skype: plasticblock, email: support@plasticblock.xyz
// project: Fractal Caves Generator (FCG)
// -----------------------------------------------------------------------
using UnityEngine;
using System.Linq;

namespace FCG
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

#if DEBUG
			Debug.Log(string.Format("\nMesh statistics.\n Vertices = {0}\n Triangles = {1}\n Normals = {2}\n UVs = {3}",
				mesh.vertexCount, mesh.triangles.Length, mesh.normals.Length, mesh.uv.Length));
#endif
		}

		/// <summary>
		/// Generating vertices.
		/// </summary>
		protected override void CalculateVertices()
		{
#if DEBUG
			Debug.Log("Mesh. Calculating Vertices.");
#endif
			Vector3[,] uVertices = new Vector3[chunk.Size.x, chunk.Size.y];
			_vertices = new Vertex[chunk.Size.x, chunk.Size.y];

			for (int y = 0, i = 0; y < chunk.Size.y; y++)
				for (int x = 0; x < chunk.Size.x; x++, i++)
				{
					uVertices[x, y] = new Vector3(x * scale, y * scale);
					_vertices[x, y] = new Vertex(uVertices[x, y], new Vector2Int(x, y), i, chunk.Cells[x, y]);
				}

			mesh.vertices = uVertices.Cast<Vector3>().ToArray();
		}

		/// <summary>
		/// Generating triangles.
		/// </summary>
		protected override void CalculateTriangles()
		{
#if DEBUG
			Debug.Log("Mesh. Calculating Triangles.");
#endif
			int[] triangles = new int[(chunk.Size.x) * (chunk.Size.y) * 6];

			for (int y = 0, i = 0; y < chunk.Size.y - 1; y++)
				for (int x = 0; x < chunk.Size.x - 1; x++)
				{
					bool a = chunk.Cells[x, y] == chunk.DefaultStatus;
					bool b = chunk.Cells[x, y + 1] == chunk.DefaultStatus;
					bool c = chunk.Cells[x + 1, y + 1] == chunk.DefaultStatus;
					bool d = chunk.Cells[x + 1, y] == chunk.DefaultStatus;


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
#if DEBUG
			Debug.Log("Mesh. Calculating Normals.");
#endif
			Vector3[] normals = new Vector3[mesh.vertexCount];

			for (int i = 0; i < normals.Length; i++)
				normals[i] = mesh.vertices[i] + Vector3.back; //Нормаль направляется в заднюю сторону (передняя сторона объекта.).

			mesh.normals = normals;
		}

		/// <summary>
		/// Generating UVs.
		/// </summary>
		protected override void CalculateUv()
		{
#if DEBUG
			Debug.Log("Mesh. Calculating UVs.");
#endif
			Vector2[,] uvs = new Vector2[chunk.Size.x, chunk.Size.y];

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
						Gizmos.DrawCube(transform.position + new Vector3(x, y) * scale, Vector3.one * 0.3f * scale);
					}

				Gizmos.color = Color.yellow;

				for (int v = 0; v < mesh.vertexCount; v++)
					Gizmos.DrawLine(mesh.vertices[v] + transform.position, mesh.normals[v] + transform.position);
			}
			catch { /* ignored */}
		}
	}
}
