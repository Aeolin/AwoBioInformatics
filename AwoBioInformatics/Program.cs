using AwoBioInformatics;
using AwoBioInformatics.AlignmentAlgorithms;
using AwoBioInformatics.ScoringAlogrithms;

while (true)
{
	//var aminoCol = AminoAcidSequence.OfLabels("EKAN");
	//var aminoRow = AminoAcidSequence.OfLabels("GRAS");
	//var alignMent = new Alignment<AminoAcid, int>(aminoCol, aminoRow, -6, AminoAcid.GetPam250Score);
	//alignMent.Align();


	var matrix1 = new double[,]
	{
		{ 0.06D, -0.25D, -2.32D, 1.38D, 0.71D, -0.12D },
		{ -0.40D, 0.21D, -0.74D, -1.18D, -1.64D, -0.18D },
		{ -0.06D, 0.64D, -0.25D, -0.25D, -0.84D, -0.25D },
		{ 0.31D, -1.18D, 1.24D, -3.06D, 0.57D, 0.44D }
	};

	var scoring = new BaseScoringAlgorithm<Nucleotide, double>(Nucleotide.DnaParts, matrix1, (a, b) => a+b, 3);
	var (best, score) = scoring.BestIndex(NucleotideSequence.OfLabels("GCAAGAGT"));

	var seq1 = AminoAcidSequence.OfLabels("IWFHGREE");
	var seq2 = AminoAcidSequence.OfLabels("WCHLREPD");
	var alignment = new Alignment<AminoAcid, int>(seq1, seq2, -8, AminoAcid.GetBlosumScore, x => x.Label.ToString());
	var aligned = alignment.Align(out var matrix, out var steps);


	Console.WriteLine(aligned);
	var seq1str = seq1.Select(x => x.Label.ToString()).ToList();
	var seq2str = seq2.Select(x => x.Label.ToString()).ToList();
	seq1str.Insert(0, "");
	seq2str.Insert(0, "");
	Console.WriteLine(Utils.PrintMatrixGeneric(seq1str.ToArray(), seq2str.ToArray(), matrix));
	Console.WriteLine(Utils.PrintMatrixGeneric(seq1str.ToArray(), seq2str.ToArray(), steps, Utils.StepTypeToString));
	Console.WriteLine(Utils.SubstitutionMatrix(seq1, seq2, AminoAcid.GetBlosumScore, x => x.Label.ToString()));

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