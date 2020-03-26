using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatedAnnealingOptimiser : OptimisationAlgorithm
{
    private List<int> newSolution = null;
    private int CurrentSolutionCost;
    private int NewSolutionCost;
    private float probability;
    public float Temperature;
    private float zero = Mathf.Pow(10, -6);// numbers bellow this value can be considered zero.

    string fileName = "Assets/Logs/" + System.DateTime.Now.ToString("ddhmmsstt") + "_SimulatedAnnealingOptimiser.csv";


    protected override void Begin()
    {
        CreateFileSA(fileName);
        bestSequenceFound = new List<GameObject>();

        // Initialization.
        base.CurrentSolution = GenerateRandomSolution(targets.Count);
        int quality = Evaluate(CurrentSolution);
        CurrentSolutionCost = quality;

    }

    protected override void Step()
    {

        if(Temperature > 0)
        {
            newSolution = GenerateNeighbourSolution(CurrentSolution);
            NewSolutionCost = Evaluate(newSolution);

            probability = Mathf.Exp((CurrentSolutionCost - NewSolutionCost) / Temperature);

            if(NewSolutionCost <= CurrentSolutionCost || probability > Random.Range(0,1))
            {
                base.CurrentSolution = new List<int>(newSolution);
                CurrentSolutionCost = NewSolutionCost;
            }

            Temperature = TemperatureSchedule();
        }
        else
        {
            base.CurrentNumberOfIterations = base.MaxNumberOfIterations - 1;
        }

        //DO NOT CHANGE THE LINES BELLOW
        AddInfoToFile(fileName, base.CurrentNumberOfIterations, CurrentSolutionCost, CurrentSolution, Temperature);
        base.CurrentNumberOfIterations++;

    }

    private float TemperatureSchedule()
    {
        return Temperature--;
    }


}
