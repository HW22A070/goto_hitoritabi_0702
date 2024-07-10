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

    /// <summary>
    /// ボスHP用バー
    /// </summary>
    private Slider _bossHpBar;

    [SerializeField,Header("ラウンド表示")]
    private Text _nowRoundText;

    [SerializeField]
    [Header("自機HP表示")]
    private Text _hpText;

    [SerializeField]
    [Header("現在のスコア表示")]
    private Text _scoreText;

    [SerializeField, Header("スコアバー伸びるとこ")]
    private GameObject _scoreBarFill;


    [SerializeField, Header("ボスHP表示")]
    private Text _bossHpText;

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
    public string _bossName="ABC";

    /// <summary>
    /// 風の音のオオキサ
    /// </summary>
    private float _windAudioVolume=2;

    [SerializeField]
    [Header("背景スプライト設定")]
    private SpriteRenderer _backGroundSR;

    [SerializeField]
    [Header("背景スプライト画像")]
    private Sprite _deadBackS;

    [SerializeField]
    [Header("ポーズ効果音")]
    private AudioClip pouseS;

    [SerializeField]
    [Header("ラウンド上昇効果音")]
    private AudioClip roundupS;

    [SerializeField]
    [Header("風効果音")]
    private AudioClip windS;

    [Header("デバッグモードTF")]
    public bool isDebug;

    [SerializeField, Header("初期ラウンド（デバッグON限定）")]
    private int _debugRound = 1;

    [SerializeField, Header("難易度（デバッグON限定）")]
    private int _debugDif = 2;


    void Start()
    {

        GameData.TimerMoving = true;

        VirusC.VirusMode = 0;

        _nowRoundText.text = "Round: " + (GameData.Round-GameData.StartRound+1).ToString();

        GameData.PlayerMoveAble = 6;
        _scoreBarFill.GetComponent<Image>().color = Color.yellow;

        _scoreBar = GameObject.Find("ScoreBar").GetComponent<Slider>();
        _playerHpBar = GameObject.Find("HPBar").GetComponent<Slider>();
        _bossHpBar = GameObject.Find("BSHPBar").GetComponent<Slider>();
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
        if (GameData.Round==0) GameData.PlayerMoveAble = 0;


        //DEBUG
        if (isDebug)
        {
            if (_debugRound >= 0)
            {
                if (GameData.GameMode == 0) GameData.Round = _debugRound;
                else if (GameData.GameMode == 1) GameData.Round = _debugRound + 100;
            }
            if (_debugDif >= 0&&_debugDif<=3)
            {
                GameData.Difficulty = _debugDif;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        //テキスト表示
        _hpText.text =GameData.HP.ToString() + " / " + maxhp.ToString();
        _scoreText.text = "Score " + GameData.Score.ToString() + " / " + _roundGoal.ToString();
        //if (GameData.Star) _nowRoundText.text = "Congratulation";
        if (GameData.EX == 1) _nowRoundText.text = "Danger: " + (GameData.Round - 30).ToString() + " / 5";
        else _nowRoundText.text = "Round: " + (GameData.Round - GameData.StartRound + 1)+ " / " + (GameData.GoalRound - GameData.StartRound+1).ToString();

        //ラウンド目標設定
        _roundGoal = (1+GameData.Difficulty + GameData.Round);
        if (GameData.Round % 5 == 0)
        {
            _roundGoal = 10000;
            _scoreText.text = "Boss Attack";
            //チュートリアル補正
            if (GameData.Round == 0) _scoreText.text = "Tutorial";
        }

        //レベルアップ処理
        if (GameData.Score >= _roundGoal)
        {
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(roundupS);
            GameData.Camera = 0;
            GameData.WindSpeed = 0;
            GameData.Boss = 0;
            if (GameData.Round > GameData.LastCrearLound)GameData.LastCrearLound = GameData.Round;
            GameData.Round++;
            GameData.Score = 0;
            GameData.Star = false;
            GameData.IceFloor = 0;
            GameData.VirusBugEffectLevel = 0;
            for (int k = 0; k < 10; k++)
            {
                LevelUpEffects();
            }
            _scoreBarFill.GetComponent<Image>().color = Color.yellow;
        }

        //BossHP
        _scoreBar.value = GameData.Score / _roundGoal;
        if (GameData.Boss == 1)
        {
            _scoreBarFill.GetComponent<Image>().color = Color.blue+(Color.white/3);
            //_bossHpBar.value = _bossNowHp / _bossMaxHp;
            _scoreBar.value = _bossNowHp / _bossMaxHp;
            //_bossHpText.text =/* "BOSS HP " + _bossName+" : " + */_bossNowHp.ToString() + " / " + _bossMaxHp.ToString();
            _scoreText.text =/* "BOSS HP " + _bossName+" : " + */_bossNowHp.ToString() + " / " + _bossMaxHp.ToString();
        }

        //EX
        if (GameData.Round>30&& GameData.GameMode==0)GameData.EX = 1;
        else GameData.EX = 0;

        //風処理
        if (GameData.WindSpeed > 500)
        {
            GameData.WindSpeed = Random.Range(450, 500);
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(windS);
        }
        if (GameData.WindSpeed < -500)
        {
            GameData.WindSpeed = Random.Range(-499, -449);
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(windS);
        }
        if (GameData.WindSpeed != 0)
        {
            if (_windAudioVolume >= 1)
            {
                GameObject.FindObjectOfType<AudioSource>().PlayOneShot(windS);
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
        if (GameData.HP <= 0)StartCoroutine("RigorMortis");
        //上限まもってね
        if (GameData.HP > maxhp)GameData.HP = maxhp;

        //クリア
        if (GameData.Round > GameData.GoalRound&&GameData.GameMode!=1)SceneManager.LoadScene("Clear");

        //時間
        if (GameData.GameMode == 0 && GameData.TimerMoving && GameData.Round >= 1) GameData.ClearTime += Time.deltaTime;
        _timeText.text =GameData.ClearTime.ToString("N2");


        if (GameData.GameMode == 1)_nowRoundText.text = "Endless: " + (GameData.Round - 100).ToString();

        if (GameData.Pouse)
        {
            
        }
        else
        {

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
                GameData.Score += 1000000000000;
                GameData.HP = maxhp;
            }
        }

    }

    /// <summary>
    /// レベルアップ時のアニメ－ション
    /// </summary>
    private void LevelUpEffects()
    {
        Instantiate(_levUpArrowEf, GameData.RandomWindowPosition(), transform.localRotation).EShot1(90, 5, 0.7f);
    }

    /// <summary>
    /// 死んだときの止まる演出
    /// </summary>
    private IEnumerator RigorMortis()
    {
        Time.timeScale = 0.01f;
        for (_counter = 0; _counter < 2; _counter++)
        {
            GameData.PlayerMoveAble = 0;
            _backGroundSR.sprite = _deadBackS;
            yield return new WaitForSeconds(0.02f);
        }
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Dead");

    }

    /*
    [UnityEditor.MenuItem("Edit/CaptureScreenshot")]
    static void Capture()

    {
        ScreenCapture.CaptureScreenshot("screen" + ".png", 1);
    }
    */

    //Imput
    public void OnPouse(InputAction.CallbackContext context)
    {
        //ポーズ処理
        if (context.started)
        {
            GameData.Pouse = !GameData.Pouse;
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(pouseS);
            if (GameData.Pouse)
            {
                _nowRoundText.text = "Round: " + GameData.Round.ToString() + "\nPouse Mode";
                Time.timeScale = 0.0f;
            }
            else
            {
                Time.timeScale = 1.0f;
                _nowRoundText.text = "Round: " + GameData.Round.ToString();
            }
        }
    }
    /*
    public void OnEnd(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Time.timeScale = 1.0f;
            GameData.Round = 1;
            GameData.HP = 20;
            GameData.Boss = 0;
            GameData.IceFloor = 0;
            GameData.Star = false;
            GameData.TP = 0;
            GameData.Score = 0;
            GameData.GameMode = 0;
            GameData.ClearTime = 0;
            SceneManager.LoadScene("Title");
        }
    }
    */
}