// Licensed under GPLv3 license or under special license
// See the LICENSE file in the project root for more information
// -----------------------------------------------------------------------
// Author: Jasur Sadikov <contact@plasticblock.xyz>
// Skype: plasticblock, E-mail: contact@plasticblock.xyz
// Project: Random Walk Caves Generator (RWCG)
// -----------------------------------------------------------------------

using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace vmp1r3.RandomWalkCavesGenerator
{
	/// <summary>
	/// Environment Generator. Use this component for initialization.
	/// </summary>
	public class EnvironmentGenerator : MonoBehaviour
	{
		[SerializeField]
		private bool _generateRandomSeed;

		[SerializeField]
		private int _seed;

		[SerializeField]
		private Vector2Int _size;

		[SerializeField]
		private int _length;

		[SerializeField]
		private int _border;

		[SerializeField]
		private int _chunkSize;

		[SerializeField]
		private float _scale = 1f;

		[SerializeField]
		private Transform _parent;

		private MatrixBase _matrix;

		/// <summary>
		/// Invokes when generation is complete.
		/// </summary>
		public event Action<EnvironmentGenerator> OnGenerationEnd = delegate { };

		private MatrixBase[,] _chunks;

		private void Start()
		{
			Generate();
		}

		public void Generate()
		{
			// Destroys all obsolete objects if they exist 
			if (_parent.childCount != 0)
				for (int i = 0; i < _parent.childCount; i++)
				{
					Destroy(_parent.GetChild(i).gameObject);
				}

			_seed = _generateRandomSeed ? Random.Range(1000000, 9999999) : _seed;

			var generator = new FractalGenerator(_size, new Vector2Int(_border, _border), _seed, _length);
			generator.Generate();

			_matrix = generator;

			var chunksCount = new Vector2Int(_matrix.Size.x, _matrix.Size.y) / _chunkSize;
			_chunks = new MatrixBase[_matrix.Size.x / _chunkSize + 1, _matrix.Size.y / _chunkSize + 1];

			// Chunks generation.
			for (int y = 0; y <= chunksCount.y; y++)
				for (int x = 0; x <= chunksCount.x; x++)
				{
					_chunks[x, y] = new MatrixBase(new Vector2Int(_chunkSize, _chunkSize));

					// Chunk generation.

					for (int matrixY = 0; matrixY < _chunkSize; matrixY++)
						for (int matrixX = 0; matrixX < _chunkSize; matrixX++)
							_chunks[x, y].Cells[matrixX, matrixY] =
								_matrix.Cells[matrixX + x * (_chunkSize - 1), matrixY + y * (_chunkSize - 1)];

					var meshGenerator = new GameObject().AddComponent<MeshGenerator>();

					meshGenerator.gameObject.name = string.Format("({0}, {1})", x, y);
					meshGenerator.transform.position =
						new Vector3((_chunkSize - 1) * x * _scale, (_chunkSize - 1) * y * _scale, 0) + _parent.position;
					meshGenerator.transform.eulerAngles = new Vector3(0, 0, 270);
					meshGenerator.transform.localScale = new Vector3(-1, 1, -1);
					meshGenerator.transform.SetParent(_parent);
					meshGenerator.chunk = _chunks[x, y];
					meshGenerator.gameObject.AddComponent<MeshRenderer>();
					meshGenerator.filter = meshGenerator.gameObject.AddComponent<MeshFilter>();
					meshGenerator.scale = _scale;
					meshGenerator.Begin();
				}

			OnGenerationEnd(this);
		}

#if UNITY_EDITOR

		private void OnDrawGizmos()
		{
			try
			{
				var height = _scale * (_size.y - 1);
				var width = _scale * (_size.x - 1);
				var chunksCount = new Vector2Int(_size.x, _size.y) / _chunkSize;

				Gizmos.color = Color.cyan;

				for (int y = 0; y <= chunksCount.y + 1; y++)
					Gizmos.DrawLine(new Vector3(_parent.position.x, _parent.position.y + (_chunkSize - 1) * y * _scale),
					                new Vector3(_parent.position.x + (_size.x - 1) * _scale,
					                            _parent.position.y + (_chunkSize - 1) * y * _scale));

				for (int x = 0; x <= chunksCount.x + 1; x++)
					Gizmos.DrawLine(new Vector3(_parent.position.x + (_chunkSize - 1) * x * _scale, _parent.position.y),
					                new Vector3(_parent.position.x + (_chunkSize - 1) * x * _scale,
					                            _parent.position.y + (_size.y - 1) * _scale));

				Gizmos.color = Color.white;

				if (_border > 0)
				{
					Gizmos.DrawLine(new Vector3(-1 + _border + _parent.position.x, _parent.position.y),
					                new Vector3(-1 + _border + _parent.position.x, _parent.position.y + height));
					Gizmos.DrawLine(new Vector3(1 + width - _border + _parent.position.x, _parent.position.y),
					                new Vector3(1 + width - _border + _parent.position.x, _parent.position.y + height));
					Gizmos.DrawLine(new Vector3(_parent.position.x, -1 + _border + _parent.position.y),
					                new Vector3(_parent.position.x + width, -1 + _border + _parent.position.y));
					Gizmos.DrawLine(new Vector3(_parent.position.x, 1 + width - _border + _parent.position.y),
					                new Vector3(_parent.position.x + width, 1 + width - _border + _parent.position.y));
				}
			}
			catch
			{
				/* ignored */
			}
		}

#endif
	}
}