using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BluesSpecial : BaseSpecial
{
    [SerializeField] private Collider2D projectileCollider;
    [SerializeField] private Renderer renderer;
    [SerializeField] private float timeToDisappear;
    [SerializeField] private float positionOffset;
    
    private const int Raycasts = 4;
    private const int Spread = 11;

    private float currentTimeToDisappear;

    private void Awake()
    {
        currentTimeToDisappear = timeToDisappear;
    }

    private void Update()
    {
        currentTimeToDisappear -= Time.deltaTime;
        
        Color matColor = renderer.material.color;
        float alphaValue = currentTimeToDisappear / timeToDisappear;
        renderer.material.color = new Color(matColor.r, matColor.g, matColor.b, alphaValue);
        
        if (alphaValue < 0) Destroy(gameObject);
    }

    /** 
     * Laser attack
     */
    public override void Fire(Transform position, Quaternion rotation)
    {
        List<RaycastHit2D> hitEnemies = new List<RaycastHit2D>();
        for (int i = 0; i < Raycasts; i++)
        {
            RaycastHit2D[] collisions = {  };
            projectileCollider.GetComponent<Collider2D>().Raycast(
                Quaternion.Euler(0, 0, -Spread + (Spread * 2.0f / Raycasts * i)) * rotation * Vector2.right,
                collisions,
                5.5f);
            
            foreach (var raycastHit2D in collisions)
            {
                if (hitEnemies.Contains(raycastHit2D)) return;

                if (!raycastHit2D.transform.TryGetComponent<FindGuy>(out _)) return;
                
                hitEnemies.Add(raycastHit2D);
            }
        }
            
        string debugOut = "hit enemy points: "; 
        foreach (var raycastHit2D in hitEnemies)
        {
            debugOut += raycastHit2D.point + ", ";
            
            raycastHit2D.transform.GetComponent<Health>().GetHit(
                projectile.GetDamage());
        }
        Debug.Log(debugOut);

        var instantiatedPrefab = Instantiate(gameObject.transform,
            position.position + rotation * new Vector3(positionOffset, 0, 0),
            rotation);
        instantiatedPrefab.gameObject.SetActive(true);
    }
}
