using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETypeSnowC : MonoBehaviour
{

    protected Vector3 _posOwn, _posPlayer;
    protected Vector3 _muki, _velocity;

    protected GameObject _goPlayer;

    protected SpriteRenderer _spOwn;
    
    [SerializeField]
    protected float _fixTargetPosTime = 1.0f,_speed=1.0f;
    protected float _time = 0;
    
    protected void Start()
    {
        _posOwn = transform.position;
        _goPlayer = GameObject.Find("Player");
    }

    // Update is called once per frame
    protected void Update()
    {
        _posOwn = transform.position;
        _posPlayer = _goPlayer.transform.position;

        if (_time >= 0) _time -= Time.deltaTime;
        else
        {
            float angle = GameData.GetAngle(_posOwn, _posPlayer);
            var direction = GameData.GetDirection(angle);
            _velocity = direction * 2;

            _time = _fixTargetPosTime;
        }
    }

    protected void FixedUpdate() => transform.position += _velocity * _speed;
}
