using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FollowPlayer : MonoBehaviour
{

    public Transform player;
    public Vector3 offset = new Vector3(0, 0.5f, -8);

    void Update()
    {
        float cameraY;
        Transform platformTransform = player.GetComponent<PlayerMovement>().platformTransform;

        if (player.position.y >= 0.35 && player.position.y < 0.4)
        {
            cameraY = 0.35f;
        }
        else if (player.position.y <= -0.35 && player.position.y > -0.4)
        {
            cameraY = -0.35f;
        } else
        {
            cameraY = player.position.y;
        }

        transform.position = new Vector3(player.position.x, cameraY, offset.z);
    }
}
