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
    private float T0;
    private float zero = Mathf.Pow(10, -6);// numbers bellow this value can be considered zero.
    public enum TemperatureSchedule { BoltzmanAnnealling, FastAnnealling, VeryFastAnnealling, AdaptativeSimulatedAnnealing };
    public TemperatureSchedule temperatureSchedule;
    public int D = 6;
    public float alpha = 1;

    string fileName = "Assets/Logs/" + System.DateTime.Now.ToString("ddhmmsstt") + "_SimulatedAnnealingOptimiser.csv";


    protected override void Begin()
    {
        CreateFileSA(fileName);
        bestSequenceFound = new List<GameObject>();

        // Initialization.
        base.CurrentSolution = GenerateRandomSolution(targets.Count);
        int quality = Evaluate(CurrentSolution);
        CurrentSolutionCost = quality;
        T0 = Temperature;

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
                base.bestIteration = base.CurrentNumberOfIterations;
            }

            TemperatureScheduleFunction();
        }
        else
        {
            base.CurrentNumberOfIterations = base.MaxNumberOfIterations - 1;
        }

        //DO NOT CHANGE THE LINES BELLOW
        AddInfoToFile(fileName, base.CurrentNumberOfIterations, CurrentSolutionCost, CurrentSolution, Temperature);
        base.CurrentNumberOfIterations++;

    }

    private void TemperatureScheduleFunction() {
        switch (temperatureSchedule) {
            case TemperatureSchedule.BoltzmanAnnealling:
                Temperature = T0 / Mathf.Log(base.CurrentNumberOfIterations);
                break;
            case TemperatureSchedule.FastAnnealling:
                Temperature = T0 / base.CurrentNumberOfIterations;
                break;
            case TemperatureSchedule.VeryFastAnnealling:
                Temperature = T0 / (Mathf.Pow(base.CurrentNumberOfIterations, 1 / D));
                break;
            case TemperatureSchedule.AdaptativeSimulatedAnnealing:
                if (NewSolutionCost > CurrentSolutionCost) Temperature = alpha * Temperature;
                break;
        }

    }
}
