using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGuy : MonoBehaviour
{
    private Vector2 playerPosition;
    private float minVelocity;
    private float maxVelocity;
    private float velocity;
    
    void Start()
    {
        minVelocity = 1.95f;
        maxVelocity = 4.5f;
        
        velocity = Random.Range(minVelocity, maxVelocity);
    }
    
    // Start is called before the first frame update

    private void Follow()
    {
        transform.Translate((playerPosition - (Vector2)transform.position).normalized * velocity * Time.deltaTime,
            Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = Player.Instance.GiveC();
        Follow();
    }
}
