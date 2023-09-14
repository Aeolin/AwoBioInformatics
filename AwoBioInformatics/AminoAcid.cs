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
	public class AminoAcid
	{
		public static readonly AminoAcid[] All;
		public static readonly ImmutableDictionary<string, AminoAcid> ShortCodeMapping;
		public static readonly ImmutableDictionary<char, AminoAcid> LabelMapping;
		public static readonly ImmutableDictionary<string, AminoAcid> CodonMapping;
		public static readonly string[] StopCodonCodes;
		public static readonly string StartCodon;

		public static readonly AminoAcid Stop;
		public static readonly AminoAcid Start;

		static AminoAcid()
		{
			var codons = Resource.Codons.Split(Environment.NewLine).Select(x => x.Split(';'));
			var preProcessed = codons.Select(x => new
			{
				ShortCode = x[1],
				Label = x[2][0],
				Name = x[3],
				Codon = Codon.ShortCodeMapping[x[0]]
			}).GroupBy(x => x.ShortCode);

			All = preProcessed.Select(x => new AminoAcid(x.First().Label, x.First().Name, x.Key, x.Select(y => y.Codon).ToArray())).ToArray();
			Start = All.First(x => x.Label == 'M');
			StartCodon = Start.Codons.First().ShortCode.ToUpper();
			Stop = All.First(x => x.Label == 'O');
			StopCodonCodes = Stop.Codons.Select(x => x.ShortCode.ToUpper()).ToArray();
			ShortCodeMapping = All.ToImmutableDictionary(x => x.ShortCode.ToLower());
			var tempDict = new Dictionary<string, AminoAcid>();
			foreach (var protein in All)
				foreach (var codon in protein.Codons)
					tempDict.Add(codon.ShortCode.ToUpper(), protein);

			CodonMapping = tempDict.ToImmutableDictionary();
			LabelMapping = All.ToImmutableDictionary(x => x.Label);
		}

		public static AminoAcid OfShortCode(string shortCode) => ShortCodeMapping[shortCode.ToLower()];
		public static AminoAcid OfLabel(char label) => LabelMapping[char.ToUpper(label)];
		public static AminoAcid OfCodon(string codon) => CodonMapping[codon.ToUpper()];

		public string Name { get; init; }
		public char Label { get; init; }
		public string ShortCode { get; init; }
		public Codon[] Codons { get; init; }

		private AminoAcid(char label, string name, string shortCode, Codon[] codons)
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
