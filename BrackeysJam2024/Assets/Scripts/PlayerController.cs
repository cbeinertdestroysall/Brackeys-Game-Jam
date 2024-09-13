using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController Controller;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float speed = 6f, dashSpeed;
    public bool dash, dashCD, teleporting;
    private float inputHorizontal, inputVertical, turnSmoothVelocity, prevAngle;
    [SerializeField] public DashMeter DM;
    [SerializeField] public int dashCDTimer, maxDash, maxDashCD, RamDMG;
    public float dashMeter;
    [SerializeField] CinemachineVirtualCamera DefaultVcam, DashVcam;
    Vector3 Velocity;
    [SerializeField] Transform TPtarget;
    public ParticleSystem motor;
    public ParticleSystem motorDash;
    // Start is called before the first frame update
    void Start()
    {
        dashMeter = maxDash;
    }

    // Update is called once per frame
    void Update()
    {
        if(!teleporting)
        {
            DoInput();
        }
    }

    void FixedUpdate()
    {
        if (!teleporting)
        {
            ShipDash();
            DoGroundMovement();
        }
    }

    void DoInput()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");


        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            motor.Play();
        }
        else
        {
            motor.Stop();
        }

        if (Input.GetKeyDown(KeyCode.T)) //to teleport
        {
            TeleportPlayer();
        }

        if (Input.GetKey(KeyCode.Space) && dashMeter > 0 && !dashCD)
        {
            dash = true;
            DefaultVcam.enabled = false;
            DashVcam.enabled = true;

            motorDash.Play();

            if (!DM.shown)
            {
                DM.ShowMeter();
            }
        }
        else
        {
            dash = false;
            DashVcam.enabled = false;
            DefaultVcam.enabled = true;

            motorDash.Stop();
        }



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
    void TeleportPlayer()
    {
        teleporting = true;
        Controller.transform.position = TPtarget.position;
        DefaultVcam.enabled = false;
        DashVcam.enabled = false;
        Invoke("ReEnable",0.1f);
    }
    public void ReEnable()
    {
        teleporting = false;
        DefaultVcam.enabled =true;
    }

    void ShipDash()
    {
        if (dashMeter == 0 && !dashCD)
        {
            dash = false;
            dashCD = true;
            dashCDTimer = maxDashCD;
        }
        if (dashCD)
        {
            dashCDTimer -= 1;
        }
        if (dashCDTimer == 0 && !dash)
        {
            dashCD = false;
        }
        if (dash)
        {
            dashMeter -= 1;
        }
        if (!dash && !dashCD && dashMeter < maxDash)
        {
            dashMeter += 1;
        }
        if (dashMeter == maxDash && DM.shown && !dash)
        {
            DM.Invoke("HideMeter", 2f);
        }
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

            //if (targetAngle == prevAngle || targetAngle == prevAngle + 45 || targetAngle == prevAngle - 45 || targetAngle == -prevAngle + 45 || targetAngle == -prevAngle - 45)
            //{
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                if (dash)
                {
                    Velocity = moveDir.normalized * (speed * dashSpeed) * Time.fixedDeltaTime;
                }
                else
                {
                    Velocity = moveDir.normalized * speed * Time.fixedDeltaTime;
                }
                //prevAngle = targetAngle;
            //}

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
