using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMCoreC : MonoBehaviour
{
    [SerializeField]
    [Tooltip("敵弾と相殺するか")]
    private bool sosai, _right = true, _left = true, _up = true, _down = true;

    private Vector3 pos;

    private GameObject PlayerGO;

    /// <summary>
    /// 敵と自機弾の衝突判定
    /// </summary>
    private RaycastHit2D _hitPmissileToEnemy;

    /// <summary>
    /// 敵弾と自機弾の衝突判定
    /// </summary>
    private RaycastHit2D _hitPmissileToEmissile;

    [SerializeField, Tooltip("攻撃種類0=beam,1=bullet,2=bomb,3=burn")]
    private int _attackType = 0;

    [SerializeField, Tooltip("攻撃力")]
    private int _attackPower = 0;

    private bool _isDeleteTrigger;

    private float[] _attackMagnif = { 1.0f, 1.1f, 1.3f, 1.6f };

    /// <summary>
    /// 攻撃した後かをカウント。連続ヒットを防ぐ
    /// </summary>
    private bool _isAttacked;

    // Start is called before the first frame update
    void Start()
    {
        PlayerGO = GameObject.Find("Player");
        //transform.position += transform.right * GetComponent<BoxCollider2D>().size.x;
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;

        if (!_isAttacked)
        {
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            Vector2 v2pos = new Vector2(transform.position.x, transform.position.y);
            _hitPmissileToEnemy = Physics2D.BoxCast(v2pos - box.offset, box.size, transform.localEulerAngles.z, Vector2.zero, 0, 128);
            if (_hitPmissileToEnemy)
            {
                //ターゲット
                if (_hitPmissileToEnemy.collider.tag == "Target")
                {
                    bool isDamage = _hitPmissileToEnemy.collider.GetComponent<TargetC>().HitTarget(_attackType, _hitPmissileToEnemy.collider.ClosestPoint(this.transform.position));
                    if (!isDamage)
                    {
                        _isDeleteTrigger = true;
                    }
                }
                else// if (_hitPmissileToEnemy.collider.tag == "Enemy"|| _hitPmissileToEnemy.collider.tag == "MechaUnit")
                {
                    _isAttacked = true;
                    float attackPower = (float)(_attackPower * _attackMagnif[GameData.Difficulty]);
                    bool delete = _hitPmissileToEnemy.collider.GetComponent<ECoreC>().Damage(attackPower, _attackType, _hitPmissileToEnemy.collider.ClosestPoint(transform.position));
                    if (delete)
                    {
                        _isDeleteTrigger = true;
                    }
                    if (sosai) _isDeleteTrigger = true;
                }
            }
        }

        _hitPmissileToEmissile = Physics2D.BoxCast(transform.position, GetComponent<BoxCollider2D>().size, transform.localEulerAngles.z, Vector2.zero, 0, 256);
        if (_hitPmissileToEmissile)
        {
            int missileValue = _hitPmissileToEmissile.collider.GetComponent<EMCoreC>().HitToPmissile(_attackType == 2);
            if ((missileValue == 2 && _attackType == 2) || missileValue == 3 || missileValue == 4) _isDeleteTrigger = true;
        }

        //down_ex
        if (pos.y <= 0 && _down) _isDeleteTrigger = true;

        //up_ex
        if (pos.y >= 480 && _up) _isDeleteTrigger = true;

        //left_ex
        if (pos.x <= 0 && _left) _isDeleteTrigger = true;

        //right_ex
        if (pos.x >= 640 && _right) _isDeleteTrigger = true;

    }

    public void SetDelete()
    {
        _isDeleteTrigger = true;
    }

    public bool DeleteMissileCheck()
    {
        return _isDeleteTrigger;
    }


}
