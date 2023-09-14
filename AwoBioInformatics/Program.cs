using AwoBioInformatics;

while (true)
{
	Console.Write("Sequence: ");
	var sequence = Console.ReadLine();
	var dna = Nucleotide.SequenceOf(sequence);

	Console.WriteLine($"DNA Counterpart: {dna.DnaCounterpart().ToString()}");
	Console.WriteLine($"RNA Counterpart: {dna.RnaCounterpart().ToString()}");
}