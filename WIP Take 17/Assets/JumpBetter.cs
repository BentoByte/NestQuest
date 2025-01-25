using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBetter : MonoBehaviour
{
    public float maxFallSpeed;
    public bool Recharge;
    public float fallMultiplier = 2.5f;
    public float lowjump = 2f;
    [SerializeField] float CirSize;
    [SerializeField] LayerMask gLayerMask;
    public bool isTouchingFront;
    public Transform frontcheck;
    public bool wallSliding;
    public float wallSlideSpeed;
    public bool wallJumping;
    public float wallJumpTime;  
    public bool UpPress; 
    public bool CanWallJump;
    Animator Animet;
    Rigidbody2D rb;
    public float Upfall;
    public float StartStamina;
    public float Stamina;
    public float StamGlideLoss;     
    public float DownStamGlideLoss;
    public float StamGain;
    public bool LeftBirb;
    public bool CanClimb;
    protected ControllerBirb character;
    protected Slaper slapcode;
    public bool Emptied;
    public bool Timer = true;
    public bool DownPress;
    public float TimerTime;

    void Awake(){
        rb = GetComponent<Rigidbody2D> ();
        Animet = GetComponentInChildren<Animator>();
        Stamina = StartStamina;
        character = GetComponent<ControllerBirb>();
        slapcode = GetComponent<Slaper>();
    }
    
    void Update()
    {        
        if (Stamina == 0)
        {
            Emptied = true;
        } else if(Stamina == StartStamina)
        {
            Emptied = false;
        }

        if (Stamina >= StartStamina)
        {
            Stamina = StartStamina;
        }
        LeftBirb = character.Left;
        if (Input.GetKeyDown(KeyCode.W))
        {
            UpPress = true;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            UpPress = false;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            DownPress = true;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            DownPress = false;
        }

        if (RechargeStam() == true)
        {
            Stamina += StamGain;
        }        
    }

    void FixedUpdate()
    {    
        if (rb.velocity.y < 0 && !GetComponent<ControllerBirb>().isGrounded && !slapcode.airstay) 
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, maxFallSpeed));
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump") && !GetComponent<ControllerBirb>().isGrounded && !slapcode.airstay) 
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowjump - 1) * Time.deltaTime;
            // rb.AddForce(1000 * Vector2.right);
        }

        if(RechargeStam())
        {
            Stamina++;
        }
        isTouchingFront = Physics2D.OverlapCircle(frontcheck.position, CirSize, gLayerMask);

        if (isTouchingFront == true && GetComponent<ControllerBirb>().isGrounded == false && GetComponent<ControllerBirb>().HorizontalValue != 0 && CanWallJump)
        {
            wallSliding = true;
        } else {
            wallSliding = false;
        }

        if (wallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, wallSlideSpeed, float.MaxValue));
        }
    }


    public bool RechargeStam()
    {
        bool CanRecharge = true;

        if (Stamina >= StartStamina)
        {
            CanRecharge = false;
        }
        if (!Timer)
        {
            CanRecharge = false;
        }
        return CanRecharge;
    }
    public IEnumerator TimeMe()
    {
        if(Timer == true)
        {
            Timer = false;
            yield return new WaitForSeconds(TimerTime);
            Timer = true;
        }
        
    }





        


    
}



