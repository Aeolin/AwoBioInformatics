using AwoBioInformatics.Resources;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoBioInformatics
{
	public class Codon
	{
		public static readonly Codon[] Codons;
		public static readonly ImmutableDictionary<string, Codon> ShortCodeMapping;

		static Codon()
		{
			var list = new List<Codon>();
			var codons = Resource.Codons.Split(Environment.NewLine).Select(x => x.Split(';'));
			foreach(var codon in codons)
			{
				var n1 = Nucleotide.OfChar(codon[0][0]);
				var n2 = Nucleotide.OfChar(codon[0][1]);
				var n3 = Nucleotide.OfChar(codon[0][2]);
				list.Add(new Codon(n1, n2, n3));
			}

			Codons = list.ToArray();
			ShortCodeMapping = Codons.ToImmutableDictionary(x => x.ShortCode);
		}

		public Nucleotide Nucleotide1 { get; init; }
		public Nucleotide Nucleotide2 { get; init; }
		public Nucleotide Nucleotide3 { get; init; }

		public string ShortCode { get; init; }

		public IEnumerable<Nucleotide> Nucleotides()
		{
			yield return Nucleotide1;
			yield return Nucleotide2;
			yield return Nucleotide3;
		}

		private Codon(Nucleotide nucleotide1, Nucleotide nucleotide2, Nucleotide nucleotide3)
		{
			ShortCode = $"{nucleotide1}{nucleotide2}{nucleotide3}";
			Nucleotide1=nucleotide1;
			Nucleotide2=nucleotide2;
			Nucleotide3=nucleotide3;
		}
	}
}
