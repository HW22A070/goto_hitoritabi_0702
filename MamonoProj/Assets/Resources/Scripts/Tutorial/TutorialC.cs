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

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    public AudioClip damageS, deadS, shotS, chargeS, effectS,_getFlag;
    public short BeamD = 2, BulletD = 1, FireD = 1, BombD = 5, ExpD = 2, RifleD = 4, MagicD = 3;

    public Text TT;

    /// <summary>
    /// テキストの進み具合
    /// </summary>
    private int _textLevel=0;

    readonly string[]
        _jp = { "チュートリアル"
            , "十字ボタン左右で　移動します。"
            , "十字ボタン左右で　移動します\nやってみましょう。"
            ,"いいね！"
            ,"十字ボタン上で　ジャンプします。"
            ,"十字ボタン上で　ジャンプします。\nやってみましょう。"
            ,"いいね！"
            ,"十字ボタン下で　降ります。"
            ,"十字ボタン下で　降ります。\nやってみましょう。"
            ,"いいね！"
            ,"R1ボタン、L1ボタンで　攻撃します。"
            ,"R1ボタンは、遠距離攻撃です。\nL1ボタンは、近距離攻撃です。"
            ,"R1ボタンは、遠距離攻撃です。\nL1ボタンは、近距離攻撃です。\nターゲットを　攻撃してみましょう。"
            ,"いいね！"
            ,"このゲームでは　武器を　チェンジすることができます。\nチェンジには　ABXYボタン（〇×△□ボタン）を\n使います。"
            ,"ボタンごとに　武器が　決まっています。\n色々な　武器で　攻撃を出してみましょう。"
            ,"いいね！"
            ,"敵ごとに　効果の高いの武器や、\n効かない　武器が　あります。\n敵に合った　武器を　使いましょう。"
            ,"いいね！"
            ,"おっと！雷が！"
            ,"ダメージを　受けました！\nＨＰが　ゼロになると　ゲームオーバーです。"
            ,"減ったＨＰは　ハートで　回復できます。\nハートのところまで行き、回復しましょう。"
            ,"いいね！"
            ,"「バッテリー」を取ると　強力な必殺技が　使えます！"
            ,"電力が　ある状態で　L2ボタンとR2ボタンを押すと\n必殺技が　使えます。"
            ,"電力が　ある状態で　L2ボタンとR2ボタンを押すと\n必殺技が　使えます。\nバッテリーを取り、必殺技を　放ってみましょう。"
            ,"いいね！\n必殺技は　武器によって　変わります。"
            ,"敵を倒すと　ポイントがたまります。"
            ,"敵を倒すと　ポイントがたまります。\nポイントがたまったら　ラウンドが進み、より強力な敵が　出てきます。"
            ,"これで　チュートリアルは　終了です。\nそれでは、頑張ってくださいね！\n健闘を祈ります。" },

        _jp_kids = { "チュートリアル"
            , "じゅうじボタンの　さゆうで　いどうします。"
            , "じゅうじボタンの　さゆうで　いどうします。\nやってみましょう。"
            ,"いいね！"
            ,"じゅうじボタンの　うえで　ジャンプします。"
            ,"じゅうじボタンの　うえで　ジャンプします。\nやってみましょう。"
            ,"いいね！"
            ,"じゅうじボタンの　したで　おります。"
            ,"じゅうじボタンの　したで　おります。\nやってみましょう。"
            ,"いいね！"
            ,"R1ボタン、L1ボタンで　こうげきします。"
            ,"R1ボタンは、えんきょり　こうげきです。\nL1ボタンは、きんきょり　こうげきです。"
            ,"R1ボタンは、えんきょり　こうげきです。\nL1ボタンは、きんきょり　こうげきです。\nターゲットを　こうげきしてみましょう。"
            ,"いいね！"
            ,"このゲームでは　ぶきを　チェンジすることができます。\nチェンジには　ABXYボタン（〇×△□ボタン）を\nつかいます。"
            ,"ボタンごとに　ぶきが　きまっています。\nいろいろな　ぶきで　こうげきを　だしてみましょう。"
            ,"いいね！"
            ,"てきごとに　こうかばつぐんの　ぶきや、\nきかない　ぶきが　あります。\nてきにあった　ぶきを　つかいましょう。"
            ,"いいね！"
            ,"おっと！カミナリが！"
            ,"ダメージを　うけました！\nＨＰが　ゼロになると　ゲームオーバーです。"
            ,"へったＨＰは　ハートで　かいふくできます。\nハートのところまでいき、かいふくしましょう。"
            ,"いいね！"
            ,"「バッテリー」をとると　きょうりょくな　ひっさつわざが　つかえます！"
            ,"でんりょくが　あるじょうたいで　L2ボタンとR2ボタンをおすと\nひっさつわざが　つかえます。"
            ,"でんりょくが　あるじょうたいで　L2ボタンとR2ボタンをおすと\nひっさつわざが　つかえます。\nアイテムをとり、ひっさつわざを　うってみましょう。"
            ,"いいね！\nひっさつわざは　ぶきによって　かわります。"
            ,"てきをたおすと　ポイントがたまります。"
            ,"てきをたおすと　ポイントがたまります。\nスコアがたまったら　ラウンドがすすみ、よりつよいてきが　でてきます。"
            ,"これで　チュートリアルは　おわりです。\nそれでは、がんばってくださいね！\nけんとうを　いのります。"},

        _en = { "Tutorial"
            , "Move left or right using the directional pad."
            , "Move left or right using the directional pad.\nLet's give it a try."
            ,"Nice!"
            ,"Press up on the D-pad to jump."
            ,"Press up on the D-pad to jump.\nLet's try it."
            ,"Nice!"
            ,"Press the down button on the directional pad to descend."
            ,"Press the down button on the directional pad to descend.\nLet's give it a try"
            ,"Nice!"
            ,"Use the R1 and L1 buttons to attack."
            ,"The R1 button is a long-range attack.\nThe L1 button is a close-range attack."
            ,"The R1 button is a long-range attack.\nThe L1 button is a close-range attack.\nLet's attack the target."
            ,"Nice!"
            ,"In this game you can change weapons.\nTo change, use the ABXY buttons (〇×△□ buttons)."
            ,"Each button has a specific weapon. Try attacking with various weapons."
            ,"Nice!"
            ,"Each enemy has weapons that are effective and weapons that are ineffective. Use the weapon that suits the enemy."
            ,"Nice!"
            ,"Oops! Lightning!"
            ,"You've taken damage! When your HP reaches zero, the game is over."
            ,"Lost HP can be restored with hearts. \nGo to the heart and recover."
            ,"Nice!"
            ,"When you collect a \"battery\" you can use powerful special moves!"
            ,"When you have power, press the L2 and R2 buttons to use special moves."
            ,"When you have power, press the L2 and R2 buttons to use special moves.\nPick up the battery and try your hand at a special move."
            ,"Nice! \nSpecial moves change depending on the weapon."
            ,"You earn points by defeating enemies."
            ,"You earn points by defeating enemies.\nAs you accumulate points, the round will progress and more powerful enemies will appear."
            ,"This is the end of the tutorial. \nGood luck!" };

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

    private AudioControlC _bgmManager;

    //Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        _bgmManager = GameObject.Find("BGMManager").GetComponent<AudioControlC>();
        //_bgmManager.ChangeAudio(-1, false, -1);

        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        Can = GameObject.Find("Canvas");
        this.transform.parent = Can.transform;
        GM = GameObject.Find("GameManager");
        GM.GetComponent<GameManagement>()._bossNowHp = hp;
        GM.GetComponent<GameManagement>()._bossMaxHp = hp;
        _point = 0;
        _textLevel = 0;
        playerGO = GameObject.Find("Player");
        _playerHP=GameData.HP;
        StartCoroutine(TutorialMove());
    }

    // Update is called once per frame
    void Update()
    {
        ppos=playerGO.transform.position;
        pos = transform.position;

        //if(ppos.y>300) transform.position= new Vector2(320, 100);
        //else transform.position = new Vector2(320, 380);
        switch (GameData.Language)
        {
            case 0:
                TT.text = _jp[_textLevel];
                break;
            case 1:
                TT.text = _jp_kids[_textLevel];
                break;
            case 2:
                TT.text = _en[_textLevel];
                break;
        }
        
    }

    private IEnumerator TutorialMove()
    {
        _point = 0;
        yield return new WaitForSeconds(3.0f);
        _textLevel++ ;
        yield return new WaitForSeconds(3.0f);
        _textLevel++;
        GameData.PlayerMoveAble = 1;
        Instantiate(_flagP, new Vector3(100, GameData.GroundPutY((int)ppos.y / 90, 48), 0), transform.localRotation);
        Instantiate(_flagP, new Vector3(540, GameData.GroundPutY((int)ppos.y / 90, 48), 0), transform.localRotation);
        while (_point < 2)
        {
            yield return new WaitForSeconds(1.0f);
        }
        _textLevel++ ;
        yield return new WaitForSeconds(2.0f);

        _textLevel++ ;
        yield return new WaitForSeconds(3.0f);
        _textLevel++ ;
        GameData.PlayerMoveAble = 2;
        Instantiate(_flagP, new Vector3(ppos.x, GameData.GroundPutY(4, 48), 0), transform.localRotation);
        while (_point < 3)
        {
            yield return new WaitForSeconds(1.0f);
        }
        _textLevel++ ;
        yield return new WaitForSeconds(2.0f);

        _textLevel++ ;
        yield return new WaitForSeconds(3.0f);
        _textLevel++ ;
        GameData.PlayerMoveAble = 3;
        Instantiate(_flagP, new Vector3(ppos.x, GameData.GroundPutY(0, 48), 0), transform.localRotation);
        while (_point < 4)
        {
            yield return new WaitForSeconds(1.0f);
        }
        _textLevel++ ;
        yield return new WaitForSeconds(3.0f);

        _textLevel++ ;
        GameData.PlayerMoveAble = 4;
        yield return new WaitForSeconds(3.0f);
        _textLevel++ ;
        yield return new WaitForSeconds(6.0f);
        _textLevel++ ;
        Instantiate(_targetP, new Vector3(100, GameData.GroundPutY((int)ppos.y / 90, 48), 0), transform.localRotation);
        Instantiate(_targetP, new Vector3(320, GameData.GroundPutY((int)ppos.y / 90, 48), 0), transform.localRotation);
        Instantiate(_targetP, new Vector3(540, GameData.GroundPutY((int)ppos.y / 90, 48), 0), transform.localRotation);
        while (_point < 7)
        {
            yield return new WaitForSeconds(1.0f);
        }
        _textLevel++ ;
        yield return new WaitForSeconds(3.0f);

        _textLevel++;
        GameData.PlayerMoveAble = 5;
        yield return new WaitForSeconds(10.0f);
        _textLevel++ ;
        Instantiate(_targetP, new Vector3(100, GameData.GroundPutY(3, 48), 0), transform.localRotation);
        Instantiate(_targetP, new Vector3(100, GameData.GroundPutY(1, 48), 0), transform.localRotation);
        Instantiate(_targetP, new Vector3(540, GameData.GroundPutY(3, 48), 0), transform.localRotation);
        Instantiate(_targetP, new Vector3(540, GameData.GroundPutY(1, 48), 0), transform.localRotation);
        while (_point < 11)
        {
            yield return new WaitForSeconds(1.0f);
        }
        _textLevel++ ;
        yield return new WaitForSeconds(3.0f);
        _textLevel++ ;

        Instantiate(_targetP, new Vector3(100, GameData.GroundPutY(0, 48), 0), transform.localRotation).Changeritical(true,false,false,false);
        Instantiate(_targetP, new Vector3(100, GameData.GroundPutY(1, 48), 0), transform.localRotation).Changeritical(false, true, false, false);
        Instantiate(_targetP, new Vector3(100, GameData.GroundPutY(2, 48), 0), transform.localRotation).Changeritical(false, false, true, false);
        Instantiate(_targetP, new Vector3(100, GameData.GroundPutY(3, 48), 0), transform.localRotation).Changeritical(false, false, false, true);
        while (_point < 15)
        {
            yield return new WaitForSeconds(1.0f);
        }
        _textLevel++ ;
        yield return new WaitForSeconds(3.0f);

        _textLevel++ ;
        for (int i = 0; i < 6; i++)
        {
            Instantiate(ThunderP, new Vector3(ppos.x, 240, 0), transform.localRotation).EShot1(270, 120, 1000);
            yield return new WaitForSeconds(0.3f);
        }
        _textLevel++ ;
        yield return new WaitForSeconds(5.0f);
        _textLevel++ ;
        Instantiate(HealPrefab, new Vector3(100, GameData.GroundPutY((int)ppos.y / 90, 48), 0), transform.localRotation);
        Instantiate(HealPrefab, new Vector3(540, GameData.GroundPutY((int)ppos.y / 90, 48), 0), transform.localRotation);
        while (GameData.HP<_playerHP)
        {
            yield return new WaitForSeconds(1.0f);
        }
        _textLevel++ ;
        yield return new WaitForSeconds(3.0f);

        _textLevel++ ;
        yield return new WaitForSeconds(3.0f);
        _textLevel++;
        yield return new WaitForSeconds(8.0f);
        _textLevel++ ;
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
        _textLevel++ ;
        yield return new WaitForSeconds(3.0f);

        _textLevel++ ;
        yield return new WaitForSeconds(3.0f);
        _textLevel++ ;
        yield return new WaitForSeconds(7.0f);
        _textLevel++ ;
        yield return new WaitForSeconds(3.0f);

        GameData.Boss = 0;
        Destroy(gameObject);
        SceneManager.LoadScene("Title");
    }

    void FixedUpdate()
    {

    }

    public void GoTutorial()
    {
        _audioGO.PlayOneShot(_getFlag);
        _point++;
       
    }
}
