using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveCoords : MonoBehaviour
{
    Vector2 coords;
    // Start is called before the first frame update

    public Vector2 GiveC()
    {
        coords = transform.position;
        return coords;
    }
}
