using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Full_Flags : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer sprite;
    private Color shade;
    private Color dcolor = new Color(0f, 0f, 0f, 1f);
    private Color acolor = new Color(1f, 1f, 1f, 1f);
    
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        shade = sprite.color;

    }
    
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Reactivate()
    {
        gameObject.SetActive(true);
    }

    public void HeartApp(string state)
    {
        if (state == "dead")
            sprite.color = dcolor;
        else if (state == "alive")
            sprite.color = acolor;
    }

    public bool isDead()
    {
        if (-0.1f <= sprite.color.r & sprite.color.r <= 0.1f)
            return true;
        else
            return false;
    }

    public bool isAlive()
    {
        if (0.9f <= sprite.color.r && sprite.color.r <= 1.1f)
            return true;
        else
            return false;
    }
}
