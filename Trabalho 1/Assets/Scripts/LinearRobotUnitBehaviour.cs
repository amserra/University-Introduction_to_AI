using System;
using System.Collections;
using UnityEngine;

// Extende a classe RobotUnit
public class LinearRobotUnitBehaviour : RobotUnit
{
    // Estes parametros sao definidos no editor do Unity. Pode ser vista a sua variacao em runtime no editor
    public float weightResource; // Peso do nosso agente. Quanto menor mais rapido andara (mais leve sera)
    public float resourceValue;
    public float resouceAngle;
    public float blockValue;
    public float blockAngle;
    public float strengthFactorResource = 1f;
    public float strengthFactorBlock = 1f;
    public float angleOffset;
    public int functionTypeResource;
    public int functionTypeBlock;


    void Update()
    {

        switch (functionTypeResource)
        {
            case 1:
                resourceValue = weightResource * resourcesDetector.GetLinearOuput() * strengthFactorResource;
                break;
            case 2:
                resourceValue = weightResource * resourcesDetector.GetGaussianOutput() * strengthFactorResource;
                break;
            case 3:
                resourceValue = weightResource * resourcesDetector.GetLogaritmicOutput() * strengthFactorResource;
                break;
            default:
                resourceValue = weightResource * resourcesDetector.GetLinearOuput() * strengthFactorResource;
                break;
        }

        switch (functionTypeBlock)
        {
            case 1:
                blockValue = weightResource * blockDetector.GetLinearOuput() * strengthFactorResource;
                break;
            case 2:
                blockValue = weightResource * blockDetector.GetGaussianOutput() * strengthFactorResource;
                break;
            case 3:
                blockValue = weightResource * blockDetector.GetLogaritmicOutput() * strengthFactorResource;
                break;
            default:
                blockValue = weightResource * blockDetector.GetLinearOuput() * strengthFactorResource;
                break;
        }


        // get sensor data
        resouceAngle = resourcesDetector.GetAngleToClosestResource();

        blockAngle = blockDetector.GetAngleToClosestObstacle() + angleOffset;


        Debug.Log("Value: " + resourceValue);

        // apply to the ball
        applyForce(resouceAngle, resourceValue); // go towards

        applyForce(blockAngle, blockValue); // go towards



    }
}