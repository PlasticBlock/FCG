using UnityEngine;

namespace vmp1r3.CavesGenerator
{
	public sealed class MeshBuilder : MonoBehaviour
	{
		private Mesh mesh;
		private Vector2Int size;
		private float scale;
		private bool[,] rule;
		private int[,] indices;
		
		public Mesh Build(Vector2Int size, bool[,] rule, float scale)
		{
			this.size = size;
			this.rule = rule;
			this.scale = scale;
			
			mesh = new Mesh();

			mesh.vertices = GetVertices();
			mesh.triangles = GetTriangles(); 
			mesh.normals = GetNormals();
			mesh.uv = GetUVs();
			mesh.Optimize();

			return mesh;
		}
		
		private Vector3[] GetVertices()
		{
			var vertices = new Vector3[size.x * size.y];
			indices = new int[size.x, size.y];

			for (int y = 0, i = 0; y < size.y; y++)
				for (int x = 0; x < size.x; x++, i++)
					vertices[indices[x, y] = i] = new Vector3(x * scale, y * scale);

			return vertices;
		}
		
		private int[] GetTriangles()
		{
			var triangles = new int[size.x * size.y * 6];

			for (int y = 0, i = 0; y < size.y - 1; y++)
				for (int x = 0; x < size.x - 1; x++)
				{
					var a = !rule[x, y];
					var b = !rule[x, y + 1];
					var c = !rule[x + 1, y + 1];
					var d = !rule[x + 1, y];

					//  b---c
					//  | / |
					//  a---d

					if (a && b && c)
					{
						triangles[i++] = indices[x, y];
						triangles[i++] = indices[x, y + 1];
						triangles[i++] = indices[x + 1, y + 1];
					}

					if (a && d && c)
					{
						triangles[i++] = indices[x, y];
						triangles[i++] = indices[x + 1, y + 1];
						triangles[i++] = indices[x + 1, y];
					}

					if (a && b && c && d)
						continue;
					
					//  b---c
					//  | \ |
					//  a---d

					if (a && b && d)
					{
						triangles[i++] = indices[x, y];
						triangles[i++] = indices[x, y + 1];
						triangles[i++] = indices[x + 1, y];
					}

					if (d && b && c)
					{
						triangles[i++] = indices[x + 1, y];
						triangles[i++] = indices[x, y + 1];
						triangles[i++] = indices[x + 1, y + 1];
					}
				}

			return triangles;
		}
		
		private Vector3[] GetNormals()
		{
			var normals = new Vector3[mesh.vertexCount];

			for (int i = 0; i < normals.Length; i++)
				normals[i] = mesh.vertices[i] + Vector3.back;

			return normals;
		}
		
		private Vector2[] GetUVs()
		{
			var uvs = new Vector2[size.x * size.y];

			for (int y = 0, i = 0; y < size.y; y++)
				for (int x = 0; x < size.x; x++, i++)
					uvs[i] = new Vector2(1f / size.x * x, 1f / size.y * y);

			return uvs;
		}

#if UNITY_EDITOR
		
		void OnDrawGizmosSelected()
		{
			try
			{
				Gizmos.matrix = transform.localToWorldMatrix;
				
				Gizmos.color = Color.gray;
				Gizmos.DrawWireMesh(mesh);

				for (int y = 0; y < size.y; y++)
					for (int x = 0; x < size.x; x++)
					{
						Gizmos.color = rule[x, y] ? Color.white : Color.black;
						Gizmos.DrawCube(new Vector3(x, y) * scale, scale * 0.3f * Vector3.one);
					}

				Gizmos.color = Color.yellow;

				for (int v = 0; v < mesh.vertexCount; v++)
					Gizmos.DrawLine(mesh.vertices[v], mesh.normals[v]);
			}
			catch { }
		}

#endif
	}
}