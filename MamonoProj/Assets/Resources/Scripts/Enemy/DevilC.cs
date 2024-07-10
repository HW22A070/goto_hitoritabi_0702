using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilC : MonoBehaviour
{
    int judge;
    float down = 0;
    int pull, tate;

    float move = 0;
    Vector3 pos;

    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;

        if (pos.x > 632)
        {
            move = -15f;
            spriteRenderer.flipX = false;
        }
        if (pos.x < 8)
        {
            move = 15f;
            spriteRenderer.flipX = true;
        }

    }

    public void Summon(int judge)
    {
        if (judge == 0)
        {
            move = -15f;
            spriteRenderer.flipX = false;
        }
        else
        {
            move = 15f;
            spriteRenderer.flipX =true;
        }
    }

    void FixedUpdate()
    {
        if (down == 0 && tate == 0)
        {
            pull = 1;
            tate = 1;
        }
        if (pos.y <= -50)
        {
            Destroy(gameObject);
        }
        down = Random.Range(0, 100);

        transform.localPosition += new Vector3(move, 0, 0);

        if (pull > 0)
        {
            pull++;
            transform.localPosition += new Vector3(0, -10, 0);
            if (pull > 9)
            {
                pull = 0;
                tate = 0;
            }
        }
    }

    
}
