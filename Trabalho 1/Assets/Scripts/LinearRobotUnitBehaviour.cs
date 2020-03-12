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
    public float resouceAngle;
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


    public enum ActivationType { Linear, Gaussiana, LogaritmicNegative };
    public ActivationType resourceActivation;
    public ActivationType blockActivation;




    void Update()
    {
        // 1 ir buscar o valor do sensor
        switch (resourceActivation)
        {

            case ActivationType.Linear:
                resourceValue = resourcesDetector.GetLinearOuput();
                break;
            case ActivationType.Gaussiana:

                resourceValue = resourcesDetector.GetGaussianOutput();
                break;
            case ActivationType.LogaritmicNegative:
                resourceValue = resourcesDetector.GetLogaritmicOutput();
                break;
            default:
                resourceValue = resourcesDetector.GetLinearOuput();
                break;
        }

        switch (blockActivation)
        {
            case ActivationType.Linear:
                blockValue = blockDetector.GetLinearOuput();
                break;
            case ActivationType.Gaussiana:
                blockValue = blockDetector.GetGaussianOutput();
                break;
            case ActivationType.LogaritmicNegative:
                blockValue = blockDetector.GetLogaritmicOutput();
                break;
            default:
                blockValue = blockDetector.GetLinearOuput();
                break;
        }

        // get sensor data
        resouceAngle = resourcesDetector.GetAngleToClosestResource();

        blockAngle = blockDetector.GetAngleToClosestObstacle() + angleOffset;

        //Ir buscar a strength
        strengthResource = resourcesDetector.strength;

        strengthBlock = blockDetector.strength;

        // 2 aplicar limiares e limites 
        //Resources
        if (strengthResource >= inferiorXresource && strengthResource <= superiorXresource) {
            if(resourceValue >= superiorYresource) {
                resourceValue = superiorYresource;
            } else if(resourceValue <= inferiorYresource) {
                resourceValue = inferiorYresource;
            }
        }
        else {
            resourceValue = inferiorYresource;
        }

        //Blocks
        if (strengthBlock >= inferiorXblock && strengthBlock <= superiorXblock) {
            if (resourceBlock >= superiorYblock) {
                resourceBlock = superiorYblock;
            }
            else if (resourceValue <= inferiorYblock) {
                resourceBlock = inferiorYblock;
            }
        }
        else {
            resourceBlock = inferiorYblock;
        }

        // 3 aplicar limites

        //Tudo no ponto 2???

        // 4 (opcional) modificar o peso

        //Colocar outro valor para o bloco

        resourceValue *= weightResource;
        blockValue *= weightBlock;

        // apply to the ball
        applyForce(resouceAngle, resourceValue); // go towards

        applyForce(blockAngle, blockValue); // go towards



    }
}