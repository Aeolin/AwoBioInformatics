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
	public class Protein
	{
		public static readonly Protein[] All;
		public static readonly ImmutableDictionary<string, Protein> ShortCodeMapping;
		public static readonly ImmutableDictionary<string, Protein> CodonMapping;
		public static readonly string[] StopCodonCodes;
		public static readonly string StartCodon;

		public static readonly Protein Stop;
		public static readonly Protein Start;

		static Protein()
		{
			var codons = Resource.Codons.Split(Environment.NewLine).Select(x => x.Split(';'));
			var preProcessed = codons.Select(x => new
			{
				ShortCode = x[1],
				Label = x[2][0],
				Name = x[3],
				Codon = Codon.ShortCodeMapping[x[0]]
			}).GroupBy(x => x.ShortCode);

			All = preProcessed.Select(x => new Protein(x.First().Label, x.First().Name, x.Key, x.Select(y => y.Codon).ToArray())).ToArray();
			Start = All.First(x => x.Label == 'M');
			StartCodon = Start.Codons.First().ShortCode.ToUpper();
			Stop = All.First(x => x.Label == 'O');
			StopCodonCodes = Stop.Codons.Select(x => x.ShortCode.ToUpper()).ToArray();
			ShortCodeMapping = All.ToImmutableDictionary(x => x.ShortCode.ToLower());
			var tempDict = new Dictionary<string, Protein>();
			foreach (var protein in All)
				foreach (var codon in protein.Codons)
					tempDict.Add(codon.ShortCode.ToUpper(), protein);

			CodonMapping = tempDict.ToImmutableDictionary();
		}

		public static Protein OfShortCode(string shortCode) => ShortCodeMapping[shortCode.ToLower()];
		public static Protein OfCodon(string codon) => CodonMapping[codon.ToUpper()];

		public string Name { get; init; }
		public char Label { get; init; }
		public string ShortCode { get; init; }
		public Codon[] Codons { get; init; }

		private Protein(char label, string name, string shortCode, Codon[] codons)
		{
			Label = label;
			Name = name;
			ShortCode = shortCode;
			Codons=codons;
		}

		public override string ToString()
		{
			return $"{Name} ({Label})";
		}
	}
}
