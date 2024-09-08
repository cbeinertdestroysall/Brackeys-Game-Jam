using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController Controller;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float speed = 6f;
    private float inputHorizontal, inputVertical, turnSmoothVelocity, prevAngle;
    Vector3 Velocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       DoInput(); 
    }

    void FixedUpdate()
    {
        DoGroundMovement();
    }

    void DoInput()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        /*Grounded = Physics.CheckSphere(playerGround.position, groundDist, groundMask);

        if (Input.GetButtonDown("Jump") && Grounded && !HasKey)
        {
            inputJump = true;
        }
        if (Input.GetButton("Fire2") && HasKey)
        {
            ToggleKey(false);
            curerntKey.ResetKey();
        }*/
    }    

    
    void DoGroundMovement()
    {
        Vector3 direction = new Vector3(inputHorizontal, 0f, inputVertical).normalized;
        

        Velocity *= 0.99f;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg /*+ Camera.eulerAngles.y*/;
            //Debug.Log("Input:");
            //Debug.Log(prevAngle);
            //Debug.Log(targetAngle);
            //Debug.Log(targetAngle +45);
            //Debug.Log(targetAngle -45);

            if(targetAngle == prevAngle ||targetAngle == prevAngle + 45 || targetAngle == prevAngle - 45 || targetAngle == -prevAngle + 45 || targetAngle == -prevAngle - 45)
            {
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                Velocity = moveDir.normalized * speed * Time.fixedDeltaTime;
                prevAngle = targetAngle;
            }
           
            //MovementAnims.SetBool("Sprint",true);
        }
        else
        {
            //MovementAnims.SetBool("Sprint",false);
            //MovementAnims.SetBool("Idle",true);
        }
        Controller.Move(Velocity);
    }
}
