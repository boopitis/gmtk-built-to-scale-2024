using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class Health : MonoBehaviour
{
    public event EventHandler OnHit;
    public event EventHandler OnDeath;
    //public delegate void Death (string a, int b);
    //public event Death NowDead; 
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] protected int health, maxHealth;
    [SerializeField] private float immunityDuration;

    protected bool isDead;
    protected bool immune;
    private const float ColorChangeDuration = 0.25f;

    public void InitializeHealth(int healthValue)
    {
        health = healthValue;
        maxHealth = healthValue;
        isDead = false;
        immunityDuration = 0.0f;
    }

    private void Awake()
    {
        health = maxHealth;
    }


    public void GetHit(int amount, GameObject sender)
    {
        if (immune || isDead || sender.layer == gameObject.layer)
            return;

        health -= amount;
        StartCoroutine(ColorChange());

        if (health > 0)
        {
            OnHit?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnDeath?.Invoke(this, EventArgs.Empty);
            isDead = true;
            Destroy(gameObject);
        }

        // TODO: Add immunity animation
        StartCoroutine(Immunity());
    }

    private IEnumerator Immunity()
    {
        immune = true;
        int tempLayer = gameObject.layer;
        gameObject.layer = 10;
        yield return new WaitForSeconds(immunityDuration);
        immune = false;
        gameObject.layer = tempLayer;
    }

    private IEnumerator ColorChange()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(ColorChangeDuration);
        spriteRenderer.color = Color.white;
    }

    public void Heal(int amount)
    {
        if (health == maxHealth)
            return;

        health += amount;
    }

    public int GetMaxHealth() => maxHealth;
    
    public int GetHealth() => health;

    protected void InvokeOnHit()
    {
        OnHit?.Invoke(this, EventArgs.Empty);
    }
    
}
