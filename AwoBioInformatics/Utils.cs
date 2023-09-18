using AwoBioInformatics.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

		public static N AMax<N>(out int index, params N[] values) where N : IMinMaxValue<N> 
		{
			var result = values.Max();
			index = Array.IndexOf(values, result);
			return result;
		}
	}
}
