using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class LockPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        Lock();
    }

    // Update is called once per frame
    void Update()
    {
        Lock();
    }

    private void Lock()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            player.position = player.position;
        }
        if (Input.GetKey(KeyCode.Backspace))
        {
            player.position = new Vector2(0,0);
        }
    }
}
