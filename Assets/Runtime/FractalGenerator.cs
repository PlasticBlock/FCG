// Licensed under GPLv3 license or under special license
// See the LICENSE file in the project root for more information
// -----------------------------------------------------------------------
// Author: Jasur Sadikov <contact@plasticblock.xyz>
// Skype: plasticblock, E-mail: contact@plasticblock.xyz
// Project: Random Walk Caves Generator (RWCG)
// -----------------------------------------------------------------------

using System;
using UnityEngine;
using Random = System.Random;

namespace vmp1r3.RandomWalkCavesGenerator
{
	/// <summary>
	/// Fractal cave generator.
	/// </summary>
	public sealed class FractalGenerator : MatrixBase
	{
		private readonly Random _random;

		private readonly int _maximumCaveLength;

		private readonly Vector2Int _border;

		public FractalGenerator(Vector2Int size, Vector2Int border, int seed, int maximumCaveLength) : base(size)
		{
			_random = new Random(seed);
			_border = border;
			_maximumCaveLength = maximumCaveLength;
		}

		/// <summary>
		/// On Generation End event.
		/// </summary>
		public event Action<MatrixBase> OnGenerationEnd = delegate { };

		/// <summary>
		/// Generation initialize method.
		/// </summary>
		public void Generate()
		{
			Fill();
			Correction();
			OnGenerationEnd(this);
		}

		/// <summary>
		/// Matrix filling.
		/// </summary>
		private void Fill()
		{
			for (int y = 0; y < Size.y; y++)
				for (int x = 0; x < Size.x; x++)
					Cells[x, y] = false;

			FillRandomPoints(Size / 2);
		}

		/// <summary>
		/// Filling matrix by choosing random neighbor. 
		/// </summary>
		/// <param name="point">Current point.</param>
		private void FillRandomPoints(Vector2Int point)
		{
			var currentPoint = point;
			var randomValue = (float) _random.NextDouble();

			randomValue = randomValue < 0.5f ? 0.5f : randomValue;

			var caveLength = _maximumCaveLength <= 0 ? Size.x * Size.y / 3 : _maximumCaveLength;
			var randomCaveLength = (int) (caveLength * randomValue);
			var n = 0;

			while (n < randomCaveLength)
			{
				bool reverse = _random.NextDouble() > 0.5f;
				int x = 0;
				int y = 0;
				if (reverse)
				{
					x = _random.NextDouble() > 0.5f ? 1 : 0;
					x = _random.NextDouble() > 0.5f ? -x : x;
				}
				else
				{
					y = _random.NextDouble() > 0.5f ? 1 : 0;
					y = _random.NextDouble() > 0.5f ? -y : y;
				}

				if (currentPoint.InBorder(Size, _border))
				{
					if (!Cells[currentPoint.x + x * 2, currentPoint.y + y * 2])
						n++;
					ActiveGroup(currentPoint, true);
					Cells[currentPoint.x, currentPoint.y] = true;
				}
				else
					currentPoint = Size / 2;

				currentPoint = currentPoint + new Vector2Int(x, y);
				if (n > randomCaveLength + 5)
					throw new StackOverflowException();
			}
		}

		/// <summary>
		/// Matrix correction.
		/// </summary>
		private void Correction()
		{
			for (int y = 1; y < Size.y - 1; y++)
				for (int x = 1; x < Size.x - 1; x++)
				{
					int n = CountTrueNeighbors(new Vector2Int(x, y));
					if (n < 2)
						Cells[x, y] = true;
				}
		}

		/// <summary>
		/// Counting alive neighbors.
		/// </summary>
		/// <param name="position">Cell position.</param>
		/// <returns>Alive neighbors count.</returns>
		private int CountTrueNeighbors(Vector2Int position)
		{
			if (!position.InRange(Vector2Int.zero, Size))
				throw new
					IndexOutOfRangeException($"Cell is out of range.\n Chord({position.x}, {position.y}) Size ({Size.x}, {Size.y})");

			int neighbors = 0;

			if (!Cells[position.x - 1, position.y])
				neighbors++;
			if (!Cells[position.x + 1, position.y])
				neighbors++;
			if (!Cells[position.x, position.y - 1])
				neighbors++;
			if (!Cells[position.x, position.y + 1])
				neighbors++;

			return neighbors;
		}

		/// <summary>
		/// Activating/Deactivating group.
		/// + - under control.
		/// X - not under control.
		/// X + X
		/// + + +
		/// X + X
		/// </summary>
		/// <param name="position">Center.</param>
		/// <param name="status">Status [activating/deactivating].</param>
		private void ActiveGroup(Vector2Int position, bool status)
		{
			Cells[position.x - 1, position.y] = status;
			Cells[position.x + 1, position.y] = status;
			Cells[position.x, position.y - 1] = status;
			Cells[position.x, position.y + 1] = status;
		}
	}
}