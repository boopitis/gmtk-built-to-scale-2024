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
    public event EventHandler OnHeal;
    public event EventHandler OnDeath;
    public static event EventHandler OnEnemyDeath;
    public event EventHandler OnMaxHealthChange;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;

    [SerializeField] private float immunityDuration;
    private Collider2D collide;

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

    private void Start()
    {
        collide = GetComponent<Collider2D>();
        MusicScaleMaker.Instance.OnConfirmation += NewWaveHealth;
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
            
            if (collide.CompareTag("Enemy"))
            {
                OnEnemyDeath?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnDeath?.Invoke(this, EventArgs.Empty);
            }
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
        {
            return;
        }

        health += amount;
        if (health > maxHealth) health = maxHealth;
        
        OnHeal?.Invoke(this,EventArgs.Empty);
    }

    public void ChangeMaxHealth(int amount)
    {
        if (maxHealth - amount <= 0)
        {
            Debug.LogError("Cannot change max health to below zero!");
        }

        maxHealth += amount;
        OnMaxHealthChange?.Invoke(this, EventArgs.Empty);
        
        if (health >= maxHealth) return;
        
        health = maxHealth;
        OnHit?.Invoke(this, EventArgs.Empty);
    }
    
    public int GetMaxHealth() => maxHealth;
    
    public int GetHealth() => health;

    public void NewWaveHealth(object sender, EventArgs e)
    {
        ChangeMaxHealth( Mathf.Clamp(FindObjectOfType<SpawnDude>().WaveNum, 0, 10));
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Backspace))
    //     {
    //         // Debug.Log("ABout to");
    //         Heal(1);
    //         // Debug.Log("Heal");
    //     }
    // }
}