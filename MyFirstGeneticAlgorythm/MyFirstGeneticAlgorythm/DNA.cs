using System;
using System.Collections.Generic;
using System.Text;

namespace MyFirstGeneticAlgorythm
{
    class DNA<T>
    {
        //genetic array

        public T[] Genes { get; set; }
        //Fitness of individual
        public float Fitness { get; set; }
        //Using random like that saves memory and ensures numbers being actually random.
        private Random random;
        //It will generate a random object of gene type
        private Func<T> getRandomGene;
        private Func<int, float> fitnessFunction;

        public DNA(int size, Random random, Func<T> getRandomGene,Func<int, float> fitnessFunction, bool shouldInitGenes = true)
        {
            Genes = new T[size];
            this.random = random;
            this.getRandomGene = getRandomGene;
            this.fitnessFunction = fitnessFunction;

            if(shouldInitGenes)
            { 
                for (int i = 0; i < Genes.Length; i++)
                {
                    Genes[i] = getRandomGene();
                    
                }
            }
        }

        //Calculating how likely is certain individual to reproduce
        public float CalculateFitness(int index)
        {
            Fitness = fitnessFunction(index);
            return Fitness;
        }

        //Crossover - returns object DNA by joining one object with the other

        public DNA<T> Crossover(DNA<T> otherParent)
        {
            //We generate a new child object with a Gene array size identical to it's parent's
            DNA<T> child = new DNA<T>(Genes.Length, random, getRandomGene, fitnessFunction, shouldInitGenes: false);
            


            //We combine genes of parents to create new genes
            for (int i = 0; i < Genes.Length; i++)
            {
                //Flip of a coin decides which parent's genes we choose
               //if a generated number is smaller than 0.5 use one parent's genes otherwise use other's
                child.Genes[i] = random.NextDouble() < 0.5f ? Genes[i] : otherParent.Genes[i];
                
            }
            return child;
        }
        //How likely is it to mutate a gene?
        public void Mutate(float mutationRate)
        {
            for (int i = 0; i < Genes.Length; i++)
            {
                //Checking if a gene will suffer a mutation or not
                if(random.NextDouble() < mutationRate)
                {
                    //If it checks out then we will assign it a new random gene
                    Genes[i] = getRandomGene();
                }
            }
        }

        
    }
}
