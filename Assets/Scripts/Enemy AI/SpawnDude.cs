using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputControlExtensions;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class SpawnDude : MonoBehaviour
{
    public static SpawnDude Instance { get; private set; }
    
    [SerializeField] private Transform playerGO;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private int enemies = 4;
    [SerializeField] private float radius = 10f;

    [FormerlySerializedAs("mindelay")] [SerializeField] private float minDelay = 3f;
    [FormerlySerializedAs("maxdelay")] [SerializeField] private float maxDelay = 6f;
    [SerializeField] private float minOffset = 1f;
    [SerializeField] private float maxOffset = 5f;
    public event EventHandler OnWaveEnded;
    public int Wave {get; private set;}
    private int maxEnemiesAtATime;
    // There might need to be an initial figure assigned to it.
    private float delay;
    private int enemyNum = 0;
    private int allEnemyNum = 0;
    private int deadEnemies = 0;
    private int totalDeadEnemies = 0;
    private int waveNum;
    private bool stopSpawn = false;
    private bool blockSpawn = false;
    private float rotateOffset;
    private float distanceOffset;
    private float border = 50;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Health.OnEnemyDeath += EnemyTracker;
        MusicScaleMaker.Instance.OnConfirmation += StartSpawn;
        OnWaveEnded += Reset_Enemies;
        OnWaveEnded += StopSpawn;
        //StopSpawnEnemies += StopSpawn
        Wave = 1;
        waveNum = 3;
        maxEnemiesAtATime = waveNum / 3;
        stopSpawn = false;
    }

    private void EnemyTracker(object sender, EventArgs e)
    {
        Debug.Log("Dead enemies");
        deadEnemies++;
        totalDeadEnemies++;
        if (deadEnemies < waveNum)
        {
            enemyNum--;
            Debug.Log("There are " + deadEnemies + " dead enemies.");
        }
        if (deadEnemies >= waveNum)
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
            Debug.Log($"{allEnemyNum} < {waveNum} && {enemyNum} < {maxEnemiesAtATime}?? (maybe)");
            Debug.Log($"{allEnemyNum} >= {waveNum}?? (maybe)");
            SetOffset();
            Vector2 euler_vector = playerGO.position +
                                   (Quaternion.Euler(0, 0, 0 + (rotateOffset + (360.0f / enemies * i))) *
                                    new Vector3(distanceOffset + radius, 0, 0));
            if (allEnemyNum < waveNum && enemyNum < maxEnemiesAtATime)
            {
                switch (euler_vector.x)
                {
                    case < 50 and > -50:
                        break;
                    case > 50:
                        euler_vector.x  = -50;
                        break;
                    case < -50:
                        euler_vector.x = 50;
                        break;
                }
                switch (euler_vector.y)
                {
                    case < 50 and > -50:
                        break;
                    case > 50:
                        euler_vector.y  = -50;
                        break;
                    case < -50:
                        euler_vector.y = 50;
                        break;
                }
                Instantiate(enemyPrefab, euler_vector, Quaternion.identity, transform);
                allEnemyNum ++;
                enemyNum++;
                Debug.Log("Enemy Number: " + enemyNum);
            }
            else if (allEnemyNum >= waveNum)
            {
                return;
            }
            else return;
        }
    }

    private void FixedUpdate()
    {
        if (stopSpawn) return;
        
        if (blockSpawn) return;

        blockSpawn = true;
        SpawnDudes();
        StartCoroutine(DelaySpawnDudes());
    }

    private void SetTime()
    {
        delay = UnityEngine.Random.Range(minDelay, maxDelay);
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
        allEnemyNum = 0;
        deadEnemies = 0;
        Wave++;
        maxEnemiesAtATime = waveNum / 3;
    }

    private void StopSpawn(object sender, EventArgs e)
    {
        stopSpawn = true;
    }

    private void StartSpawn(object sender, EventArgs e)
    {
        stopSpawn = false;
    }

    public int GetTotalDeadEnemies() => totalDeadEnemies;
}
