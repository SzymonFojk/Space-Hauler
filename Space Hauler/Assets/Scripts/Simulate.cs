using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulate : MonoBehaviour
{
    private Planet[] planets;
    private static Simulate instance;
    private readonly float timeStep = 0.01f;

    private void Awake() {
        planets = FindObjectsOfType<Planet>();
        Time.fixedDeltaTime = timeStep;
    }

    private void FixedUpdate() {
        for (int i = 0; i < planets.Length; i++) {
            planets[i].UpdateVelocity(planets, timeStep);
        }

        for (int i = 0; i < planets.Length; i++) {
            planets[i].UpdatePosition(timeStep);
        }
    }

    static Simulate Instance{
        get {
            if (instance == null) {
                instance = FindObjectOfType<Simulate>();
            }
            return instance;
        }
    }

    public static Planet[] Planets {
        get {
            return Instance.planets;
        }
    }
}
