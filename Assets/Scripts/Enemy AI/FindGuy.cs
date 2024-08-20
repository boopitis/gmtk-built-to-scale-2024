using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGuy : MonoBehaviour
{
    private Vector2 playerp;
    private float minvelocity;
    private float maxvelocity;
    private float velocity;
    //float[] d_interest = new float[7];
    //float[] d_danger = new float[7];
    
    void Start()
    {
        minvelocity = 1.95f;
        maxvelocity = 4.5f;
        
        velocity = Random.Range(minvelocity, maxvelocity);
    }
    
    // Start is called before the first frame update

    private void Follow()
    {
        transform.Translate((playerp - (Vector2)transform.position).normalized * velocity * Time.deltaTime,Space.Self);
    }

    // private void Directional_Interest()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
        playerp = Player.Instance.GiveC();
        //playerp[0] = Mathf.Sign(playerp[0] - transform.position.x);
        //playerp[1] = Mathf.Sign(playerp[1] - transform.position.y);
        Follow();
    }
}
