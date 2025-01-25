using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int CurrentHealth;
    public int MaxHealth;
    Rigidbody2D rb2d;
    Animator Anime;
    public bool CanDie;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Anime = GetComponentInChildren<Animator>();
        CurrentHealth = MaxHealth;
    }

    public void Knockback(Vector2 Direction, float Force)
    {
        rb2d.AddForce(Direction * Force, ForceMode2D.Impulse);
    }
    public void GetHurt(int Damage)
    {
        //PlayHurtAnimation
        //Blink
        CurrentHealth -= Damage;
        Debug.Log(CurrentHealth + " OUCH!");
        if(CurrentHealth <= 0 && CanDie)
        {
            StartCoroutine(Die());
        }
    }
    
    public void ChangeAnimationState(string newState)
    {
        if (Anime.GetCurrentAnimatorClipInfo(0)[0].clip.name == newState) return;

        Anime.Play(newState);
    }

    IEnumerator Die()
    {
        //ChangeAnimationState("Death");
        Debug.Log("You are dead, no big suprise");
        yield return new WaitForSeconds(0.1f);
        Destroy(transform.gameObject);
    }


}
