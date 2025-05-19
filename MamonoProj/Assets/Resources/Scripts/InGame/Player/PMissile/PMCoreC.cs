using EnumDic.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMCoreC : MonoBehaviour
{
    [SerializeField]
    [Tooltip("敵弾と相殺するか")]
    protected bool  _right = true, _left = true, _up = true, _down = true;

    protected Vector3 _posOwn;

    /// <summary>
    /// 敵と自機弾の衝突判定
    /// </summary>
    protected RaycastHit2D _hitPmissileToEnemy;

    /// <summary>
    /// 敵弾と自機弾の衝突判定
    /// </summary>
    protected RaycastHit2D _hitPmissileToEmissile;

    [SerializeField, Tooltip("攻撃種類")]
    protected MODE_GUN _attackType;

    [SerializeField, Tooltip("攻撃力")]
    protected int _attackPower = 0;

    protected bool _isDeleteTrigger;

    protected float[] _attackMagnif = { 1.0f, 1.1f, 1.3f, 1.3f };

    /// <summary>
    /// 攻撃をしたオブジェをカウント
    /// </summary>
    protected List<GameObject> _listHitOnj = new List<GameObject> { };

    /// <summary>
    /// 貫通力
    /// </summary>

    [SerializeField, Tooltip("貫通力")]
    protected int _penetrationHit = 1;

    // Start is called before the first frame update
    protected void Start()
    {
        //transform.position += transform.right * GetComponent<BoxCollider2D>().size.x;
    }

    // Update is called once per frame
    protected void Update()
    {
        _posOwn = transform.position;


            BoxCollider2D box = GetComponent<BoxCollider2D>();
            Vector2 v2pos = new Vector2(transform.position.x, transform.position.y);
            _hitPmissileToEnemy = Physics2D.BoxCast(v2pos - box.offset, box.size, transform.localEulerAngles.z, Vector2.zero, 0, 128);
        if (_hitPmissileToEnemy)
        {
            if (CheckIsFirstAttackByHit(_hitPmissileToEnemy.collider.gameObject))
            {
                //ターゲット
                if (_hitPmissileToEnemy.collider.tag == "Target")
                {
                    bool isDamage = _hitPmissileToEnemy.collider.GetComponent<TargetC>().HitTarget(_attackType, _hitPmissileToEnemy.collider.ClosestPoint(transform.position));
                    if (!isDamage)
                    {
                        _isDeleteTrigger = true;
                    }
                }
                else
                {
                    float attackPower = (float)(_attackPower * _attackMagnif[(int)GameData.Difficulty]);
                    int damage = _hitPmissileToEnemy.collider.GetComponent<ECoreC>().DoGetDamage(attackPower, _attackType, _hitPmissileToEnemy.collider.ClosestPoint(transform.position));
                    switch (damage)
                    {
                        //判定なし
                        case 0:
                            break;

                        ///効果なし
                        case 1:
                            _listHitOnj.Add(_hitPmissileToEnemy.collider.gameObject);
                            _isDeleteTrigger = true;
                            break;

                        ///効果あり
                        case 2:
                            _listHitOnj.Add(_hitPmissileToEnemy.collider.gameObject);
                            SetDeleteByHitCount();
                            break;
                    }
                }
            }
            
        }

        _hitPmissileToEmissile = Physics2D.BoxCast(transform.position, GetComponent<BoxCollider2D>().size, transform.localEulerAngles.z, Vector2.zero, 0, 256);
        if (_hitPmissileToEmissile)
        {
            int missileValue = _hitPmissileToEmissile.collider.GetComponent<EMCoreC>().HitToPmissile(_attackType == MODE_GUN.Crash);
            if ((missileValue == 2 && _attackType == MODE_GUN.Crash) || missileValue == 3 || missileValue == 4) _isDeleteTrigger = true;
        }

        //down_ex
        if (_posOwn.y <= 0 && _down) _isDeleteTrigger = true;

        //up_ex
        if (_posOwn.y >= 480 && _up) _isDeleteTrigger = true;

        //left_ex
        if (_posOwn.x <= 0 && _left) _isDeleteTrigger = true;

        //right_ex
        if (_posOwn.x >= 640 && _right) _isDeleteTrigger = true;

    }

    protected void SetDeleteByHitCount()
    {
        if (_listHitOnj.Count >= _penetrationHit)
        {
            _isDeleteTrigger = true;
        }
    }

    /// <summary>
    /// すでにヒットした敵でないか確認
    /// </summary>
    /// <param name="hit"></param>
    /// <returns></returns>
    protected bool CheckIsFirstAttackByHit(GameObject hit)
    {
        foreach(GameObject yetHit in _listHitOnj)
        {
            if (yetHit == hit)
            {
                return false;
            }
        }
        return true;
    }

    public void SetDelete() => _isDeleteTrigger = true;

    public bool DeleteMissileCheck() => _isDeleteTrigger;


}
