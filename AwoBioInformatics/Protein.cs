using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoBioInformatics
{
	public class Protein
	{
		public AminoAcidSequence _acids;
		public AminoAcidSequence Acids => _acids;
		public Protein(AminoAcidSequence acids)
		{
			_acids=acids;
		}

		public override string ToString()
		{
			return string.Join("", _acids.Where(x => x != AminoAcid.Stop).Select(x => x.Label));
		}
	}
}
