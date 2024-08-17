using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEditor;
using UnityEngine;

public class Cashier : MonoBehaviour
{
    private Movement movement;

    [SerializeField]
    private Transform firePointLeft, firePointRight;
    public GameObject projectilePrefab;
    public float projectileSpeed = 8f;
    public int shootAmount = 3;
    public float shootSpreadTime = 0.25f;

    private Vector2 pointerInput, movementInput;

    public Vector2 PointerInput { get => pointerInput; set => pointerInput = value; }
    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

    public AudioSource shootAudio;

    public void Attack()
    {
        StartCoroutine(BurstAttack());
    }

    private IEnumerator BurstAttack()
    {
        yield return new WaitForSeconds(shootSpreadTime);
        for (int i = 0; i < shootAmount; i++)
        {
            
            shootAudio.Play();
            GameObject projectile1 = Instantiate(projectilePrefab, firePointLeft.position, firePointLeft.transform.rotation);
            Rigidbody2D projectile_rb1 = projectile1.GetComponent<Rigidbody2D>();
            projectile_rb1.AddForce(firePointLeft.right * projectileSpeed, ForceMode2D.Impulse);

            yield return new WaitForSeconds(shootSpreadTime / 2);

            shootAudio.Play();
            GameObject projectile2 = Instantiate(projectilePrefab, firePointRight.position, firePointRight.transform.rotation);
            Rigidbody2D projectile_rb2 = projectile2.GetComponent<Rigidbody2D>();
            projectile_rb2.AddForce(firePointRight.right * projectileSpeed, ForceMode2D.Impulse);
            
            yield return new WaitForSeconds(shootSpreadTime);
        }
    }

    private void Awake()
    {
        movement = GetComponent<Movement>();
    }

    void Update()
    {
        firePointLeft.transform.right = pointerInput - (Vector2)firePointLeft.transform.position;
        firePointRight.transform.right = pointerInput - (Vector2)firePointRight.transform.position;
        
        movement.MovementInput = movementInput;
        // TODO: Add movement animation
    }
}
