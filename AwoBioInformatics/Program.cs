using AwoBioInformatics;

while (true)
{
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
			Console.WriteLine("ShortCodes: " + ProteinSequence.OfRna(seq).ToShortCodes());
			Console.WriteLine("Labels: " + ProteinSequence.OfRna(seq).ToLabels());
		}
		else
		{
			Console.WriteLine($"Rna: " + ProteinSequence.OfShortCodes(seq).ToRna());
			Console.WriteLine("Labels: " + ProteinSequence.OfShortCodes(seq).ToLabels());
		}
	}


}