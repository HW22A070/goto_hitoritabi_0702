using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMCoreC : MonoBehaviour
{
    [SerializeField, Header("0=自機弾に負ける\n1=自機ボムに負ける\n2=自機ボムと相殺\n3=自機弾と相殺\n4=自機弾に勝つ\n5=すり抜け")]
    protected int _destroyToPM;

    [SerializeField]
    [Tooltip("座標削除")]
    protected bool _right = true, _left = true, _up = true, _down = true,_palyerDelete=true;

    [SerializeField, Tooltip("この弾に当たった時に発生する無敵時間")]
    protected float _HitInvincible = 0.5f;

    /// <summary>
    /// 敵と自機弾の衝突判定
    /// </summary>
    protected RaycastHit2D _hitEmissileToPlayer;

    [SerializeField, Tooltip("攻撃力")]
    protected int _attackPower = 0;

    /// <summary>
    /// オンになったら消滅か爆発
    /// </summary>
    protected bool _isDeleteTrigger;

    /// <summary>
    /// 自分の座標
    /// </summary>
    protected Vector3 _posOwn;

    // Update is called once per frame
    protected void FixedUpdate()
    {
        _posOwn = transform.position;
        if (TryGetComponent<BoxCollider2D>(out BoxCollider2D box))
        {
            Vector2 v2pos = new Vector2(_posOwn.x, _posOwn.y);
            _hitEmissileToPlayer = Physics2D.BoxCast(v2pos - box.offset, box.size, transform.localEulerAngles.z, Vector2.zero, 0, 64);
            if (_hitEmissileToPlayer)
            {
                _hitEmissileToPlayer.collider.GetComponent<PlayerC>().SetDamage(_attackPower, _HitInvincible);
                if (_palyerDelete)
                {
                    _isDeleteTrigger = true;
                }

            }
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

    /// <summary>
    /// 0=自機弾に負ける\n1=自機ボムに負ける\n2=自機ボムと相殺\n3=自機弾と相殺\n4=自機弾に勝つ\n5=すり抜け
    /// </summary>
    /// <returns></returns>
    public int HitToPmissile(bool isBomb)
    {
        if (((_destroyToPM==1|| _destroyToPM == 2) &&isBomb)||_destroyToPM == 0 || _destroyToPM == 3)
        {
            _isDeleteTrigger = true;
        }
        return _destroyToPM;
    }

    public bool DeleteMissileCheck() => _isDeleteTrigger;
}
