using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputControlExtensions;

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
    public event EventHandler OnWaveEnded;
    private int Wave = 1;
    private int MaxEnemiesAtATime;
    // There might need to be an initial figure assigned to it.
    private float delay;
    private int enemyNum = 0;
    private int AllEnemyNum = 0;
    private int DeadEnemies = 0;
    private int WaveNum;
    private bool stopspawn = false;
    private bool blockSpawn = false;
    private float rotateOffset;
    private float distanceOffset;

    
    private void Start()
    {
        Health.OnEnemyDeath += EnemyTracker;
        OnWaveEnded += Reset_Enemies;
        OnWaveEnded += StopSpawn;
        //OnConfirmation += ;
        //StopSpawnEnemies += StopSpawn;
        WaveNum = 100;
        MaxEnemiesAtATime = (int)(0.5 * WaveNum);
    }

    private void EnemyTracker(object sender, EventArgs e)
    {
        Debug.Log("Dead enemies");
        DeadEnemies++;
        if (DeadEnemies < WaveNum)
        {
            enemyNum--;
            Debug.Log("There are " + DeadEnemies + " dead enemies.");
        }
        if (DeadEnemies >= WaveNum)
        {
            Debug.Log("Good job putting them all to rest.");
            OnWaveEnded?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SpawnDudes()
    {
        
        for (int i = 0; i < enemies; i++)
        {
            
            Debug.Log(i + "loop");
            Debug.Log("E: " + enemyNum);
            SetOffset();
            Vector2 euler_vector = playerGO.position + (Quaternion.Euler(0, 0, 0 + (rotateOffset + (360 / enemies * i))) * new Vector3(distanceOffset + radius, 0, 0));
            if (AllEnemyNum < WaveNum && enemyNum < MaxEnemiesAtATime)
            {
                Instantiate(enemyPrefab, euler_vector, Quaternion.identity, transform);
                AllEnemyNum ++;
                enemyNum++;
                Debug.Log("Enemy Number: " + enemyNum);
            }
            else if (AllEnemyNum >= WaveNum)
            {
                return;
            }
            else return;
        }
    }

    private void FixedUpdate()
    {
        if(stopspawn == false)
        {
            if (blockSpawn) return;

            blockSpawn = true;
            SpawnDudes();
            StartCoroutine(DelaySpawnDudes());
        }
        else return;
    }

    private void SetTime()
    {
        delay = UnityEngine.Random.Range(mindelay, maxdelay);
    }

    private void SetOffset()
    {
        rotateOffset = UnityEngine.Random.Range(-maxOffset*3, maxOffset*3);
        distanceOffset = UnityEngine.Random.Range(minOffset, maxOffset);
    }

    private IEnumerator DelaySpawnDudes()
    {
        SetTime();
        yield return new WaitForSeconds(delay);
        blockSpawn = false;
    }

    private void Reset_Enemies(object sender, EventArgs e)
    {
        enemyNum = 0;
        AllEnemyNum = 0;
        DeadEnemies = 0;
        Wave++;
        WaveNum = Wave * 100;
        MaxEnemiesAtATime = (int)(0.5 * WaveNum);
    }

    private void StopSpawn(object sender, EventArgs e)
    {
        stopspawn = true;
    }

}