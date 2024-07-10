using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Text.RegularExpressions;
using System.Drawing;
using UnityEngine.SceneManagement;

public class TutorialC : MonoBehaviour
{
    private int hp = 120;

    private float time = 5;
    private float futurex;

    private int action = 0;

    private Vector3 pos, ppos;

    public SpriteRenderer spriteRenderer;
    public Sprite normal, lightning, dead;

    public EMissile1C EMissile1Prefab,ThunderP;

    private GameObject GM,Can;
    public AudioClip damageS, deadS, shotS, chargeS, effectS,_getFlag;
    public short BeamD = 2, BulletD = 1, FireD = 1, BombD = 5, ExpD = 2, RifleD = 4, MagicD = 3;

    public Text TT;

    /// <summary>
    /// 0=日本語
    /// 1=にほんご
    /// 2=English
    /// </summary>
    private int _lang = 0;

    /// <summary>
    /// チュートリアル進む
    /// </summary>
    [SerializeField]
    private int _point;
        
    private float _playerHP;

    [SerializeField]
    [Tooltip("PlayerGameObject")]
    private GameObject playerGO;

    [SerializeField]
    private FlagC _flagP;
    [SerializeField]
    private TargetC _targetP;

    [SerializeField]
    [Tooltip("アイテム")]
    private HealC HealPrefab, MagicPrefab;

