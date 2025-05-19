using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using EnumDic.System;
using EnumDic.Stage;

public class GameManagement : MonoBehaviour
{
    [SerializeField, Header("Player")]
    private PlayersManagerC _scPlsM;

    /// <summary>
    /// ラウンドごとの目標スコア
    /// </summary>
    private int _roundGoal;

    /// <summary>
    /// 風演出用の塵の発射トリガー
    /// </summary>
    private float _windDustTimer;

    /// <summary>
    /// スコア用バー
    /// </summary>
    [SerializeField]
    private Slider _scoreBar;

    [SerializeField, Header("ラウンド表示")]
    private Text _nowRoundText;

    [SerializeField]
    [Header("現在のスコア表示")]
    private Text _scoreText;

    [SerializeField, Header("スコアバー伸びるとこ")]
    private GameObject _scoreBarFill;

    [SerializeField]
    private RoundMeterC _scRoundMeter;

    /// <summary>
    /// ボス襲来
    /// </summary>
    private string[] _bossAttackLang = new string[3] { "ボス襲来！", "ボスしゅうらい！", "Boss Invasion!" };

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

    private List<StageStates> _stagesStages=new List<StageStates> { };



    [Header("デバッグモードTF")]
    public bool isDebug;

    [SerializeField, Header("初期ラウンド（デバッグON限定）")]
    private int _debugRound = 1;

    //[SerializeField, Header("ゲームモード（デバッグON限定）")]
    private MODE_GAMEMODE _debugGamemode = MODE_GAMEMODE.Normal;

    [SerializeField, Header("難易度（デバッグON限定）")]
    private MODE_DIFFICULTY _debugDif;

    [SerializeField, Header("無限エネルギー（デバッグON限定）")]
    private bool _isInfinityEnergyDenug;

    private void Awake()
    {
        //DEBUG
        if (isDebug)
        {
            if (_debugRound >= 0)
            {
                switch (GameData.GameMode)
                {
                    case MODE_GAMEMODE.Normal:
                        GameData.Round = _debugRound;
                        break;

                    case MODE_GAMEMODE.MultiTower:
                        GameData.Round = _debugRound + 100;
                        break;
                }
            }
            GameData.Difficulty = _debugDif;
            GameData.GameMode = _debugGamemode;
        }
        if (GameData.Round > 30 && GameData.GameMode == 0) GameData.EX = 1;
        else GameData.EX = 0;
    }

    void Start()
    {
        SetGameStage();

        //オーディオ系初期設定
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        _bgmManager = GameObject.Find("BGMManager").GetComponent<AudioControlC>();

        //画面サイズ設定
        Screen.SetResolution(GameData.FirstWidth, GameData.FirstWidth, true);

        

        GameData.IsTimerMoving = true;

        _nowRoundText.text = (GameData.Round - GameData.StartRound + 1).ToString();

        GameData.PlayerMoveAble = 6;
        _scoreBarFill.GetComponent<Image>().color = Color.yellow;

        GameData.RotationCamera = 0;
        GameData.WindSpeed = 0;
        GameData.IsInvincible = false;
        GameData.VirusBugEffectLevel = 0;

        //チュートリアル補正
        if (GameData.Round == 0) GameData.PlayerMoveAble = 0;

        GameData.IsStageMovingAction = false;

        switch (GameData.GameMode)
        {
            case MODE_GAMEMODE.Normal:
                for (int i = 0; i < StageData.DataNormalStages.Length; i++)
                {
                    _stagesStages.Add(StageData.DataNormalStages[i]);
                }
                break;

            case MODE_GAMEMODE.MultiTower:
                for (int i = 0; i < StageData.DataMultiTowerStages.Length; i++)
                {
                    _stagesStages.Add(StageData.DataMultiTowerStages[i]);
                }
                break;
        }



        BGMStart();
    }

