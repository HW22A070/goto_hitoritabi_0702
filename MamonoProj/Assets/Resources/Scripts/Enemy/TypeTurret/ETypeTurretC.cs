using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETypeTurretC : MonoBehaviour
{
    protected int judge;
    protected float down = 0;
    protected int pull, tate;
    
    protected Vector3 pos,ppos;

    protected bool _isDontDown = false;

    protected GameObject playerGO;

    protected SpriteRenderer spriteRenderer;

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

    /// <summary>
    /// 足の位置と足の広さ
    /// </summary>
    //[SerializeField,Header("足の位置と足の広さ")]
    private Vector2 _posFoot = new Vector3(32, 32);

    /// <summary>
    /// プレイヤーと床の判定
    /// </summary>
    private RaycastHit2D _hitEnemyToFloor;

    /// <summary>
    /// スピーカ
    /// </summary>
    protected AudioSource _audioGO;


    // Start is called before the first frame update
    protected void Start()
    {
        playerGO = GameObject.Find("Player");
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    protected void Update()
    {
        pos = transform.position;
        ppos = playerGO.transform.position;
    }

    protected void FixedUpdate()
    {
        //重力
        Ray2D playerFootRay = new Ray2D(transform.position - new Vector3(_posFoot.x/2, _posFoot.y*1.2f + 1.0f, 0), new Vector2(_posFoot.x, 0));
        //Debug.DrawRay(playerFootRay.origin, playerFootRay.direction, Color.gray);
        if (_isDowning)
        {
            //床から出たら降下中を切る。自分が乗ってる台だけすり抜ける
            _isGround = false;

            if (!Physics2D.Raycast(playerFootRay.origin, playerFootRay.direction, _posFoot.y, 8)) _isDowning = false;
        }
        else
        {

            _hitEnemyToFloor = Physics2D.Raycast(playerFootRay.origin, playerFootRay.direction, _posFoot.y, 8);
            //足元に地面がなければ空中
            _isGround = _hitEnemyToFloor;
        }

        //地面
        if (_isGround)
        {
            GameObject floor = _hitEnemyToFloor.collider.gameObject;
            pos = transform.position;

            transform.position = new Vector3(pos.x
                , floor.transform.position.y + (floor.GetComponent<BoxCollider2D>().size.y / 2) + ((GetComponent<BoxCollider2D>().size.y - GetComponent<BoxCollider2D>().offset.y) / 2), 0);

            //重力ゼロ
            _gravityNow = 0;

            //炎上！
            _hitEnemyToFloor = Physics2D.Raycast(playerFootRay.origin, playerFootRay.direction, _posFoot.y, 8);
            if (_hitEnemyToFloor)
            {
                ECoreC eCore = GetComponent<ECoreC>();
                if (_hitEnemyToFloor.collider.gameObject.GetComponent<FloorC>()._floorMode == 3)
                {
                    //もし炎弱点であればダメージくらう
                    if(eCore.GetIsCritical(3)&&eCore.TotalHp>1)eCore.Damage(1, 3, _hitEnemyToFloor.collider.gameObject.transform.position);
                }
            }
        }
        //空中
        else
        {
            //重力加速
            if (_gravityNow >= _gravityMax) _gravityNow = _gravityMax;
            else _gravityNow += _gravityDelta;
        }
        transform.position -= new Vector3(0, _gravityNow, 0);

        //降下
        if (down == 0)
        {
            if (_isGround)
            {
                _isDowning = true;
            }
        }
        if (pos.y <= -(_posFoot.y-1))
        {
            Destroy(gameObject);
        }

        if (!_isDontDown) down = Random.Range(0, 100);
        else down = 1;
    }
}
