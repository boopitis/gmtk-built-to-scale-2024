using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGuy : MonoBehaviour
{
    [SerializeField] GiveCoords playerc;
    Vector2 playerp;
    // Start is called before the first frame update
    private void Follow()
    {
        transform.Translate(playerp.normalized * Time.deltaTime,Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        playerp = playerc.GiveC();
        Follow();
    }
}
