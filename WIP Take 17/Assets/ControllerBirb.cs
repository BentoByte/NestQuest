
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ControllerBirb : MonoBehaviour
{
    public bool isDead = false;
    [SerializeField] public float MaxSpeed;
    [SerializeField] public float StartSpeed;
    [SerializeField] public float WhenSpeed;
    [SerializeField] private float MaxAddedSpeed;
    [SerializeField] public float Speed;
    [SerializeField] public float Gain;
    [SerializeField] public float SlowGain;
    [SerializeField] public float FastGain;
    [SerializeField] public float AddGain;
    [SerializeField] public float Loss;
    [SerializeField] public float CrouchSpeed = 1;
    [SerializeField] public float Crouchdecceleration;
    [SerializeField] bool Crouch;
    bool PrevIsOnSlope;
    public float CrouchSpeedMod = 0.5f;
    public float velPower;
    public float acceleration;
    public float decceleration;
    public float airdecceleration;
    public float airacceleration;
    
    [Header("GroundStuff")]
    [SerializeField] public bool isGrounded;
    [SerializeField] LayerMask gLayerMask;
    [SerializeField]
    private PhysicsMaterial2D noFriction;
    [SerializeField]
    private PhysicsMaterial2D fullFriction;
    [SerializeField] float fricAmount;
    public float slopeCheckDistance;
    [SerializeField] 
    private float maxSlopeAngle;
    public int MultSlope; 
    [SerializeField] Transform GroundChecker;
    [SerializeField] Transform OverheadChecker;
    [SerializeField] public float CirSize = 0.4f;
    [SerializeField] public float CelingCirSize = 0.01f;
    [Header("Jump")]
    public bool Jumping;
    [SerializeField] float jumpHangAccelerationMult;
    [SerializeField] float jumpHangMaxSpeedMult;
    [SerializeField] public int TotalJumps;
    [SerializeField] float jumpower = 500;
    public float XwallJump;
    public float ywallJump;
    public float Coyjumpower;
    public float DownMultijumpower;
    public float Multijumpower;
    bool Jumped;
    public float xJumpForce;
    public float UpSpeed;
    public float BufJumpTime = 0.5f;
    public float JumpTime = 0.4f;
    public int AvailableJumps;
    public float AirRes;
    [SerializeField] float JumpCurve;
    [Header("Gravity")]
    [SerializeField] float AirGravity;
    [SerializeField] float GroundedGravity;
    [SerializeField] float HangGravity;
    [SerializeField] float TestGravity;
    [Header("Knockback")]
    public bool knockFromRight;
    public float Knockback;
    float knockBackCount;
    public float KnockbackTime;
    public float KnockbackForce;

    public ParticleSystem dustJump;
    [Header("Input")]
    [SerializeField] public BirbDefault PlayerControls;
    private InputAction moveact;
    [SerializeField] CapsuleCollider2D Stand,Croucher;
    [SerializeField]
    Rigidbody2D rb2d;
    Animator Anime;
    Vector2 TargetVeloCity;
    public float HorizontalValue = 0;
    public float PrevHori = 0;
    public float MoveDir;
    public float offset;
    public bool Left = true;
    public bool isOnSlope;
    Vector2 Velo;
    Vector2 PrevVelo;
    //Private Fields
    Slaper slap;
    bool Stopping;
    protected JumpBetter JumpScript;
    private Vector2 slopeNormalPerp;
    float slopeDownAngle;
    float slopeDownAngleAld;
    float Timer;
    bool MultipleJump;
    bool coyoteJump;
    float BufJumpCount;
    float JumpCount;
    Vector2 colliderSize;
    private float slopeSideAngle;
    private float lastSlopeAngle;
    private bool canWalkOnSlope;
    public bool hold = false;
    protected ControllerBirb character;
    Transform checkPos;

    [HideInInspector]
    public bool grabbingLedge;
    string currentState; 
    const string AtWallAni = "AtWall";
    const string WalkAni = "SlowRun";
    const string IdleAni = "Idleing";
    const string JumpAni = "Jump";
    const string RunAni = "Running";
    const string SuperRunAni = "FastRun";
    const string CrouchAni = "Crouching";
    const string CrouchRunAni = "CrouchWalk";
    const string RisingAni = "Rising";
    const string HighRiseAni = "Rising2";
    const string MidAirAni = "MIdAir";
    const string HighFallAni = "Falling2";
    const string FallingAni = "Falling";
    void OnEnable()
    {
        moveact = PlayerControls.Player.Move;
        moveact.Enable();
    }
    void OnDisable()
    {
        moveact.Disable();
    }
    void Awake()
    {
        Application.targetFrameRate = 300;
        PlayerControls = new BirbDefault();
        AvailableJumps = TotalJumps;
        JumpScript = GetComponent<JumpBetter>();
        rb2d = GetComponent<Rigidbody2D>();
        Anime = GetComponentInChildren<Animator>();
        slap = GetComponent<Slaper>();
        
        colliderSize = Stand.size;
        Speed = StartSpeed;
    }

    public void ChangeAnimationState(string newState)
    {
        if (Anime.GetCurrentAnimatorClipInfo(0)[0].clip.name == newState) return;

        Anime.Play(newState);
    }

    #region SlopeStuff...
    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, colliderSize.y / 2));

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, gLayerMask);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, gLayerMask);

        if (slopeHitFront)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

        }
        else if (slopeHitBack)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }

    }
    private void SlopeCheckVertical(Vector2 checkPos)
    {      
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, gLayerMask);

        if (hit)
        {

            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;            

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if(slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }    
            if(slopeDownAngle > 3)    
            {
                isOnSlope = true;
            } else {
                isOnSlope = false;
            }              

            lastSlopeAngle = slopeDownAngle;
           
            Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }
        if(!Crouch)
        {
            if (isOnSlope && canWalkOnSlope && HorizontalValue == 0f)
            {
                rb2d.sharedMaterial = fullFriction;

                
            }
            else
            {
                rb2d.sharedMaterial = noFriction;
            }
        }
    }
    #endregion
     
    void Update()
    {
        PrevVelo = Velo;
        Velo = rb2d.velocity;

        if (CanMove() == true){ 

            PrevHori = HorizontalValue;
            if(Mathf.Abs(rb2d.velocity.x) > MaxSpeed * 0.75)
            {
                Gain = SlowGain;
            } 
            else {
                Gain = FastGain;
            }
            if(Input.GetKeyDown(KeyCode.Return))
            {
                hold = true;
            }
            if(Input.GetKeyUp(KeyCode.Return))
            {
                hold = false;
            } 

            if(Mathf.Abs(rb2d.velocity.x) > MaxSpeed + 1 && Speed < MaxAddedSpeed)
            {
                Speed += AddGain;
            }
            HorizontalValue = moveact.ReadValue<Vector2>().x;
            if (Mathf.Abs(PrevVelo.x) >= 50 && PrevHori != HorizontalValue)
            {
                rb2d.AddForce((PrevVelo.x * HorizontalValue * 10) * Vector2.right);  
            }

            if (Input.GetButtonDown("Jump") && !Jumped)
            {     
                BufJumpCount = BufJumpTime;
            }
            else
            {
                BufJumpCount -= Time.deltaTime;
            }
            if(BufJumpCount > 0f && !Jumped) 
            {
                StartCoroutine(Jump());
            }
            if (Input.GetButtonUp("Jump"))
            {
                coyoteJump = false;
            }
            if (Input.GetButtonDown("Crouch"))
            {     
                Crouch = true;
                
            }
            else if(Input.GetButtonUp("Crouch"))
            {
                Crouch = false;
            }
            // JumpCount -= Time.deltaTime;
            // if (JumpCount >= 0)
            // {
            //     Jumped = true;
            // } else {
            //     Jumped = false;
            // }

        }  else {
            HorizontalValue = 0;
        }

        if(isOnSlope && canWalkOnSlope) 
        {
            isGrounded = true;
        }
        if(isGrounded && !Crouch && !slap.attacking)
        {
            if(HorizontalValue != 0 && Mathf.Abs(rb2d.velocity.x) <= 0.2f)
            {
                ChangeAnimationState(AtWallAni);
            }
            else if(HorizontalValue == 0 && Mathf.Abs(rb2d.velocity.x) >  3 && Speed > 77)
            {
                if (Anime.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Slide")
                {
                    ChangeAnimationState("IntroSlide");
                }
            }
            else if(HorizontalValue != 0 && Mathf.Abs(rb2d.velocity.x) <= 45 && Mathf.Abs(rb2d.velocity.x) > 0)
            {
                ChangeAnimationState(WalkAni);
            } else if(HorizontalValue != 0 &&  Mathf.Abs(rb2d.velocity.x) > StartSpeed &&  Mathf.Abs(rb2d.velocity.x) > 0 && Mathf.Abs(rb2d.velocity.x) < MaxSpeed * 0.90){
                ChangeAnimationState(RunAni);
            }
            else if(HorizontalValue == 0){
                ChangeAnimationState(IdleAni);
            }
            else if(HorizontalValue != 0 &&  Mathf.Abs(rb2d.velocity.x) > MaxSpeed * 0.95)
            {
                ChangeAnimationState(SuperRunAni);
            } 
        } else if(!isGrounded && !Crouch && !slap.attacking)
        {
            if(rb2d.velocity.y > 20)    
            {
                if(rb2d.velocity.y >= 65)
                {
                    ChangeAnimationState("Rise1");
                } else if(rb2d.velocity.y < 65 && rb2d.velocity.y > 30)
                {
                    ChangeAnimationState("Rise2");
                } else if(rb2d.velocity.y < 30)
                {
                    ChangeAnimationState("Rise3");
                }
            }
            else if(rb2d.velocity.y < -10 && !slap.attacking)
            {
                if(rb2d.velocity.y >= -30)
                {
                   ChangeAnimationState("Fall1");
                } else if(rb2d.velocity.y <= -30 && rb2d.velocity.y >= -50)
                {
                    ChangeAnimationState("Fall2");
                } else if(rb2d.velocity.y <= -50)
                {
                    ChangeAnimationState("Fall3");
                }
            }
            else if(!isGrounded && !slap.attacking){
                if(rb2d.velocity.y >= 5 && Anime.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Fall2" && Anime.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Fall3")
                {
                    ChangeAnimationState("Mid1");
                } else if(rb2d.velocity.y <= -5)
                {
                    ChangeAnimationState("Mid3");
                } else {
                    ChangeAnimationState("Mid2");
                }
            }

        } else if(!isGrounded && !Crouch)
        {
            if(JumpScript.DownPress)
            {

            } else {
                ChangeAnimationState("Glide");
            }

        } else if (Crouch && !slap.attacking)
        {
            ChangeAnimationState("Crouching");
        } else {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Time.timeScale == 1.0f)
                Time.timeScale = 0.1f;
            else
                Time.timeScale = 1.0f;
        } 

    }



    void FixedUpdate()
    { 
        Gcheck();
        Move(HorizontalValue, Crouch);
        PrevIsOnSlope = isOnSlope;
        SlopeCheck();
        if(!slap.airstay)
        {
            if(isGrounded)
            {
                SetGravity(GroundedGravity);
            } else {
                SetGravity(AirGravity);
            }
        }
        if(hold == true)
        {
            rb2d.AddForce(100 * Vector2.right);  
        }
        if (Mathf.Abs(rb2d.velocity.x) >= Speed * WhenSpeed && Speed < MaxSpeed)
        {
            Speed += Gain;
        } else if (Mathf.Abs(rb2d.velocity.x) < MaxSpeed * WhenSpeed && Speed > StartSpeed){
            Speed -= Loss;
        }



    }

    void Gcheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;


        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundChecker.position, CirSize, gLayerMask);
        if (colliders.Length > 0)
        {
            isGrounded = true;
            if (!wasGrounded)
            {
                Shaker.Instance.ShakeCamera(0.5f, .1f);
                AvailableJumps = TotalJumps;
                MultipleJump = true;
                Jumping = false;
                Jumped = false;
            }
        }
        else
        {
            if (wasGrounded)
                StartCoroutine(coyoteJumpDelay());

        }
        if (wasGrounded && !isGrounded && BufJumpCount != 0 && !Jumped){
            AvailableJumps--;
            AvailableJumps--;
        }

    }

    IEnumerator coyoteJumpDelay()
    {
        coyoteJump = true;
        yield return new WaitForSeconds(0.1f);
        coyoteJump = false;
    }

    public void KnockbackFunk(Vector2 direction)
    {
        knockBackCount = KnockbackTime;
        rb2d.AddForce(direction, ForceMode2D.Impulse);
    }    

    IEnumerator Jump()
    {
        Jumped = true;
        if (isGrounded)
        {
            MultipleJump = true;
            AvailableJumps--;

            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.AddForce(jumpower * Vector2.up, ForceMode2D.Impulse);    
            BufJumpCount = 0f;  
            Jumping = true;
        }
        else
        {
            if(coyoteJump && !Jumped)
            { 
                MultipleJump = true;
                AvailableJumps--;
                BufJumpCount = 0f;
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                rb2d.AddForce(jumpower * Vector2.up, ForceMode2D.Impulse);          
            }
            else if (Input.GetButtonDown("Jump") && JumpScript.wallSliding == true)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                rb2d.AddForce(new Vector2(XwallJump * -HorizontalValue, ywallJump), ForceMode2D.Impulse);
                MultipleJump = true;
                BufJumpCount = 0f;
            }
            else if(MultipleJump && AvailableJumps>0)
            { 
                if (rb2d.velocity.y < 0)
                {
                    AvailableJumps--;
                    rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                    rb2d.AddForce(DownMultijumpower * Vector2.up, ForceMode2D.Impulse);
                    
                    BufJumpCount = 0f;
                }else{
                    AvailableJumps--;
                    rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                    rb2d.AddForce(Multijumpower * Vector2.up, ForceMode2D.Impulse);
                    BufJumpCount = 0f;
                }
            }
        }
        yield return null;
    }
    void Move(float dir,bool crouchFlag)
    {
        if(!crouchFlag)
        {
            if(Physics2D.OverlapCircle(OverheadChecker.position,CelingCirSize,gLayerMask))
            {
                crouchFlag = true;
            }
        }
        Stand.enabled = !crouchFlag;

        Croucher.enabled = crouchFlag;
        #region Move & run
        if (isGrounded && !crouchFlag && !isOnSlope)
        {        
            float TargetSpeed = dir * Speed;

            float TargetSpeedDif = TargetSpeed - rb2d.velocity.x;
        
            float accelerRate = (Mathf.Abs(TargetSpeed) > 0.01f) ? acceleration : decceleration;
        
            float MoveDir = Mathf.Pow(Mathf.Abs(TargetSpeedDif) * accelerRate, velPower) * Mathf.Sign(TargetSpeedDif);

            rb2d.AddForce(MoveDir * Vector2.right);

        } else if (isGrounded && isOnSlope && canWalkOnSlope) 
        {

            float TargetSpeed = dir * Speed;

            float TargetSpeedDif = TargetSpeed - rb2d.velocity.x;
        
            float accelerRate = (Mathf.Abs(TargetSpeed) > 0.01f) ? acceleration : decceleration;
            if(crouchFlag)
            {
                if(Mathf.Abs(rb2d.velocity.x) > 45f)
                {
                    accelerRate = 0.08f;
                    rb2d.sharedMaterial = noFriction;
                } else {
                    rb2d.sharedMaterial = fullFriction;
                }
            }
            float MoveDir = Mathf.Pow(Mathf.Abs(TargetSpeedDif) * accelerRate, velPower) * Mathf.Sign(TargetSpeedDif);

            if(!PrevIsOnSlope && isOnSlope)
            {
                rb2d.velocity = new Vector2(TargetSpeed, rb2d.velocity.y);
            }

            rb2d.AddForce(MoveDir * Vector2.right);

        }
        else if (!isGrounded)
        {
            float TargetSpeed = dir * Speed;

            float TargetSpeedDif = TargetSpeed - rb2d.velocity.x;

            float accelerRate = (Mathf.Abs(TargetSpeed) > 0.01f) ? airacceleration : airdecceleration;

            if ((Jumping) && Mathf.Abs(rb2d.velocity.y) < JumpCurve)
		    {
			    accelerRate *= jumpHangAccelerationMult;
			    TargetSpeed *= jumpHangMaxSpeedMult;
		    }

            float MoveDir = Mathf.Pow(Mathf.Abs(TargetSpeedDif) * accelerRate, velPower) * Mathf.Sign(TargetSpeedDif);
            if (rb2d.velocity.y > 0 && !Input.GetButton("Jump") && Jumped)
            {
                MoveDir = MoveDir * xJumpForce;
            }
            rb2d.AddForce(MoveDir * Vector2.right);  
        } else if (isGrounded && crouchFlag)
        {
            if(Mathf.Abs(rb2d.velocity.x) > 45f)
            {
                Crouchdecceleration = 0.08f;
                rb2d.sharedMaterial = noFriction;
            } else {
                rb2d.sharedMaterial = fullFriction;
                Crouchdecceleration = 3f;
            }
            float TargetSpeed = dir * CrouchSpeed;
        
            float TargetSpeedDif = TargetSpeed - rb2d.velocity.x;
        
            float accelerRate = (Mathf.Abs(TargetSpeed) > 0.01f) ? acceleration : Crouchdecceleration;
        
            float MoveDir = Mathf.Pow(Mathf.Abs(TargetSpeedDif) * accelerRate, velPower) * Mathf.Sign(TargetSpeedDif);
            rb2d.AddForce(MoveDir * Vector2.right);  
        }
        if (isGrounded && Mathf.Abs(HorizontalValue) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(rb2d.velocity.x), Mathf.Abs(fricAmount));

            amount *= Mathf.Sign(rb2d.velocity.x);

            rb2d.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }

        Vector3 CurrentScale = transform.localScale;

        if(Left && dir > 0)
        {
            transform.localScale = new Vector3(1.4f, 1.4f, 1);
            Left = false;
            slap.JabDiff = new Vector3(Mathf.Abs(slap.JabDiff.x), slap.JabDiff.y, slap.JabDiff.z);
        }
        else if (!Left == dir < 0)
        {
            slap.JabDiff = new Vector3(-Mathf.Abs(slap.JabDiff.x), slap.JabDiff.y, slap.JabDiff.z);
            transform.localScale = new Vector3(-1.4f, 1.4f, 1);
            Left = true;

        }
        #endregion   
    }
    public void Die()
    {
        isDead = true;
        FindObjectOfType<GameMaster>().Restart();
    }
    public void SetGravity(float G)
    {
        rb2d.gravityScale = G;
    }
    public bool CanMove()
    {
        bool Can = true;
        if (isDead)
        {
            Can = false;
        }
        if (knockBackCount > 0)
        {
            // Can = false;
            knockBackCount -= Time.fixedDeltaTime;
        }
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            Can = false;
        }
        if (slap.Jab > 0)
        {
            Can = false;
        }
        return Can;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(GroundChecker.position, CirSize);
    }
}
