using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMapOnDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<GameManager>().SpawnPlatformAndPortal(true);
            Destroy(gameObject);
        }
    }
}
