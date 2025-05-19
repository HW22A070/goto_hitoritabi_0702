using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETypeDevilC : ETypeTurretC
{
    [SerializeField]
    protected float _moveSpeedBound;

    protected float _move = 0;


    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        if (Random.Range(0, 2) == 0)
        {
            _move = _moveSpeedBound;
            _srOwnBody.flipX = true;
        }
        else
        {
            _move = -_moveSpeedBound;
            _srOwnBody.flipX = false;
        }
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
        if (_posOwn.x > 632)
        {
            _move = -_moveSpeedBound;
            _srOwnBody.flipX = false;
        }
        if (_posOwn.x < 8)
        {
            _move = _moveSpeedBound;
            _srOwnBody.flipX = true;
        }

    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        transform.position += new Vector3(_move, 0, 0);
    }


}
