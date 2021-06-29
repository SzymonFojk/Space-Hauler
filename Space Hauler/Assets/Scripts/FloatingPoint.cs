using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloatingPoint : MonoBehaviour
{
    [SerializeField] private float threshold = 1000.0f;

    private void LateUpdate() {
        Vector3 camPos = transform.position;

        if (camPos.magnitude > threshold) {
            for (int z = 0; z < SceneManager.sceneCount; z++) {
                foreach (GameObject g in SceneManager.GetSceneAt(z).GetRootGameObjects()) {
                    g.transform.position -= camPos;
                }
            }
        }
    }
}