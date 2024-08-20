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
    //public event EventHandler IsMaxEnemies;
    public event EventHandler StopSpawnEnemies;
    public event EventHandler ResetEnemies;
    //private GameObject[] Enemyarray = new GameObject[100];
    private int Wave = 1;
    private int MaxEnemiesAtATime = 10;
    // Max enemies should probably be 100.
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
        EnemyHealth.OnAnyDeath += EnemyTracker;
        ResetEnemies += Restart_Enemies;
        StopSpawnEnemies += StopSpawn;
    }

    private void EnemyTracker(object sender, EventArgs e)
    {
        DeadEnemies++;
        if (DeadEnemies < WaveNum)
        {
            enemyNum--;
        }
        if (DeadEnemies >= WaveNum)
        {
            enemyNum--;
            ResetEnemies?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SpawnDudes()
    {
        
        for (int i = 0; i < enemies; i++)
        {
            
            //Debug.Log(i + "loop");
            SetOffset();
            Vector2 euler_vector = playerGO.position + (Quaternion.Euler(0, 0, 0 + (rotateOffset + (360 / enemies * i))) * new Vector3(distanceOffset + radius, 0, 0));
            //Debug.Log("euler is " + euler_vector);
            if (AllEnemyNum < WaveNum && enemyNum < MaxEnemiesAtATime)
            {
                Instantiate(enemyPrefab, euler_vector, Quaternion.identity, transform);
                //GameObject Instantiated_enemy = Instantiate(enemyPrefab, euler_vector, Quaternion.identity, transform);
                AllEnemyNum ++;
                enemyNum++;
                //Enemyarray[enemyNum] = Instantiated_enemy;
                //Debug.Log(Enemyarray[1]);
                //Debug.Log("spawned enemy " + i);
                Debug.Log("There are " + enemyNum + " enemies.");
            }
            if (AllEnemyNum >= WaveNum)
            {
                return;
            }
        }
    }

    private void FixedUpdate()
    {
        if(!stopspawn)
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

    private void Refresh_EnemyArray()
    {
        //var apos = new List<int>();
        //for (int i = 0; i < enemyNum; i++)
        //{
        //    if (Enemyarray[i].name == "null")
        //    {
        //        foreach (int x in Enumerable.Range(0, 99))
        //        {
        //            if (Enemyarray[x].name == "Enemy(Clone) (UnityEngine.GameObject)")
        //            {
        //                Enemyarray[i] = Enemyarray[x];
        //                Enemyarray[x] = null;
        //                return;
        //            }
        //        }
        //    }
        //}

        //if (apos.ToArray().Length >= 1)
        //    {
        //    int[] EnemyNumsh1 = apos.ToArray();
        //    foreach (int i in EnemyNumsh1)
        //        {
                    //Enemyarray[i].GetComponent().HeartApp("alive");
        //        }
        //    }
    }

    private void Restart_Enemies(object sender, EventArgs e)
    {
        enemyNum = 0;
        AllEnemyNum = 0;
        DeadEnemies = 0;
        Wave++;
        WaveNum = Wave * 100;
    }

    private void StopSpawn(object sender, EventArgs e)
    {
        stopspawn = true;
    }

}
