using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AwoBioInformatics.AlignmentAlgorithms
{
	public class Alignment<T, N> where N : INumber<N>, IMinMaxValue<N>
	{
		public T[] SourceColumn { get; init; }
		public T[] SourceRow { get; init; }
		public Func<T, T, N> ScoringFunction { get; set; }
		public N GapPenalty { get; set; }
		public Func<T, string> StringFunc { get; set; }
		public char AlignmentChar { get; set; } = '-';

		public Alignment(IEnumerable<T> sourceColumn, IEnumerable<T> sourceRow, N gapPenalty, Func<T, T, N> scoring = null, Func<T, string> stringFunc = null)
		{
			SourceColumn = sourceColumn.ToArray();
			SourceRow = sourceRow.ToArray();
			GapPenalty = gapPenalty;
			ScoringFunction = scoring;
			StringFunc = stringFunc ?? (x => x.ToString());
		}


		public string Align(out N[,] matrix, out StepType[,] steps)
		{
			matrix = new N[SourceColumn.Length + 1, SourceRow.Length + 1];
			steps = new StepType[SourceColumn.Length + 1, SourceRow.Length + 1];
			for (int i = 0; i < Math.Max(SourceColumn.Length, SourceRow.Length); i++)
			{
				if (i < SourceColumn.Length)
					matrix[i, 0] = GapPenalty * N.CreateChecked(i);

				if (i < SourceRow.Length)
					matrix[0, i] = GapPenalty * N.CreateChecked(i);
			}

			int x, y;

			for (y = 1; y < SourceRow.Length; y++)
			{
				for (x = 1; x < SourceColumn.Length; x++)
				{
					var score = ScoringFunction(SourceColumn[x-1], SourceRow[y-1]);
					var left = matrix[x - 1, y] + GapPenalty;
					var top = matrix[x, y - 1] + GapPenalty;
					var diagonal = matrix[x - 1, y - 1] + score;
					matrix[x, y] = Utils.AMax(out var index, diagonal, left, top);
					steps[x, y] = (StepType)index;
				}
			}

			string columnAligned = "";
			string rowAligned = "";
			x = SourceColumn.Length;
			y = SourceRow.Length;
			while (x > 0 && y > 0)
			{
				switch (steps[x, y])
				{
					case StepType.Diagonal:
						columnAligned = StringFunc(SourceColumn[x-1]) + columnAligned;
						rowAligned = StringFunc(SourceRow[y-1]) + rowAligned;
						x--;
						y--;
						break;

					case StepType.Top:
						columnAligned = AlignmentChar + columnAligned;
						rowAligned = StringFunc(SourceRow[y-1]) + rowAligned;
						y--;
						break;

					case StepType.Left:
						columnAligned = StringFunc(SourceColumn[x-1]) + columnAligned;
						rowAligned = AlignmentChar + rowAligned;
						x--;
						break;
				}
			}

			return columnAligned+"\n"+rowAligned;
		}
	}
}
