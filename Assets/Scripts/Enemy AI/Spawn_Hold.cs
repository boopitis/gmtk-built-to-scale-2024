using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputControlExtensions;

public class Spawn_Hold : MonoBehaviour
{
    [SerializeField] private GameObject[] Spawner_Holder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Positioning_Spawners()
    {
        var pos = new List<int>();
        for (int i = 8; i >= 0; i--)
        // This implies there are eigth spawners in the holder.
            {
                if (Spawner_Holder[i].activeInHierarchy)
                     pos.Add(i);
            }         
            if (pos.ToArray().Length >= 1)
            {
                int[] spawnNumsD1 = pos.ToArray();
                foreach (int i in spawnNumsD1)
                {
                //Spawner_Holder[i].GetComponent<Full_Heart>();
                int angle = 360 * ((Array.IndexOf(spawnNumsD1, i) + 1) / spawnNumsD1.Length);

                
                }
            }
    }
}
