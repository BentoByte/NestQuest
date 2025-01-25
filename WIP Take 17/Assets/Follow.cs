using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform Target;
    public float MinMod;
    public float MaxMod;

    public Vector2 _Velocity = Vector2.zero;
    // public Vector3 _Velocity;
    public bool _isFollowing = false;

    void Start()
    {
        
    }

    public void StartFollow()
    {
        _isFollowing = true;

    }

    
    void Update()
    {
        if (_isFollowing)
        {
            
            transform.position = Vector2.SmoothDamp(transform.position, Target.position, ref _Velocity, Time.deltaTime * Random.Range(MinMod, MaxMod));
        }
    }
}
