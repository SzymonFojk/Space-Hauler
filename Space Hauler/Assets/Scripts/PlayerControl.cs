using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Camera cam { get; set; }
    public Rigidbody rb { get; set; }

    [SerializeField] public LayerMask groundMask;
    [SerializeField] public Transform groundChecker;
    [SerializeField] public float jumpForce = 20f;
    [SerializeField] public float vSmoothTime = 0.1f;
    [SerializeField] public float airSmoothTime = 0.5f;
    [SerializeField] public float walkSpeed = 8;
    [SerializeField] public float runSpeed = 14;
    [SerializeField] public float mouseSensitivity = 10;
    [SerializeField] public float GroundDistance = 0.1f;

    private Vector3 targetVelocity;
    private Vector3 smoothVelocity;
    private Vector3 smoothVRef;
    private Light flashLight;
    private float xRotate = 0f;
    private float mouseX;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.mass = 1f;
        cam = GetComponentInChildren<Camera>();
        flashLight = GetComponentInChildren<Light>();

        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update() {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotate -= mouseY;
        xRotate = Mathf.Clamp(xRotate, -90f, 90f);
        cam.transform.localRotation = Quaternion.Euler(xRotate, 0f, 0f);
        flashLight.transform.localRotation = Quaternion.Euler(xRotate, 0f, 0f);
        
        bool isGrounded = Physics.CheckSphere(groundChecker.position, GroundDistance, groundMask, QueryTriggerInteraction.Ignore);

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        targetVelocity = transform.TransformDirection(input.normalized) * currentSpeed;
        smoothVelocity = Vector3.SmoothDamp(smoothVelocity, targetVelocity, ref smoothVRef, (isGrounded) ? vSmoothTime : airSmoothTime);

        if(Input.GetKeyDown(KeyCode.L)) flashLight.enabled = !flashLight.enabled;

        if (isGrounded){
            if (Input.GetKeyDown(KeyCode.Space)){
                rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
                isGrounded = false;
            } else {
                rb.AddForce(-transform.up * 3f, ForceMode.VelocityChange);
            }
        }
    }


    private void FixedUpdate() {
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

        rb.MovePosition(rb.position + smoothVelocity * Time.fixedDeltaTime);
        transform.Rotate(Vector3.up * mouseX);
    }
}
