using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private CharacterController controller;
    private Vector3 direction;
    
    public float forwardSpeed;
    public float maxSpeed;
    public const float SpeedModifier = 0.2f;
    public float displayedSpeed = 0; 

    //0 - left
    //1 - middle
    //2 - right
    private int desiredLane = 1;
    public float laneDistance = 4; //distance beetwen two lanes

    public float jumpForce;
    public float gravity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerManager.isGameStarted)
            return;

        //speed increase over time
        IncreaseSpeed();

        direction.z = forwardSpeed;
        
        PerformJump();
        PerformTurn();

        //Calculate where we should be next
        Vector3 targetPosition = transform.position.z * transform.forward
                               + transform.position.y * transform.up;

        //moving player correctly
        if (desiredLane == 0)
            targetPosition += Vector3.left * laneDistance;
        else if (desiredLane == 2)
            targetPosition += Vector3.right * laneDistance;

        //transform.position = Vector3.Lerp(transform.position,targetPosition, forwardSpeed);
        //controller.center = controller.center;
        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);
    }

    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted)
            return;
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void PerformTurn()
    {
        TurnRight();
        TurnLeft();
    }

    private void TurnLeft()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || SwipeManager.swipeLeft)
        {
            FindObjectOfType<AudioManager>().PlaySound("Turn");

            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }
    }

    private void TurnRight()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || SwipeManager.swipeRight)
        {
            FindObjectOfType<AudioManager>().PlaySound("Turn");

            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }
    }

    private void PerformJump()
    {
        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || SwipeManager.swipeUp)
                Jump();
        }
        else
        {
            direction.y += gravity * Time.deltaTime;
        }
    }

    private void Jump()
    {
        FindObjectOfType<AudioManager>().PlaySound("Jump");
        direction.y = jumpForce;
    }

    private void IncreaseSpeed()
    {
        if (forwardSpeed < maxSpeed)
        {
            forwardSpeed += SpeedModifier * Time.deltaTime;
            displayedSpeed = forwardSpeed * 10;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            var am = FindObjectOfType<AudioManager>();
            am.PlaySound("Crash");
            StartCoroutine(AudioManager.FadeOut(am.GetComponent<AudioSource>(), 2, 0.0f));

            PlayerManager.gameOver = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        Vector3 newPosition = other.transform.localPosition;
        newPosition.z = Mathf.Lerp(other.transform.localPosition.z, transform.localPosition.z, Time.deltaTime * 1);

        other.transform.localPosition = newPosition;
    }
}
