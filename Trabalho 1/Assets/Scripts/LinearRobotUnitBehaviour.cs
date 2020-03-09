using System;
using System.Collections;
using UnityEngine;

// Extende a classe RobotUnit
public class LinearRobotUnitBehaviour : RobotUnit {
    // Estes parametros sao definidos no editor do Unity. Pode ser vista a sua variacao em runtime no editor
    public float weightResource; // Peso do nosso agente. Quanto menor mais rapido andara (mais leve sera)
    public float resourceValue;
    public float resouceAngle;
    public float blockValue;
    public float blockAngle;
    public float strengthFactor;
    public float angleOffset;


    void Update() {
        // get sensor data
        resouceAngle = resourcesDetector.GetAngleToClosestResource();

        resourceValue = weightResource * resourcesDetector.GetLogaritmicOutput();

        blockAngle = blockDetector.GetAngleToClosestObstacle() + angleOffset;

        blockValue = weightResource * blockDetector.GetLogaritmicOutput() * strengthFactor;

        // apply to the ball
        applyForce(resouceAngle, resourceValue); // go towards

        applyForce(blockAngle, blockValue); // go towards



    }
}