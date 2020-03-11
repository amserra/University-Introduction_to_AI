using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Criar sensor de proximidade aos blocos
public class BlockDetectorScript : MonoBehaviour {

    public float angleOfSensors;
    public float rangeOfSensors;
    protected Vector3 initialTransformUp;
    protected Vector3 initialTransformFwd;
    public float strength;
    public float s;
    public float angleToClosestObstacle;
    public int numObjects;
    public bool debugMode;
    public RobotUnit agent;

    public float mean = 0.5f, variance = 0.12f;
    public float inferiorX = 0, superiorX = 1, inferiorY = 0 , superiorY = 1;
    // Start is called before the first frame update
    void Start() {
        initialTransformUp = this.transform.up;
        initialTransformFwd = this.transform.forward;
        agent = GameObject.FindObjectsOfType<RobotUnit>()[0];

    }

    // Update is called once per frame
    void FixedUpdate() {
        ObjectInfo obstacle;
        obstacle = GetClosestObstacle();
        //Caso ainda nao tenha apanhado os recursos todos, continua a afastar se de obstaculos
        if(agent.resourcesGathered < agent.maxObjects) {
        	//Caso encontre um obstaculo roda afasta se num angulo dado pela variavel angleOffSet
            if (obstacle != null) {
                angleToClosestObstacle = obstacle.angle; 
                // Formula no enunciado
                strength = 1.0f / (obstacle.distance + 1.0f);
            }
        } else { //Caso apanhou os recursos todos, o robo fica parado
            strength = 0;
            angleToClosestObstacle = 0;
        }
    }

    public float GetAngleToClosestObstacle() {
        return angleToClosestObstacle;
    }

    public float GetLinearOuput() {
        if (strength >= inferiorX && strength <= superiorX) {
            if (strength > inferiorY && strength < superiorY) {
                return strength;// Retorna float, math.log e double
            } else if(strength >= superiorY) {
                return superiorY;
            } else if(strength <= inferiorY) {
                return inferiorY;
            }
        }
        //  else if(strength < inferiorX || strength > superiorX) {
        else {
            return inferiorY;
        }
        // So se n der
        return strength;
    }

    // https://en.wikipedia.org/wiki/Normal_distribution (Coluna direita, PDF)
    public virtual float GetGaussianOutput() {
        if (strength >= inferiorX && strength <= superiorX) {
            s = (1 / (variance * (float)Math.Sqrt(2 * Math.PI))) * (float)Math.Exp(-0.5 * Math.Pow((strength - mean) / variance, 2));
            if (s > inferiorY && s < superiorY) {
                return s;// Retorna float, math.log e double
            } else if(s >= superiorY) {
                return superiorY;
            } else if(s <= inferiorY) {
                return inferiorY;
            }
        }
        //  else if(strength < inferiorX || strength > superiorX) {
        else {
            return inferiorY;
        }
        // So se n der
        return strength;
    }

    // Os valores depois do = sao os default
    // Strength(x) e s(y, output) e entre 0 e 1
    public virtual float GetLogaritmicOutput() {
        if (strength >= inferiorX && strength <= superiorX && strength != 0) {
            s = (float) (-Math.Log(strength)); 
            if (s > inferiorY && s < superiorY) {
                return s;// Retorna float, math.log e double
            } else if(s >= superiorY) {
                return superiorY;
            } else if(s <= inferiorY) {
                return inferiorY;
            }
        }
        //  else if(strength < inferiorX || strength > superiorX) {
        else {
            return inferiorY;
        }
        // So se n der
        return strength;
    }

    public List<ObjectInfo> GetVisibleObjects(string objectTag) {
        RaycastHit hit;
        List<ObjectInfo> objectsInformation = new List<ObjectInfo>();

        // Se angleOfSensors = 10, entao tem 360/10=36 sensores em torno de si
        for (int i = 0; i * angleOfSensors <= 360f; i++) {
            // Physics.Raycast retorna True se o ray inteseta com um collider
            //Raycast(Vector3 origin, Vector3 direction, float maxDistance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);
            // Quaternion.AngleAxis: Creates a rotation which rotates angle degrees around axis. Neste caso a volta do eixo y (vertical)
            // Out faz passar por referencia: hit passa a ter a informacao do objeto atingido pelo Raycast
            if (Physics.Raycast(this.transform.position, Quaternion.AngleAxis(-angleOfSensors * i, initialTransformUp) * initialTransformFwd, out hit, rangeOfSensors)) {
                if (hit.transform.gameObject.CompareTag(objectTag)) {
                    if (debugMode) {
                        Debug.DrawRay(this.transform.position, Quaternion.AngleAxis((-angleOfSensors * i), initialTransformUp) * initialTransformFwd * hit.distance, Color.green);
                    }
                    ObjectInfo info = new ObjectInfo(hit.distance, angleOfSensors * i + 90);
                    objectsInformation.Add(info);
                }
            }
        }
        // Sort baseado no metodo compareTo em ObjectInfo. Em primeiro os mais longe e em ultimo os mais perto
        objectsInformation.Sort();

        return objectsInformation;
    }

    public ObjectInfo[] GetVisibleObstacles() {
        return (ObjectInfo[]) GetVisibleObjects("Wall").ToArray();
    }

    // 
    public ObjectInfo GetClosestObstacle() {
        ObjectInfo [] a = (ObjectInfo[])GetVisibleObjects("Wall").ToArray();
        if(a.Length == 0) {
            return null;
        }
        // Como a lista esta sorted o ultimo elemento e o mais perto
        return a[a.Length-1];
    }

    // Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis; applied in that order.
    private void LateUpdate() {
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, this.transform.parent.rotation.z * -1.0f);
    }
}
