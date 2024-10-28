using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETypeEelC : MonoBehaviour
{
    protected Vector3 pos;

    [SerializeField]
    protected float _moveSpeed = 10, _moveDelta = 0;

    // Update is called once per frame
    protected void Update()
    {
        pos = transform.position;

        if (pos.y >= 1200)
        {
            Destroy(gameObject);
        }
    }

    protected void FixedUpdate()
    {
        _moveSpeed += _moveDelta;
        transform.localPosition += new Vector3(0, _moveSpeed, 0);
    }
}
