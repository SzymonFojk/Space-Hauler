using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{

    public bool IsPiloted { get; set; }
    public bool IsGrounded { get; protected set; }
    public Rigidbody rb { get; set; }

    [SerializeField] public Transform groundChecker;
    [SerializeField] public LayerMask groundMask;

    private Vector3 thruster;
    private Quaternion targetRot;
    private Quaternion smoothedRot;

    private void Awake() {
        targetRot = transform.rotation;
        smoothedRot = transform.rotation;
        rb = GetComponent<Rigidbody>();
        rb.mass = 200;
        rb.useGravity = false;
        IsPiloted = false;
    }
    private void Update() {
        if(IsPiloted) Movement();

        IsGrounded = Physics.CheckSphere(groundChecker.position, 3f, groundMask, QueryTriggerInteraction.Ignore);
    }

    private void FixedUpdate() {
        Planet[] allPlanets = Simulate.Planets;
        Vector3 strongestGravitionalPull = Vector3.zero;

        foreach (Planet body in allPlanets) {
            float r2 = (body.Position - rb.position).sqrMagnitude;
            Vector3 fDir = (body.Position - rb.position).normalized;
            Vector3 acc = fDir * 0.0001f * body.planetMass / r2;
            rb.AddForce(acc, ForceMode.Acceleration);

            if (acc.sqrMagnitude > strongestGravitionalPull.sqrMagnitude) strongestGravitionalPull = acc;
        }

        Vector3 gravityUp = -strongestGravitionalPull.normalized;
        rb.rotation = Quaternion.FromToRotation(transform.up, gravityUp) * rb.rotation;

        rb.AddForce(transform.TransformVector(thruster) * 20f, ForceMode.Acceleration);
        rb.MoveRotation (smoothedRot);
    }

    private void Movement() {

        float axisY = GetAxisFromInput(KeyCode.Space, KeyCode.LeftShift);
        float axisZ = GetAxisFromInput(KeyCode.A, KeyCode.D);
        float axisX = GetAxisFromInput(KeyCode.W, KeyCode.S);

        thruster = new Vector3(axisX, axisY, axisZ);

        float yawInput = Input.GetAxisRaw ("Mouse X") * 5;
        float pitchInput = Input.GetAxisRaw ("Mouse Y") * 5;
        float rollInput = GetAxisFromInput (KeyCode.E, KeyCode.Q) * 30 * Time.deltaTime;
 
        var yaw = Quaternion.AngleAxis (yawInput, transform.up);
        var pitch = Quaternion.AngleAxis (-pitchInput, transform.forward);
        var roll = Quaternion.AngleAxis (-rollInput, transform.right);

        targetRot = yaw * pitch * roll * targetRot;
        smoothedRot = Quaternion.Slerp (transform.rotation, targetRot, Time.deltaTime * 10);
    }


    float GetAxisFromInput(KeyCode pos, KeyCode neg) {
        float axis = 0;
        if(Input.GetKey(pos)) axis+=1f;
        else if(Input.GetKey(neg)) axis-=1f;
        return axis;
    }
}
