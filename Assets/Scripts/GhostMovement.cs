using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{

    public Transform player;
    public float moveSpeed = 1;

    public Vector3 offsetFromPlayer = new Vector3(5f, 5f, 0);

    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.position - offsetFromPlayer;
    }

    // Update is called once per frame
    void Update()
    {

        //move towards the player
        transform.LookAt(player);
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

    }
}
