using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    

    public static void XMove(float Dir ,float Speed ,Rigidbody2D rb ,float velPower ,float acceleration ,float decceleration)
    {
        float TargetSpeed = Dir * Speed;

        float TargetSpeedDif = TargetSpeed - rb.velocity.x;
        
        float accelerRate = (Mathf.Abs(TargetSpeed) > 0.01f) ? acceleration : decceleration;
        
        float MoveDir = Mathf.Pow(Mathf.Abs(TargetSpeedDif) * accelerRate, velPower) * Mathf.Sign(TargetSpeedDif);

        rb.AddForce(MoveDir * Vector2.right);
    }

    

}
