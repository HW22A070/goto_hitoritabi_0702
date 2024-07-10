using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastC : MonoBehaviour
{
    int judge;
    float down = 0;
    int pull, tate;

    float move = 0;
    Vector3 pos,ppos;

    public SpriteRenderer spriteRenderer;

    [SerializeField]
    [Tooltip("PlayerGameObject")]
    private GameObject playerGO;

    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        ppos = playerGO.transform.position;
        pos = transform.position;

        if (pos.x > 632)
        {
            move = -2f;
            spriteRenderer.flipX = false;
        }
        if (pos.x < 8)
        {
            move = 2f;
            spriteRenderer.flipX = true;
        }
    }

    public void Summon(int judge, int t)
    {
        if (judge == 0)
        {
            move = -7f;
            spriteRenderer.flipX = false;
        }
        else
        {
            move = 7f;
            spriteRenderer.flipX = true;
        }
        down = t;
    }

    void FixedUpdate()
    {
        if (pos.x > ppos.x - 32)
        {
            if (move > -5) move -= 0.1f;
            spriteRenderer.flipX = false;
        }
        if (pos.x < ppos.x + 32)
        {
            if (move < 5) move += 0.1f;
            spriteRenderer.flipX = true;
        }

        if (down == 0 && tate == 0)
        {
            pull = 1;
            tate = 1;
        }
        if (pos.y <= -30)
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
