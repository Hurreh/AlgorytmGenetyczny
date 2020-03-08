using System;
using System.Collections.Generic;
using System.Text;

namespace MyFirstGeneticAlgorythm
{
    class GeneticAlgorithm<T>
    {
        public List<DNA<T>> Population { get; private set; }
        public int Generation { get; private set; }
        public float BestFitness { get; private set; }
        public T[] BestGenes { get; private set; }

        public int Elitism;
        public float MutationRate;

        private Random random;
        private float fitnessSum;
        private int dnaSize;
        private Func<T> getRandomGene;
        private Func<int, float> fitnessFunction;
        //default constructor
        public GeneticAlgorithm(int populationSize, int dnaSize, Random random, Func<T> getRandomGene, Func<int, float> fitnessFunction,int elitism, float mutationRate = 0.01f)
        {
            Elitism = elitism;
            //iteration
            Generation = 1;
            //Mutation rate is equal to given mutation rate
            MutationRate = mutationRate;
            //Population which is a list of DNA type. That makes sense because population is defined by certain atributtes
            Population = new List<DNA<T>>(populationSize);
            //new random apperance which is of type random(generates random numbers)
            this.random = random;
            this.dnaSize = dnaSize;
            this.getRandomGene = getRandomGene;
            this.fitnessFunction = fitnessFunction;
            //New subject generator
            for (int i = 0; i < populationSize; i++)
            {
                Population.Add(new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction, shouldInitGenes: true));
            }
            //Initilazing the BestGenes array for it to not throw nulls
            BestGenes = new T[dnaSize];
        }
        //Creates a new generation
        public void NewGeneration(int numNewDNA = 0, bool crossoverNewDNA = false)
        {
            int finalCount = Population.Count + numNewDNA;

            if (finalCount <= 0)
            {
                return;
            }

            if(Population.Count>0)
            {
                CalculateFitness();
                Population.Sort(CompareDNA);
            }
            
            //Creating new population based on the old one
            List<DNA<T>> newPopulation = new List<DNA<T>>();

            for (int i = 0; i < Population.Count; i++)
            {   
                if(i < Elitism && i < Population.Count)
                {
                    Population.Add(Population[i]);
                }
                else if (i < Population.Count || crossoverNewDNA)
                {
                    //mix'em up
                    DNA<T> parent1 = ChooseParent();
                    DNA<T> parent2 = ChooseParent();
                    DNA<T> child = parent1.Crossover(parent2);

                    //Decides whether the gene mutates or not
                    child.Mutate(MutationRate);

                    newPopulation.Add(child);              
                }
                else
                { 
                    newPopulation.Add(new DNA<T>(dnaSize, random, getRandomGene,fitnessFunction,shouldInitGenes: true));
                }
                
                
                
            }
            //After creating a new generation, the old population is replaced with a new one
            Population = newPopulation;
            Generation++;

        }
        //Chooses a parent for the function above
        private DNA<T> ChooseParent()
        {
            //we set our prime requirement
            double randomNumber = random.NextDouble() * fitnessSum;
            for (int i = 0; i < Population.Count; i++)
            {
                //We check if individual is stronger that generated number
                if (randomNumber < Population[i].Fitness)
                {
                    //if it is then we return him
                    return Population[i];
                }
                //otherwise we lower the requirement
                randomNumber -= Population[i].Fitness;
            }
            return null;
        }
        public int CompareDNA(DNA<T> a, DNA<T> b)
        {
            if (a.Fitness > b.Fitness)
                return -1;
            else if (a.Fitness < b.Fitness)
                return 1;
            else
                return 0;           
        }
        //Calculates the fitness of certain individual 
        public void CalculateFitness()
        {
            fitnessSum = 0;
            //Initiating 
            DNA<T> best = Population[0];
            for (int i = 0; i < Population.Count; i++)
            {   
               //misc. data on fitness 
               fitnessSum += Population[i].CalculateFitness(i);
               //Search for the best individual
                if (Population[i].Fitness > best.Fitness)
                {
                    best = Population[i];
                }
            }
            //Collecting data on the best individual
            BestFitness = best.Fitness;
            best.Genes.CopyTo(BestGenes, 0);
        }
        

    }
}