    // Update is called once per frame
    void Update()
    {

        //ラウンド目標設定
        _roundGoal = _stagesStages[GameData.Round].score;
        //マルチ補正
        _roundGoal += (int)(_stagesStages[GameData.Round].score *0.7f * (GameData.MultiPlayerCount-1));

        //チュートリアル補正
        if (GameData.Round == 0) _scoreText.text = "Tutorial";

        //レベルアップ処理
        if (GameData.Point >= _roundGoal)
        {
            DoLevelUp();
        }

        //BossHP
        _scoreBar.value = GameData.Point / _roundGoal;
        _scoreIconSR.sprite = _scoreIconS;

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

        //時間
        if (GameData.IsBossFight) GameData.IsTimerMoving = false;
        if (GameData.GameMode == 0 && GameData.IsTimerMoving && GameData.Round >= 1) GameData.ClearTime += Time.deltaTime;



        //テキスト表示
        if (GameData.VirusBugEffectLevel ==EnumDic.Enemy.Virus.MODE_VIRUS.None)
        {
            if (GameData.IsBossFight||_roundGoal==10000) _scoreText.text = _bossAttackLang[GameData.Language];
            else _scoreText.text = GameData.Point.ToString() + " / " + _roundGoal.ToString();

            if (GameData.EX == 1) _nowRoundText.text = "Danger: " + (GameData.Round - 30).ToString() + " / 5";
            else _nowRoundText.text = (GameData.Round - GameData.StartRound + 1).ToString() + " / " + (GameData.GoalRound - GameData.StartRound + 1).ToString();

            _timeText.text = GameData.ClearTime.ToString("N1");
        }

        //debug
        if (isDebug)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                foreach(GameObject player in _scPlsM.GetAlivePlayers())
                {
                    PlayerC playerC = player.GetComponent<PlayerC>();
                    playerC.SetAddTPPlus1();
                    playerC.SetHP(GameData.GetMaxHP());
                }
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                GameData.Point += 1000000000000;
                foreach (GameObject player in _scPlsM.GetAlivePlayers())
                {
                    player.GetComponent<PlayerC>().SetHP(GameData.GetMaxHP());
                }
            }

            if (_isInfinityEnergyDenug)
            {
                foreach (GameObject player in _scPlsM.GetAlivePlayers())
                {
                    player.gameObject.GetComponent<PlayerAttackC>().SetAllEnergyHeal();
                }
            }
        }

    }

    private void SetGameStage()
    {
        if (GameData.Round == 0) GameData.StageMode = KIND_STAGE.Tutorial;
        else if (1 <= GameData.Round && GameData.Round <= 35) GameData.StageMode = (KIND_STAGE)((GameData.Round - 1) / 5);
    }

    /// <summary>
    /// レベルアップ
    /// </summary>
    private void DoLevelUp()
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
        SetGameStage();
        _scRoundMeter.SetTileSpritesByRound();

        _audioGO.PlayOneShot(roundupS);
        GameData.RotationCamera = 0;
        GameData.IsBossFight = false;
        GameData.Point = 0;
        GameData.IsInvincible = false;
        
        GameData.VirusBugEffectLevel = 0;
        for (int k = 0; k < 1; k++)
        {
            LevelUpEffects();
        }
        if (GameData.Round % 5 == 1)BGMStart();

        _scoreBarFill.GetComponent<Image>().color = Color.yellow;

        GetComponent<StageGimicSummonerC>().DoSpecialStageCreate();
    }

    

    /// <summary>
    /// BGM再生スタート
    /// </summary>
    private void BGMStart()
    {
        _bgmManager.ChangeAudio((int)GameData.StageMode, false, 0.5f);
    }

    /// <summary>
    /// レベルアップ時のアニメ－ション
    /// </summary>
    private void LevelUpEffects()
    {


        Vector3 _posPlayer = _scPlsM.GetRandomAlivePlayer().transform.position;
        Instantiate(_levUpArrowEf, _posPlayer + (transform.right * 200), transform.localRotation).ShotEXP(90, 5, 1.5f);
        Instantiate(_levUpArrowEf, _posPlayer - (transform.right * 200), transform.localRotation).ShotEXP(90, 5, 1.5f);
        Instantiate(_levUpArrowEf, new Vector3(_posPlayer.x, GameData.GetGroundPutY((int)(_posPlayer.y / 90) + 1, 48), 0), transform.localRotation).ShotEXP(90, 5, 1.5f);
        Instantiate(_levUpArrowEf, new Vector3(_posPlayer.x, GameData.GetGroundPutY((int)(_posPlayer.y / 90) - 1, 48), 0), transform.localRotation).ShotEXP(90, 5, 1.5f);
    }

    public void SetDeadPlayer()
    {
        
        StartCoroutine(DoRigorMortis(_scPlsM.GetAlivePlayersCount()<=0));
    }

    /// <summary>
    /// 死んだときの止まる演出
    /// </summary>
    private IEnumerator DoRigorMortis(bool isAllDead)
    {
        if (isAllDead)
        {
            GameData.PlayerMoveAble = 0;
        }
        _backGroundSR.sprite = _deadBackS;

        TimeManager.ChangeTimeValue(0.2f);
        yield return new WaitForSeconds(0.2f);
        TimeManager.ChangeTimeValue(1.0f);

        if (isAllDead)
        {
            _bgmManager.VolumefeedInOut(3f, 0f);

            //敵キャラを帰らせる
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                enemy.GetComponent<ECoreC>().SetLeave();
            }

            yield return new WaitForSeconds(3f);

            SceneManager.LoadScene("Dead");
        }
    }
}