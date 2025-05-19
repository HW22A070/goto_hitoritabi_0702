using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDic.Stage;

public class MissileGravityC : MonoBehaviour
{
    private Vector3 _posOwn;

    /// <summary>
    /// 下降中なう
    /// </summary>
    private bool _isDowning;

    /// <summary>
    /// 空中
    /// </summary>
    private bool _isGround;

    /// <summary>
    /// 最大重力
    /// </summary>
    private int _gravityMax = 10;

    /// <summary>
    /// 重力加速度
    /// </summary>
    private int _gravityDelta = 4;

    /// <summary>
    /// 現在重力
    /// </summary>
    private int _gravityNow = 0;

    private MODE_FLOOR _floorMode=MODE_FLOOR.Normal;

    /// <summary>
    /// 足の位置と足の広さ
    /// </summary>
    //[SerializeField,Header("足の位置と足の広さ")]
    private Vector2 _posFoot = new Vector3(32, 32);

    /// <summary>
    /// プレイヤーと床の判定
    /// </summary>
    private RaycastHit2D _hitEnemyToFloor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _posOwn = transform.position;

        //炎の床なら爆発
        if (_floorMode == MODE_FLOOR.Burning)
        {
            if(GetComponent<PMCoreC>())GetComponent<PMCoreC>().SetDelete();
        }
    }

    private void FixedUpdate()
    {
        //重力
        Ray2D playerFootRay = new Ray2D(transform.position - new Vector3(_posFoot.x / 2, _posFoot.y * 1.2f + 1.0f, 0), new Vector2(_posFoot.x, 0));

        _hitEnemyToFloor = Physics2D.Raycast(playerFootRay.origin, playerFootRay.direction, _posFoot.y, 8);
        //足元に地面がなければ空中
        _isGround = _hitEnemyToFloor;

        //地面
        if (_isGround)
        {
            GameObject floor = _hitEnemyToFloor.collider.gameObject;
            _posOwn = transform.position;

            transform.position = new Vector3(_posOwn.x
                , floor.transform.position.y + (floor.GetComponent<BoxCollider2D>().size.y / 2) + ((GetComponent<BoxCollider2D>().size.y - GetComponent<BoxCollider2D>().offset.y) / 2), 0);

            _floorMode = floor.GetComponent<FloorC>().GetFloorMode();

            //重力ゼロ
            _gravityNow = 0;
        }
        //空中
        else
        {
            //重力加速
            if (_gravityNow >= _gravityMax) _gravityNow = _gravityMax;
            else _gravityNow += _gravityDelta;
        }
        transform.position -= new Vector3(0, _gravityNow, 0);
    }

}
