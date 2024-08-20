using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEngine.InputSystem.InputControlExtensions;

public class SpawnHold : MonoBehaviour
{
    [FormerlySerializedAs("Spawner_Holder")] [SerializeField] private GameObject[] spawnerHolder;
    [SerializeField] private float distanceMag;
    private Vector2 playerPos;
    private Vector2 coords;

    // Start is called before the first frame update
    private void Start()
    {
        Positioning_Spawners();
        Debug.Log(Mathf.Cos(Mathf.PI));
    }

    // Update is called once per frame
    void Update()
    {
        Positioning_Spawners();
        playerPos = transform.position;
    }

    private void Positioning_Spawners()
    {
        var pos = new List<int>();
        for (int i = spawnerHolder.Length - 1; i >= 0; i--)
        // int i shows the number of spawners in the holder.
        {
            if (spawnerHolder[i].activeInHierarchy)
            {pos.Add(i);}

            // This implies there can be lots of spawners in the spawn holder, with some always being activate and others getting
            // activated at certain times.
        }

        if (pos.ToArray().Length < 1) return;
        
        int[] spawnNumsD1 = pos.ToArray();
        foreach (int i in spawnNumsD1)
        {
            float angles = 360 * ((Array.IndexOf(spawnNumsD1, i) + 1.0f) / spawnNumsD1.Length);
            //
            //Debug.Log(Array.IndexOf(spawnNumsD1, i) + 1);
            //Debug.Log(spawnNumsD1.Length);
            float piAngles = (2 * angles * Mathf.PI)/ 360;
                
            coords = new Vector2 (playerPos[0] + Mathf.Cos(piAngles) * distanceMag, playerPos[1] + Mathf.Sin(piAngles) * distanceMag);
            // This would position the spawners at equidistant places around the player in a circle.
            spawnerHolder[i].transform.position = coords;
        }
    }
}
