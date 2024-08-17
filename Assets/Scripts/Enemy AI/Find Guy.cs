using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGuy : MonoBehaviour
{
    private Vector2 playerp;
    private float minvelocity;
    private float maxvelocity;
    private float velocity;
    
    void Start()
    {
        minvelocity = 3;
        maxvelocity = 6.5f;
        
        velocity = Random.Range(minvelocity, maxvelocity);
    }
    
    // Start is called before the first frame update

    private void Follow()
    {
        transform.Translate((playerp - (Vector2)transform.position).normalized * velocity * Time.deltaTime,Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        playerp = Player.Instance.GiveC();
        //playerp[0] = Mathf.Sign(playerp[0] - transform.position.x);
        //playerp[1] = Mathf.Sign(playerp[1] - transform.position.y);
        Follow();
    }
}
