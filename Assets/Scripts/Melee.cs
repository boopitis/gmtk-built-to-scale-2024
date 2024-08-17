using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public SpriteRenderer characterRenderer, gunRenderer, meleeRenderer;
    public Vector2 PointerPosition { get; set; }

    public Animator animator;
    public float delay = 0.3f;
    private bool attackBlocked;

    public int damage = 1;

    public Transform circleOrigin;
    public float radius;

    public bool isAttacking { get; private set; }

    public AudioSource audioSource;

    public void ResetIsAttacking()
    {
        isAttacking = false;
        meleeRenderer.enabled = false;
        gunRenderer.enabled = true;
    }

    private void Update()
    {
        if (isAttacking)
            return;

        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        if (direction.x < 0)
        {
            scale.y = -1;
            scale.x = -1;
        }
        else if (direction.x > 0)
        {
            scale.y = 1;
            scale.x = 1;
        }
        transform.localScale = scale;

        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            meleeRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
        else
        {
            meleeRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }
    }

    public void Attack()
    {
        if (attackBlocked)
            return;

        animator.SetTrigger("Attack");
        audioSource.Play();
        meleeRenderer.enabled = true;
        gunRenderer.enabled = false;
        isAttacking = true;
        attackBlocked = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            // Debug.Log(collider.name);
            Health health;
            if (health = collider.GetComponent<Health>())
            {
                health.GetHit(damage, transform.parent.gameObject);
            }
        }
    }
}
