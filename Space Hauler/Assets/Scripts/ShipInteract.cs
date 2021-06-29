using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInteract : MonoBehaviour
{

    [SerializeField] public PlayerControl pControls;

    private ShipControl sControls;
    private Transform door1;
    private Transform door2;
    private Transform console;

    private RaycastHit hit;
    private bool isClosed;
    private void Awake() {
        door1 = transform.Find("Door1");
        door2 = transform.Find("Door2");
        console = transform.Find("Console");
        sControls = GetComponent<ShipControl>();

        isClosed = true;
    }

    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Input.GetKeyDown(KeyCode.F)) {
            if(Physics.Raycast(ray, out hit, 2f)){
                if(isClosed && (hit.collider.transform == door1.transform || hit.collider.transform == door2.transform)){
                    door1.localPosition += new Vector3(0f, 0f, -1.97f);
                    door2.localPosition += new Vector3(0f, 0f, 1.82f);
                    isClosed = false;
                } else if(!isClosed && (hit.collider.transform == door1.transform || hit.collider.transform == door2.transform)){
                    door1.localPosition -= new Vector3(0f, 0f, -1.97f);
                    door2.localPosition -= new Vector3(0f, 0f, 1.82f);
                    isClosed = true;
                } else if(!sControls.IsPiloted && hit.collider.transform == console.transform){
                    sControls.IsPiloted = true;
                    pControls.cam.transform.parent = transform;
                    //pControls.cam.transform.localPosition = new Vector3(13f, 0f, 0f);
                    pControls.cam.transform.localPosition = new Vector3(-30f, 2f, 0f);
                    pControls.cam.transform.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
                    pControls.gameObject.SetActive(false);
                    Debug.Log("Pilotowany");
                }
            } else {
                if(sControls.IsPiloted && sControls.IsGrounded){
                    sControls.IsPiloted = false;
                    pControls.transform.position = console.transform.position + new Vector3(-1f, 0f ,0f);
                    pControls.transform.rotation = console.transform.rotation;
                    pControls.rb.velocity = GetComponent<Rigidbody>().velocity;
                    pControls.gameObject.SetActive(true);
                    pControls.cam.transform.parent = pControls.transform;
                    pControls.cam.transform.localPosition = new Vector3(0f, 0.81f, 0.049f);
                }
            }
        }
    }
}
