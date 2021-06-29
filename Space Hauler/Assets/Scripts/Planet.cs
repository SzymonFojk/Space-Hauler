using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody))]
public class Planet : MonoBehaviour
{

    [SerializeField] public float radius;
    [SerializeField] public float surfaceGravity;
    [SerializeField] public Vector3 initPlanetVelocity;

    public Vector3 velocity { get; private set; }
    public float planetMass { get; private set; }
    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.mass = planetMass;
        velocity = initPlanetVelocity;
    }

    public void UpdateVelocity(Planet[] allBodies, float timeStep) {
        foreach (var otherBody in allBodies) {
            if (otherBody != this) {
                float sqrDst = (otherBody.rb.position - rb.position).sqrMagnitude;
                Vector3 forceDir = (otherBody.rb.position - rb.position).normalized;

                Vector3 acceleration = forceDir * 0.0001f * otherBody.planetMass / sqrDst;
                velocity += acceleration * timeStep;
            }
        }
    }


    public void UpdatePosition(float timeStep) {
        rb.MovePosition(rb.position + velocity * timeStep);
    }

    void OnValidate() {
        planetMass = surfaceGravity * radius * radius / 0.0001f;

    }
    
    public Vector3 Position {
        get{ return rb.position; }
    }

}