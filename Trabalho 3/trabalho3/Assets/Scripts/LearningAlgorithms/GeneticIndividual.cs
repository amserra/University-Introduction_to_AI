using System.Linq;
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
        int size = nn.weights.Length * nn.weights[0].Length * nn.weights[0][0].Length;
        if (size != totalSize)
        {
            throw new System.Exception("The Networks do not have the same size!");
        }

        float[] weights = new float[size];
        int weightPos = 0;
        for (int i = 0; i < nn.weights.Length; i++)
        {
            for (int j = 0; j < nn.weights[i].Length; j++)
            {
                for (int k = 0; k < nn.weights[i][j].Length; k++)
                {
                    weights[weightPos++] = nn.weights[i][j][k];
                }
            }
        }
        weights.CopyTo(genotype, 0);
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

        // Antes
        //for(int i = 0; i < totalSize; i++)
        // Depois
        for (int i = 0; i < this.genotype.Length; i++)
        {
            if (Random.Range(0.0f, 1.0f) < probability)
            {
                this.genotype[i] = this.genotype[i] + NextGaussian(mean, stdev);
            }
        }
    }

    public override void Crossover(Individual partner, float probability)
    {
        /* YOUR CODE HERE! */
        /* Nota: O crossover deverá alterar ambos os indivíduos */
        // int locus = Random.Range(0, partner.Size);

        // float[] partnerGenotype = partner.getGenotype();


        // for(int i = locus; i < genotype.Length; i++)
        // {
        //     // Nao sei se este if e preciso
        //     if (Random.Range(0.0f, 1.0f) < probability)
        //     {
        //         partnerGenotype[i] = this.genotype[i];
        //     }
        // }

        // partner.setGenotype(partnerGenotype);

        /* ----------- COMO ESTAVA ANTES ------------
         * 
         * int locus = Random.Range(0, this.genotype.Length);

        float[] partnerGenotype = partner.getGenotype();
        float tmpGenotype;


        for(int i = locus; i < this.genotype.Length; i++) {
            // E preciso. Ha uma probabilidade de recombinacao
            if (Random.Range(0.0f, 1.0f) < probability) {
                // Troca-se o material genetico
                tmpGenotype = partnerGenotype[i];
                partnerGenotype[i] = this.genotype[i];
                this.genotype[i] = tmpGenotype;
            }
        }*/




        // --------- Single-point crossover --------- Nova versao

        float[] partnerGenotype = partner.getGenotype();
        float tmpGenotype;

        if (Random.Range(0.0f, 1.0f) < probability)
        {
            int locus = Random.Range(0, this.genotype.Length);

            for (int i = locus; i < this.genotype.Length; i++)
            {
                tmpGenotype = partnerGenotype[i];
                partnerGenotype[i] = this.genotype[i];
                this.genotype[i] = tmpGenotype;
            }

        }

        // Nao e preciso fazer setGenotype do this, ja esta feito
        partner.setGenotype(partnerGenotype);
    }


}
