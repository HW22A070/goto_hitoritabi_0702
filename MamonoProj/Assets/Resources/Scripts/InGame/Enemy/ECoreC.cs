using System;
using System.Collections;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using EnumDic.System;
using EnumDic.Enemy;
using EnumDic.Player;

/// <summary>
/// 敵キャラのステータス制御
/// </summary>
[DefaultExecutionOrder(-1)]
public class ECoreC : MonoBehaviour
{
    /// <summary>
    /// 敵CSVからデータを持ってくるスクリプト
    /// </summary>
    private EnemyDataBookC _csEnemyDataBook;

    [SerializeField]
    private string _nameEnemy;

    /// <summary>
    /// 初期ステータス
    /// </summary>
    private EnemyDatas _enemyData;

    private bool _isBoss = false;

    private bool _isAliveAction = true;

    [NonSerialized]
    public int TotalHp = 1, TotalHpFirst = 1;

    [SerializeField]
    [Tooltip("エフェクト")]
    private ExpC _invalidEP, _damageEP, _criticalEP, _criticalEP2;

    /// <summary>
    /// 敵キャラの行動モード
    /// </summary>
    [NonSerialized]
    private MODE_LIFE _modeBossLife;

    /// <summary>
    /// 形態
    /// </summary>
    [NonSerialized]
    public short EvoltionMode = 0;

    [SerializeField]
    [Tooltip("第一形態HP")]
    public int[] hp;

    [SerializeField]
    [Tooltip("近接攻撃力")]
    public int[] _melleAttack;

    [SerializeField]
    [Tooltip("近接攻撃速度")]
    public float[] _melleHitInvincible;

    /// <summary>
    /// 敵とプレイヤーの衝突判定
    /// </summary>
    private RaycastHit2D _hitEnemyToPlayer;

    [NonSerialized]
    [Tooltip("撃破時の獲得スコア")]
    private int score;

    /// <summary>
    /// ダメージを喰らった回数
    /// </summary>
    private int _countDamage = 0;

    private Vector3 _posOwn;

    private Quaternion rot;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;


    [SerializeField]
    [Tooltip("効果音")]
    private AudioClip criticalS, damageS, invalidS;

    [SerializeField, Tooltip("死ぬときの音")]
    private AudioClip deadS;

    private bool death, deathStarted;

    [SerializeField]
    [Tooltip("被ダメージ倍率")]
    private float[] BeamD, BulletD, FireD, BombD;

    [SerializeField]
    [Tooltip("アイテム")]
    private HealC HealPrefab, MagicPrefab;

    private PMCoreC playerMissileP;

    [SerializeField]
    private PointEnergyC _scoreEnegyPrhb;

    private PlayersManagerC _scPlsM;

    private ETypeCoreC _scETypeCore;

    /// <summary>
    /// ダメージ喰らう
    /// </summary>
    private bool _isSetUpped;

    /// <summary>
    /// カメラオブジェクト
    /// </summary>
    private GameObject _goCamera;

    [SerializeField]
    private ExpC _prfbFlashEffect;

    // Start is called before the first frame update
    void Start()
    {
        
        _enemyData = GameObject.Find("EnemyDataBook").GetComponent<EnemyDataBookC>().sendEnenyData(_nameEnemy);
        SetStates();
        TotalHp = hp.Sum();
        TotalHpFirst = TotalHp;

        _scETypeCore = GetComponent<ETypeCoreC>();
        _scPlsM = GameObject.Find("PlayersManager").GetComponent<PlayersManagerC>();
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        _goCamera = GameObject.Find("Main Camera");

        _isSetUpped = true;
    }

    // Update is called once per frame
    void Update()
    {
        TotalHp = hp.Sum();
        _posOwn = transform.position;
        rot = transform.localRotation;

        if (death && !deathStarted)
        {
            SummonItems();
            _audioGO.PlayOneShot(deadS);
            if (_isBoss) StartCoroutine(DoDeathStop(0.3f));
            else
            {
                /*
                if(_countDamage<=1) StartCoroutine(DoDeathStopHeadShot());
                else SetPointAndDelete();
                */
                SetPointAndDelete();
            }
            deathStarted = true;
        }
    }

    /// <summary>
    /// ポイントを付与し、消滅
    /// </summary>
    public void SetPointAndDelete()
    {
        for (int hoge = 0; hoge < score; hoge++)
        {
            Instantiate(_scoreEnegyPrhb, _posOwn, rot);
        }
        Destroy(gameObject);
    }

