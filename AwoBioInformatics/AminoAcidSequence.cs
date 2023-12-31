﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AwoBioInformatics
{
	public class AminoAcidSequence : IEnumerable<AminoAcid>
	{
		private IEnumerable<AminoAcid> _proteins;

		public AminoAcidSequence(IEnumerable<AminoAcid> proteins)
		{
			_proteins=proteins;
		}

		public static AminoAcidSequence OfShortCodes(string @string) => new AminoAcidSequence(@string.ToLower().Split(' ').Select(x => AminoAcid.OfShortCode(x)).ToArray());
		public static AminoAcidSequence OfLabels(string @string) => new AminoAcidSequence(@string.Select(x => AminoAcid.OfLabel(x)).ToArray());
		public static AminoAcidSequence OfNucleotides(IEnumerable<Nucleotide> nucleotides)
		{
			var array = nucleotides.ToArray();
			var res = new List<AminoAcid>();
			for (int i = 0; i < array.Length; i += 3)
			{
				res.Add(AminoAcid.OfCodon(string.Join("", array.Skip(i).Take(3).Select(x => x.Label.ToString()))));
			}

			return new AminoAcidSequence(res.ToArray());
		}

		public IEnumerable<Protein> FindProteins()
		{
			var acids = this.ToList();
			int index = 0;
			while ((index = acids.FindIndex(index, x => x == AminoAcid.Start)) > 0)
			{
				var stop = acids.FindIndex(index, x => x == AminoAcid.Stop);
				if (stop == -1)
					yield break;

				yield return new Protein(new AminoAcidSequence(acids.GetRange(index, stop - index + 1)));
			}
		}

		public static IEnumerable<Protein> OfRna(string rna)
		{
			rna = rna.ToUpper();
			int index;
			var list = new List<AminoAcid>();
			while (rna.Length >= 3 && (index = rna.IndexOf(AminoAcid.StartCodon)) >= 0)
			{
				rna = rna[index..];
				while (rna.Length > 3)
				{
					var codon = rna[0..3];
					if (AminoAcid.StopCodonCodes.Contains(codon))
					{
						rna = rna[3..];
						yield return new Protein(new AminoAcidSequence(list));
						list.Clear();
						break;
					}

					var amino = AminoAcid.OfCodon(codon);
					list.Add(amino);
					rna = rna[3..];
				}
			}

			if (list.Count > 0)
				yield return new Protein(new AminoAcidSequence(list));
		}

		public string ToShortCodes() => string.Join(" ", _proteins.Select(x => x.ShortCode));
		public NucleotideSequence ToRna() => new NucleotideSequence(_proteins.SelectMany(x => x.Codons.First().Nucleotides()));
		public string ToLabels() => string.Join("", _proteins.Select(x => x.Label));

		public IEnumerator<AminoAcid> GetEnumerator() => _proteins.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => _proteins.GetEnumerator();
	}
}
