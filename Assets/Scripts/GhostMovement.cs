using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public float moveSpeed = 1;
    GameManager gameManager;
    PlayerMovement playerMovement;

    //public Vector3 offsetFromPlayer = new Vector3(5f, 5f, 0);

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerMovement = gameManager.player.GetComponent<PlayerMovement>();
        //transform.position = player.position - offsetFromPlayer;
    }

    // Update is called once per frame
    void Update()
    {

        //move towards the player
        transform.LookAt(playerMovement.transform);
        if (playerMovement.getIsDreaming())
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

    }
}
