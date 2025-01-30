using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using unityroom.Api;
using UnityEngine.InputSystem;

public class CrearC : MonoBehaviour
{
    public Text roundText, LevelText, TimeText, PointText, RankText;

    private float sc;

    private int _loaded = 0;

    private float lastscore;

    private float _exitTimer = 0;

    [SerializeField]
    private float _exitFireTimeS = 20;

    private string _timeRank;
    private float _timeScore = 0;

    [SerializeField]
    private SpriteRenderer _background;
    [SerializeField]
    private Sprite[] _back;

    public static bool _isGiveUp;

    /// <summary>
    /// ボス撃破ボーナス
    /// </summary>
    public static float BossBonus=0;

    /// <summary>
    /// 死亡回数
    /// </summary>
    public static int DeathCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(GameData.FirstWidth, GameData.FirstWidth, true);

        _background.sprite = _back[(int)(GameData.Round - 1) / 5];

        GameData.LastCrearLound = 1 + GameData.LastCrearLound - GameData.StartRound;

        if (!_isGiveUp)
        {
            if (GameData.ClearTime < 14 * GameData.LastCrearLound)
            {
                _timeScore = 120;
                _timeRank = "S";
            }
            else if (GameData.ClearTime < 25 * GameData.LastCrearLound)
            {
                _timeScore = 70;
                _timeRank = "A";
            }
            else if (GameData.ClearTime < 40 * GameData.LastCrearLound)
            {
                _timeScore = 40;
                _timeRank = "B";
            }
            else
            {
                _timeScore = 20;
                _timeRank = "C";
            }
        }
        else
        {
            _timeScore = 0;
            _timeRank = "F";
        }

        lastscore = (_timeScore + (BossBonus * 10))+(GameData.Difficulty*10) / ((DeathCount * 0.5f) + 1);

        /*
        if (GameData.ClearTime != 0) lastscore = (GameData.LastCrearLound * 10) * 150 / GameData.ClearTime;
        else lastscore = 0;
        */

        if (_isGiveUp)
        {
            _isGiveUp = false;
            lastscore /= 3;
        }


        if (lastscore < 0) lastscore = 0;

        TimeText.text = _timeRank;

        LevelText.text = GameData.TextDifficulty[GameData.Difficulty, GameData.Language];

        roundText.text = "" + GameData.LastCrearLound.ToString();
        if (GameData.LastCrearLound > 30)
        {
            roundText.text = "D" + (GameData.LastCrearLound - 30).ToString();
        }
        else if (GameData.LastCrearLound <= 0)
        {
            roundText.text = "Non";
        }


    }

    private void Update()
    {
        _exitTimer += Time.deltaTime;
        if (_exitTimer >= _exitFireTimeS)
        {
            if (_loaded < 2)
            {
                _loaded = 2;
                StartCoroutine(Load());
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (_loaded < 2)
        {
            if (sc < lastscore) sc += lastscore / 10;
            else if (_loaded == 0)
            {
                sc = lastscore;
                _loaded = 1;
            }
        }
        else
        {
            sc = lastscore;
        }

        if (sc >= 130) RankText.text = "S+";
        else if (sc >= 80) RankText.text = "S";
        else if (sc >= 50) RankText.text = "A";
        else if (sc >= 10) RankText.text = "B";
        else RankText.text = "C";

        PointText.text = sc.ToString("N2");


        if (GameData.GameMode == 0)
        {
            UnityroomApiClient.Instance.SendPoint(1, lastscore, PointboardWriteMode.Always);
        }
        else
        {
            UnityroomApiClient.Instance.SendPoint(2, GameData.Round - 100, PointboardWriteMode.Always);
        }
    }

    private IEnumerator Load()
    {
        yield return new WaitForSeconds(0.1f);
        GameData.Round = 1;
        GameData.HP = 20;
        GameData.IsBossFight = false;
        FloorManagerC.StageGimic(100, 0);
        GameData.Star = false;
        GameData.TP = 0;
        GameData.Point = 0;
        GameData.GameMode = 0;
        GameData.LastCrearLound = 0;
        GameData.EX = 0;
        GameData.ClearTime = 0;
        SceneManager.LoadScene("Title");
    }

    //Imput
    public void OnTitle(InputAction.CallbackContext context)
    {
        if (context.performed && _loaded < 2)
        {
            switch (_loaded)
            {
                case 0:
                    _loaded = 1;
                    break;
                case 1:
                    _loaded = 2;
                    StartCoroutine(Load());
                    break;
                case 2:
                    break;
            }

        }
    }
}



/*
score>33=S+
score>25=S
score>20=A
score>10=B
>C
*/