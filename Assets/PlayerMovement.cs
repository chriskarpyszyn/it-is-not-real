using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody rb;
    public float movementSpeed = 10;
    public float gravity = -9.98f;

    void Start()
    {
        Application.targetFrameRate = 144;
        Physics.gravity = new Vector3(0, gravity, 0);
    }

    // Update is called once per frame
    // We marked this as FixedUpdate because we are using it with Unity Physics
    //todo-ck better to check for input in the update method and set a bool to update in fixedUpdate

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gravity = -gravity;
            Physics.gravity = new Vector3(0, gravity, 0);
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(movementSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-movementSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

   
    }
}
