using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TutorialC : MonoBehaviour
{
    private int hp = 120;

    private float time = 5;
    private float futurex;

    private int action = 0;

    private Vector3 _posOwn, _posPlayer;

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

    private string[] _texts;

    private string[,]
        _ctrMove = new string[3, 2]{
            {"十字ボタン左右","Aキー、Dキー"},
            { "じゅうじボタンの　さゆう","Aキー、Dキー"},
            {"left and right on the D-pad" ,"A key, D key"}
        },

    _ctrJump = new string[3, 2]{
            {"十字ボタン上","Wキー"},
            { "じゅうじボタンの　うえ","Wキー"},
            {"up on the D-pad" ,"W key"}
        },

    _ctrDown = new string[3, 2]{
            {"十字ボタン下","Sキー"},
            { "じゅうじボタンの　した","Sキー"},
            {"down on the D-pad" ,"S key"}
        },

              _ctrFire1 = new string[3, 2]{
            {"L1ボタン","左クリック"},
            { "L1ボタン","ひだりクリック"},
            {"L1" ,"left click"}
        },

    _ctrFire2 = new string[3, 2]{
            {"R1ボタン","右クリック"},
            { "R1ボタン","みぎクリック"},
            {"R1" ,"right click"}
        },

            _ctrTransfomation = new string[3, 2]{
            {"ABXYボタン（〇×△□ボタン）","マウスホイール"},
            { "ABXYボタン（〇×△□ボタン）","マウスホイール"},
            {"the ABXY buttons (〇×△□ buttons)" ,"mouse wheel"}
        },

                    _ctrTransfomationHow = new string[3, 2]{
            {"ボタン","ホイールの回転"},
            { "ボタン","ホイールのかいてん"},
            {"Each button" ,"wheel rotation"}
        },

    _ctrSpecial = new string[3, 2]{
            {"L2ボタンとR2ボタン","ミドルクリック"},
            { "L2ボタンとR2ボタン","ミドルクリック"},
            {"the L2 and R2 buttons" ,"middle click"}
        };

/// <summary>
/// チュートリアル進む
/// </summary>
[SerializeField]
    private int _point;
        
    private float _playerHP;

    [SerializeField]
    [Tooltip("PlayerGameObject")]
    private GameObject _goPlayer;

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
        Can = GameObject.Find("UIs");
        this.transform.parent = Can.transform;
        GM = GameObject.Find("GameManager");
        GM.GetComponent<GameManagement>()._bossNowHp = hp;
        GM.GetComponent<GameManagement>()._bossMaxHp = hp;
        _point = 0;
        _textLevel = 0;
        _goPlayer = GameObject.FindWithTag("Player");
        _playerHP=GameData.GetMaxHP();


        _texts = SetTextByController();
        StartCoroutine(MoveTutorial());
    }

    // Update is called once per frame
    void Update()
    {
        _posPlayer=_goPlayer.transform.position;
        _posOwn = transform.position;
    }

    private IEnumerator MoveTutorial()
    {
        _point = 0;
        TT.text = _texts[_textLevel];
        yield return new WaitForSeconds(3.0f);

        yield return StartCoroutine(SetNextTextAndDelay(3.0f));

        SetNextText();
        GameData.PlayerMoveAble = 1;
        Instantiate(_flagP, new Vector3(100, GameData.GetGroundPutY((int)_posPlayer.y / 90, 48), 0), transform.localRotation);
        Instantiate(_flagP, new Vector3(540, GameData.GetGroundPutY((int)_posPlayer.y / 90, 48), 0), transform.localRotation);
        while (_point < 2)
        {
            yield return new WaitForSeconds(1.0f);
        }

        yield return StartCoroutine(SetNextTextAndDelay(2.0f));
        yield return StartCoroutine(SetNextTextAndDelay(3.0f));

        SetNextText();
        GameData.PlayerMoveAble = 2;
        Instantiate(_flagP, new Vector3(_posPlayer.x, GameData.GetGroundPutY(4, 48), 0), transform.localRotation);
        while (_point < 3)
        {
            yield return new WaitForSeconds(1.0f);
        }

        yield return StartCoroutine(SetNextTextAndDelay(2.0f));
        yield return StartCoroutine(SetNextTextAndDelay(3.0f));


        SetNextText();
        GameData.PlayerMoveAble = 3;
        Instantiate(_flagP, new Vector3(_posPlayer.x, GameData.GetGroundPutY(0, 48), 0), transform.localRotation);
        while (_point < 4)
        {
            yield return new WaitForSeconds(1.0f);
        }

        yield return StartCoroutine(SetNextTextAndDelay(3.0f));
        yield return StartCoroutine(SetNextTextAndDelay(3.0f));
        yield return StartCoroutine(SetNextTextAndDelay(3.0f));

        GameData.PlayerMoveAble = 4;
        SetNextText();
        Instantiate(_targetP, new Vector3(100, GameData.GetGroundPutY((int)_posPlayer.y / 90, 48), 0), transform.localRotation);
        Instantiate(_targetP, new Vector3(320, GameData.GetGroundPutY((int)_posPlayer.y / 90, 48), 0), transform.localRotation);
        Instantiate(_targetP, new Vector3(540, GameData.GetGroundPutY((int)_posPlayer.y / 90, 48), 0), transform.localRotation);
        while (_point < 7)
        {
            yield return new WaitForSeconds(1.0f);
        }

        yield return StartCoroutine(SetNextTextAndDelay(3.0f));

        GameData.PlayerMoveAble = 5;
        yield return StartCoroutine(SetNextTextAndDelay(10.0f));


        SetNextText();
        Instantiate(_targetP, new Vector3(100, GameData.GetGroundPutY(3, 48), 0), transform.localRotation);
        Instantiate(_targetP, new Vector3(100, GameData.GetGroundPutY(1, 48), 0), transform.localRotation);
        Instantiate(_targetP, new Vector3(540, GameData.GetGroundPutY(3, 48), 0), transform.localRotation);
        Instantiate(_targetP, new Vector3(540, GameData.GetGroundPutY(1, 48), 0), transform.localRotation);
        while (_point < 11)
        {
            yield return new WaitForSeconds(1.0f);
        }

        yield return StartCoroutine(SetNextTextAndDelay(3.0f));

        SetNextText();

        Instantiate(_targetP, new Vector3(100, GameData.GetGroundPutY(0, 48), 0), transform.localRotation).Changeritical(true,false,false,false);
        Instantiate(_targetP, new Vector3(100, GameData.GetGroundPutY(1, 48), 0), transform.localRotation).Changeritical(false, true, false, false);
        Instantiate(_targetP, new Vector3(100, GameData.GetGroundPutY(2, 48), 0), transform.localRotation).Changeritical(false, false, true, false);
        Instantiate(_targetP, new Vector3(100, GameData.GetGroundPutY(3, 48), 0), transform.localRotation).Changeritical(false, false, false, true);
        while (_point < 15)
        {
            yield return new WaitForSeconds(1.0f);
        }

        yield return StartCoroutine(SetNextTextAndDelay(3.0f));


        SetNextText();
        for (int i = 0; i < 6; i++)
        {
            Instantiate(ThunderP, new Vector3(_posPlayer.x, 240, 0), transform.localRotation).ShotMissile(270, 120, 1000);
            yield return new WaitForSeconds(0.3f);
        }

        yield return StartCoroutine(SetNextTextAndDelay(5.0f));


        SetNextText();
        Instantiate(HealPrefab, new Vector3(100, GameData.GetGroundPutY((int)_posPlayer.y / 90, 48), 0), transform.localRotation);
        Instantiate(HealPrefab, new Vector3(540, GameData.GetGroundPutY((int)_posPlayer.y / 90, 48), 0), transform.localRotation);
        while (_goPlayer.GetComponent<PlayerC>().GetHP()<_playerHP)
        {
            yield return new WaitForSeconds(1.0f);
        }

        yield return StartCoroutine(SetNextTextAndDelay(3.0f));

        Instantiate(MagicPrefab, new Vector3(320, GameData.GetGroundPutY(3, 32), 0), transform.localRotation);
        yield return StartCoroutine(SetNextTextAndDelay(2.0f));

        yield return StartCoroutine(SetNextTextAndDelay(3.0f));
        yield return StartCoroutine(SetNextTextAndDelay(8.0f));


        SetNextText();
        GameData.PlayerMoveAble = 6;
        for(int i = 80; i < 640; i += 80)
        {
            Instantiate(_targetP, new Vector3(i, GameData.GetGroundPutY(3, 48), 0), transform.localRotation);
        }
        for (int i = 80; i < 640; i += 80)
        {
            Instantiate(_targetP, new Vector3(i, GameData.GetGroundPutY(1, 48), 0), transform.localRotation);
        }
        while (_point < 29)
        {
            yield return new WaitForSeconds(1.0f);
        }

        yield return StartCoroutine(SetNextTextAndDelay(3.0f));
        yield return StartCoroutine(SetNextTextAndDelay(3.0f));
        yield return StartCoroutine(SetNextTextAndDelay(7.0f));
        yield return StartCoroutine(SetNextTextAndDelay(3.0f));


        GameData.IsBossFight = false;
        Destroy(gameObject);
        SceneManager.LoadScene("Title");
    }

    private void SetNextText()
    {
        _textLevel++;
        TT.text = _texts[_textLevel];
    }

    public void GoToTutorial()
    {
        _audioGO.PlayOneShot(_getFlag);
        _point++;
       
    }

    private IEnumerator SetNextTextAndDelay(float delayTime)
    {
        SetNextText();
        yield return new WaitForSeconds(delayTime);
    }


    private string[] SetTextByController()
    {
        int key = GameData.IsJoyStick1P ? 0 : 1;

        switch (GameData.Language)
        {
            case 0:
                return new List<string>
                { "チュートリアル"
            , _ctrMove[GameData.Language,key]+"で　移動します。"
            , _ctrMove[GameData.Language,key]+"で　移動します\nやってみましょう。"
            ,"いいね！"
            ,_ctrJump[GameData.Language,key]+"で　ジャンプします。"
            ,_ctrJump[GameData.Language,key]+"で　ジャンプします。\nやってみましょう。"
            ,"いいね！"
            ,_ctrDown[GameData.Language,key]+"で　降ります。"
            ,_ctrDown[GameData.Language,key]+"で　降ります。\nやってみましょう。"
            ,"いいね！"
            ,_ctrFire2[GameData.Language,key]+"、"+_ctrFire1[GameData.Language,key]+"で　攻撃します。"
            ,_ctrFire2[GameData.Language,key]+"は、遠距離攻撃です。\n"+_ctrFire1[GameData.Language,key]+"は、近距離攻撃です。"
            ,_ctrFire2[GameData.Language,key]+"は、遠距離攻撃です。\n"+_ctrFire1[GameData.Language,key]+"は、近距離攻撃です。\nターゲットを　攻撃してみましょう。"
            ,"その調子！"
            ,"このゲームでは　武器を　チェンジすることができます。\nチェンジには　"+_ctrTransfomation[GameData.Language,key]+"を\n使います。"
            ,_ctrTransfomationHow[GameData.Language,key]+"ごとに　武器が　決まっています。\n色々な　武器で　攻撃を出してみましょう。"
            ,"その調子！"
            ,"敵ごとに　効果の高いの武器や、\n効かない　武器が　あります。\n敵に合った　武器を　使いましょう。"
            ,"素晴らしい！\nこれで本ゲームの基本操作はバッチリ！"
            ,"おっと！雷が！"
            ,"ダメージを　受けました！\nＨＰが　ゼロになると　ゲームオーバーです。"
            ,"減ったＨＰは　ハートで　回復できます。\nハートのところまで行き、回復しましょう。"
            ,"いいね！"
            ,"おや…これは何でしょうか。\nおっと！これは「バッテリー」ですね。"
            ,"「バッテリー」を取ると　自らを一定時間、超強化することができます！"
            ,"電力が　ある状態で　"+_ctrSpecial[GameData.Language,key]+"を押すと\n発動します。"
            ,"電力が　ある状態で　"+_ctrSpecial[GameData.Language,key]+"を押すと\n発動します。\nバッテリーを取得し、強化を用いて”暴れて”みましょう！"
            ,"クール！"
            ,"敵を倒すと　ポイントがたまります。"
            ,"敵を倒すと　ポイントがたまります。\nポイントがたまったら　ラウンドが進み、より強力な敵が　出てきます。"
            ,"これで　チュートリアルは　終了です。\nそれでは、頑張ってくださいね！\n健闘を祈ります。" }.ToArray();

            case 1:
                return new List<string>
                { "チュートリアル"
            , _ctrMove[GameData.Language,key]+"で　いどうします。"
            , _ctrMove[GameData.Language,key]+"で　いどうします。\nやってみましょう。"
            ,"いいね！"
            ,_ctrJump[GameData.Language,key]+"で　ジャンプします。"
            ,_ctrJump[GameData.Language,key]+"で　ジャンプします。\nやってみましょう。"
            ,"いいね！"
            ,_ctrDown[GameData.Language,key]+"で　おります。"
            ,_ctrDown[GameData.Language,key]+"で　おります。\nやってみましょう。"
            ,"いいね！"
            ,_ctrFire2[GameData.Language,key]+"、"+_ctrFire1[GameData.Language,key]+"で　こうげきします。"
            ,_ctrFire2[GameData.Language,key]+"は、えんきょり　こうげきです。\n"+_ctrFire1[GameData.Language,key]+"は、きんきょり　こうげきです。"
            ,_ctrFire2[GameData.Language,key]+"は、えんきょり　こうげきです。\n"+_ctrFire1[GameData.Language,key]+"は、きんきょり　こうげきです。\nターゲットを　こうげきしてみましょう。"
            ,"そのちょうし！"
            ,"このゲームでは　ぶきを　チェンジすることができます。\nチェンジには　"+_ctrTransfomation[GameData.Language,key]+"を\nつかいます。"
            ,_ctrTransfomationHow[GameData.Language,key]+"ごとに　ぶきが　きまっています。\nいろいろな　ぶきで　こうげきを　だしてみましょう。"
            ,"そのちょうし！"
            ,"てきごとに　こうかばつぐんの　ぶきや、\nきかない　ぶきが　あります。\nてきにあった　ぶきを　つかいましょう。"
            ,"すばらしい！\nこれで　このゲームのそうさは　バッチリ！"
            ,"おっと！カミナリが！"
            ,"ダメージを　うけました！\nＨＰが　ゼロになると　ゲームオーバーです。"
            ,"へったＨＰは　ハートで　かいふくできます。\nハートのところまでいき、かいふくしましょう。"
            ,"いいね！"
            ,"おや…これは　なにでしょうか。\nおっと！これは「バッテリー」ですね。"
            ,"「バッテリー」をとると　しばらくのあいだ　パワーアップします！"
            ,"でんりょくが　あるじょうたいで　"+_ctrSpecial[GameData.Language,key]+"をおすと\nつかえます。"
            ,"でんりょくが　あるじょうたいで　"+_ctrSpecial[GameData.Language,key]+"をおすと\nつかえます。\nバッテリーをとり、パワーアップした　こうげきをつかってみましょう"
            ,"クール！"
            ,"てきをたおすと　ポイントがたまります。"
            ,"てきをたおすと　ポイントがたまります。\nスコアがたまったら　ラウンドがすすみ、よりつよいてきが　でてきます。"
            ,"これで　チュートリアルは　おわりです。\nそれでは、がんばってくださいね！\nけんとうを　いのります。"}.ToArray();

            case 2:
                return new List<string>
                 { "Tutorial"
            , "Move left or right using "+_ctrMove[GameData.Language,key]+"."
            , "Move left or right using "+_ctrMove[GameData.Language,key]+".\nLet's give it a try."
            ,"Nice!"
            ,"Press "+_ctrJump[GameData.Language,key]+" to jump."
            ,"Press "+_ctrJump[GameData.Language,key]+" to jump.\nLet's try it."
            ,"Nice!"
            ,"Press "+_ctrDown[GameData.Language,key]+" to descend."
            ,"Press "+_ctrDown[GameData.Language,key]+" to descend.\nLet's give it a try"
            ,"Nice!"
            ,"Use the "+_ctrFire2[GameData.Language,key]+" and "+_ctrFire1[GameData.Language,key]+" buttons to attack."
            ,"The "+_ctrFire2[GameData.Language,key]+" is a long-range attack.\nThe "+_ctrFire1[GameData.Language,key]+" is a close-range attack."
            ,"The "+_ctrFire2[GameData.Language,key]+" is a long-range attack.\nThe "+_ctrFire1[GameData.Language,key]+" is a close-range attack.\nLet's attack the target."
            ,"Nice!"
            ,"In this game you can change weapons.\nTo change, use "+_ctrTransfomation[GameData.Language,key]+"."
            ,_ctrTransfomationHow[GameData.Language,key]+" has a specific weapon. Try attacking with various weapons."
            ,"Nice!"
            ,"Each enemy has weapons that are effective and weapons that are ineffective. Use the weapon that suits the enemy."
            ,"Nice!\nNow you know the basic controls of this game perfectly!XD"
            ,"Oops! Lightning!"
            ,"You've taken damage! When your HP reaches zero, the game is over."
            ,"Lost HP can be restored with hearts. \nGo to the heart and recover."
            ,"Nice!"
            ,"Oh... what's this?\n Oops! This is a \"battery.\" "
            ,"When you collect a \"battery\" you can use powerful special moves!"
            ,"When you have power, press "+_ctrSpecial[GameData.Language,key]+" to use special moves."
            ,"When you have power, press "+_ctrSpecial[GameData.Language,key]+" to use special moves.\nLet's go wild using the enhancements!"
            ,"Cool!!"
            ,"You earn points by defeating enemies."
            ,"You earn points by defeating enemies.\nAs you accumulate points, the round will progress and more powerful enemies will appear."
            ,"This is the end of the tutorial. \nGood luck!" }.ToArray();


        }

        return new string[0];
    }
}
