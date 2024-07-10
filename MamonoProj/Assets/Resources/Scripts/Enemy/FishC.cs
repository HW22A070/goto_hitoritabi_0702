using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishC : MonoBehaviour
{
    int judge;
    float down = 0;
    int pull, tate;

    float move = 0;
    Vector3 pos, ppos;

    public SpriteRenderer spriteRenderer;
    public Sprite noemalS, gachiS;

    bool gachi;

    [SerializeField]
    [Tooltip("PlayerGameObject")]
    private GameObject playerGO;

    private ECoreC core;

    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        ppos = playerGO.transform.position;
        if (!gachi)
        {
            if (pos.x > 628)
            {
                move = -2f;
                spriteRenderer.flipX = false;
            }
            if (pos.x < 16)
            {
                move = 2f;
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            if (pos.x > 628)
            {
                move = -5f;
                spriteRenderer.flipX = false;
            }
            if (pos.x < 16)
            {
                move = 5f;
                spriteRenderer.flipX = true;
            }

        }

    }

    public void Summon(int judge, int t)
    {
        core = GetComponent<ECoreC>();
        if (judge == 0)
        {
            move = -3f;
            spriteRenderer.flipX = false;
        }
        else
        {
            move = 3f;
            spriteRenderer.flipX = true;
        }
        down = t;
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


            if (core.EvoltionMode==1)
            {
                gachi = true;
                spriteRenderer.sprite = gachiS;
            }
    }
}
