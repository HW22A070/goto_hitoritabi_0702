﻿using UnityEngine;

public class CubeC : ETypeCoreC
{
    private bool _isExpansion = true;

    private Vector3 _scareOwn;
    private float _size = 0;

    private BoxCollider2D _boxColliderOwn;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        _boxColliderOwn = GetComponent<BoxCollider2D>();
        _scareOwn = _tfOwnBody.localScale;
        _scareOwn = new Vector3(0.01f, 0.01f, 0);
        _tfOwnBody.localScale = _scareOwn;
        _boxColliderOwn.size = new Vector3(_size, _size, 0);
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        if (_isExpansion)
        {
            _scareOwn = _tfOwnBody.localScale;
            _scareOwn += new Vector3(0.02f, 0.02f, 0);
            _size += 2.56f;
            _boxColliderOwn.size = new Vector3(_size, _size, 0);
            _tfOwnBody.localScale = _scareOwn;
            if (_scareOwn.x >= 1)
            {
                _isExpansion = false;
            }
        }

        else
        {
            _scareOwn = _tfOwnBody.localScale;
            _scareOwn -= new Vector3(0.02f, 0.02f, 0);
            _size -= 2.56f;
            _boxColliderOwn.size = new Vector3(_size, _size, 0);
            _tfOwnBody.localScale = _scareOwn;
            if (_scareOwn.x <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    
}
