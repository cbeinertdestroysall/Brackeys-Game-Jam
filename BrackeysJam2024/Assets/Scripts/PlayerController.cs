using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController Controller;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float speed = 6f, dashSpeed;
    public bool dash, dashCD, teleporting, Grounded;
    private float inputHorizontal, inputVertical, turnSmoothVelocity, groundDist = 0.4f, gravity = -13.0f, velocityY;
    [SerializeField] public DashMeter DM;
    [SerializeField] public int dashCDTimer, maxDash, maxDashCD, RamDMG;
    public float dashMeter;
    [SerializeField] public CinemachineVirtualCamera DefaultVcam, DashVcam;
    Vector3 Velocity;
    [SerializeField] Transform TPtarget,playerGround;
    public ParticleSystem motor;
    public ParticleSystem motorDash;
    public GameObject healthBar;
    public bool resetFunctionCalled = false;
    public GameObject boat;
    public BaseScript LH;
    public LayerMask groundMask;

    // Start is called before the first frame update
    void Start()
    {
        dashMeter = maxDash;
        resetFunctionCalled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!teleporting)
        {
            DoInput();
        }

        if (this.GetComponent<PlayerHealth>().currentHealth <= 0 && !resetFunctionCalled && !LH.gameOver)
        {
            resetFunctionCalled = true;
            teleporting = true;
            TeleportPlayer();
        }
    }

    void FixedUpdate()
    {
        if (!teleporting)
        {
            ShipDash();
            DoGroundMovement();
            Gravity();
        }
    }

    void DoInput()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        Grounded = Physics.CheckSphere(playerGround.position, groundDist, groundMask);

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
    }
    void TeleportPlayer()
    {
        teleporting = true;
        Controller.transform.position = TPtarget.position;
        DefaultVcam.enabled = false;
        DashVcam.enabled = false;
        this.GetComponent<BoxCollider>().enabled = false;
        boat.SetActive(false);
        Invoke("ReEnable", 1f);
    }
    public void ReEnable()
    {
        teleporting = false;
        DefaultVcam.enabled =true;
        resetFunctionCalled = false;
        this.GetComponent<BoxCollider>().enabled = true;
        boat.SetActive(true);
        this.GetComponent<PlayerHealth>().currentHealth = this.GetComponent<PlayerHealth>().maxHealth;
        healthBar.GetComponent<HealthBar>().SetMaxHealth(this.GetComponent<PlayerHealth>().maxHealth);
        
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
    void Gravity()
    {
        if (Controller.isGrounded)
        {
            velocityY = 0.0f;
        }
        velocityY += gravity * Time.fixedDeltaTime;

        if (Grounded && velocityY < 0)
        {
            velocityY = -2f;
        }

        Controller.Move(Vector3.up * velocityY * Time.fixedDeltaTime);
    }


    void DoGroundMovement()
    {
        Vector3 direction = new Vector3(inputHorizontal, 0f, inputVertical).normalized;


        Velocity *= 0.975f;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg /*+ Camera.eulerAngles.y*/;
           
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
               
           
        }
        Velocity.y = 0;
        Controller.Move(Velocity);
    }
}
