using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class ECoreC : MonoBehaviour
{
    [NonSerialized]
    public bool IsBoss;

    [NonSerialized]
    public int TotalHp=0;

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
    public short EvoltionMode=0;

    [SerializeField]
    [Tooltip("第一形態HP")]
    public int[] hp= {10};

    [SerializeField]
    [Tooltip("撃破時の獲得スコア")]
    private int score;

    private Vector3 pos;

    private Quaternion rot;

    [SerializeField]
    [Tooltip("死ぬときの音")]
    private AudioClip deadS;

    private bool death,deathStarted;

    [SerializeField]
    [Tooltip("被ダメージ")]
    private int[] BeamD = { 2 }, BulletD = { 1 }, FireD = { 1 }, BombD = { 5 };

    [SerializeField]
    [Tooltip("クリティカル発生")]
    private bool[] _isBeamCritical = { false }, _isBulletCritical = { false }, _isFireCritical = { false }, _isBombCritical = { false };

    [SerializeField]
    [Tooltip("アイテム")]
    private HealC HealPrefab, MagicPrefab;

    private PMCoreC playerMissileP;

    // Start is called before the first frame update
    void Start()
    {
        TotalHp = hp.Sum();
    }

    // Update is called once per frame
    void Update()
    {
        TotalHp = hp.Sum();
        pos = transform.position;
        rot = transform.localRotation;

        if (death&&!deathStarted)
        {
            SummonItems();
            GameData.Score += score;
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(deadS);
            if (IsBoss) BossLifeMode = 2;
            else Destroy(gameObject);
            deathStarted = true;

        }

    }
    
    private void Damage(int hit,GameObject PMissile,bool isCritical,Vector3 attackPos)
    {
        hp[EvoltionMode] -= hit;
        if (hit > 0)
        {
            if (hp[EvoltionMode] <= 0)
            {
                hp[EvoltionMode] = 0;
                if (EvoltionMode>=hp.Length-1)
                {
                    death = true;
                }
                else
                {
                    EvoltionMode++;
                }
            }

        }
        playerMissileP = PMissile.GetComponent<PMCoreC>();
        if(hit>0)
        {
            playerMissileP.DamageEffect(true/*isCritical*/,attackPos);
        }
        else
        {
            playerMissileP.InvalidEffect(attackPos);
        }
    }

    /// <summary>
    /// アイテム生成
    /// </summary>
    public void SummonItems()
    {
        Vector3 direction = new Vector3(pos.x, GameData.GroundPutY((int)pos.y/90, 30), 0);
        if (GameData.Difficulty != 3)
        {
            //HealItemSummon
            if (IsBoss)
            {
                for (int j = -20; j <= 20; j += 20)
                {
                    Instantiate(HealPrefab, direction + (transform.right * j), rot).EShot1();
                }
            }
            else if (Random.Range(0, 3) == 0) Instantiate(HealPrefab, direction, rot).EShot1();
        }
        if (Random.Range(0, 100) == 0) Instantiate(MagicPrefab, direction, rot).EShot1();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 hitPos = collision.ClosestPoint(this.transform.position);
        if (collision.gameObject.tag == "PBeam")
        {
            Damage(BeamD[EvoltionMode], collision.gameObject,_isBeamCritical[EvoltionMode], hitPos);
        }
        if (collision.gameObject.tag == "PBullet")
        {
            Damage(BulletD[EvoltionMode], collision.gameObject,_isBulletCritical[EvoltionMode], hitPos);
        }
        if (collision.gameObject.tag == "PFire")
        {
            Damage(FireD[EvoltionMode], collision.gameObject, _isFireCritical[EvoltionMode], hitPos);
        }
        if (collision.gameObject.tag == "PBomb")
        {
            Damage(BombD[EvoltionMode], collision.gameObject, _isBombCritical[EvoltionMode], hitPos);
        }
        if (collision.gameObject.tag == "PExp")
        {
            Damage(BombD[EvoltionMode]/2, collision.gameObject, _isBombCritical[EvoltionMode], hitPos);
        }
    }
}
