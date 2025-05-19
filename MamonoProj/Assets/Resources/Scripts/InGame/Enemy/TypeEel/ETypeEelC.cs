using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETypeEelC : ETypeCoreC
{

    [SerializeField]
    protected float _moveSpeed = 10, _moveDelta = 0;

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();

        if (_posOwn.y >= 1200)
        {
            Destroy(gameObject);
        }
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        _moveSpeed += _moveDelta;
        transform.localPosition += new Vector3(0, _moveSpeed, 0);
    }
}
