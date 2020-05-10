﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MetaHeuristic;

public class GeneticIndividual : Individual {


	public GeneticIndividual(int[] topology, int numberOfEvaluations, MutationType mutation) : base(topology, numberOfEvaluations, mutation) {
	}

	public override void Initialize () 
	{
		for (int i = 0; i < totalSize; i++)
		{
			genotype[i] = Random.Range(-1.0f, 1.0f);
		}
	}

   public override void Initialize(NeuralNetwork nn)
    {
        if (nn.networkSize != totalSize)
        {
            throw new System.Exception("The Networks do not have the same size!");
        }
        Debug.Log(nn.weights.SelectMany(listsLevel0 => listsLevel0.SelectMany(a => a).ToArray()).ToArray());
    }

    public override Individual Clone()
    {
        GeneticIndividual new_ind = new GeneticIndividual(this.topology, this.maxNumberOfEvaluations, this.mutation);

        genotype.CopyTo(new_ind.genotype, 0);
        new_ind.fitness = this.Fitness;
        new_ind.evaluated = false;

        return new_ind;
    }


    public override void Mutate(float probability)
    {
        switch (mutation)
        {
            case MetaHeuristic.MutationType.Gaussian:
                MutateGaussian(probability);
                break;
            case MetaHeuristic.MutationType.Random:
                MutateRandom(probability);
                break;
        }
    }
    public void MutateRandom(float probability)
    {
        for (int i = 0; i < totalSize; i++)
        {
            if (Random.Range(0.0f, 1.0f) < probability)
            {
                genotype[i] = Random.Range(-1.0f, 1.0f);
            }
        }
    }

    
    public void MutateGaussian(float probability)
    {
        float mean = 0.0f;
        float stdev = 0.5f;

        for(int i = 0; i < totalSize; i++)
        {
            if(Random.Range(0.0f, 1.0f) < probability)
            {
                genotype[i] = genotype[i] + NextGaussian(mean, stdev);
            }
        }
        
    }

    public override void Crossover(Individual partner, float probability)
    {
        /* YOUR CODE HERE! */
        /* Nota: O crossover deverá alterar ambos os indivíduos */
        int locus = Random.Range(0, partner.Size);

        float[] partnerGenotype = partner.getGenotype();

        for(int i = locus; i < genotype.Length; i++)
        {
            // Nao sei se este if e preciso
            if (Random.Range(0.0f, 1.0f) < probability)
            {
                partnerGenotype[i] = this.genotype[i];
            }
        }

        partner.setGenotype(partnerGenotype);
    }


}
