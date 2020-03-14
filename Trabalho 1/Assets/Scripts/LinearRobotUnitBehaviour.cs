
using System;
using System.Collections;
using UnityEngine;

// Extende a classe RobotUnit
public class LinearRobotUnitBehaviour : RobotUnit
{
    // Estes parametros sao definidos no editor do Unity. Pode ser vista a sua variacao em runtime no editor
    public float weightResource; // Peso do nosso agente. Quanto menor mais rapido andara (mais leve sera)
    public float weightBlock;
    public float resourceValue;
    public float resourceAngle;
    public float blockValue;
    public float blockAngle;
    public float strengthResource;
    public float strengthBlock;
    public float angleOffset;

    public float inferiorXresource = 0f;
    public float superiorXresource = 1f;
    public float inferiorYresource = 0f;
    public float superiorYresource = 1f;
    public float inferiorXblock = 0f;
    public float superiorXblock = 1f;
    public float inferiorYblock = 0f;
    public float superiorYblock = 1f;

    public float meanResource = 0.5f, varianceResource = 0.12f;
    public float meanBlock = 0.5f, varianceBlock = 0.12f;


    public enum ActivationType { Linear, Gaussiana, LogaritmicNegative };




    public ActivationType resourceActivation;
    public ActivationType blockActivation;




    void Update()
    {

        //Ir buscar a strength
        strengthResource = resourcesDetector.strength;


        strengthBlock = blockDetector.strength;

        // Ir buscar o angulo
        resourceAngle = resourcesDetector.GetAngleToClosestResource();

        blockAngle = blockDetector.GetAngleToClosestObstacle() + angleOffset;






        // Caso esteja dentro do limiar ir buscar o valor do sensor

        //Para o resource
        if (strengthResource >= inferiorXresource && strengthResource <= superiorXresource)
        {
            resourceValue = resourceActivationFunction();
        }
        else
        {
            resourceValue = inferiorYresource;
        }

        //Para o block
        if (strengthBlock >= inferiorXblock && strengthBlock <= superiorXblock)
        {
            blockValue = blockActivationFunction();
        }
        else
        {
            blockValue = inferiorYblock;
        }






        // Aplicar limites 

        //Resources
        if (resourceValue > superiorYresource)
        {
            resourceValue = superiorYresource;
        }
        else if (resourceValue < inferiorYresource)
        {
            resourceValue = inferiorYresource;
        }



        //Blocks
        if (blockValue > superiorYblock)
        {
            blockValue = superiorYblock;
        }
        else if (resourceValue < inferiorYblock)
        {
            blockValue = inferiorYblock;
        }


        Debug.Log("Block Value: " + blockValue);



        // (opcional) modificar o peso

        //Colocar outro valor para o bloco

        resourceValue *= weightResource;
        blockValue *= weightBlock;


        if (resourcesGathered == maxObjects)
        {
            resourceValue = 0;
            blockValue = 0;

        }


        // apply to the ball
        applyForce(resourceAngle, resourceValue); // go towards


        applyForce(blockAngle, blockValue); // go towards



    }

    private float resourceActivationFunction()
    {
        float value;
        switch (resourceActivation)
        {
            case ActivationType.Linear:
                value = resourcesDetector.GetLinearOuput();
                break;
            case ActivationType.Gaussiana:
                value = resourcesDetector.GetGaussianOutput(meanResource, varianceResource);
                break;
            case ActivationType.LogaritmicNegative:
                value = resourcesDetector.GetLogaritmicOutput();
                break;
            default:
                value = resourcesDetector.GetLinearOuput();
                break;
        }

        Debug.Log("Gaussiana(" + strengthResource + ") = " + value);
        
        return value;
    }

    private float blockActivationFunction()
    {
        float value;
        switch (blockActivation)
        {
            case ActivationType.Linear:
                value = blockDetector.GetLinearOuput();
                break;
            case ActivationType.Gaussiana:
                value = blockDetector.GetGaussianOutput(meanBlock, varianceBlock);
                break;
            case ActivationType.LogaritmicNegative:
                value = blockDetector.GetLogaritmicOutput();
                break;
            default:
                value = blockDetector.GetLinearOuput();
                break;
        }

        return value;
    }

}


