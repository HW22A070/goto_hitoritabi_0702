using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class ECoreC : MonoBehaviour
{
    public bool IsBoss = false;

    [NonSerialized]
    public int TotalHp = 1, TotalHpFirst = 1;

    [SerializeField]
    [Tooltip("エフェクト")]
    private ExpC _invalidEP, _damageEP, _criticalEP, _criticalEP2;


    [NonSerialized]
    /// <summary>
    /// 状態
    /// 0=登場
    /// 1=戦闘
    /// 2=死
    /// </summary>
    public int BossLifeMode = 0;

    [NonSerialized]
    /// <summary>
    /// 形態
    /// </summary>
    public short EvoltionMode = 0;

    [SerializeField]
    [Tooltip("第一形態HP")]
    public int[] hp = { 10 };

    [SerializeField]
    [Tooltip("近接攻撃力")]
    public int[] _melleAttack = { 1 };

    [SerializeField]
    [Tooltip("近接攻撃力")]
    public float[] _melleHitInvincible = { 0.5f };

    /// <summary>
    /// 敵とプレイヤーの衝突判定
    /// </summary>
    private RaycastHit2D _hitEnemyToPlayer;

    [SerializeField]
    [Tooltip("撃破時の獲得スコア")]
    private int score;

    private Vector3 pos;

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
    private float[] BeamD = { 2 }, BulletD = { 1 }, FireD = { 1 }, BombD = { 5 };

    [SerializeField]
    [Tooltip("クリティカル発生")]
    private bool[] _isBeamCritical = { false }, _isBulletCritical = { false }, _isFireCritical = { false }, _isBombCritical = { false };

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

    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.Find("Player");
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        _goCamera = GameObject.Find("Main Camera");
        TotalHp = hp.Sum();
        TotalHpFirst = TotalHp;
        _isSetUpped = true;
    }

    // Update is called once per frame
    void Update()
    {
        TotalHp = hp.Sum();
        pos = transform.position;
        rot = transform.localRotation;

        if (death && !deathStarted)
        {
            SummonItems();
            //GameData.Point += score;
            _audioGO.PlayOneShot(deadS);
            if (IsBoss)
            {
                BossLifeMode = 2;

            }
            else
            {
                for (int hoge = 0; hoge < score; hoge++)
                {
                    Instantiate(_scoreEnegyPrhb, pos, rot);
                }
                Destroy(gameObject);
            }
            deathStarted = true;

        }
    }

    private void FixedUpdate()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        Vector2 v2pos = new Vector2(transform.position.x, transform.position.y);
        _hitEnemyToPlayer = Physics2D.BoxCast(v2pos + box.offset, box.size, transform.localEulerAngles.z, Vector2.zero, 0, 64);
        if (_hitEnemyToPlayer)
        {
            _hitEnemyToPlayer.collider.GetComponent<PlayerC>().Damage(_melleAttack[EvoltionMode], _melleHitInvincible[EvoltionMode]);

        }
    }

    public bool Damage(float hitf, int type, Vector3 attackPos)
    {
        if (_isSetUpped)
        {
            //Debug.Log("ダメージ喰らった！");
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


            Debug.Log("攻撃力：" + hit.ToString());

            //エフェクト発生
            if (hit <= 0)
            {
                InvalidEffect(attackPos);
            }
            else
            {
                bool critical = (type == 0 && _isBeamCritical[EvoltionMode]) || (type == 1 && _isBulletCritical[EvoltionMode]) ||
                (type == 2 && _isBombCritical[EvoltionMode]) || (type == 3 && _isFireCritical[EvoltionMode]);
                DamageEffect(critical, attackPos);
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

    private void DamageEffect(bool isCritical, Vector3 effectPos)
    {
        //Debug.Log("エフェクトスタート");
        if (isCritical)
        {
            for (int ddd = 20; ddd <= 60; ddd += 20)
            {
                ExpC shot2 = Instantiate(_criticalEP2, effectPos + new Vector3(Random.Range(-ddd, ddd), Random.Range(-ddd, ddd), 0), transform.localRotation);
                shot2.EShot1(Random.Range(0, 360), 0.2f, 0.4f);
            }
            //Debug.Log("キラキラ");
            _audioGO.PlayOneShot(criticalS);
            EffectSummon(_criticalEP, effectPos);
            _goCamera.GetComponent<CameraC>().StartShakeVertical(5, 10);
            playerGO.GetComponent<PlayerC>().CriticalVibration();
        }
        else
        {
            _audioGO.PlayOneShot(damageS);
            EffectSummon(_damageEP, effectPos);
        }
        //Debug.Log("エフェクトエンド");
    }

    public void InvalidEffect(Vector3 effectPos)
    {
        _audioGO.PlayOneShot(invalidS);
        Vector3 direction2 = new Vector3(pos.x, pos.y, 0);
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
    private void EffectSummon(ExpC damageE, Vector3 effectGenten)
    {
        float angle2 = 0;
        Vector3 direction2 = new Vector3(pos.x, pos.y, 0);
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
        Vector3 direction = new Vector3(pos.x, GameData.GroundPutY((int)pos.y / 90, 30), 0);
        if (GameData.Difficulty != 3)
        {
            //HealItemSummon
            if (IsBoss)
            {
                switch (GameData.Difficulty)
                {
                    case 0:
                        for (int j = -20; j <= 20; j += 2) Instantiate(HealPrefab, direction + (transform.right * j), rot).EShot1();
                        break;
                    case 1:
                        for (int j = -20; j <= 20; j += 5) Instantiate(HealPrefab, direction + (transform.right * j), rot).EShot1();
                        break;
                    case 2:
                        for (int j = -20; j <= 20; j += 20) Instantiate(HealPrefab, direction + (transform.right * j), rot).EShot1();
                        break;
                }
            }
            else if (Random.Range(0, 3) == 0) Instantiate(HealPrefab, direction, rot).EShot1();
        }
        if (Random.Range(0, 80) == 0) Instantiate(MagicPrefab, direction, rot).EShot1();
    }

    public struct EnemyStates
    {
        public int HP;
        public int Point;
        public int MellePower;
    }

    /// <summary>
    /// 弱点かどうかを調べる　
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool GetIsCritical(int value)
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


    public void SetMuteki(bool isSet)
    {
        _isSetUpped = !isSet;
    }
}
