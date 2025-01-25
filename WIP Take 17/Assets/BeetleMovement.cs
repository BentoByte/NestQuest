using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleMovement : MonoBehaviour
{
    bool IsGrounded;
    [SerializeField] float CirSize;
    [SerializeField] float Speed;
    [SerializeField] Vector3 Diff;
    [SerializeField] Vector3 DiffFront;
    [SerializeField] LayerMask gLayerMask;
    [SerializeField] LayerMask PlayerMask;
    [SerializeField] float SeeDis;
    [SerializeField] bool Left;
    Rigidbody2D rb2d;
    public enum EnemyState
    {
        Idle,
        Chase,
        Stop
    }
    [SerializeField] 
    EnemyState CurrentState = EnemyState.Idle;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + Diff, CirSize, gLayerMask); 
        Collider2D[] colliders2 = Physics2D.OverlapCircleAll(transform.position + DiffFront, CirSize, gLayerMask); 
    }
    void Update()
    {
        switch (CurrentState)
        {
            case EnemyState.Idle:
            {
                SearchForPlayer();
                break;
            }
            case EnemyState.Chase:
            {
                Move();
                break;
            }
            case EnemyState.Stop:
            {
                break;
            }
            default:
            {
                break;
            }
        }
    }
    void SearchForPlayer()
    {
        if(Left)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.right, SeeDis, PlayerMask);
            rb2d.AddForce(Speed * -Vector2.right, ForceMode2D.Impulse);    
            if (hit)
            {
                Debug.Log("BALLS");
            }
        } else {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, SeeDis, PlayerMask);
            rb2d.AddForce(Speed * Vector2.right, ForceMode2D.Impulse);  
            if (hit)
            {
                Debug.Log("BALLS");  
            }
        }
    }
    void Move()
    {
        
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + Diff, CirSize);
        Gizmos.DrawWireSphere(transform.position + DiffFront, CirSize);
        Debug.DrawRay(transform.position, -Vector2.right * SeeDis, Color.green);
        Debug.DrawRay(transform.position, Vector2.right * SeeDis, Color.red);

    }
}
