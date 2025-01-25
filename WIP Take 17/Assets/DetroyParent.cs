using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetroyParent : MonoBehaviour
{
    public int AddMe;
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            // GetComponent<Money>().MakeMoney(AddMe);
            FindObjectOfType<Money>().MakeMoney(AddMe);
            Destroy(transform.parent.gameObject);
        }
    }
}
