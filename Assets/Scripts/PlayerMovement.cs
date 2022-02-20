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

    public float maxSpeed = 3f;

    private bool playerIsInAir = false;


    void Start()
    {
        Application.targetFrameRate = 144;
        //Physics.gravity = new Vector3(0, gravity, 0);
    }

    // Update is called once per frame
    // We marked this as FixedUpdate because we are using it with Unity Physics
    //todo-ck better to check for input in the update method and set a bool to update in fixedUpdate

    private void Update()
    {
        float playerY = transform.position.y;


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
            rb.AddForce(0, getJumpSpeed(), 0, ForceMode.Impulse);
        }

        //***********


        //restart level if player goes too high or low
        int deathHeight = 20;
        if (playerY > deathHeight || playerY < -deathHeight)
        {
            FindObjectOfType<GameManager>().EndGame();
            resetPlayerProps();
            
        }

    }

    private void resetPlayerProps()
    {
        playerIsInAir = true;
        setGravity(-9.98f);
    }

    private void setGravity(float gravity)
    {
        Physics.gravity = new Vector3(0, gravity, 0);
    }

    private void OnCollisionExit(Collision collision)
    {
        //todo-ck I know there is a better way to do this
        if (collision.gameObject.tag == "Platform")
        {
            Debug.Log("Exit Platform");
            playerIsInAir = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //todo-ck need to check for platform and not other types of collision
        if (collision.gameObject.tag == "Platform")
        {
            Debug.Log("Collision with Platform");
            playerIsInAir = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Portal")
        {
            Debug.Log("Enter Portal");
            gravity *= -1;
            setGravity(gravity);
            rb.AddForce(0, getJumpSpeed(-1), 0, ForceMode.Impulse);
        }
    }

    private bool isGravityInverted()
    {
        if (gravity>0)
        {
            return true;
        }
        return false;
    }


    private float getJumpSpeed()
    {
        return getJumpSpeed(0);
    }
    private float getJumpSpeed(float offset)
    {
        int invert = 1;
        if (isGravityInverted())
        {
            invert = -1;
        }

        return (jumpForce+offset)*invert;
    }
}