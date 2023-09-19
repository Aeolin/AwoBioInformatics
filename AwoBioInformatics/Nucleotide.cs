using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoBioInformatics
{
	public class Nucleotide
	{
		public static readonly Nucleotide A;
		public static readonly Nucleotide G;
		public static readonly Nucleotide C;
		public static readonly Nucleotide T;
		public static readonly Nucleotide U;

		public static readonly Nucleotide[] All;
		public static readonly Nucleotide[] DnaParts;
		public static readonly Dictionary<char, Nucleotide> CharMapping;

		static Nucleotide()
		{
			A = new Nucleotide('A');
			G = new Nucleotide('G');
			C = new Nucleotide('C');
			T = new Nucleotide('T');
			U = new Nucleotide('U');

			A.DnaCounterpart = T;
			A.RnaCounterpart = U;

			G.DnaCounterpart = C;
			G.RnaCounterpart = C;

			C.DnaCounterpart = G;
			C.RnaCounterpart = G;

			T.DnaCounterpart = A;
			T.RnaCounterpart = A;

			U.DnaCounterpart = A;
			All = new[] { A, G, C, T, U };
			DnaParts = new[] { A, T, C, G };
			CharMapping = All.ToDictionary(x => x.Label);
		}

		public static Nucleotide OfChar(char c) => All.First(x => x.Label == char.ToUpper(c));

		public char Label { get; set; }
		public Nucleotide DnaCounterpart { get; private set; }
		public Nucleotide RnaCounterpart { get; private set; }

		private Nucleotide(char label)
		{
			Label=label;
		}

		public override string ToString()
		{
			return Label.ToString();
		}
	}
}
