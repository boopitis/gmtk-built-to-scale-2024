using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject _Enemy_prefab;
    [SerializeField] float Min_timeSpawn;
    [SerializeField] float Max_timeSpawn;
    [SerializeField] float timeSpawn;
    [SerializeField] float stepTimeSpawn;
    // Start is called before the first frame update
    void Start()
    {
        SetTime();
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        timeSpawn -= Time.deltaTime;
        if (timeSpawn < 0)
        {
            Instantiate(_Enemy_prefab, transform.position, Quaternion.identity);
            SetTime();
        }
    }

    //private void InStep_Spawn()
    //{
    //    if (stepTimeSpawn < 0)
    //}

    private void SetTime()
    {
        timeSpawn = Random.Range(Min_timeSpawn, Max_timeSpawn);
    }
}
