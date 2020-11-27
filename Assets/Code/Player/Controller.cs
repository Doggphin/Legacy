using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Controller : NetworkBehaviour
{
    //Importation and setup
    public CharacterController controller;
    public GameObject Hookshot;
    Hookshot hookshotScript;
    public int targetFrameRate = 60;
    public float testdistance;
    public LayerMask groundMask;

    //Player values
    public float accelerationConstant = 12f;
    public float gravity = -19.62f;
    public float jumpHeight = 3f;
    public bool isGrounded;
    public float sphereRadius = 0.5f;
    public float sphereCheckLength = 0.5f;
    private bool allowSnapping;
    public float friction = .97f;
    public float maxPlayerInputSpeed = 1f;
    public float runningMultiplier = 1.4f;

    public bool conserveMomentum;

    //Vectors
    Vector3 velocity;
    Vector3 forceApplied;

    void Start() //largely importation
    {
        hookshotScript = Hookshot.GetComponent<Hookshot>();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        isGrounded = false; //default to not touching the ground
        runningMultiplier = 1;

        //START PLAYER AXIAL MOVEMENT

        if (Input.GetKey("left shift"))
        {
            runningMultiplier = 1.4f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 inputDirection = ((transform.right * x + transform.forward * z)).normalized; //will always have a magnitude of 1, does not take into account 

        Vector3 playerAxisVel = new Vector3(velocity.x, 0f, velocity.z);
        float playerAxisVelMag = playerAxisVel.magnitude;

        if (playerAxisVelMag > maxPlayerInputSpeed * runningMultiplier) //if dealing with a vector over movement speed cap, find what acceleration direction must be restricted
        {
            if(playerAxisVel.x > 0f && inputDirection.x > 0f) //if on right half of circle and moving right, stop movement in that direction 
            {
                inputDirection.x = 0;
            }
            if (playerAxisVel.x < 0f && inputDirection.x < 0f) //if on left half of circle and moving left, stop movement in that direction 
            {
                inputDirection.x = 0;
            }
            if (playerAxisVel.z > 0f && inputDirection.z > 0f) //if on top half of circle and moving up, stop movement in that direction 
            {
                inputDirection.z = 0;
            }
            if (playerAxisVel.z < 0f && inputDirection.z < 0f) //if on bottom half of circle and moving down, stop movement in that direction 
            {
                inputDirection.z = 0;
            }
        }
        if (isGrounded)
        {
            forceApplied += inputDirection * accelerationConstant * runningMultiplier * Time.deltaTime;
        } else
        {
            forceApplied += inputDirection * accelerationConstant * runningMultiplier * .5f * Time.deltaTime;
        }
        forceApplied += (Vector3.down) * gravity * Time.deltaTime; //gravity input
        velocity += forceApplied; 

        //END PLAYER AXIAL MOVEMENT
        //START GROUND DETECTION

        RaycastHit hit;
        Physics.SphereCast(this.transform.position, sphereRadius, Vector3.down, out hit, 100, groundMask);
        testdistance = hit.distance;

        if (hit.distance < 0.56f && hit.distance > 0.01f) //if touching ground allow snapping
        {
            isGrounded = true;
            velocity.y = 0f;
            allowSnapping = true;
        }
        else if (hit.distance < 1f && hit.distance > 0.56f && allowSnapping) //if able to snap to ground
        {
            isGrounded = true;
            controller.enabled = false;
            transform.Translate(new Vector3(0f, -(hit.distance - 0.55f), 0f));
            controller.enabled = true;
        }
        else //if far above the ground, disable snapping and uncheck grounded
        {
            isGrounded = false;
            allowSnapping = false;
        }

        //END GROUND DETECTION
        //START JUMPING SCRIPT

        if (Input.GetButtonDown("Jump")) // && isGrounded
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity * runningMultiplier);
            isGrounded = false;
            allowSnapping = false;
        }

        //END JUMPING SCRIPT
        //START GRAPPLING & GRAVITY SCRIPT


        if (hookshotScript.isGrappling == true) {
            // && Vector3.Magnitude(hookshotScript.placeholder.transform.position - transform.position) >= hookshotScript.grappleScript.initialLength 
            if (((transform.position + velocity * Time.deltaTime) - hookshotScript.placeholder.transform.position).magnitude > hookshotScript.grappleScript.initialLength)
            {
                float ropeLength = hookshotScript.grappleScript.initialLength;
                Vector3 pivotPoint = hookshotScript.placeholder.transform.position;

                Vector3 velocityOnRopeDirection = Vector3.Project(velocity, pivotPoint - transform.position);
                float velocityMagnitude = velocity.magnitude;
                velocity -= velocityOnRopeDirection;

                if (conserveMomentum == true) //if beginning to grapple, set magnitude of 
                {
                    velocity = velocity.normalized * velocityMagnitude;
                    conserveMomentum = false;
                }

                if (velocity.magnitude > 1f)
                {
                    velocity = velocity.normalized * (velocity.magnitude + (velocity.magnitude - velocityMagnitude)); //make the grappling hook more fun?
                }

                Vector3 newPosition = transform.position + velocity * Time.deltaTime;
                Vector3 clampedPosition = (newPosition - pivotPoint).normalized * ropeLength;

                controller.Move(((pivotPoint + clampedPosition) - transform.position));
                Debug.DrawLine(transform.position, newPosition, Color.red);
                Debug.Log(ropeLength);
            }
            else
            {
                controller.Move((velocity) * Time.deltaTime);
                conserveMomentum = true;
            }
        } else
        {
            controller.Move((velocity) * Time.deltaTime);
            conserveMomentum = true;
        }

        //END GRAPPLING & GRAVITY SCRIPT

        if (isGrounded) {
            velocity *= friction;
        } else
        {
            velocity *= friction + ((1 - friction) / 1.05f);
        }
        forceApplied = Vector3.zero;
        


    } //end update()

} //end script