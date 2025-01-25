using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    int Jumps = 2;
    private Object thisObject;
    void Awake()
    {
        thisObject = GetComponent<Object>();
    }

    void Update()
    {
        GetComponent<ControllerBirb>().TotalJumps = Jumps;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Jumps++;
            Destroy(gameObject);
        }
        
    }
}
