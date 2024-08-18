using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputControlExtensions;

public class Spawn_Hold : MonoBehaviour
{
    [SerializeField] private GameObject[] Spawner_Holder;
    [SerializeField] private float distanceMag;

    // Start is called before the first frame update
    void Start()
    {
        Positioning_Spawners();
    }

    // Update is called once per frame
    void Update()
    {
        Positioning_Spawners();
    }

    private void Positioning_Spawners()
    {
        var pos = new List<int>();
        for (int i = Spawner_Holder.Length - 1; i >= 0; i--)
        // int i shows the number of spawners in the holder.
            {
                if (Spawner_Holder[i].activeInHierarchy)
                     {pos.Add(i);}

                // This implies there can be lots of spawners in the spawn holder, with some always being activate and others getting
                // activated at certain times.
            }    

            if (pos.ToArray().Length >= 1)
            {
                int[] spawnNumsD1 = pos.ToArray();
                foreach (int i in spawnNumsD1)
                {
                    //Spawner_Holder[i].GetComponent<Full_Heart>();
                    int angles = 360 * ((Array.IndexOf(spawnNumsD1, i) + 1) / spawnNumsD1.Length);
                    float pi_angles = 2 * Mathf.PI/ 360 * angles;
                    
                    Vector2 coords = new Vector2 (transform.position.x + Mathf.Cos(pi_angles) * distanceMag, transform.position.y + Mathf.Sin(pi_angles) * distanceMag);
                    // This would position the spawners at equidistant places around the player in a circle.
                    Spawner_Holder[i].transform.position = coords;
                }
            }
    }
}
