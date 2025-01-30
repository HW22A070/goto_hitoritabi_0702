using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETypeSnowC : MonoBehaviour
{

    protected Vector3 pos, ppos;
    protected Vector3 muki, velocity;

    protected GameObject playerGO;

    protected SpriteRenderer spriteRenderer;
    
    [SerializeField]
    protected float _fixTargetPosTime = 1.0f,_speed=1.0f;
    protected float _time = 0;
    
    protected void Start()
    {
        pos = transform.position;
        playerGO = GameObject.Find("Player");
    }

    // Update is called once per frame
    protected void Update()
    {
        pos = transform.position;
        ppos = playerGO.transform.position;

        if (_time >= 0) _time -= Time.deltaTime;
        else
        {
            float angle = GameData.GetAngle(pos, ppos);
            var direction = GameData.GetDirection(angle);
            velocity = direction * 2;

            _time = _fixTargetPosTime;
        }
    }

    protected void FixedUpdate()
    {
        transform.position += velocity*_speed;
    }
}
