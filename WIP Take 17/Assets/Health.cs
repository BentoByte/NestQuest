using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
    public static event Action onPlayerDeath;
    public Image fillbar;
    public float health;
    public Image head;
    public Vector2 Vect;
    private SpriteRenderer Rend;
    [SerializeField] private float InvinLength;
    [SerializeField] private float Flashes;
    private void Awake()
    {
        Rend = GetComponentInChildren<SpriteRenderer>();
    }
    public void LoseHealth(int value, Vector2 direction2)
    {
        if (health <= 0)
            return;
        health -= value;
        FindObjectOfType<ControllerBirb>().KnockbackFunk(direction2);
        fillbar.fillAmount = health / 100;
        StartCoroutine(Invincibility());
        
        if(health <= 0)
        {       
            FindObjectOfType<ControllerBirb>().isDead = true;
            onPlayerDeath?.Invoke();
        }
    }
    private IEnumerator Invincibility()
    {
        Physics2D.IgnoreLayerCollision(12,9, true);
        yield return new WaitForSeconds(5);
        Physics2D.IgnoreLayerCollision(12,9, false);

    }

}
