using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDudesExample : MonoBehaviour
{
    [SerializeField] GameObject playerGO;
    [SerializeField] GameObject enemyPrefab;

    private void Awake()
    {
        playerGO = FindObjectOfType<Player>().gameObject;
    }

    private void SpawnDudes()
    {
        int enemies = 4;
        int offset = 10;
        for (int i = 0; i < enemies; i++)
        {
            Instantiate(enemyPrefab, Quaternion.Euler(0, 0, 0 + 360 / enemies * i) * (playerGO.transform.right + new Vector3(offset, 0, 0)), Quaternion.identity);
        }
    }
}
