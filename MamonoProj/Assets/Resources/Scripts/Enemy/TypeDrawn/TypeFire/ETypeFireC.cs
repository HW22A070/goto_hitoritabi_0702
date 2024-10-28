using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETypeFireC : ETypeDrawnC
{
    protected int mode = 0;

    [SerializeField]
    protected Vector2 _attackValue = new Vector2(5,0);

    protected Vector3 ppos;

    private GameObject playerGO;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        playerGO = GameObject.Find("Player");
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
        ppos = playerGO.transform.position;

        if (ppos.y >= pos.y && mode == 0)
        {
            mode = 1;
            if (pos.x > ppos.x)
            {
                move = -_attackValue;
                spriteRenderer.flipX = false;
            }
            else
            {
                move = _attackValue;
                spriteRenderer.flipX = true;
            }
        }
    }
}
