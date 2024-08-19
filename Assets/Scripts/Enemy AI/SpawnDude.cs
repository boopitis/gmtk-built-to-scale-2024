using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDude : MonoBehaviour
{
    [SerializeField] private Transform playerGO;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private int enemies = 4;
    [SerializeField] private float radius = 10f;

    [SerializeField] private float mindelay = 3f;
    [SerializeField] private float maxdelay = 6f;
    [SerializeField] private float minOffset = 1f;
    [SerializeField] private float maxOffset = 5f;
    private float delay;
    
    private bool blockSpawn = false;

    private float rotateOffset;
    private float distanceOffset;

    private void SpawnDudes()
    {
        for (int i = 0; i < enemies; i++)
        {
            
            //Debug.Log(i + "loop");
            SetOffset();
            Vector2 euler_vector = playerGO.position + (Quaternion.Euler(0, 0, 0 + (rotateOffset + (360 / enemies * i))) * new Vector3(distanceOffset + radius, 0, 0));
            //Debug.Log("euler is " + euler_vector);
            Instantiate(enemyPrefab, euler_vector, Quaternion.identity, transform);
            //Debug.Log("spawned enemy " + i);
        }
    }

    private void FixedUpdate()
    {
        if (blockSpawn) return;

        blockSpawn = true;
        SpawnDudes();
        StartCoroutine(DelaySpawnDudes());
    }

    private void SetTime()
    {
        delay = Random.Range(mindelay, maxdelay);
    }

    private void SetOffset()
    {
        rotateOffset = Random.Range(-maxOffset*3, maxOffset*3);
        distanceOffset = Random.Range(minOffset, maxOffset);
    }

    private IEnumerator DelaySpawnDudes()
    {
        SetTime();
        yield return new WaitForSeconds(delay);
        blockSpawn = false;
    }
}