    //Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        Can= GameObject.Find("Canvas");
        this.transform.parent = Can.transform;
        GM = GameObject.Find("GameManager");
        GM.GetComponent<GameManagement>()._bossNowHp = hp;
        GM.GetComponent<GameManagement>()._bossMaxHp = hp;
        _point = 0;
        playerGO = GameObject.Find("Player");
        _playerHP=GameData.HP;
    }

    // Update is called once per frame
    void Update()
    {
        ppos=playerGO.transform.position;
        pos = transform.position;

        //if(ppos.y>300) transform.position= new Vector2(320, 100);
        //else transform.position = new Vector2(320, 380);

    }

    public void Summon()
    {
        StartCoroutine(TutorialMove());
    }

    private IEnumerator TutorialMove()
    {
        _point = 0;
        Debug.Log(_point);
        TT.text = "チュートリアル";
        yield return new WaitForSeconds(3.0f);
        TT.text = "十字ボタン左右で　いどうします。";
        yield return new WaitForSeconds(3.0f);
        TT.text = "十字ボタン左右で　いどうします。\nやってみましょう。";
        GameData.PlayerMoveAble = 1;
        Instantiate(_flagP, new Vector3(100, GameData.GroundPutY((int)ppos.y / 90, 48), 0), transform.localRotation);
        Instantiate(_flagP, new Vector3(540, GameData.GroundPutY((int)ppos.y / 90, 48), 0), transform.localRotation);
        while (_point < 2)
        {
            yield return new WaitForSeconds(1.0f);
        }
        TT.text = "いいね！";
        yield return new WaitForSeconds(2.0f);

        TT.text = "十字ボタン上で　ジャンプします。";
        yield return new WaitForSeconds(3.0f);
        TT.text = "十字ボタン上で　ジャンプします。\nやってみましょう。";
        GameData.PlayerMoveAble = 2;
        Instantiate(_flagP, new Vector3(ppos.x, GameData.GroundPutY(4, 48), 0), transform.localRotation);
        while (_point < 3)
        {
            yield return new WaitForSeconds(1.0f);
        }
        TT.text = "いいね！";
        yield return new WaitForSeconds(2.0f);

        TT.text = "十字ボタン下で　おります。";
        yield return new WaitForSeconds(3.0f);
        TT.text = "十字ボタン下で　おります。\nやってみましょう。";
        GameData.PlayerMoveAble = 3;
        Instantiate(_flagP, new Vector3(ppos.x, GameData.GroundPutY(0, 48), 0), transform.localRotation);
        while (_point < 4)
        {
            yield return new WaitForSeconds(1.0f);
        }
        TT.text = "いいね！";
        yield return new WaitForSeconds(3.0f);

        TT.text = "RSボタン、LSボタンで　こうげきします。";
        yield return new WaitForSeconds(3.0f);
        TT.text = "RSボタンは、えんきょり　こうげきです。\nLSボタンは、きんきょり　こうげきです。";
        yield return new WaitForSeconds(6.0f);
        TT.text = "RSボタンは、えんきょり　こうげきです。\nLSボタンは、きんきょり　こうげきです。\nターゲットを　こうげきしてみましょう。";
        GameData.PlayerMoveAble = 4;
        Instantiate(_targetP, new Vector3(100, GameData.GroundPutY((int)ppos.y / 90, 48), 0), transform.localRotation);
        Instantiate(_targetP, new Vector3(320, GameData.GroundPutY((int)ppos.y / 90, 48), 0), transform.localRotation);
        Instantiate(_targetP, new Vector3(540, GameData.GroundPutY((int)ppos.y / 90, 48), 0), transform.localRotation);
        while (_point < 7)
        {
            yield return new WaitForSeconds(1.0f);
        }
        TT.text = "いいね！";
        yield return new WaitForSeconds(3.0f);

        TT.text = "このゲームでは　ぶきを　チェンジすることができます。\nチェンジには　ABXYボタン（〇×△□ボタン）を\nつかいます。";
        GameData.PlayerMoveAble = 5;
        yield return new WaitForSeconds(10.0f);
        TT.text = "ボタンごとに　ぶきが　決まっています。\nいろいろな　ぶきで　こうげきを　出してみましょう。";
        Instantiate(_targetP, new Vector3(100, GameData.GroundPutY(3, 48), 0), transform.localRotation);
        Instantiate(_targetP, new Vector3(100, GameData.GroundPutY(1, 48), 0), transform.localRotation);
        Instantiate(_targetP, new Vector3(540, GameData.GroundPutY(3, 48), 0), transform.localRotation);
        Instantiate(_targetP, new Vector3(540, GameData.GroundPutY(1, 48), 0), transform.localRotation);
        while (_point < 11)
        {
            yield return new WaitForSeconds(1.0f);
        }
        TT.text = "いいね！";
        yield return new WaitForSeconds(3.0f);
        TT.text = "てきごとに　こうかばつぐんの　ぶきや、\n効かない　ぶきが　あります。\nてきに合った　ぶきを　つかいましょう。";

        Instantiate(_targetP, new Vector3(100, GameData.GroundPutY(0, 48), 0), transform.localRotation).Changeritical(true,false,false,false);
        Instantiate(_targetP, new Vector3(100, GameData.GroundPutY(1, 48), 0), transform.localRotation).Changeritical(false, true, false, false);
        Instantiate(_targetP, new Vector3(100, GameData.GroundPutY(2, 48), 0), transform.localRotation).Changeritical(false, false, true, false);
        Instantiate(_targetP, new Vector3(100, GameData.GroundPutY(3, 48), 0), transform.localRotation).Changeritical(false, false, false, true);
        while (_point < 15)
        {
            yield return new WaitForSeconds(1.0f);
        }
        TT.text = "いいね！";
        yield return new WaitForSeconds(3.0f);

        TT.text = "おっと！雷が！";
        for (int i = 0; i < 6; i++)
        {
            Instantiate(ThunderP, new Vector3(ppos.x, 240, 0), transform.localRotation).EShot1(270, 120, 1000);
            yield return new WaitForSeconds(0.3f);
        }
        TT.text = "ダメージを　うけました！\nＨＰが　ゼロになると　ゲームオーバーです。";
        yield return new WaitForSeconds(5.0f);
        TT.text = "へったＨＰは　ハートで　かいふくできます。\nハートのところまで行き、かいふくしましょう。";
        Instantiate(HealPrefab, new Vector3(100, GameData.GroundPutY((int)ppos.y / 90, 48), 0), transform.localRotation);
        Instantiate(HealPrefab, new Vector3(540, GameData.GroundPutY((int)ppos.y / 90, 48), 0), transform.localRotation);
        while (GameData.HP<_playerHP)
        {
            yield return new WaitForSeconds(1.0f);
        }
        TT.text = "いいね！";
        yield return new WaitForSeconds(3.0f);

        TT.text = "アイテムをとると　きょうりょくな　ひっさつわざが　つかえます！";
        yield return new WaitForSeconds(3.0f);
        TT.text = "ほしが　あるじょうたいで　LTボタンとRTボタンをおすと\nひっさつわざが　つかえます。";
        yield return new WaitForSeconds(8.0f);
        TT.text = "ほしが　あるじょうたいで　LTボタンとRTボタンをおすと\nひっさつわざが　つかえます。\nアイテムをとり、ひっさつわざを　うってみましょう。";
        GameData.PlayerMoveAble = 6;
        Instantiate(MagicPrefab, new Vector3(540, GameData.GroundPutY(2, 48), 0), transform.localRotation);
        for(int i = 80; i < 640; i += 80)
        {
            Instantiate(_targetP, new Vector3(i, GameData.GroundPutY(3, 48), 0), transform.localRotation);
        }
        for (int i = 80; i < 640; i += 80)
        {
            Instantiate(_targetP, new Vector3(i, GameData.GroundPutY(1, 48), 0), transform.localRotation);
        }
        while (_point < 29)
        {
            yield return new WaitForSeconds(1.0f);
        }
        TT.text = "いいね！\nひっさつわざは　ぶきによって　かわります。";
        yield return new WaitForSeconds(3.0f);

        TT.text = "てきをたおすと　スコアがたまります。";
        yield return new WaitForSeconds(3.0f);
        TT.text = "てきをたおすと　スコアがたまります。\nスコアがたまったら　ラウンドがすすみ、よりつよいてきが　でてきます。";
        yield return new WaitForSeconds(7.0f);
        TT.text = "これで　チュートリアルは　おわりです。\nそれでは、がんばってくださいね！\nけんとうを　いのります。";
        yield return new WaitForSeconds(3.0f);

        GameData.Boss = 0;
        Destroy(gameObject);
        SceneManager.LoadScene("Title");
    }

    void FixedUpdate()
    {

    }

    public float GetAngle(Vector2 direction)
    {
        float rad = Mathf.Atan2(direction.y, direction.x);
        return rad * Mathf.Rad2Deg;
    }

    private void Damage(int hit)
    {
        hp -= hit;
        GM.GetComponent<GameManagement>()._bossNowHp = hp;
        if (hp <= 0)
        {
            spriteRenderer.sprite = dead;
        }
        GameObject.FindObjectOfType<AudioSource>().PlayOneShot(damageS);
    }

    public void GoTutorial()
    {
        GameObject.FindObjectOfType<AudioSource>().PlayOneShot(_getFlag);
        _point++;
        Debug.Log(_point);
       
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PBeam")
        {
            Damage(BeamD);
        }
        if (collision.gameObject.tag == "PBullet")
        {
            Damage(BulletD);
        }
        if (collision.gameObject.tag == "PFire")
        {
            Damage(FireD);
        }
        if (collision.gameObject.tag == "PBomb")
        {
            Damage(BombD);
        }
        if (collision.gameObject.tag == "PExp")
        {
            Damage(ExpD);
        }
        if (collision.gameObject.tag == "PRifle")
        {
            Damage(RifleD);
        }
        if (collision.gameObject.tag == "PMagic")
        {
            Damage(MagicD);
        }
    }
}
