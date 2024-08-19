using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDudesExample : MonoBehaviour
{
    private GameObject playerGO;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private int enemies = 4;
    [SerializeField] private float radius = 10f;

    private void Awake()
    {
        playerGO = FindObjectOfType<Player>().gameObject;
    }

    private void SpawnDudes()
    {
        for (int i = 0; i < enemies; i++)
        {
            Instantiate(enemyPrefab, Quaternion.Euler(0, 0, 0 + 360 / enemies * i) * (playerGO.transform.right + new Vector3(radius, 0, 0)), Quaternion.identity);
        }
    }
}
