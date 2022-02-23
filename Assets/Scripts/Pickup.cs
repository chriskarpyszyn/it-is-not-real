using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Audio;

public class Pickup : MonoBehaviour
{
    public Transform pickupEffect;
    //private int value = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Transform effect = Instantiate(pickupEffect, transform.position, transform.rotation);

            Destroy(effect.gameObject, 3);
            Destroy(gameObject);

            FindObjectOfType<GameManager>().SpawnShield();
        }
    }
}
