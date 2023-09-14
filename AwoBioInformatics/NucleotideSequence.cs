using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoBioInformatics
{
	public class NucleotideSequence : IEnumerable<Nucleotide>
	{
		private IEnumerable<Nucleotide> _nucleotides;

		public NucleotideSequence(IEnumerable<Nucleotide> nucleotides, bool order = true)
		{
			_nucleotides=nucleotides;
			Order =order;
		}

		public bool Order { get; set; }
		public char StartCap => Order ? '5' : '3';
		public char EndCap => Order ? '3' : '5';

		public override string ToString()
		{
			return $"{StartCap}-{string.Join("", _nucleotides)}-{EndCap}";
		}

		public static NucleotideSequence OfLabels(string @string)
		{
			if (@string.Contains('-'))
			{
				return new NucleotideSequence(@string[2..^2].ToUpper().Select(x => Nucleotide.CharMapping[x]), @string.StartsWith('5'));
			}
			else
			{
				return new NucleotideSequence(@string.Select(x => Nucleotide.CharMapping[x]));
			}
		}

		public NucleotideSequence DnaCounterpart() => new NucleotideSequence(_nucleotides.Select(x => x.DnaCounterpart), !Order);
		public NucleotideSequence RnaCounterpart() => new NucleotideSequence(_nucleotides.Select(x => x.RnaCounterpart), !Order);

		public IEnumerator<Nucleotide> GetEnumerator() => _nucleotides.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => _nucleotides.GetEnumerator();
	}
}
