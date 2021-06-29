using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject target;
    [SerializeField] private float distanceToTarget = 10;


    [SerializeField] public float ScrollSensitvity = 2f;


    private Vector3 previousPosition;

    private Vector3 mousePos;

    private void LateUpdate()
    {
        cam.transform.position = target.transform.position;
        cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));
    }
}
