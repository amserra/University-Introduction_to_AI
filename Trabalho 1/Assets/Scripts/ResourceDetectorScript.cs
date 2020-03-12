using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDetectorScript : MonoBehaviour {

    public float angleOfSensors = 10f;
    public float rangeOfSensors = 0.1f;
    protected Vector3 initialTransformUp;
    protected Vector3 initialTransformFwd;
    public float strength;
    public float s;
    public float angle;
    public int numObjects;
    public bool debug_mode;

    public float mean = 0.5f, variance = 0.12f;
    public float inferiorX = 0, superiorX = 1, inferiorY = 0 , superiorY = 1;
    // Start is called before the first frame update
    void Start() {
        // Transform da acesso a posicao, rotacao e escala de um objeto. Todos os objetos numa scene tem uma.
        initialTransformUp = this.transform.up; // Vetor dos Y(verde)
        initialTransformFwd = this.transform.forward; // Vetor normalizado do eixo dos Z(azul)
    }

    // FixedUpdate is called at fixed intervals of time
    void FixedUpdate() {
        ObjectInfo pickup;
        pickup = GetClosestPickup();
        if (pickup != null) {
            angle = pickup.angle;
            // Formula no enunciado
            strength = 1.0f / (pickup.distance + 1.0f);
        }
        else { // no object detected: nao se mexe
            strength = 0;
            angle = 0;
        }
    }

    public float GetAngleToClosestResource() {
        return angle;
    }

    public float GetLinearOuput() {
        return strength;
    }

    // https://en.wikipedia.org/wiki/Normal_distribution (Coluna direita, PDF)
    public float GetGaussianOutput() {
        return Mathf.Exp(-0.5f * Mathf.Pow((strength - mean) / variance, 2));

    }

    // Os valores depois do = sao os default
    // Strength(x) e s(y, output) e entre 0 e 1
    public float GetLogaritmicOutput() {
        return -Mathf.Log(strength);
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
                    if (debug_mode) {
                        Debug.DrawRay(this.transform.position, Quaternion.AngleAxis((-angleOfSensors * i), initialTransformUp) * initialTransformFwd * hit.distance, Color.red);
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

    public ObjectInfo[] GetVisiblePickups() {
        return (ObjectInfo[]) GetVisibleObjects("Pickup").ToArray();
    }

    // 
    public ObjectInfo GetClosestPickup() {
        ObjectInfo [] a = (ObjectInfo[])GetVisibleObjects("Pickup").ToArray();
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
