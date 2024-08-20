using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected int damage;
    protected Health phealth;
    protected Collision2D col;
    //public int damage;

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        col = collision;
        if (collision.collider.CompareTag("Player"))
        {
            phealth = collision.collider.GetComponent<Health>();
            phealth.GetHit(damage);
        }
            
    }

    protected void OnCollisionExit2D(Collision2D collision)
    {
        phealth = null;
    }

    protected int Value()
    {
        return damage;
    }
}
