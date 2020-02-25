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

    void Update() {
        // get sensor data
        resouceAngle = resourcesDetector.GetAngleToClosestResource();

        resourceValue = weightResource * resourcesDetector.GetLinearOuput();

        blockAngle = blockDetector.GetAngleToClosestObstacle();

        blockValue = weightResource * blockDetector.GetLinearOuput();

        // apply to the ball
        applyForce(resouceAngle, resourceValue); // go towards

        applyForce(blockAngle, blockValue); // go towards



    }
}