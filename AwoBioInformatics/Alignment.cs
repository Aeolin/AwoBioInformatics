using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AwoBioInformatics
{
	public class Alignment<T, N> where N : INumber<N>
	{
		public T[] SourceColumn { get; init; }
		public T[] SourceRow { get; init; }
		public Func<T, T, N> ScoringFunction { get; set; }
		public N GapPenalty { get; set; }

		public Alignment(IEnumerable<T> sourceColumn, IEnumerable<T> sourceRow, N gapPenalty, Func<T, T, N> scoring = null)
		{
			SourceColumn = sourceColumn.ToArray();
			SourceRow = sourceRow.ToArray();
			GapPenalty = gapPenalty;
			ScoringFunction = scoring;
		}

		public N[,] Align()
		{
			var matrix = new N[SourceColumn.Length + 1, SourceRow.Length + 1];
			for (int i = 0; i < Math.Max(SourceColumn.Length, SourceRow.Length); i++)
			{
				if (i < SourceColumn.Length)
					matrix[i, 0] = GapPenalty * N.CreateChecked(i);

				if (i < SourceRow.Length)
					matrix[0, i] = GapPenalty * N.CreateChecked(i);
			}

			for (int y = 1; y < SourceRow.Length; y++)
			{
				for (int x = 1; x < SourceColumn.Length; x++)
				{
					var score = ScoringFunction(SourceColumn[x], SourceRow[y]);
					var left = matrix[x-1, y] + GapPenalty;
					var top = matrix[x, y-1] + GapPenalty;
					var diagonal = matrix[x-1, y-1] + score;
					matrix[x, y] = N.Max(N.Max(left, top), diagonal);
				}
			}

			return matrix;
		}
	}
}
