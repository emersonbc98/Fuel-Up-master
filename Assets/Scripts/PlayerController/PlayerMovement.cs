using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    private float sprint = 1f;
    public float maxFuel = 100f;
    public float jetpackBoost = 30f;
    public float jumpSpeed = 10f;
    public float fuel = 10f;

    public float sprintSpeed = 1.5f;
    public float normalSpeed = 1;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool sprinting;
    public bool flying;

    Vector3 velocity;
    bool isGrounded;

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * sprint * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded && !sprinting)
        {
            sprint = sprintSpeed;
            sprinting = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || !isGrounded && sprinting)
        {
            if (sprinting)
            {
                sprint = normalSpeed;
                sprinting = false;
            }
        }

        //GetKeyDown only triggers once, meaning the fuel will only drop once and nothing else will happen.
        //GetKey will check every single frame if the key is being held, so this works better
        if (Input.GetKey(KeyCode.E) && fuel > 0)
        {
            //Check if we've not set flying to True while the key is held and we have the fuel
            //This will only trigger when we start holding E.
            if (!flying)
            {
                flying = true;
            }
        }
        //The Else statement here triggers when we've stopped holding E OR fuel has dropped below 0.
        else
        {
            //We don't need a separate velocity mod here as the gravity code below will drag us back to the ground
            flying = false;
            Debug.Log("Dropping");

            //Fuel got added every frame, which meant we could fly infinitely as the jetpack would just turn on and off. So I changed the value here.
            if (fuel < 100)
            {
                //I added a check here to see if we are grounded, so fuel charges faster when we're on the ground
                if (isGrounded)
                {
                    //Multiplying this ammount by time.deltaTime means it's fairly accurate
                    fuel += 30f * Time.deltaTime;
                }
                else
                {
                    //We're in the air, so charge fuel slower
                    //Multiplying this ammount by time.deltaTime means it's fairly accurate
                    fuel += 0.01f * Time.deltaTime;
                }

                //Before we print the fuel, let's make sure it's not over filled and cap it.
                if (fuel > 100)
                {
                    fuel = 100;
                }

                print(fuel);
            }
        }

        //This is where we check if we're flying and apply our velocity as desired
        if (flying)
        {
            velocity.y = jetpackBoost;
            if (fuel > 0)
            {
                //Removing 10 fuel a frame typically means losing all your fuel instantly
                //Multiplying this ammount by time.deltaTime means it's fairly accurate
                fuel -= 20f * Time.deltaTime;
                print(fuel);
            }
        }

        Debug.Log("Flying = " + flying);

        //This is your code and will just pull the player down at all times. So our velocity has to pull against it, which is how gravity works.
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }

}
