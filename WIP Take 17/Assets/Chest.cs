using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public int MoneyDrop;
    public GameObject SomeDropPerfab;
    public Transform _dropLootTarget;
    public bool Done;
    public Animator Anime;
    void Update()
    {
        
    }

    public void DropCash()
    {
        if(!Done)
        {
            Anime.SetTrigger("Sesame");
            Done = true;
            for (int i = 0; i < MoneyDrop; i++)
            {
                var go = Instantiate(SomeDropPerfab, transform.position + new Vector3(0, Random.Range(0,2)), Quaternion.identity);

                go.GetComponentInChildren<Follow>().Target = _dropLootTarget.transform;
            }
        }
    }
}
