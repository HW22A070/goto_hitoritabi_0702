using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETypeDevilC : ETypeTurretC
{
    [SerializeField]
    protected float _moveSpeedBound;

    protected float move = 0;


    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        if (Random.Range(0, 2) == 0)
        {
            move = _moveSpeedBound;
            spriteRenderer.flipX = true;
        }
        else
        {
            move = -_moveSpeedBound;
            spriteRenderer.flipX = false;
        }
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
        if (pos.x > 632)
        {
            move = -_moveSpeedBound;
            spriteRenderer.flipX = false;
        }
        if (pos.x < 8)
        {
            move = _moveSpeedBound;
            spriteRenderer.flipX = true;
        }

    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        transform.position += new Vector3(move, 0, 0);
    }


}
