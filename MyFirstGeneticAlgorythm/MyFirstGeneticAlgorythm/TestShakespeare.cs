using System;
using System.Collections.Generic;
using System.Text;

namespace MyFirstGeneticAlgorythm
{


	public class TestShakespeare
	{

		string targetString = "To be or not to be.";
		string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,.|!#$%&/()=? ";
		int populationSize = 100;
		float mutationRate = 0.01f;
		int elitism = 5;

		string targetText;

		private GeneticAlgorithm<char> ga;
		private Random random;
		public void Start()
		{
			targetText = targetString;

			if(string.IsNullOrEmpty(targetString))
			{
				Console.WriteLine("Target string cannot be empty.");
				Console.WriteLine();

			}
			random = new Random();
			ga = new GeneticAlgorithm<char>(populationSize, targetString.Length, random, GetRandomCharacter, FitnessFunction, elitism, mutationRate);
		}
		
		public void Reader()
		{
			for (int i = 0; i < ga.BestGenes.Length; i++)
			{
				Console.Write(ga.BestGenes[i]);
			}
			
		}
		public void Update()
		{
			
			while(ga.BestFitness != 1)
			{
				ga.NewGeneration();
				Reader();
				Console.WriteLine($" {ga.BestFitness}, {ga.Generation}, {ga.Population.Count}");
				Console.WriteLine();
				Console.ReadLine();
				////////////////////////////////////
				System.Threading.Thread.Sleep(100);
				////////////////////////////////////
			}
		}

		private char GetRandomCharacter()
		{
			int i = random.Next(validCharacters.Length);
			return validCharacters[i];
		}
		private float FitnessFunction(int index)
		{
			float score = 0;
			DNA<char> dna = ga.Population[index];

			for (int i = 0; i < dna.Genes.Length; i++)
			{
				if(dna.Genes[i] == targetString[i])
				{
					score += 1;
				}
			}
			score /= targetString.Length;

			score = (MathF.Pow(2, score) - 1) / (2 - 1);
			return score;
		}

	}
}	