using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnockbackFeedback : MonoBehaviour
{
    public event EventHandler OnBegin, OnDone;

    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private float strength;
    [SerializeField] private float delay;

    public void Awake()
    {
        strength = 8;
        delay = 0.15f;
    }
    
    public void PlayFeedback(GameObject sender)
    {
        StopAllCoroutines();
        OnBegin?.Invoke(this, EventArgs.Empty);
        Vector2 direction = (transform.position - sender.transform.position).normalized;
        rb2d.AddForce(direction * strength, ForceMode2D.Impulse);
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rb2d.velocity = Vector3.zero;
        OnDone?.Invoke(this, EventArgs.Empty);
    }
}
