using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRespawn : MonoBehaviour
{
    public string planetToStart = "Mercury";
    public PlayerControl pControl;
    public ShipControl sControl;

    void Start(){
        GameObject startPlanet = GameObject.Find(planetToStart);
        Vector3 startLocation  = startPlanet.transform.position + Vector3.up * startPlanet.GetComponent<Planet>().radius * 1f;

        pControl.transform.position = startLocation + Vector3.right;
        pControl.rb.velocity = startPlanet.GetComponent<Planet>().initPlanetVelocity;

        sControl.transform.position = startLocation + Vector3.right * 20;
        sControl.rb.velocity = startPlanet.GetComponent<Planet>().initPlanetVelocity;
    }
}
