using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DeadC : MenuSystemC
{
    public Text roundText, roundText2, LevelText;

    public AudioClip reS;

    [SerializeField]
    private SpriteRenderer _lolEnemy;

    [SerializeField]
    private Sprite[] _enemy;

    private float _exitTimer = 0;

    [SerializeField]
    private float _exitFireTimeS = 3;

    private string[] _textNone = new string[3]
        {
           "なし","なし","None"
        },
        _textNowRound = new string[3]
        {
           "現在のラウンド","いまのラウンド","Current Round"
        },
        _textNewRound = new string[3]
        {
           "再挑戦ラウンド","やりなおしラウンド","Retry Round"
        };

    // Update is called once per frame
    new void Start()
    {
        _optionMax = 1;
        base.Start();
        Screen.SetResolution(GameData.FirstWidth, GameData.FirstWidth, true);

        _lolEnemy.sprite = _enemy[(int)(GameData.Round - 1) / 5];

        var gamepad = Gamepad.current;
        if (gamepad != null) gamepad.SetMotorSpeeds(0.0f, 0.0f);


        LevelText.text = GameData.TextDifficulty[GameData.Difficulty, GameData.Language];

        roundText2.text = "" + GameData.LastCrearLound.ToString();
        if (GameData.GameMode == 1)
        {
            roundText2.text = "BOSS " + (GameData.LastCrearLound - 100).ToString();
        }
        else if (GameData.LastCrearLound >= 31)
        {
            roundText2.text = "D " + (GameData.LastCrearLound - 30).ToString();
        }

        if (GameData.LastCrearLound <= 0)
        {
            roundText2.text = _textNone[GameData.Language];
        }

        if (!start)
        {
            if (GameData.GameMode == 1)
            {
                roundText.text = _textNowRound[GameData.Language] + ": BOSS" + (GameData.Round - 100).ToString();
            }
            else if (GameData.EX == 1)
            {
                roundText.text = _textNowRound[GameData.Language] + ": D " + (GameData.Round - 30).ToString();
            }
            else
            {
                roundText.text = _textNowRound[GameData.Language] + ": " + GameData.Round.ToString();
            }
        }
    }

    void Update()
    {

        _exitTimer += Time.deltaTime;
        if (_exitTimer >= _exitFireTimeS)
        {
            if (!start)
            {
                start = true;
                _titleMode = 1;
                Option2();
                _audioSource.PlayOneShot(reS);
            }
        }
    }


    protected override void Option1() { StartCoroutine(DoOption()); }
    protected override void Option2() { StartCoroutine(DoOption()); }

    private IEnumerator DoOption()
    {
        if (_titleMode == 0)
        {

            if (GameData.EX == 0)
            {
                if (GameData.Difficulty == 2)
                {
                    GameData.Round = StageRestart(GameData.Round);
                    if (GameData.Round < GameData.StartRound) GameData.Round = GameData.StartRound;
                }
            }
            else
            {
                if (GameData.Difficulty == 2 || GameData.Difficulty == 1)
                {
                    GameData.Round = 31;
                }
            }
            if (GameData.Difficulty == 3) GameData.Round = GameData.StartRound;

            if (GameData.GameMode == 1)
            {
                roundText.text = _textNewRound[GameData.Language] + ": BOSS " + (GameData.Round - 100).ToString();
            }
            else if (GameData.EX == 1)
            {
                roundText.text = _textNewRound[GameData.Language] + ": D " + (GameData.Round - 30).ToString();
            }
            else
            {
                roundText.text = _textNewRound[GameData.Language] + ": " + GameData.Round.ToString();
            }
        }

        yield return new WaitForSeconds(1.0f);
        if (_titleMode == 0)
        {
            if (GameData.EX == 0)
            {
                if (GameData.Difficulty == 2)
                {
                    GameData.Round = StageRestart(GameData.Round);
                    if (GameData.Round < GameData.StartRound) GameData.Round = GameData.StartRound;
                }
            }
            else
            {
                if (GameData.Difficulty == 2 || GameData.Difficulty == 1)
                {
                    GameData.Round = 31;
                }
            }
            if (GameData.Difficulty == 3) GameData.Round = GameData.StartRound;
            GameData.Boss = 0;
            FloorManagerC.StageGimic(100, 0);
            GameData.Star = false;
            GameData.TP = 0;
            GameData.Point = 0;
            GameData.WindSpeed = 0;
            GameData.VirusBugEffectLevel = 0;
            SceneManager.LoadScene("Game");
        }
        else if (_titleMode == 1)
        {
            CrearC._isGiveUp = true;
            SceneManager.LoadScene("Clear");
        }
    }

    public void OnEnd(InputAction.CallbackContext context)
    {
        if (context.performed && !start)
        {
            _titleMode = _optionMax;
            MoveFlash();
        }
    }

    private int StageRestart(int round)
    {
        round = (round - 1) / 5;
        round *= 5;
        round++;
        return round;
    }
}
