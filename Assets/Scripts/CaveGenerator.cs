using UnityEngine;

namespace vmp1r3.CavesGenerator
{
	[AddComponentMenu("Caves Generator/Generator")]
	public sealed class CaveGenerator : MonoBehaviour
	{
		[SerializeField] private Vector2Int size = new Vector2Int(64, 64);
		[SerializeField] private int boundary = 8;
		[SerializeField] private int splits = 16;
		[SerializeField] private float scale = 1f;
		[SerializeField] private int caveCells = 666;
		[SerializeField] private bool useAnySeed;
		[SerializeField] private int seed = 420;

		private Cave cave;
		private bool[,][,] blocks;

		private void Start() => Generate();

		public void Generate()
		{
			// deleting last result 
			if (transform.childCount != 0)
				for (int i = 0; i < transform.childCount; i++)
					Destroy(transform.GetChild(i).gameObject);

			cave = new Cave(size, new Vector2Int(boundary, boundary), useAnySeed ? new System.Random() : new System.Random(seed), caveCells);
			blocks = new bool[splits, splits][,];

			var blockSize = new Vector2Int(size.x / splits, size.y / splits);

			for (int y = 0; y < splits; y++)
				for (int x = 0; x < splits; x++)
				{
					blocks[x, y] = new bool[blockSize.x, blockSize.y];

					for (int mY = 0; mY < blockSize.y; mY++)
						for (int mX = 0; mX < blockSize.x; mX++)
							blocks[x, y][mX, mY] = cave.Cells[x * (blockSize.x - 1) + mX, y * (blockSize.y - 1) + mY];

					var block = new GameObject().AddComponent<MeshBuilder>();
					var blockTransform = block.transform;
					var mesh = block.Build(blockSize, blocks[x, y], scale);
					blockTransform.SetParent(transform);
					blockTransform.localPosition = ((Vector2) blockSize - Vector2Int.one) * new Vector2Int(x, y) * scale;
					
					block.gameObject.AddComponent<MeshRenderer>();
					block.gameObject.AddComponent<MeshFilter>().mesh = mesh;
				}
		}

#if UNITY_EDITOR

		void OnDrawGizmos()
		{
			try
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireCube(Vector2.one * size / 2f, (Vector2) size);
				
				var blockSize = new Vector2((float) size.x / splits, (float) size.y / splits);

				for (int y = 0; y < splits; y++)
					for (int x = 0; x < splits; x++)
						Gizmos.DrawWireCube(blockSize / 2f + blockSize * new Vector2(x, y), blockSize);

				if (boundary <= 0)
					return;
				
				Gizmos.color = Color.yellow * 0.75f;
				Gizmos.matrix = transform.localToWorldMatrix;
				Gizmos.DrawWireCube((Vector2) size / 2f, (Vector2) (size - Vector2Int.one * boundary));
			}
			catch { }
		}

#endif
	}
}