using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;


public class HillClimberOptimiser : OptimisationAlgorithm
{

    private int bestCost;
    private List<int> newSolution = null;
    

    string fileName = "Assets/Logs/" + System.DateTime.Now.ToString("ddhmmsstt") + "_HillClimberOptimiser.csv";


    protected override void Begin()
    {
        CreateFile(fileName);
        bestSequenceFound = new List<GameObject>();

        // Initialization.
        base.CurrentSolution = GenerateRandomSolution(targets.Count);
        int quality = Evaluate(CurrentSolution);
        bestCost = quality;
    }

    protected override void Step()
    {

        this.newSolution = GenerateNeighbourSolution(CurrentSolution);
        int cost = Evaluate(newSolution);
        if (cost <= bestCost)
        {
            base.CurrentSolution = new List<int>(newSolution);
            bestCost = cost;
            base.bestIteration = base.CurrentNumberOfIterations;

        }

        //DO NOT CHANGE THE LINES BELLOW
        AddInfoToFile(fileName, base.CurrentNumberOfIterations, this.Evaluate(base.CurrentSolution), base.CurrentSolution);
        base.CurrentNumberOfIterations++;

    }

   

}
