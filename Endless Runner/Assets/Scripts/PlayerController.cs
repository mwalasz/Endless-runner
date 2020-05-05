using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;

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
        direction.z = forwardSpeed;

        if(controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || SwipeManager.swipeUp)
                Jump();
        }
        else
            direction.y += gravity * Time.deltaTime;

        //Player lane bounds
        if (Input.GetKeyDown(KeyCode.RightArrow) || SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || SwipeManager.swipeLeft)
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }

        //Calculate where we should be next
        Vector3 targetPosition = transform.position.z * transform.forward
                               + transform.position.y * transform.up;

        //moving player correctly
        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

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
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag =="Obstacle")
        {
            PlayerManager.gameOver = true;
        }
    }
}
