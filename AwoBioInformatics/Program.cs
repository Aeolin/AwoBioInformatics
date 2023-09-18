using AwoBioInformatics;
using AwoBioInformatics.AlignmentAlgorithms;

while (true)
{
	//var aminoCol = AminoAcidSequence.OfLabels("EKAN");
	//var aminoRow = AminoAcidSequence.OfLabels("GRAS");
	//var alignMent = new Alignment<AminoAcid, int>(aminoCol, aminoRow, -6, AminoAcid.GetPam250Score);
	//alignMent.Align();

	var alignment = new Alignment<AminoAcid, int>(AminoAcidSequence.OfLabels("IWFHGREE"), AminoAcidSequence.OfLabels("WCHLREPD"), -8, AminoAcid.GetBlosumScore, x => x.Label.ToString());
	var aligned = alignment.Align(out var matrix, out var steps);


	Console.WriteLine(aligned);

	Console.Write("Enter a sequence type (N)ucleotide / (P)rotein: ");
	var type = Console.ReadKey();
	Console.WriteLine();
	if (type.Key == ConsoleKey.N)
	{
		Console.Write("Sequence: ");
		var sequence = Console.ReadLine();
		var dna = NucleotideSequence.OfLabels(sequence);

		Console.WriteLine($"DNA Counterpart: {dna.DnaCounterpart()}");
		Console.WriteLine($"RNA Counterpart: {dna.RnaCounterpart()}");
	}
	else if (type.Key == ConsoleKey.P)
	{
		Console.Write("Sequence Type (R)na (S)hortCodes: ");
		var sType = Console.ReadKey();
		Console.WriteLine();
		Console.Write("Sequence: ");
		var seq = Console.ReadLine();
		if (sType.Key == ConsoleKey.R)
		{
			var proteins = AminoAcidSequence.OfRna(seq.ToUpper().Replace(Nucleotide.T.Label, Nucleotide.U.Label)).ToArray();
			Console.WriteLine($"Found {proteins.Length} proteins");
			foreach (var protein in proteins)
				Console.WriteLine(protein);
		}
		else
		{
			Console.WriteLine($"Rna: " + AminoAcidSequence.OfShortCodes(seq).ToRna());
			Console.WriteLine("Labels: " + AminoAcidSequence.OfShortCodes(seq).ToLabels());
		}
	}


}