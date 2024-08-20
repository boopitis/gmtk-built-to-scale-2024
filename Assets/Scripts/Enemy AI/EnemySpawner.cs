using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemySpawner : MonoBehaviour
{
    [FormerlySerializedAs("_Enemy_prefab")] [SerializeField] GameObject enemyPrefab;
    [FormerlySerializedAs("Min_timeSpawn")] [SerializeField] float minTimeSpawn;
    [FormerlySerializedAs("Max_timeSpawn")] [SerializeField] float maxTimeSpawn;
    [SerializeField] private float timeSpawn;
    [SerializeField] private float stepTimeSpawn;
    // Start is called before the first frame update
    void Start()
    {
        SetTime();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        timeSpawn -= Time.deltaTime;
        if (!(timeSpawn < 0)) return;
        
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        SetTime();
    }

    private void SetTime()
    {
        timeSpawn = Random.Range(minTimeSpawn, maxTimeSpawn);
    }
}
