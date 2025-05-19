using UnityEngine;

/// <summary>
/// プレイヤーの爆弾
/// </summary>
public class PHowitzerC : PMissile
{

    protected float _expCount, _expCounttime=99;
    protected int i, _countDusts;

    /// <summary>
    /// 0=未設定状態
    /// 1=カウントスタート
    /// 2=起爆
    /// </summary>
    protected int _expCountMode;

    [SerializeField]
    [Tooltip("爆発物")]
    protected PExpC ExpPrefab;

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
    /// <param name="exp">　起爆カウントダウン</param>
    /// <param name="dusts">爆発粉塵数</param>
    /// <param name="exptime">粉塵が消えるまで</param>
    public void ShotHowitzer(float angle, float speed, float kasoku, float exp, int dusts, float exptime)
    {
        ShotMissle(angle, speed, kasoku);

        _expCount = exp;
        _expCounttime = exptime;
        _countDusts = dusts;
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
            Explosion(_countDusts);
        }
    }

    protected void Explosion(int hunj)
    {
        _posOwn = transform.position;
        _audioGO.PlayOneShot(expS);
        _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(3, 6);

        ExpEffect(4);

        for (int j = 0; j < hunj; j++)
        {
            Instantiate(ExpPrefab, _posOwn, transform.rotation).ShotEXP((360 / hunj) * j, 10, _expCounttime);
        }
        Destroy(gameObject);
    }

    protected void ExpEffect(int shiningValue)
    {
        Instantiate(_prhbExpShining, _posOwn, Quaternion.Euler(0, 0, 0)).ShotEXP(0, 0, 0.3f);
        for (int i = 0; i < shiningValue; i++)
        {
            Instantiate(_prhbExpShining, _posOwn + new Vector3(Random.Range(-48, 48), Random.Range(-48, 48), 0), Quaternion.Euler(0, 0, 0))
                .ShotEXP(0, 0, 0.3f);
        }
    }


}
