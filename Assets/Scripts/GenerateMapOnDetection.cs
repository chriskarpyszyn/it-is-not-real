using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMapOnDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager gm = FindObjectOfType<GameManager>();
            gm.SpawnPlatformAndPortal();
            Destroy(gameObject);
            gm.doDestroyObjects();

        }
    }
}
