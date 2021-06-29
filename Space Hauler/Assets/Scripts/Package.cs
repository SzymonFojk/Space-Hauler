using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour
{
    public GameObject destinationPlanet { get; protected set; }

    [SerializeField] public Transform groundChecker;
    [SerializeField] public LayerMask groundMask;
    [SerializeField] public float GroundDistance = 0.3f;

    private Rigidbody rb;
    private GameObject player;
    private bool notPicked;
    private GameObject chosenPlanet;
    private bool isDelivered = false;
    private bool firstResp = true;

    void Start(){
        rb = GetComponent<Rigidbody>();

        Planet planet = GameObject.Find("Mercury").GetComponent<Planet>();
        rb.velocity = planet.initPlanetVelocity;
        notPicked = true;

        player = GameObject.Find("Player");
        NewPackage();
    }
    private void Update(){

        bool isGrounded = Physics.CheckSphere(groundChecker.position, GroundDistance, groundMask, QueryTriggerInteraction.Ignore);

        if (isGrounded) rb.AddForce(-transform.up * 3f, ForceMode.VelocityChange);
        
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var destRay = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        RaycastHit desthit;

        if(Input.GetKeyDown(KeyCode.F)) {
            bool _ = Physics.Raycast(ray, out hit, 2f);

            if (notPicked && hit.transform == transform){
                Debug.Log("Picked");
                transform.parent = player.transform;
                rb.isKinematic = true;
                rb.detectCollisions = false;
                rb.interpolation = RigidbodyInterpolation.None;
                transform.localPosition = new Vector3(0f, 0.5f, 1.5f);
                notPicked = false;
            } else if (!notPicked) {
                Debug.Log("Dropped");
                transform.parent = null;
                rb.velocity = player.GetComponent<Rigidbody>().velocity;
                rb.isKinematic = false;
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.detectCollisions = true;
                notPicked = true;
                bool __ = Physics.Raycast(destRay, out desthit, 10f, groundMask);
                try{
                    Debug.Log(desthit.transform);
                    Debug.Log(destinationPlanet);
                } finally {}
  
                if(desthit.transform == destinationPlanet.transform) isDelivered = true;
                Debug.Log("EZ");
            }
        }
    }
   
    private void FixedUpdate() {
        if(notPicked) Gravity();
        if(isDelivered) NewPackage();
    }

    private void Gravity() {
        Planet[] allPlanets = Simulate.Planets;
        Vector3 strongestGravitionalPull = Vector3.zero;

        foreach (Planet body in allPlanets){
            float r2 = (body.Position - rb.position).sqrMagnitude;
            Vector3 fDir = (body.Position - rb.position).normalized;
            Vector3 acc = fDir * 0.0001f * body.planetMass / r2;
            rb.AddForce(acc, ForceMode.Acceleration);

            if (acc.sqrMagnitude > strongestGravitionalPull.sqrMagnitude){
                strongestGravitionalPull = acc;
            }
        }

        Vector3 gravityUp = -strongestGravitionalPull.normalized;
        rb.rotation = Quaternion.FromToRotation(transform.up, gravityUp) * rb.rotation;
    }

    private void NewPackage() {
        Debug.Log("FUNKCJA NEWPACKAGE");
        List<string> planetNames = new List<string> {"Mercury", "Venus"};

        if(firstResp)
            chosenPlanet = GameObject.Find("Mercury");
        else
            chosenPlanet = GameObject.Find(planetNames[Random.Range(0, planetNames.Count)]);

        Debug.Log(chosenPlanet);
        Vector3 respLocation  = chosenPlanet.transform.position + Vector3.up * chosenPlanet.GetComponent<Planet>().radius * 1f;
        transform.position = respLocation;
        rb.velocity = chosenPlanet.GetComponent<Planet>().initPlanetVelocity;
        foreach(string name in planetNames)
            if(name == chosenPlanet.name){
                planetNames.Remove(name);
                break;
            }

        destinationPlanet = GameObject.Find(planetNames[Random.Range(0, planetNames.Count)]);

        firstResp = false;
        isDelivered = false;
    }
}