    private void SetStates()
    {
        hp = _enemyData.HP;
        _melleAttack = _enemyData.MELLE_POWER;
        _melleHitInvincible = _enemyData.MELLE_SPEED;
        score = _enemyData.SCORE;
        BeamD = _enemyData.BEAM_DAMAGE;
        BulletD = _enemyData.BULLET_DAMAGE;
        FireD = _enemyData.FIRE_DAMAGE;
        BombD = _enemyData.BOMB_DAMAGE;
        _isBoss = _enemyData.IS_BOSS;
        if (_isBoss)
        {
            SetMultiHP();
        }
    }

    private void FixedUpdate()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        Vector2 v2pos = new Vector2(transform.position.x, transform.position.y);
        _hitEnemyToPlayer = Physics2D.BoxCast(v2pos + box.offset, box.size, transform.localEulerAngles.z, Vector2.zero, 0, 64);
        if (_hitEnemyToPlayer)
        {
            _hitEnemyToPlayer.collider.GetComponent<PlayerC>().SetDamage(_melleAttack[EvoltionMode], _melleHitInvincible[EvoltionMode]);

        }
    }
   
    /// <summary>
    /// 0=影響なし
    /// 1=効果なし
    /// 2=効果あり
    /// </summary>
    /// <param name="hitf"></param>
    /// <param name="type"></param>
    /// <param name="attackPos"></param>
    /// <returns></returns>
    public int DoGetDamage(float hitf, MODE_GUN type, Vector3 attackPos)
    {
        if (_isSetUpped)
        {
            if (!death)
            {
                //ダメージ喰らう
                switch (type)
                {
                    case MODE_GUN.Shining:
                        hitf *= BeamD[EvoltionMode];
                        break;
                    case MODE_GUN.Physical:
                        hitf *= BulletD[EvoltionMode];
                        break;
                    case MODE_GUN.Crash:
                        hitf *= BombD[EvoltionMode];
                        break;
                    case MODE_GUN.Heat:
                        hitf *= FireD[EvoltionMode];
                        break;
                }
                //Debug.Log(hitf.ToString() + " -> " + hit.ToString());
                int hit = (int)hitf;
                hp[EvoltionMode] -= hit;

                _scETypeCore.GetDamageValue(hit);

                //エフェクト発生
                if (hit <= 0)
                {
                    RunInvalidEffect(attackPos);
                }
                else
                {
                    bool critical = CheckIsCritical(type);
                    RunDamageEffect(critical, attackPos);
                }

                if (hit > 0)
                {
                    _countDamage++;
                    if (hp[EvoltionMode] <= 0)
                    {
                        hp[EvoltionMode] = 0;
                        if (EvoltionMode >= hp.Length - 1)
                        {
                            death = true;
                        }
                        else
                        {
                            EvoltionMode++;
                        }
                    }
                }

            //ダメージを喰らってなければプレイヤー弾を消す
            if (hit > 0) return 2;
            else return 1;

            }
            else return 0;
        }
        else return 0;
    }

    /// <summary>
    /// ダメージエフェクト発生
    /// </summary>
    /// <param name="isCritical"></param>
    /// <param name="effectPos"></param>
    private void RunDamageEffect(bool isCritical, Vector3 effectPos)
    {
        //クリティカル
        if (isCritical)
        {
            for (int ddd = 20; ddd <= 60; ddd += 20)
            {
                ExpC shot2 = Instantiate(_criticalEP2, effectPos + new Vector3(Random.Range(-ddd, ddd), Random.Range(-ddd, ddd), 0), transform.localRotation);
                shot2.ShotEXP(Random.Range(0, 360), 0.2f, 0.4f);
            }
            _audioGO.PlayOneShot(criticalS);
            RunEffectSummon(_criticalEP, effectPos);
            _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(5, 10);
            foreach (GameObject player in GetAlivePlayers())
            {
                player.GetComponent<PlayerC>().VibrationCritical();
            }
        }
        else
        {
            _audioGO.PlayOneShot(damageS);
            RunEffectSummon(_damageEP, effectPos);
        }
    }

    /// <summary>
    /// ガードエフェクト発生
    /// </summary>
    /// <param name="effectPos"></param>
    public void RunInvalidEffect(Vector3 effectPos)
    {
        //_audioGO.PlayOneShot(invalidS);
        Vector3 direction2 = new Vector3(_posOwn.x, _posOwn.y, 0);
        for (int ddd = 0; ddd < 4; ddd++)
        {
            Quaternion rot2 = transform.localRotation;
            ExpC shot2 = Instantiate(_invalidEP, effectPos + new Vector3(Random.Range(-32, 32), Random.Range(-32, 32), 0), rot2);
            shot2.ShotEXP(0, 0, 0.2f);
        }
        //Debug.Log("エフェクトエンド");
    }

    /// <summary>
    /// エフェクト発生
    /// </summary>
    /// <param name="damageE"></param>
    /// <param name="effectGenten"></param>
    private void RunEffectSummon(ExpC damageE, Vector3 effectGenten)
    {
        float angle2 = 0;
        Vector3 direction2 = new Vector3(_posOwn.x, _posOwn.y, 0);
        for (int ddd = 0; ddd < 10; ddd++)
        {
            angle2 += 36;
            Quaternion rot2 = transform.localRotation;
            ExpC shot2 = Instantiate(damageE, effectGenten, rot2);
            shot2.ShotEXP(angle2, 10, 0.2f);
        }
    }

    /// <summary>
    /// アイテム生成
    /// </summary>
    public void SummonItems()
    {
        Vector3 direction = new Vector3(_posOwn.x, GameData.GetGroundPutY((int)_posOwn.y / 90, 30), 0);
        if (GameData.Difficulty != MODE_DIFFICULTY.Berserker)
        {
            //HealItemSummon
            if (_isBoss)
            {
                switch (GameData.Difficulty)
                {
                    case MODE_DIFFICULTY.Safety:
                        for (int j = -20; j <= 20; j += 2) Instantiate(HealPrefab, direction + (transform.right * j), rot);
                        break;
                    case MODE_DIFFICULTY.General:
                        for (int j = -20; j <= 20; j += 5) Instantiate(HealPrefab, direction + (transform.right * j), rot);
                        break;
                    case MODE_DIFFICULTY.Assault:
                        for (int j = -20; j <= 20; j += 20) Instantiate(HealPrefab, direction + (transform.right * j), rot);
                        break;
                }
            }
            else if (Random.Range(0, 3) == 0) Instantiate(HealPrefab, direction, rot);
        }
        if (Random.Range(0, 80) == 0) Instantiate(MagicPrefab, direction, rot);
    }

    /// <summary>
    /// 弱点かどうかを調べる　
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool CheckIsCritical(MODE_GUN value)
    {
        switch (value)
        {
            case MODE_GUN.Shining:
                return BeamD[EvoltionMode]>=3;
            case MODE_GUN.Physical:
                return BulletD[EvoltionMode] >= 3;
            case MODE_GUN.Crash:
                return BombD[EvoltionMode]>=3;
            case MODE_GUN.Heat:
                return FireD[EvoltionMode] >= 3;
        }
        return false;
    }

    private GameObject[] GetAlivePlayers() => _scPlsM.GetAlivePlayers().ToArray();

    /// <summary>
    /// ダメージを受けなくする
    /// </summary>
    /// <param name="isSet"></param>
    public void SetInvisible(bool isSet)
    {
        _isSetUpped = !isSet;
    }

    /// <summary>
    /// 致命傷ヒットストップ
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoDeathStop(float stopTime)
    {
        ClearC.BossBonus += 1;
        Instantiate(_prfbFlashEffect, GameData.WindowSize / 2, rot).ShotEXP(0, 0, 0.3f);
        float timespeed = Time.timeScale;
        TimeManager.ChangeTimeValue(0.1f);
        yield return new WaitForSeconds(stopTime/10);
        TimeManager.ChangeTimeValue(timespeed);
        _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(10, 30);
        _modeBossLife = MODE_LIFE.Dead;
    }

    /// <summary>
    /// 一撃必殺ヒットストップ
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoDeathStopHeadShot()
    {
        Instantiate(_prfbFlashEffect, GameData.WindowSize / 2, rot).ShotEXP(0, 0, 0.1f);
        float timespeed = Time.timeScale;
        TimeManager.ChangeTimeValue(0.1f);
        yield return new WaitForSeconds(0.1f / 10);
        TimeManager.ChangeTimeValue(timespeed);
        _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(7, 10);
        SetPointAndDelete();
    }

    /// <summary>
    /// HPのマルチ補正
    /// </summary>
    private void SetMultiHP()
    {
        int multi = GameData.MultiPlayerCount;
        for(int i=0;i< hp.Length; i++)
        {
            hp[i] += (int)(hp[i] * 0.7f * (multi - 1));
        }
    }

    /// <summary>
    /// モードを変更し、帰らせる
    /// </summary>
    public void SetLeave()
    {
        _modeBossLife = MODE_LIFE.Leave;
    }

    public void SetDamageValue(float beam,float bullet,float bomb,float fire)
    {
        BeamD[EvoltionMode] = beam;
        BulletD[EvoltionMode] = bullet;
        BombD[EvoltionMode] = bomb;
        FireD[EvoltionMode] = fire;
    }

    public bool CheckIsBoss() => _isBoss;
    public void SetIsBoss(bool isBoss) => _isBoss = isBoss;

    public bool CheckIsAlive() => _isAliveAction;
    public void SetIsAlive(bool isAlive) => _isAliveAction = isAlive;

    public MODE_LIFE GetModeBossLife() => _modeBossLife;
    public void SetModeBossLife(MODE_LIFE mode) => _modeBossLife = mode;
}
