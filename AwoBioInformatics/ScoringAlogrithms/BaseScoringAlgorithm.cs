using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AwoBioInformatics.ScoringAlogrithms
{
	public class BaseScoringAlgorithm<T, N> where N : INumber<N>, IComparable<N>
	{
		private readonly N[,] _slidingWindow;
		private readonly T[] _elements;
		private Func<N, N, N> _aggregator;
		public int CutPosition { get; init; } 
		public int Length { get; init; }

		public BaseScoringAlgorithm(T[] elements, N[,] slidingWindow, Func<N, N, N> aggregator, int cutPosition)
		{
			_slidingWindow=slidingWindow;
			_elements = elements;
			_aggregator = aggregator;
			Length = slidingWindow.GetLength(1);
			CutPosition = cutPosition;
		}

		public (int, N) BestIndex(IEnumerable<T> elements) 
		{
			var array = elements.Select(x => Array.IndexOf(_elements, x)).ToArray();
			if(array.Length < Length)
				throw new ArgumentException($"The number of elements must be greater than or equal to {Length}");

			N _maxResult = N.CreateSaturating(long.MinValue);
			int _maxIndex = 0;
			for(int i = 0; i <= (array.Length - Length); i++)
			{
				var aggregated = Enumerable.Range(0, Length).Select(x => _slidingWindow[array[i+x], x]).Aggregate(_aggregator);
				if(aggregated > _maxResult)
				{
					_maxIndex = CutPosition+i;
					_maxResult = aggregated;
				}
			}

			return (_maxIndex, _maxResult);
		}
	}
}
