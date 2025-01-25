using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slaper : MonoBehaviour
{
    Rigidbody2D rb2d;
    Animator Anime;

    ControllerBirb cntrlrbrb;
    JumpBetter jmpbtr;
    int dir;
    bool AttackDelayIsOn;

    [Header("AttackDamage")]
    [SerializeField] int HeavyAttackDamage;
    [SerializeField] int LightAttackDamge;   
    [SerializeField] int MediumAttackDamge;   
    [Header("AttackSizes")]

    [SerializeField] float AirCirSize;
    [SerializeField] float JabCirSize;

    [SerializeField] float DairCirSize;
    [SerializeField] float FinalCirSize;

    [SerializeField] float UpairCirSize;
    [Header("AttackDiffs")]
    
    [SerializeField] Vector3 AirDiff;
    public Vector3 JabDiff;
    [SerializeField] Vector3 DairDiff;
    [SerializeField] Vector3 UpairDiff;
    [SerializeField] Vector3 FinalDiff;

    [Header("Attack Wait Time")]

    [SerializeField] float AirWaittime;
    [SerializeField] float JabWaittime;   
    [SerializeField] float Jab2Waittime;
    [SerializeField] float DairWaittime;
    [SerializeField] float UpairWaittime;
    [SerializeField] float FinalWaittime;      
    [Header("Attack After Wait Time")]

    [SerializeField] float AirAfterWaittime;
    [SerializeField] float JabAfterWaittime;  
    [SerializeField] float Jab2AfterWaittime; 
    [SerializeField] float FinalAfterWaittime;  
    [SerializeField] float DairAfterWaittime;
    [SerializeField] float UpairAfterWaittime;    
    [Header("Attack KnockBack Force")]

    [SerializeField] float AirForce;
    [SerializeField] float JabForce;
    [SerializeField] float DairForce;
    [SerializeField] float FinalForce;
    [SerializeField] float UpairForce;  
    [Header("Attack Knockback Direction")]

    [SerializeField] Vector2 AirKnockback;
    [SerializeField] Vector2 JabsKnockback;   
    [SerializeField] Vector2 DairKnockback;
    [SerializeField] Vector2 UpairKnockback;
    [SerializeField] Vector2 FinalKnockback;   
    [Header("Delays")]
    [SerializeField] float MediumDelay; 
    [SerializeField] float longdelay;
    [SerializeField] float JabDelay;
    [Header("SelfKnockbackStuff")]
    [SerializeField] float UpairSelfKnockbackFor;
    [SerializeField] Vector2 UpairSelfKnockbackDir;

    bool yOverride;
    [SerializeField] float FinalSelfKnockbackFor;
    [SerializeField] Vector2 FinalSelfKnockbackDir;
    [SerializeField] float JabKnockForce;
    [SerializeField] Vector2 JabKnockDir;
    [SerializeField] float DairSelfKnockForce ;
    [SerializeField] Vector2 DairSelfKnockbackDir;

    [Header("Other")]

    [SerializeField] float AirStayTime;
    public bool attacking = false;
    bool Done;
    
    [SerializeField] bool attackhold;
    public int Jab = 0;
    public bool airstay;

    [SerializeField] LayerMask CanHit;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Anime = GetComponentInChildren<Animator>();
        cntrlrbrb = GetComponent<ControllerBirb>();
        jmpbtr = GetComponent<JumpBetter>();
    }

    public IEnumerator Attack(Vector3 Origin, float CircleSize, float waitime, float afterwaitime, int attackdamage, string animationstring, Vector2 knockdir, float knockfor, bool KnockbackFromOnlyHits, float afterdelay, float selfKnockForce, Vector2 selfknockdir)
    { 
        if(Jab <= 3)
        {        
            Done = false;
            attacking = true;
            if(!KnockbackFromOnlyHits)
            {
                rb2d.velocity = Vector3.zero;
                rb2d.AddForce((new Vector2(Mathf.Abs(selfknockdir.x) * dir, Mathf.Abs(selfknockdir.y)) * selfKnockForce), ForceMode2D.Impulse);
            }
            cntrlrbrb.ChangeAnimationState(animationstring);
            yield return new WaitForSeconds(waitime);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + Origin, CircleSize, CanHit);
            for (int i = 0; i < colliders.Length; i++)
            {
                if(gameObject.transform.position.y > colliders[i].transform.position.y) //BirbisAbove
                {
                    if(gameObject.transform.position.x > colliders[i].transform.position.x) //BirbToTheRight
                    {
                        if(!yOverride) 
                        {
                            knockdir = new Vector2(-knockdir.x, -knockdir.y);
                        } else {
                            knockdir = new Vector2(-knockdir.x, knockdir.y);
                        }
                    } else {
                        if(!yOverride)
                        {
                            knockdir = new Vector2(knockdir.x, -knockdir.y);
                        } else {
                            knockdir = new Vector2(knockdir.x, knockdir.y);
                        }
                    }
                } else { //BirbisBelow
                    if(gameObject.transform.position.x > colliders[i].transform.position.x) //BirbToTheRight
                    {
                        if(!yOverride)
                        {
                            knockdir = new Vector2(-knockdir.x, knockdir.y);
                        } else {
                            knockdir = new Vector2(-knockdir.x, knockdir.y);
                        }
                    } else {
                        if(!yOverride)
                        {
                            knockdir = new Vector2(knockdir.x, knockdir.y);
                        } else {
                            knockdir = new Vector2(knockdir.x, knockdir.y);
                        }
                    }
                } 
                EnemyHealth enemyhealth = colliders[i].gameObject.GetComponent<EnemyHealth>();
                if(KnockbackFromOnlyHits && Done == false)     
                {
                    rb2d.velocity = Vector3.zero;
                    Done = true;
                    rb2d.AddForce((new Vector2(Mathf.Abs(selfknockdir.x) * dir, Mathf.Abs(selfknockdir.y)) * selfKnockForce), ForceMode2D.Impulse);
                }
                if (enemyhealth != null)
                {
                    enemyhealth.GetHurt(attackdamage);
                    if(cntrlrbrb.Left)
                    {
                        enemyhealth.Knockback(new Vector2(-Mathf.Abs(knockdir.x),knockdir.y), knockfor);
                    } else 
                    {
                        enemyhealth.Knockback(knockdir, knockfor);
                    }
                }
            }
            yield return new WaitForSeconds(afterwaitime);
            StartCoroutine(AttackDelay(afterdelay));
            attacking = false;
            yOverride = false;
            StartCoroutine(JabCombo(JabDelay));

            
        }
    }
    void Update()
    {
        if(cntrlrbrb.Left)
        {
            dir = 1;
        } else {
            dir = -1;
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            attackhold = true;
        } 
        if(Input.GetKeyUp(KeyCode.K))
        {
            attackhold = false;
            Jab = 0;
        } 
        if (airstay == true)
        {
            rb2d.gravityScale = 0;
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
        } else {
            rb2d.gravityScale = 11;
        }
        if(attackhold && !attacking && CanAttack())
        {
            if(jmpbtr.UpPress)
            {
                StartCoroutine(Attack(UpairDiff, UpairCirSize, UpairWaittime, UpairAfterWaittime, LightAttackDamge, "UpAttack", UpairKnockback, UpairForce, true, MediumDelay, UpairSelfKnockbackFor, UpairSelfKnockbackDir));
            } else if(cntrlrbrb.isGrounded)
            {
                if(Jab == 0)
                {
                    Jab += 1;
                }                    
                StartCoroutine(Attack(JabDiff, JabCirSize, JabWaittime, JabAfterWaittime, LightAttackDamge, "Attack", JabsKnockback, JabForce, false, MediumDelay, JabKnockForce, JabKnockDir));
            } else if(jmpbtr.DownPress)
            {
                StartCoroutine(Attack(DairDiff, DairCirSize, DairWaittime, DairAfterWaittime, LightAttackDamge, "DownAttack", DairKnockback, DairForce, true, MediumDelay, DairSelfKnockForce, DairSelfKnockbackDir));
            } else {
                StartCoroutine("StayInAir");
                StartCoroutine(Attack(AirDiff, AirCirSize, AirWaittime, AirAfterWaittime, MediumAttackDamge, "AirAttack", AirKnockback, AirForce, true, MediumDelay, 0, new Vector2(0,0)));
            }
        } 
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + JabDiff.x,transform.position.y + JabDiff.y), JabCirSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + AirDiff.x,transform.position.y + AirDiff.y), AirCirSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + FinalDiff.x,transform.position.y + FinalDiff.y), FinalCirSize);        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + UpairDiff.x,transform.position.y + UpairDiff.y), UpairCirSize);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + DairDiff.x,transform.position.y + DairDiff.y), DairCirSize);
    }
    IEnumerator StayInAir()
    {
        airstay = true;
        yield return new WaitForSeconds(AirStayTime);
        airstay = false;
    }
    IEnumerator AttackDelay(float delay)
    {
        AttackDelayIsOn = true;        
        yield return new WaitForSeconds(delay);
        AttackDelayIsOn = false;

    }
    public bool CanAttack()
    {
        bool Can = true;

        if (!cntrlrbrb.CanMove())
        {
            Can = false;
        }
        if(AttackDelayIsOn)
        {
            Can = false;
        }
        return Can;
    }
    IEnumerator JabCombo(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(Jab == 1 && attackhold)
        {
            Jab += 1;
            StartCoroutine(Attack(JabDiff, JabCirSize, Jab2Waittime, Jab2AfterWaittime, MediumAttackDamge, "Attack2", JabsKnockback, JabForce, false, longdelay, JabKnockForce, JabKnockDir));
        } else if(Jab >= 2 && attackhold)
        {
            Jab += 1;
            StartCoroutine(Attack(FinalDiff, JabCirSize, FinalWaittime, FinalAfterWaittime, HeavyAttackDamage, "LastAttack", FinalKnockback, FinalForce, false, longdelay, FinalSelfKnockbackFor, FinalSelfKnockbackDir));
        }

    }
}
