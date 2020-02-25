using System;
using System.Collections;
using UnityEngine;

// Extende a classe RobotUnit
public class LinearRobotUnitBehaviour : RobotUnit {
    public float weightResource;
    public float resourceValue;
    public float resouceAngle;
    public float blockValue;
    public float blockAngle;

    void Update() {
        // get sensor data
        resouceAngle = resourcesDetector.GetAngleToClosestResource();

        resourceValue = weightResource * resourcesDetector.GetLinearOuput();

        // blockAngle = blockDetector.GetAngleToClosestObstacle();

        // blockValue = weightResource * blockDetector.GetLinearOuput();

        // apply to the ball
        applyForce(resouceAngle, resourceValue); // go towards

        //applyForce(blockAngle, blockValue); // go towards



    }
}