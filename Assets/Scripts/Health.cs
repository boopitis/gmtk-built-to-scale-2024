using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Health : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public int health, maxHealth;

    public float immunityDuration = 0f;
    public bool immune { get; private set; }

    private float colorChangeDuration = 0.25f;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;

    public void InitializeHealth(int healthValue)
    {
        health = healthValue;
        maxHealth = healthValue;
        isDead = false;
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
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            OnDeathWithReference?.Invoke(sender);
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
        yield return new WaitForSeconds(colorChangeDuration);
        spriteRenderer.color = Color.white;
    }

    public void Heal(int amount)
    {
        if (health == maxHealth)
            return;

        health += amount;
    }
}
