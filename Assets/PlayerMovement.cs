using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody rb;
    public float movementSpeed = 10;
    public float jumpForce = 5;
    public float gravity = -9.98f;
    public float gravityOffset = 2;

    public float maxSpeed = 4f;

    private bool playerIsInAir = false;


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
        float playerY = transform.position.y;

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    gravity = -gravity;
        //    Physics.gravity = new Vector3(0, gravity, 0);
        //}


        if ((playerY > gravityOffset && gravity > 0) || (playerY < -gravityOffset && gravity < 0))
        {
            Debug.Log("In Gravity Switch");
            gravity *= -1;
            Physics.gravity = new Vector3(0, gravity, 0);
        }

       


        //player input
        if (Input.GetKey(KeyCode.W) && rb.velocity.x < maxSpeed)
        {
            rb.AddForce(movementSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        if (Input.GetKey(KeyCode.S) && rb.velocity.x > -maxSpeed)
        {
            rb.AddForce(-movementSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !playerIsInAir)
        {
            int invert = 1;
            if (isGravityInverted())
            {
                invert = -1;
            }
            rb.AddForce(0, jumpForce*invert, 0, ForceMode.Impulse);
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        //todo-ck I know there is a better way to do this
        playerIsInAir = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //todo-ck need to check for platform and not other types of collision
        playerIsInAir = false;
    }

    private bool isGravityInverted()
    {
        if (gravity>0)
        {
            return true;
        }
        return false;
    }
}
