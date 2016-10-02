// Licensed under GPLv3 license or under special license
// See the LICENSE file in the project root for more information
// -----------------------------------------------------------------------
// Author: Plastic Block <admin@plasticblock.xyz>
// skype: plasticblock, email: support@plasticblock.xyz
// project: Fractal Caves Generator (FCG)
// -----------------------------------------------------------------------
using System;

namespace FCG
{
	[Serializable]
	public struct Vector2Int
	{
		#region Fields

		public int x, y;

		#endregion

		#region Constructors

		public Vector2Int(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		#endregion

		#region Functions

		public bool InRange(Vector2Int range, bool fromZero)
		{
			Vector2Int start = fromZero ? zero : one;
			return x > start.x && y > start.y && x < range.x - 1 && y < range.y - 1;
		}

		public bool InRange(Vector2Int start, Vector2Int end)
		{
			return x > start.x && y > start.y && x < end.x && y < end.y;
		}

		public bool InBorder(Vector2Int size, Vector2Int border)
		{
			Vector2Int start = border;
			Vector2Int end = size - new Vector2Int(border.x + 1, border.y + 1);
			return x > start.x && y > start.y && x < end.x && y < end.y;
		}

		#endregion

		#region Operators

		public static Vector2Int operator +(Vector2Int self, Vector2Int add)
		{
			return new Vector2Int(self.x + add.x, self.y + add.y);
		}

		public static Vector2Int operator -(Vector2Int self, Vector2Int des)
		{
			return new Vector2Int(self.x - des.x, self.y - des.y);
		}

		public static Vector2Int operator *(Vector2Int self, Vector2Int multiplier)
		{
			return new Vector2Int(self.x * multiplier.x, self.y * multiplier.y);
		}

		public static Vector2Int operator *(Vector2Int self, int multiplier)
		{
			return new Vector2Int(self.x * multiplier, self.y * multiplier);
		}

		public static Vector2Int operator /(Vector2Int self, Vector2Int div)
		{
			return new Vector2Int(self.x / div.x, self.y / div.y);
		}

		public static Vector2Int operator /(Vector2Int self, int div)
		{
			return new Vector2Int(self.x / div, self.y / div);
		}

		public static bool operator ==(Vector2Int self, Vector2Int a)
		{
			return self.x == a.x && self.y == a.y;
		}

		public static bool operator !=(Vector2Int self, Vector2Int a)
		{
			return !(self.x == a.x && self.y == a.y);
		}

		public static implicit operator UnityEngine.Vector2(Vector2Int self)
		{
			return new UnityEngine.Vector2(self.x, self.y);
		}

		public static implicit operator UnityEngine.Vector3(Vector2Int self)
		{
			return new UnityEngine.Vector3(self.x, self.y);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return GetHashCode() == obj.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("({0}, {1})", x, y);
		}

		#endregion

		#region Proporties

		public static Vector2Int zero
		{
			get { return new Vector2Int(0, 0); }
		}

		public static Vector2Int one
		{
			get { return new Vector2Int(1, 1); }
		}

		#endregion
	}
}
