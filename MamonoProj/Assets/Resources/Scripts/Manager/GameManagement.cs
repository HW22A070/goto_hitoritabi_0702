using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManagement : MonoBehaviour
{
    /// <summary>
    /// ラウンドごとの目標スコア
    /// </summary>
    private float _roundGoal;

    /// <summary>
    /// 風演出用の塵の発射トリガー
    /// </summary>
    private float _windDustTimer;

    /// <summary>
    /// スコア用バー
    /// </summary>
    private Slider _scoreBar;

    /// <summary>
    /// 自機HP用バー
    /// </summary>
    private Slider _playerHpBar;

    [SerializeField, Header("ラウンド表示")]
    private Text _nowRoundText;

    /// <summary>
    /// ラウンド
    /// </summary>
    private string[] _roundLang = new string[3] { "ラウンド", "ラウンド", "Round" };

    [SerializeField]
    [Header("自機HP表示")]
    private Text _hpText;

    [SerializeField]
    [Header("現在のスコア表示")]
    private Text _scoreText;

    /// <summary>
    /// ポイント
    /// </summary>
    private string[] _pointLang = new string[3] { "ポイント", "ポイント", "Point" };

    [SerializeField, Header("スコアバー伸びるとこ")]
    private GameObject _scoreBarFill;

    [SerializeField, Header("Player")]
    private GameObject _playerGO;

    /// <summary>
    /// ボス襲来
    /// </summary>
    private string[] _bossAttackLang = new string[3] { "ボス襲来！", "ボスしゅうらい！", "Boss Invasion!" };

    [SerializeField, Header("風速表示")]
    private Text _windSpeedText;

    [SerializeField, Header("経過時間表示")]
    private Text _timeText;

    [SerializeField]
    [Header("風の前の塵")]
    private DustC _windDustP;

    [SerializeField, Header("ラウンドアップエフェクト")]
    private ExpC _levUpArrowEf;

    /// <summary>
    /// for文用の汎用繰り返し変数
    /// </summary>
    private int _counter;

    /// <summary>
    /// 自機の最大HP
    /// </summary>
    public static float maxhp;

    /// <summary>
    /// ボスの現在HP
    /// </summary>
    public float _bossNowHp;

    /// <summary>
    /// ボスの最大HP
    /// </summary>
    public float _bossMaxHp = 1;

    /// <summary>
    /// ボス名前
    /// </summary>
    public string _bossName = "ABC";

    /// <summary>
    /// 風の音のオオキサ
    /// </summary>
    private float _windAudioVolume = 2;

    [SerializeField]
    [Header("背景スプライト設定")]
    private SpriteRenderer _backGroundSR;

    [SerializeField]
    [Header("背景スプライト画像")]
    private Sprite _deadBackS;

    [SerializeField, Header("スコアアイコン設定")]
    private SpriteRenderer _scoreIconSR;

    [SerializeField, Header("画像")]
    private Sprite _scoreIconS, _bossIconS;

    [SerializeField]
    [Header("ラウンド上昇効果音")]
    private AudioClip roundupS;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    [SerializeField]
    [Header("風効果音")]
    private AudioClip windS;

    /// <summary>
    /// BGM管理
    /// </summary>
    private AudioControlC _bgmManager;

    private float _timeBeforePouse;

    [Header("デバッグモードTF")]
    public bool isDebug;

    [SerializeField, Header("初期ラウンド（デバッグON限定）")]
    private int _debugRound = 1;

    [SerializeField, Header("難易度（デバッグON限定）")]
    private int _debugDif = 2;


    void Start()
    {
        //オーディオ系初期設定
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        _bgmManager = GameObject.Find("BGMManager").GetComponent<AudioControlC>();

        //画面サイズ設定
        Screen.SetResolution(GameData.FirstWidth, GameData.FirstWidth, true);

        GameData.TimerMoving = true;

        VirusC.VirusMode = 0;

        _nowRoundText.text = _roundLang[GameData.Language] + ": " + (GameData.Round - GameData.StartRound + 1).ToString();

        GameData.PlayerMoveAble = 6;
        _scoreBarFill.GetComponent<Image>().color = Color.yellow;

        _scoreBar = GameObject.Find("PointBar").GetComponent<Slider>();
        _playerHpBar = GameObject.Find("HPBar").GetComponent<Slider>();
        if (GameData.Difficulty == 0)
        {
            GameData.HP = 100;
            maxhp = 100;
        }
        if (GameData.Difficulty == 1)
        {
            GameData.HP = 50;
            maxhp = 50;
        }
        if (GameData.Difficulty == 2)
        {
            GameData.HP = 20;
            maxhp = 20;
        }
        if (GameData.Difficulty == 3)
        {
            GameData.HP = 1;
            maxhp = 1;
        }
        if (GameData.GameMode == 1)
        {
            GameData.Round = 101;
        }
        GameData.Camera = 0;
        GameData.WindSpeed = 0;
        GameData.Star = false;
        GameData.VirusBugEffectLevel = 0;

        //チュートリアル補正
        if (GameData.Round == 0) GameData.PlayerMoveAble = 0;


        //DEBUG
        if (isDebug)
        {
            if (_debugRound >= 0)
            {
                if (GameData.GameMode == 0) GameData.Round = _debugRound;
                else if (GameData.GameMode == 1) GameData.Round = _debugRound + 100;
            }
            if (_debugDif >= 0 && _debugDif <= 3)
            {
                GameData.Difficulty = _debugDif;
            }

        }

        if (GameData.Round > 30 && GameData.GameMode == 0) GameData.EX = 1;
        else GameData.EX = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //ラウンド目標設定
        _roundGoal = (1 + GameData.Difficulty + GameData.Round);
        if (GameData.Round % 5 == 0)
        {
            _roundGoal = 10000;
            //チュートリアル補正
            if (GameData.Round == 0) _scoreText.text = "Tutorial";
        }

        //レベルアップ処理
        if (GameData.Point >= _roundGoal)
        {
            LevelUp();
        }

        //BossHP
        _scoreBar.value = GameData.Point / _roundGoal;
        _scoreIconSR.sprite = _scoreIconS;
        if (GameData.IsBossFight)
        {
            //_bossHp.GetComponent<Image>().color = Color.blue+(Color.white/3);
            //_scoreBar.value = _bossNowHp / _bossMaxHp;
            //_bossHpText.text =/* "BOSS HP " + _bossName+" : " + */_bossNowHp.ToString() + " / " + _bossMaxHp.ToString();
            //_scoreText.text =/* "BOSS HP " + _bossName+" : " + */_bossNowHp.ToString() + " / " + _bossMaxHp.ToString();
            //_scoreIconSR.sprite = _bossIconS;
        }

        //EX
        if (GameData.Round > 30 && GameData.GameMode == 0) GameData.EX = 1;
        else GameData.EX = 0;

        //風処理
        if (GameData.WindSpeed > 500)
        {
            GameData.WindSpeed = Random.Range(450, 500);
            _audioGO.PlayOneShot(windS);
        }
        if (GameData.WindSpeed < -500)
        {
            GameData.WindSpeed = Random.Range(-499, -449);
            _audioGO.PlayOneShot(windS);
        }
        if (GameData.WindSpeed != 0)
        {
            if (_windAudioVolume >= 1)
            {
                _audioGO.PlayOneShot(windS);
                _windAudioVolume = 0;
            }
            _windAudioVolume += Time.deltaTime;
        }

        //風の前の塵
        _windDustTimer += Time.deltaTime;
        if (GameData.WindSpeed != 0)
        {
            _windSpeedText.text = "WindSpeed : " + GameData.WindSpeed.ToString();
            if (GameData.WindSpeed > 0)
            {
                if (_windDustTimer >= 1 / GameData.WindSpeed)
                {
                    Vector3 direction = new Vector3(Random.Range(-150, 640), Random.Range(0, 480), 0);
                    Quaternion rot = transform.localRotation;
                    DustC shot = Instantiate(_windDustP, direction, rot);
                    shot.EShot1();
                    _windDustTimer = 0;
                }
            }
            else if (GameData.WindSpeed < 0)
            {
                if (_windDustTimer >= 1 / -GameData.WindSpeed)
                {
                    Vector3 direction = new Vector3(Random.Range(0, 790), Random.Range(0, 480), 0);
                    Quaternion rot = transform.localRotation;
                    DustC shot = Instantiate(_windDustP, direction, rot);
                    shot.EShot1();
                    _windDustTimer = 0;
                }
            }
        }
        else _windSpeedText.text = " ";

        //HPメーター処理
        _playerHpBar.value = GameData.HP / maxhp;

        //Death
        if (GameData.HP <= 0) StartCoroutine(RigorMortis());
        //上限まもってね
        if (GameData.HP > maxhp) GameData.HP = maxhp;

        //時間
        if (GameData.IsBossFight) GameData.TimerMoving = false;
        if (GameData.GameMode == 0 && GameData.TimerMoving && GameData.Round >= 1) GameData.ClearTime += Time.deltaTime;



        //テキスト表示
        if (GameData.VirusBugEffectLevel < 1)
        {
            _hpText.text = GameData.HP.ToString() + " / " + maxhp.ToString();

            if (GameData.IsBossFight) _scoreText.text = _bossAttackLang[GameData.Language];
            else _scoreText.text = _pointLang[GameData.Language] + " " + GameData.Point.ToString() + " / " + _roundGoal.ToString();

            if (GameData.EX == 1) _nowRoundText.text = "Danger: " + (GameData.Round - 30).ToString() + " / 5";
            else _nowRoundText.text = _roundLang[GameData.Language] + ": " + (GameData.Round - GameData.StartRound + 1) + " / " + (GameData.GoalRound - GameData.StartRound + 1).ToString();

            _timeText.text = GameData.ClearTime.ToString("N1");
        }


        //debug
        if (isDebug)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                GameData.TP += 1;
                GameData.HP = maxhp;
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                GameData.Point += 1000000000000;
                GameData.HP = maxhp;
            }
        }

    }

    /// <summary>
    /// レベルアップ
    /// </summary>
    private void LevelUp()
    {
        //最終クリアラウンド更新
        if (GameData.Round > GameData.LastCrearLound) GameData.LastCrearLound = GameData.Round;
        //クリア
        if (GameData.Round + 1 > GameData.GoalRound)
        {
            GameData.Round++;
            SceneManager.LoadScene("Clear");
        }
        GameData.Round++;

        _audioGO.PlayOneShot(roundupS);
        GameData.Camera = 0;
        GameData.IsBossFight = false;
        GameData.Point = 0;
        GameData.Star = false;
        
        GameData.VirusBugEffectLevel = 0;
        for (int k = 0; k < 1; k++)
        {
            LevelUpEffects();
        }
        if (GameData.Round % 5 == 1)BGMStart();

        _scoreBarFill.GetComponent<Image>().color = Color.yellow;

        GetComponent<StageGimicSummonerC>().SpecialStageCreate();
    }

    

    /// <summary>
    /// BGM再生スタート
    /// </summary>
    private void BGMStart()
    {
        _bgmManager.ChangeAudio(GameData.GetRoundNumber(), false, 0.5f);
    }

    /// <summary>
    /// レベルアップ時のアニメ－ション
    /// </summary>
    private void LevelUpEffects()
    {
        Vector3 ppos = _playerGO.transform.position;
        Instantiate(_levUpArrowEf, ppos + (transform.right * 200), transform.localRotation).EShot1(90, 5, 1.5f);
        Instantiate(_levUpArrowEf, ppos - (transform.right * 200), transform.localRotation).EShot1(90, 5, 1.5f);
        Instantiate(_levUpArrowEf, new Vector3(ppos.x, GameData.GroundPutY((int)(ppos.y / 90) + 1, 48), 0), transform.localRotation).EShot1(90, 5, 1.5f);
        Instantiate(_levUpArrowEf, new Vector3(ppos.x, GameData.GroundPutY((int)(ppos.y / 90) - 1, 48), 0), transform.localRotation).EShot1(90, 5, 1.5f);
    }

    /// <summary>
    /// 死んだときの止まる演出
    /// </summary>
    private IEnumerator RigorMortis()
    {
        TimeManager.ChangeTimeValue(0.2f);
        GameData.PlayerMoveAble = 0;
        _backGroundSR.sprite = _deadBackS;
        yield return new WaitForSeconds(0.2f);
        TimeManager.ChangeTimeValue(1.0f);
        SceneManager.LoadScene("Dead");

    }

    /*
    [UnityEditor.MenuItem("Edit/CaptureScreenshot")]
    static void Capture()

    {
        ScreenCapture.CaptureScreenshot("screen" + ".png", 1);
    }
    */
    /*
    //Imput
    public void OnPouse(InputAction.CallbackContext context)
    {
        //ポーズ処理
        if (context.started)
        {
            GameData.Pouse = !GameData.Pouse;
            _audioGO.PlayOneShot(pouseS);
            if (GameData.Pouse)
            {
                _nowRoundText.text = _roundLang[GameData.Language]+": " + GameData.Round.ToString() + "\nPouse Mode";
                GameData.PlayerMoveAble = 0;
                _timeBeforePouse = Time.timeScale;
                TimeManager.ChangeTimeValue(0.0f);
            }
            else
            {
                _nowRoundText.text = _roundLang[GameData.Language] + ": " + GameData.Round.ToString();
                GameData.PlayerMoveAble = 6;
                TimeManager.ChangeTimeValue(_timeBeforePouse);
            }
        }
    }
    */
    /*
    public void OnEnd(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Time.timeScale = 1.0f;
            GameData.Round = 1;
            GameData.HP = 20;
            GameData.Boss = 0;
            FloorManagerC.StageIce(100) = 0;
            GameData.Star = false;
            GameData.TP = 0;
            GameData.Point = 0;
            GameData.GameMode = 0;
            GameData.ClearTime = 0;
            SceneManager.LoadScene("Title");
        }
    }
    */
}