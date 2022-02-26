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

    public Vector3 ghostStartingOffset = new Vector3(-2, 0, 0);

    public float maxSpeed = 3f;

    private bool playerIsInAir = false;

    private static bool isMovementDisabled = false;
    private int deathHeight = 12;

    private static float skyboxRotation = 3.302045f;
    private static float skyboxRotationSpeed = 0.5f;

    private bool forwardButtonPressed = false;
    private bool backwardButtonPressed = false;

    static PlayerAudio playerAudio;

    public Transform platformTransform;

    void Start()
    {
        Application.targetFrameRate = 60;
        //Physics.gravity = new Vector3(0, gravity, 0);
        playerAudio = GetComponent<PlayerAudio>();

    }


    // Update is called once per frame
    // We marked this as FixedUpdate because we are using it with Unity Physics
    //todo-ck better to check for input in the update method and set a bool to update in fixedUpdate
    private void FixedUpdate()
    {
        if (forwardButtonPressed && rb.velocity.x < maxSpeed && !isMovementDisabled)
        {
            //if (rb.velocity.x == 0)
            //{
            //    movementSpeed = 50;
            //}
            rb.AddForce(movementSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            skyboxRotation += skyboxRotationSpeed * Time.deltaTime;
            if (!playerIsInAir)
            {
                playerAudio.playPlayerMovementSound();
            }
        }

        if (backwardButtonPressed && rb.velocity.x > -maxSpeed && !isMovementDisabled)
        {
            rb.AddForce(-movementSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            skyboxRotation -= skyboxRotationSpeed * Time.deltaTime;
            if (!playerIsInAir)
            {
                playerAudio.playPlayerMovementSound();
            }
        }
    }
    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", skyboxRotation);

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

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                forwardButtonPressed = true;
            }
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                forwardButtonPressed = false;
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                backwardButtonPressed = true;
            }
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                backwardButtonPressed = false;
            }

            if (Input.GetKey(KeyCode.D) )
            {



            }
            if (Input.GetKey(KeyCode.A) )
            {
 

            }
            if (Input.GetKeyDown(KeyCode.Space) && !playerIsInAir)
            {
                rb.AddForce(0, getJumpSpeed(), 0, ForceMode.Impulse);
                playerAudio.playJumpSound();
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

    public void enablePlayerMovement()
    {
        isMovementDisabled = false;

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
        if (collision.gameObject.tag == "Platform")
        {
            playerIsInAir = false;
            playerAudio.playLandingSound();
            platformTransform = collision.transform;
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
            FindObjectOfType<GameManager>().CreateGhostAtPosition(transform.position+ghostStartingOffset);
        }

        if (other.gameObject.tag == "Ghost")
        {
            FindObjectOfType<GameManager>().CheckSheidOrDie(other.gameObject);
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
