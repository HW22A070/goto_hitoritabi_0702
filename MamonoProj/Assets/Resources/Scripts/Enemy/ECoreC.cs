using System;
using System.Collections;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using EnumDic.System;
using EnumDic.Enemy;

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
    private EnemyDataC _enemyData;

    [NonSerialized]
    public bool IsBoss = false;

    [NonSerialized]
    public int TotalHp = 1, TotalHpFirst = 1;

    [SerializeField]
    [Tooltip("エフェクト")]
    private ExpC _invalidEP, _damageEP, _criticalEP, _criticalEP2;


    [NonSerialized]
    public MODE_LIFE BossLifeMode;

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
    [Tooltip("クリティカル発生")]
    private bool[] _isBeamCritical, _isBulletCritical, _isFireCritical, _isBombCritical;

    [SerializeField]
    [Tooltip("アイテム")]
    private HealC HealPrefab, MagicPrefab;

    private PMCoreC playerMissileP;

    [SerializeField]
    private PointEnergyC _scoreEnegyPrhb;

    /// <summary>
    /// PlayerGameObject
    /// </summary>
    private GameObject playerGO;

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

        playerGO = GameObject.Find("Player");
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
            //GameData.Point += score;
            _audioGO.PlayOneShot(deadS);
            if (IsBoss)
            {

                StartCoroutine(DoDeathStop(0.3f));

            }
            else
            {
                for (int hoge = 0; hoge < score; hoge++)
                {
                    Instantiate(_scoreEnegyPrhb, _posOwn, rot);
                }
                Destroy(gameObject);
            }
            deathStarted = true;

        }
    }

    private void SetStates()
    {
        hp = _enemyData.HP;
        _melleAttack = _enemyData.MELLE_POWER;
        _melleHitInvincible = _enemyData.MELLE_SPEED;
        score = _enemyData.SCORE;
        BeamD = _enemyData.BEAM_DAMAGE;
        _isBeamCritical = _enemyData.BEAM_CRI;
        BulletD = _enemyData.BULLET_DAMAGE;
        _isBulletCritical = _enemyData.BULLET_CRI;
        FireD = _enemyData.FIRE_DAMAGE;
        _isFireCritical = _enemyData.FIRE_CRI;
        BombD = _enemyData.BOMB_DAMAGE;
        _isBombCritical = _enemyData.BOMB_CRI;
        IsBoss = _enemyData.IS_BOSS;
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
    /// 
    /// </summary>
    /// <param name="hitf"></param>
    /// <param name="type"></param>
    /// <param name="attackPos"></param>
    /// <returns></returns>
    public bool DoGetDamage(float hitf, int type, Vector3 attackPos)
    {
        if (_isSetUpped)
        {
            //ダメージ喰らう
            switch (type)
            {
                case 0:
                    hitf *= BeamD[EvoltionMode];
                    break;
                case 1:
                    hitf *= BulletD[EvoltionMode];
                    break;
                case 2:
                    hitf *= BombD[EvoltionMode];
                    break;
                case 3:
                    hitf *= FireD[EvoltionMode];
                    break;
            }
            //Debug.Log(hitf.ToString() + " -> " + hit.ToString());
            int hit = (int)hitf;
            hp[EvoltionMode] -= hit;

            //エフェクト発生
            if (hit <= 0)
            {
                RunInvalidEffect(attackPos);
            }
            else
            {
                bool critical = (type == 0 && _isBeamCritical[EvoltionMode]) || (type == 1 && _isBulletCritical[EvoltionMode]) ||
                (type == 2 && _isBombCritical[EvoltionMode]) || (type == 3 && _isFireCritical[EvoltionMode]);
                RunDamageEffect(critical, attackPos);
            }

            if (hit > 0)
            {
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
            if (hit > 0) return false;
            else return true;
        }
        else return false;
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
                shot2.EShot1(Random.Range(0, 360), 0.2f, 0.4f);
            }
            _audioGO.PlayOneShot(criticalS);
            RunEffectSummon(_criticalEP, effectPos);
            _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(5, 10);
            playerGO.GetComponent<PlayerC>().VibrationCritical();
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
        _audioGO.PlayOneShot(invalidS);
        Vector3 direction2 = new Vector3(_posOwn.x, _posOwn.y, 0);
        for (int ddd = 0; ddd < 4; ddd++)
        {
            Quaternion rot2 = transform.localRotation;
            ExpC shot2 = Instantiate(_invalidEP, effectPos + new Vector3(Random.Range(-32, 32), Random.Range(-32, 32), 0), rot2);
            shot2.EShot1(0, 0, 0.2f);
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
            shot2.EShot1(angle2, 10, 0.2f);
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
            if (IsBoss)
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
    public bool CheckIsCritical(int value)
    {
        switch (value)
        {
            case 0:
                return _isBeamCritical[EvoltionMode];
            case 1:
                return _isBulletCritical[EvoltionMode];
            case 2:
                return _isBombCritical[EvoltionMode];
            case 3:
                return _isFireCritical[EvoltionMode];
        }
        return false;
    }

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
        Instantiate(_prfbFlashEffect, GameData.WindowSize / 2, rot).EShot1(0, 0, 0.3f);
        float timespeed = Time.timeScale;
        TimeManager.ChangeTimeValue(0.1f);
        yield return new WaitForSeconds(stopTime/10);
        TimeManager.ChangeTimeValue(timespeed);
        _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(10, 30);
        BossLifeMode = MODE_LIFE.Dead;
    }
}
