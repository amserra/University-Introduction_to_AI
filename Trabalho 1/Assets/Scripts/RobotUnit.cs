using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
MonoBehaviour is the base class from which every Unity script derives.
When you use C#, you must explicitly derive from MonoBehaviour.
The functions are: Start(), Update(), FixedUpdate(), LateUpdate(), OnGUI(), OnDisable(), OnEnable()
*/
public class RobotUnit : MonoBehaviour {
    public int resourcesGathered;
    /*
    Control of an object's position through physics simulation.
    Adding a Rigidbody component to an object will put its motion under the control of Unity's physics engine. 
    Even without adding any code, a Rigidbody object will be pulled downward by gravity and will react to collisions with incoming objects 
    if the right Collider component is also present.
    */
    protected Rigidbody rb;
    public float speed = 1.0f;
    //protected float angle;
    //public float strength;
    public Text countText;
    public float startTime;
    public float timeElapsed = 0.0f;
    public ResourceDetectorScript resourcesDetector; // Usado pelo LinearRobotUnitBehaviour
    public BlockDetectorScript blockDetector; // Para ser usado pelo BlockDectorScript
    private List<Tuple<float, float>> listAngleStr;
    public bool debugMode = true;
    protected int maxObjects = 0;

    /*GameObjects are the fundamental objects in Unity that represent characters, props and scenery. 
    They do not accomplish much in themselves but they act as containers for Components,
    which implement the real functionality. 
    For example, a Light object is created by attaching a Light component to a GameObject.
    */

    // Start is called before the first frame update
    void Start() {
        //strength = 0.0f;
        // Objetos que tem de apanhar
        maxObjects = GameObject.FindGameObjectsWithTag("Pickup").Length;
        resourcesGathered = 0;
        rb = GetComponent<Rigidbody>();
        listAngleStr = new List<Tuple<float, float>>();
        this.startTime = Time.time;
        timeElapsed = Time.time - startTime;
        // Define o texto apresentado (canto sup. esquerdo)
        SetCountText();
    }

    /* Enquanto update e executado 1 vez/frame, fixedUpdate esta sincronizado com as fisicas do corpo. Como usamos
    fisicas, usamos fixedUpdate em vez de update */
    void FixedUpdate() {
        int i = 0;
        foreach(Tuple<float,float> tmp in listAngleStr) {
            
            float angle = tmp.Item1;
            float strength = tmp.Item2;
            angle *= Mathf.Deg2Rad;
            float xComponent = Mathf.Cos(angle);
            float zComponent = Mathf.Sin(angle);
            Vector3 forceDirection = new Vector3(xComponent, 0, zComponent);
            if (debugMode) {
                Debug.DrawRay(this.transform.position, (forceDirection * strength * speed) , i == 0 ? Color.black :Color.magenta );
            }
            // Define o vetor forca(angulo e intensidade) e aplica-a no robo
            rb.AddForce(forceDirection * strength * speed);

            i++;
        }
        listAngleStr.Clear(); // cleanup
    }

    // Chamado depois do Update. Atualiza o texto
    private void LateUpdate() {
        SetCountText();
    }

    // Define o texto apresentado e atualiza
    void SetCountText() {
        // Se ainda nao apanhou todos, atualiza o tempo
        if(resourcesGathered < maxObjects) {
            this.timeElapsed = Time.time - this.startTime;
        }
        string minutes = ((int)(timeElapsed / 60)).ToString();
        string seconds = (timeElapsed % 60).ToString("f0");
        countText.text = "Resources Gathered: " + resourcesGathered.ToString() + "/" + maxObjects + "\nTime Elapsed: " + minutes + ":" + seconds; //start
    }

    // Adiciona forca a ser aplicada com base nos dados recebidos pelo sensor
    public void applyForce(float angle, float strength) {
        listAngleStr.Add(new Tuple<float, float>(angle, strength));
    }

    // Callback chamado quando um GameObject colide com outro GameObject
    private void OnTriggerEnter(Collider other) {
        // E se for uma parede? Comportamento para isso?
        // Se for uma pickup, apanha-a
        if(other.gameObject.CompareTag("Pickup")) {
            other.gameObject.SetActive(false);
            resourcesGathered++;
        }
        else {
            // Morre
            if (other.gameObject.CompareTag("Deadly")) {
                Debug.Log("Destroyed!");
                this.gameObject.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}