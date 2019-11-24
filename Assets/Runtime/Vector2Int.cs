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
	public static class Vector2IntExtensions
	{
		public static bool InRange(this Vector2Int self, Vector2Int range, bool fromZero)
		{
			var start = fromZero ? Vector2Int.zero : Vector2Int.one;
			return self.x > start.x && self.y > start.y && self.x < range.x - 1 && self.y < range.y - 1;
		}

		public static bool InRange(this Vector2Int self, Vector2Int start, Vector2Int end) => self.x > start.x && self.y > start.y && self.x < end.x && self.y < end.y;

		public static bool InBorder(this Vector2Int self, Vector2Int size, Vector2Int border)
		{
			Vector2Int start = border;
			Vector2Int end = size - new Vector2Int(border.x + 1, border.y + 1);
			return self.x > start.x && self.y > start.y && self.x < end.x && self.y < end.y;
		}
	}
}