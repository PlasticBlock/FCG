using UnityEngine;
using Random = System.Random;

namespace vmp1r3.CavesGenerator
{
	public sealed class Cave
	{
		private readonly Random random;
		private readonly Vector2Int size;
		private readonly Vector2Int boundary;
		private readonly int target;

		public Cave(Vector2Int size, Vector2Int boundary, Random random, int target)
		{
			this.size = size;
			this.boundary = boundary;
			this.target = target;
			this.random = random;

			Cells = new bool[size.x + 2, size.y + 2];

			Walk(size / 2);

			for (int y = 1; y < size.y - 1; y++)
				for (int x = 1; x < size.x - 1; x++)
					Cells[x, y] = CountActiveNeighbors(new Vector2Int(x, y)) < 2 || Cells[x, y];
		}

		public bool[,] Cells { get; }

		/// <exception cref="System.StackOverflowException" />
		private void Walk(Vector2Int startPoint)
		{
			var point = startPoint;

			for (int n = 0; n < target;)
			{
				var direction = Random() ?
					Random() ? Vector2Int.up : Vector2Int.down :
					Random() ? Vector2Int.left : Vector2Int.right;

				if (point.x > boundary.x && point.y > boundary.y &&
					point.x < size.x - boundary.x + 1 && point.y < size.y - boundary.y + 1)
				{
					if (!Cells[point.x + direction.x * 2, point.y + direction.y * 2])
						n++;

					Cells[point.x, point.y] = true;
					Cells[point.x - 1, point.y] = true;
					Cells[point.x + 1, point.y] = true;
					Cells[point.x, point.y - 1] = true;
					Cells[point.x, point.y + 1] = true;
				}
				else
					point = size / 2;

				point += direction;

				if (n > target + 5)
					throw new System.StackOverflowException();
			}
		}

		/// <exception cref="System.ArgumentOutOfRangeException" />
		private int CountActiveNeighbors(Vector2Int position)
		{
			if (!(position.x > 0 && position.y > 0 && position.x < size.x && position.y < size.y))
				throw new System.ArgumentOutOfRangeException(nameof(position));

			var neighbors = 0;

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

		private bool Random() => random.Next() % 2 == 0;
	}
}