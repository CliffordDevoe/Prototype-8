using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

    private const float LANE_DISTANCE = 2.5f;
    private const float Turn_Speed = 0.05f;

    //
    private bool isRunning = false;

    private Animator anim;

    //Movement
    private CharacterController controller;
    private float jumpForce = 3.0f;
    private float gravity = 4.0f;
    private float VerticalVelocity;
    private int desiredLane = 1;


    //Speed Modifier
    private float Originalspeed = 7.0f;
    private float speed = 7.0f;
    private float SpeedIncreaseLastTick;
    private float SpeedIncreaseTime = 2.5f;
    private float SpeedIncreaseAmount = 0.1f;



    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isRunning)
            return;

        if(Time.time - SpeedIncreaseLastTick > SpeedIncreaseTime)
        {
            SpeedIncreaseLastTick = Time.time;
            speed += SpeedIncreaseAmount;

            // Change Modifier Text
            GameManager.Instance.UpdateModifier(speed - Originalspeed);
        }


        //Input
        if (MoblieInput.Instance.SwipeLeft)
        {
            MoveLane(false);
        }

        if (MoblieInput.Instance.SwipeRight)
        {
            MoveLane(true);
        }

        //Calculate future Position
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (desiredLane == 0)
            targetPosition += Vector3.left * LANE_DISTANCE;
        else if (desiredLane == 2)
            targetPosition += Vector3.right * LANE_DISTANCE; ;


        //Calculate Move Delta
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).normalized.x * speed;

        bool isGrounded = IsGrounded();
        anim.SetBool("Grounded", isGrounded);


        //Calculate Y
        if (isGrounded)     //if it is Grounded
        {
            VerticalVelocity = -0.1f;

            if (MoblieInput.Instance.SwipeUp)
            {
                //Jump
                anim.SetTrigger("Jump");

                VerticalVelocity = jumpForce;
            }
            else if (MoblieInput.Instance.SwipeDown)
            {
                // Slide
                StartSliding();
                Invoke("StopSliding", 1.0f);
            }
        }
        else
        {
            VerticalVelocity -= (gravity * Time.deltaTime);


            // Fast Falling Mechanics
            if (MoblieInput.Instance.SwipeDown)
            {

                VerticalVelocity = -jumpForce;
            }
        }
        moveVector.y = VerticalVelocity;
        moveVector.z = speed;

        //Move the Penguin

        controller.Move(moveVector * Time.deltaTime);

        //Rotate the Penguin based on where it is moving to
        Vector3 Dir = controller.velocity;
        if (Dir != Vector3.zero)
        {
            Dir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, Dir, Turn_Speed);

        }


    }

    private void MoveLane(bool goingRIght)
    {

        desiredLane += (goingRIght) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);

    }


    private bool IsGrounded()
    {
        Ray groundRay = new Ray(new Vector3(controller.bounds.center.x, (controller.bounds.center.y - controller.bounds.extents.y) + 0.2f, controller.bounds.center.z), Vector3.down);
        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.blue, 1.0f);

        return Physics.Raycast(groundRay, 0.2f + 0.1f);
    }


    public void StartRunning()

    {
        isRunning = true;
        anim.SetTrigger("StartRunning");
    }

    private void StartSliding()
    {
        anim.SetBool("Sliding", true);
        controller.height /= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y / 2, controller.center.z);

    }

    private void StopSliding()
    {
        anim.SetBool("Sliding", false);
        controller.height *= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y * 2, controller.center.z);
    }

    private void Crash()
    {
        anim.SetTrigger("Death");
        isRunning = false;
        GameManager.Instance.OnDeath();
        
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        /*if(hit .gameObject .tag == "Obstacle"){
            Crash();
        }*/
        switch (hit.gameObject.tag)
        {
            case "Obstacle":

                Crash();
                Debug.Log("Hit");
                break;
        }
    }
}
