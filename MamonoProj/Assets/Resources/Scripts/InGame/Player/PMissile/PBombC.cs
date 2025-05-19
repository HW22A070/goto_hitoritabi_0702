using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBombC : PMissile
{
    /// <summary>
    /// 爆発カウント制御
    /// </summary>
    protected float _expCount, _expCounttime = 99;

    /// <summary>
    /// 爆発半径
    /// </summary>
    protected float _radiusExp;

    /// <summary>
    /// 0=未設定状態
    /// 1=カウントスタート
    /// 2=起爆
    /// </summary>
    protected int _expCountMode;

    [SerializeField]
    [Tooltip("爆発エフェクト")]
    protected ExpC _prhbExpShining;

    /// <summary>
    /// スピーカ
    /// </summary>
    protected AudioSource _audioGO;

    [SerializeField]
    protected AudioClip expS;

    protected Coroutine _movingCoroutine;

    /// <summary>
    /// カメラオブジェクト
    /// </summary>
    protected GameObject _goCamera;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        _goCamera = GameObject.Find("Main Camera");
    }

    /// <summary>
    /// 爆発
    /// </summary>
    /// <param name="angle">発射向き</param>
    /// <param name="speed">発射速度</param>
    /// <param name="kasoku">発射加速度</param>
    /// <param name="expCount">　起爆カウントダウン</param>
    public void ShotBomb(float angle, float speed, float kasoku, float expCount, float radius)
    {
        ShotMissle(angle, speed, kasoku);

        _expCount = expCount;
        _radiusExp = radius;
        _expCountMode = 1;
    }

    // Update is called once per frame
    protected new void FixedUpdate()
    {

        if (_expCountMode == 1)
        {
            base.FixedUpdate();

            //time_ex
            _expCount--;
            if (_expCount <= 0) DoDelete();
        }
    }

    protected override void DoDelete()
    {
        if (_expCountMode == 1)
        {
            _expCountMode = 2;
            Explosion();
        }
    }

    protected void Explosion()
    {
        _posOwn = transform.position;
        _audioGO.PlayOneShot(expS);
        _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(3, 6);

        ExpEffect();

        RaycastHit2D[] hitPmissileToEnemys = Physics2D.CircleCastAll(_posOwn,_radiusExp,Vector2.zero,0,128);
        foreach (RaycastHit2D hit in hitPmissileToEnemys)
        {

            if (CheckIsFirstAttackByHit(hit.collider.gameObject))
            {
                //ターゲット
                if (hit.collider.tag == "Target")
                {
                    bool isDamage = hit.collider.GetComponent<TargetC>().HitTarget(_attackType, hit.collider.ClosestPoint(transform.position));
                    if (!isDamage)
                    {
                        _isDeleteTrigger = true;
                    }
                }
                else
                {
                    float attackPower = (float)(_attackPower * _attackMagnif[(int)GameData.Difficulty]);
                    hit.collider.GetComponent<ECoreC>().DoGetDamage(attackPower, _attackType, hit.collider.ClosestPoint(transform.position));
                }
            }
        }
        Destroy(gameObject);
    }

    protected void ExpEffect()
    {
        Instantiate(_prhbExpShining, _posOwn, Quaternion.Euler(0, 0, 0)).ShotEXP(0, 0, 0.3f);

        for (int r=1; r < _radiusExp / 48; r++)
        {
            
            for (int i = 0; i < 360; i += ((64 * Random.Range(30,45) / (int)_radiusExp) + 1))
            {
                Instantiate(_prhbExpShining, _posOwn + new Vector3(Mathf.Cos(i * Mathf.Deg2Rad), Mathf.Sin(i * Mathf.Deg2Rad) ,0) * r*48, Quaternion.Euler(0, 0, 0))
                    .ShotEXP(0, 0, 0.3f);
            }
        }
        for (int i = 0; i < 360; i += ((64*45 / (int)_radiusExp) + 1))
        {
            Instantiate(_prhbExpShining, _posOwn + new Vector3(Mathf.Cos(i * Mathf.Deg2Rad), Mathf.Sin(i * Mathf.Deg2Rad), 0) *_radiusExp, Quaternion.Euler(0, 0, 0))
                .ShotEXP(0, 0, 0.3f);
        }

    }
}
