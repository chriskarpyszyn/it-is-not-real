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

    private bool isMovementDisabled = false;
    private int deathHeight = 15;

    static PlayerAudio playerAudio;

    void Start()
    {
        Application.targetFrameRate = 60;
        //Physics.gravity = new Vector3(0, gravity, 0);
        playerAudio = GetComponent<PlayerAudio>();

    }

    // Update is called once per frame
    // We marked this as FixedUpdate because we are using it with Unity Physics
    //todo-ck better to check for input in the update method and set a bool to update in fixedUpdate

    private void Update()
    {
        float playerY = transform.position.y;

        //player input
        if (!isMovementDisabled)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
            {
                if (!playerIsInAir)
                {
                    playerAudio.setFirstMovement();
                }
           
            }
            if (Input.GetKey(KeyCode.D) && rb.velocity.x < maxSpeed)
            {
                rb.AddForce(movementSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);

                if (!playerIsInAir)
                {
                    playerAudio.playPlayerMovementSound();
                }
            }
            if (Input.GetKey(KeyCode.A) && rb.velocity.x > -maxSpeed)
            {
                rb.AddForce(-movementSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
                if (!playerIsInAir)
                {
                    playerAudio.playPlayerMovementSound();
                }

            }
            if (Input.GetKeyDown(KeyCode.Space) && !playerIsInAir)
            {
                rb.AddForce(0, getJumpSpeed(), 0, ForceMode.Impulse);
            }

       
        }


        //***********


        //restart level if player goes too high or low
        if (playerY > deathHeight || playerY < -deathHeight)
        {
            playerDied();
            
        }

    }

    public void disablePlayerMovement()
    {
        isMovementDisabled = true;
    }

    /**
     * call me when the player dies
     **/
    private void playerDied()
    {
        FindObjectOfType<GameManager>().EndGame();
    }

    public void resetPlayerProps()
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
            playerIsInAir = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //todo-ck need to check for platform and not other types of collision
        if (collision.gameObject.tag == "Platform")
        {
            playerIsInAir = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Portal")
        {
            FindObjectOfType<GameManager>().ToggleDreamMode();
            gravity *= -1;
            setGravity(gravity);
            rb.AddForce(0, getJumpSpeed(5), 0, ForceMode.Impulse);
            FindObjectOfType<GameManager>().CreateGhostAtPosition(transform.position);
        }

        if (other.gameObject.tag == "Ghost")
        {
            FindObjectOfType<GameManager>().CheckSheidOrDie();
        }

        if (other.gameObject.tag == "Spikes")
        {
            FindObjectOfType<GameManager>().EndGame();
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
