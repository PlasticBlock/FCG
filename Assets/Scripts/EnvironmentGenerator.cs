// Licensed under GPLv3 license or under special license
// See the LICENSE file in the project root for more information
// -----------------------------------------------------------------------
// Author: Plastic Block <admin@plasticblock.xyz>
// skype: plasticblock, email: support@plasticblock.xyz
// project: Fractal Caves Generator (FCG)
// -----------------------------------------------------------------------

using System;
using UnityEngine;
using Random = UnityEngine.Random;

// NOTICE: Use this component for initialization.
// Create GameObject, select Add Component, and add this component.

namespace FCG
{
	/// <summary>
	/// Environment Generator. Use this component for initialization.
	/// </summary>
	public class EnvironmentGenerator : MonoBehaviour
	{
		/// <summary>
		/// Matrix.
		/// </summary>
		public Matrix matrix;

		/// <summary>
		/// Generating random seed for every instance. 
		/// </summary>
		public bool generateRandomSeed;

		/// <summary>
		/// Random seed. [Random.seed value]
		/// </summary>
		public int seed;

		///<summary>
		/// Cave size.
		///</summary>
		public Vector2Int size;

		/// <summary>
		/// Cave length. (amount of unfillable cells)
		/// </summary>
		public int caveLength;

		/// <summary>
		/// Border. Cave would not be generated out of this border.
		/// </summary>
		public int border;

		/// <summary>
		/// Size of each chunk (a*a).
		/// </summary>
		public int chunkSize;

		/// <summary>
		/// Scale.
		/// </summary>
		public float scale = 1f;

		/// <summary>
		/// transform.parent foreach generated GameObject.
		/// </summary>
		public Transform pack;

		/// <summary>
		/// On Generating End event.
		/// </summary>
		public event Action OnGenerationEnd = delegate { };

		private MatrixBase[,] _chunks;

		// MonoBehaviour Script Initialization point.
		private void Start()
		{
			Generate();
		}

		/// <summary>
		/// Initialization point.
		/// </summary>
		public void Generate()
		{
			// Deleting all of the children of pack gameObject (deleting previous cave if it was generated).
			if (pack.childCount != 0)
				for (int i = 0; i < pack.childCount; i++)
				{
					Destroy(pack.GetChild(i).gameObject);
				}

#if DEBUG
			Debug.Log("Environment. Environment generation started.");
#endif
			seed = generateRandomSeed ? Random.Range(1000000, 9999999) : seed;

			// Matrix construction.
			matrix = new FractalGenerator(size, seed) {border = new Vector2Int(border, border), caveLength = caveLength};
			// Cave generation.
			matrix.Generate();

			Vector2Int chunksCount = new Vector2Int(matrix.Size.x, matrix.Size.y)/chunkSize;
			_chunks = new MatrixBase[matrix.Size.x/chunkSize + 1, matrix.Size.y/chunkSize + 1];

			// Chunks generation.
			for (int y = 0; y <= chunksCount.y; y++)
				for (int x = 0; x <= chunksCount.x; x++)
				{
#if DEBUG
					Debug.Log(string.Format("Chunk ({0}, {1}) generation started.", x, y));
#endif
					_chunks[x, y] = new MatrixBase(new Vector2Int(chunkSize, chunkSize))
					{
						Cells = new bool[chunkSize , chunkSize]
					};

					// Chunk generation.

					for (int matrixY = 0; matrixY < chunkSize; matrixY++)
						for (int matrixX = 0; matrixX < chunkSize; matrixX++)
							_chunks[x, y].Cells[matrixX, matrixY] = matrix.Cells[matrixX + x * (chunkSize - 1), matrixY + y * (chunkSize - 1)];

					#region Correction post-processes.

					MeshGenerator meshGenerator = new GameObject().AddComponent<MeshGenerator>();
					meshGenerator.gameObject.name = string.Format("({0}, {1})", x, y);
					meshGenerator.transform.position = new Vector3((chunkSize - 1)*x*scale, (chunkSize - 1)*y*scale, 0) + pack.position;
					meshGenerator.transform.eulerAngles = new Vector3(0, 0, 270);
					meshGenerator.transform.localScale = new Vector3(-1, 1, -1);
					meshGenerator.transform.SetParent(pack);
					meshGenerator.chunk = _chunks[x, y];
					meshGenerator.gameObject.AddComponent<MeshRenderer>();
					meshGenerator.filter = meshGenerator.gameObject.AddComponent<MeshFilter>();
					meshGenerator.scale = scale;
					meshGenerator.Begin();
					
					#endregion

#if DEBUG
					Debug.Log(string.Format("Chunk ({0}, {1}). generation ended.", x, y));
#endif
				}

			OnGenerationEnd();

		}

		private void OnDrawGizmos()
		{
			float height = scale * (size.y - 1);
			float width = scale * (size.x - 1);
			Vector2Int chunksCount = new Vector2Int(size.x, size.y) / chunkSize;

			try
			{
				Gizmos.color = Color.cyan;

				for (int y = 0; y <= chunksCount.y + 1; y++)
					Gizmos.DrawLine(new Vector3(pack.position.x, pack.position.y + (chunkSize - 1) * y * scale), new Vector3(pack.position.x + (size.x - 1) * scale, pack.position.y + (chunkSize - 1) * y * scale));

				for (int x = 0; x <= chunksCount.x + 1; x++)
					Gizmos.DrawLine(new Vector3(pack.position.x + (chunkSize - 1) * x * scale, pack.position.y), new Vector3(pack.position.x + (chunkSize - 1) * x * scale, pack.position.y + (size.y - 1) * scale));
			}
			catch { /* ignored */ }
			
			Gizmos.color = Color.white;

			if (border > 0)
			{
				Gizmos.DrawLine(new Vector3(-1 + border + pack.position.x, pack.position.y), new Vector3(-1 + border + pack.position.x, pack.position.y + height));
				Gizmos.DrawLine(new Vector3(1 + width - border + pack.position.x, pack.position.y), new Vector3(1 + width - border + pack.position.x, pack.position.y + height));
				Gizmos.DrawLine(new Vector3(pack.position.x, -1 + border + pack.position.y), new Vector3(pack.position.x + width, -1 + border + pack.position.y));
				Gizmos.DrawLine(new Vector3(pack.position.x, 1 + width - border + pack.position.y), new Vector3(pack.position.x + width, 1 + width - border + pack.position.y));
			}
		}
	}
}
