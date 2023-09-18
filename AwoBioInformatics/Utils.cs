using AwoBioInformatics.AlignmentAlgorithms;
using AwoBioInformatics.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AwoBioInformatics
{
	public class Utils
	{
		public static Dictionary<TKey, TValue> ParseMatrix<TKey, TValue>(string matrix, Func<string, TValue> valueParser, Func<string, string, TKey> keyParser, string rowSeparator = "\r\n", string columnSeparator = ";")
		{
			var result = new Dictionary<TKey, TValue>();
			var data = matrix.Split(rowSeparator).Select(x => x.Split(columnSeparator)).ToArray();
			for (int x = 0; x < data.Length; x++)
			{
				var c1 = data[x][0];
				for (int y = 0; y <= x; y++)
				{
					var c2 = data[y][0];
					var score = valueParser(data[x][y + 1]);
					var key1 = keyParser(c1, c2);
					var key2 = keyParser(c2, c1);
					result.Add(key1, score);
					if (key1.Equals(key2) == false)
						result.Add(key2, score);
				}
			}

			return result;
		}

		public static string StepTypeToString(StepType stepType)
		{
			switch (stepType)
			{
				case StepType.Diagonal:
					return "D";

				case StepType.Left:
					return "L";

				case StepType.Top:
					return "T";

				default:
					throw new ArgumentOutOfRangeException(nameof(stepType), stepType, null);
			}
		}

		public static N AMax<N>(out int index, params N[] values) where N : IMinMaxValue<N>
		{
			var result = values.Max();
			index = Array.IndexOf(values, result);
			return result;
		}

		public static string PrintMatrixGeneric<N>(string[] rows, string[] cols, N[,] matrix, Func<N, string> nString = null, char cross = '+', char hline = '-', char vline = '|')
		{
			nString ??= (x => x.ToString());
			var matrixStr = new string[rows.Length, cols.Length];
			for (int x = 0; x < rows.Length; x++)
				for (int y = 0; y< cols.Length; y++)
					matrixStr[x, y] = nString(matrix[x, y]);

			return PrintMatrix(rows, cols, matrixStr, cross, hline, vline);
		}

		public static string PrintMatrix(string[] rows, string[] cols, string[,] matrix, char cross = '+', char hline = '-', char vline = '|')
		{
			var builder = new StringBuilder();
			var dict = new Dictionary<int, int>();
			for (int x = 0; x < rows.Length; x++)
				dict[x] = Math.Max(rows[x].Length, Enumerable.Range(0, cols.Length).Select(y => matrix[x, y].Length).Max());

			dict[-1] = cols.Max(x => x.Length);

			var separator = '+' + new string('-', dict[-1]) + '+' + string.Join("+", Enumerable.Range(0, rows.Length).Select(x => new string('-', dict[x]))) + '+';
			builder.AppendLine(separator);
			builder.Append('|');
			builder.Append(new string(' ', dict[-1]));
			builder.Append('|');
			builder.Append(string.Join("|", Enumerable.Range(0, rows.Length).Select(x => rows[x].PadRight(dict[x]))));
			builder.Append('|');
			builder.AppendLine();
			builder.AppendLine(separator);

			for (int y = 0; y < cols.Length; y++)
			{
				builder.Append('|');
				builder.Append(cols[y].PadRight(dict[-1]));
				builder.Append('|');
				builder.Append(string.Join("|", Enumerable.Range(0, rows.Length).Select(x => matrix[x, y].PadRight(dict[x]))));
				builder.Append('|');
				builder.AppendLine();
				builder.AppendLine(separator);
			}

			return builder.ToString();
		}

		public static string SubstitutionMatrix<T, N>(IEnumerable<T> seq1, IEnumerable<T> seq2, Func<T, T, N> scoring, Func<T, string> tString = null, Func<N, string> nString = null)
		{
			tString ??= (x => x.ToString());
			nString ??= (x => x.ToString());
			var arr1 = seq1.ToArray();
			var arr2 = seq2.ToArray();

			var arr1Str = arr1.Select(tString).ToArray();
			var arr2Str = arr2.Select(tString).ToArray();

			var matrix = new string[arr1.Length, arr2.Length];
			int x, y;
			for (x = 0; x < arr1.Length; x++)
				for (y = 0; y < arr2.Length; y++)
					matrix[x, y] = nString(scoring(arr1[x], arr2[y]));

			return PrintMatrix(arr1Str, arr2Str, matrix);
		}
	}
}
