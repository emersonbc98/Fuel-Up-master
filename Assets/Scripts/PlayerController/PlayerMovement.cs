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
    public float noJetpackBoost = 1f;
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

    // Update is called once per frame
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

        if (Input.GetKeyDown(KeyCode.E) && fuel > 0)
        {
            velocity.y = jetpackBoost;
            fuel -= 10;
            print(fuel);
            flying = true;
        }

        if (Input.GetKeyUp(KeyCode.E) || fuel < maxFuel && isGrounded == true)
        {
            fuel += 10;
            print(fuel);

            if (flying)
             {
                velocity.y = noJetpackBoost;
                flying = false;
             }
            
        }
                
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }      

}
