using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombC : EMissile1C
{
    protected float _expCount, _expCountTime;

    /// <summary>
    /// 爆発半径
    /// </summary>
    protected float _radiusExp;

    /// <summary>
    /// スピーカ
    /// </summary>
    protected private AudioSource _audioGO;

    [SerializeField]
    protected AudioClip expS;

    [SerializeField]
    protected bool bombbarrier = true, bombsosai;

    [SerializeField]
    [Tooltip("爆発エフェクト")]
    protected ExpC _prhbExpShining;

    // Start is called before the first frame update
    protected void Start()
    {
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    public void ShotBomb(float angle, float speed, float kasoku, float expCountdown, float radius)
    {
        ShotMissile(angle, speed, kasoku);
        _expCount = expCountdown;
        _radiusExp = radius;
    }

    // Update is called once per frame
    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        //time_ex
        _expCount--;
        if (_expCount <= 0) DoDelete();
    }

    protected override void DoDelete()
    {
        if (_prhbExpShining != null) ExpEffect();
        
        foreach (RaycastHit2D hit in Physics2D.CircleCastAll(_posOwn, _radiusExp, Vector2.zero, 0, 64))
        {
            hit.collider.GetComponent<PlayerC>().SetDamage(_attackPower, _HitInvincible);
        }

        _audioGO.PlayOneShot(expS);
        Destroy(gameObject);
    }

    protected void ExpEffect()
    {
        Instantiate(_prhbExpShining, _posOwn, Quaternion.Euler(0, 0, 0)).ShotEXP(0, 0, 0.3f);

        for (int r = 1; r < _radiusExp / 48; r++)
        {

            for (int i = 0; i < 360; i += ((64 * Random.Range(30, 45) / (int)_radiusExp) + 1))
            {
                Instantiate(_prhbExpShining, _posOwn + new Vector3(Mathf.Cos(i * Mathf.Deg2Rad), Mathf.Sin(i * Mathf.Deg2Rad), 0) * r * 48, Quaternion.Euler(0, 0, 0))
                    .ShotEXP(0, 0, 0.3f);
            }
        }
        for (int i = 0; i < 360; i += ((64 * 45 / (int)_radiusExp) + 1))
        {
            Instantiate(_prhbExpShining, _posOwn + new Vector3(Mathf.Cos(i * Mathf.Deg2Rad), Mathf.Sin(i * Mathf.Deg2Rad), 0) * _radiusExp, Quaternion.Euler(0, 0, 0))
                .ShotEXP(0, 0, 0.3f);
        }

    }

}
