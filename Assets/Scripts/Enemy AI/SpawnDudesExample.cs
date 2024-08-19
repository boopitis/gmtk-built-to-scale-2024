using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDudesExample : MonoBehaviour
{
    private GameObject playerGO;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private int enemies = 4;
    [SerializeField] private float radius = 10f;

    [SerializeField] private float mindelay = 3f;
    [SerializeField] private float maxdelay = 8f;
    [SerializeField] private float minOffset = 1f;
    [SerializeField] private float maxOffset = 6f;
    private float delay;
    
    private bool blockSpawn = false;

    private float rotateOffset;
    private float distanceOffset;

    private void Awake()
    {
        playerGO = FindObjectOfType<Player>().gameObject;
    }

    private void SpawnDudes()
    {
        for (int i = 0; i < enemies; i++)
        {
            SetOffset();
            Instantiate(enemyPrefab, Quaternion.Euler(0, 0, 0  + rotateOffset + 360 / enemies * i) * (playerGO.transform.right + new Vector3(radius + distanceOffset, 0, 0)), Quaternion.identity);
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
        rotateOffset = Random.Range(-maxOffset*10, maxOffset*10);
        distanceOffset = Random.Range(minOffset, maxOffset);
    }

    private IEnumerator DelaySpawnDudes()
    {
        SetTime();
        yield return new WaitForSeconds(delay);
        blockSpawn = false;
    }
}
