// Licensed under GPLv3 license or under special license
// See the LICENSE file in the project root for more information
// -----------------------------------------------------------------------
// Author: Plastic Block <admin@plasticblock.xyz>
// skype: plasticblock, email: support@plasticblock.xyz
// project: Fractal Caves Generator (FCG)
// -----------------------------------------------------------------------
using System;
using Random = UnityEngine.Random;

namespace FCG
{
	/// <summary>
	/// Fractal cave generator.
	/// </summary>
    public sealed class FractalGenerator : Matrix
    {
	    public FractalGenerator(Vector2Int size, int seed) : base(size, seed) { }

		/// <summary>
		/// Cave length. (amount of unfillable cells)
		/// </summary>
		public int caveLength;

		/// <summary>
		/// Border. Cave would not be generated out of this border.
		/// </summary>
		public Vector2Int border;

		/// <summary>
		/// Generation initialize method.
		/// </summary>
		public override void Generate()
        {
            OnGenerationEnd += Correction;
            base.Generate();
        }

		/// <summary>
		/// Matrix filling.
		/// </summary>
        protected override void Fill()
        {
            base.Fill();
            caveLength = caveLength <= 0 ? size.x*size.y/3 : caveLength;
            FillRandomPoints(size/2);
        }

		/// <summary>
		/// Filling matrix by choosing random neighbor. 
		/// </summary>
		/// <param name="point">Current point.</param>
		private void FillRandomPoints(Vector2Int point)
        {
            Vector2Int currentPoint = point;
            float rndValue = Random.value;
            rndValue = rndValue < 0.5f ? 0.5f : rndValue;
            int rndCaveLength = (int)(caveLength * rndValue);
            int n = 0;
            while (n < rndCaveLength)
            {
                bool reverse = Random.value > 0.5f;
                int x = 0;
                int y = 0;
                if (reverse)
                {
                    x = Random.value > 0.5f ? 1 : 0;
                    x = Random.value > 0.5f ? -x : x;
                }
                else
                {
                    y = Random.value > 0.5f ? 1 : 0;
                    y = Random.value > 0.5f ? -y : y;
                }
                if (currentPoint.InBorder(size, border))
                {
                    if (cells[currentPoint.x + x * 2, currentPoint.y + y * 2] == defaultStatus)
                        n++;
                    ActiveGroup(currentPoint, !defaultStatus);
                    cells[currentPoint.x, currentPoint.y] = !defaultStatus;
                }
                else
                    currentPoint = size / 2;
                currentPoint = currentPoint + new Vector2Int(x, y);
                if (n > rndCaveLength + 5)
                    throw new StackOverflowException();
            }
        }

		/// <summary>
		/// Matrix correction.
		/// </summary>
	    private void Correction()
	    {
		    for (int y = 1; y < size.y - 1; y++)
			    for (int x = 1; x < size.x - 1; x++)
			    {
				    int n = CountTrueNeighbors(new Vector2Int(x, y));
				    if (n < 2)
					    cells[x, y] = !defaultStatus;
			    }
	    }
    }
}