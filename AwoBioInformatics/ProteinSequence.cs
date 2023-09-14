using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AwoBioInformatics
{
	public class ProteinSequence : IEnumerable<Protein>
	{
		private IEnumerable<Protein> _proteins;

		public ProteinSequence(IEnumerable<Protein> proteins)
		{
			_proteins=proteins;
		}

		public static ProteinSequence OfShortCodes(string @string) => new ProteinSequence(@string.ToLower().Split(' ').Select(x => Protein.OfShortCode(x)).ToArray());
		public static ProteinSequence OfRna(string rna, bool validate = true)
		{
			rna = rna.ToUpper();
			var index = rna.IndexOf(Protein.StartCodon);
			if (validate)
			{
				if (index == -1)
				{
					throw new ArgumentException("RNA does not contain start codon");
				}
				rna = rna[index..];
			}

			var list = new List<Protein>();
			while (rna.Length > 3)
			{
				var codon = rna[0..3];
				if (Protein.StopCodonCodes.Contains(codon))
					return new ProteinSequence(list);

				var protein = Protein.OfCodon(codon);
				list.Add(protein);
				rna = rna[3..];
			}

			if (validate)
			{
				throw new ArgumentException("No Stop Codon Found");
			}
			else
			{
				return new ProteinSequence(list.ToArray());
			}
		}

		public string ToShortCodes() => string.Join(" ", _proteins.Select(x => x.ShortCode));
		public NucleotideSequence ToRna() => new NucleotideSequence(_proteins.SelectMany(x => x.Codons.First().Nucleotides()));
		public string ToLabels() => string.Join("", _proteins.Select(x => x.Label));

		public IEnumerator<Protein> GetEnumerator() => _proteins.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => _proteins.GetEnumerator();
	}
}
