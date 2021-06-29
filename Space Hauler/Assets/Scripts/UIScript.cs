using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIScript : MonoBehaviour
{
    [SerializeField] public Text destinationText;
    [SerializeField] public Package package;
    string destination;
    
    private void Update() {
        destinationText.text = "Package delivery to: " + package.destinationPlanet.name;

    }
}
