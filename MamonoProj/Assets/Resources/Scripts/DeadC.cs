using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DeadC : MonoBehaviour
{
    int conti = 0;
    bool end,car;
    float time = 1.0f;

    public Text roundText, roundText2, LevelText;
    public AudioClip reS,endS;

    // Update is called once per frame
    void Start()
    {
        var gamepad = Gamepad.current;
        if(gamepad != null) gamepad.SetMotorSpeeds(0.0f, 0.0f);

        if (GameData.Difficulty == 0)
        {
            LevelText.text = "イージー";
        }
        else if (GameData.Difficulty == 1)
        {
            LevelText.text = "ノーマル";
        }
        else if (GameData.Difficulty == 2)
        {
            LevelText.text = "ハード";
        }
        else if (GameData.Difficulty == 3)
        {
            LevelText.text = "無理！";
        }

        roundText2.text = "" + GameData.LastCrearLound.ToString();
        if (GameData.GameMode == 1)
        {
            roundText2.text = "BOSS " + (GameData.LastCrearLound-100).ToString();
        }
        else if (GameData.LastCrearLound >=31)
        {
            roundText2.text = "D " + (GameData.LastCrearLound-30).ToString();
        }

        if (GameData.LastCrearLound <= 0)
        {
            roundText2.text = "なし";
        }
    }

    void Update()
    {
        if (GameData.Zuru)
        {
            GameData.Zuru = false;
            GameData.Round = 1;
            GameData.HP = 20;
            GameData.Boss = 0;
            GameData.IceFloor = 0;
            GameData.Star = false;
            GameData.TP = 0;
            GameData.Score = 0;
            GameData.GameMode = 0;
            GameData.LastCrearLound = 0;
            GameData.EX = 0;
            GameData.ClearTime = 0;
            SceneManager.LoadScene("Title");
        }
        if (conti == 0)
        {
            if (GameData.GameMode == 1)
            {
                roundText.text = "Reached Round: BOSS" + (GameData.Round - 100).ToString();
            }
            else if (GameData.EX==1)
            {
                roundText.text = "Reached Round: D " + (GameData.Round - 30).ToString();
            }
            else
            {
                roundText.text = "Reached Round: " + GameData.Round.ToString();
            }
        }
        else
        {
            if (GameData.GameMode == 1)
            {
                roundText.text = "NEW Round: BOSS " + (GameData.Round - 100).ToString();
            }
            else if (GameData.EX == 1)
            {
                roundText.text = "NEW Round: D " + (GameData.Round - 30).ToString();
            }
            else
            {
                roundText.text = "NEW Round: " + GameData.Round.ToString();
            }
        }

        if (car) time -= Time.deltaTime;
        if (time <= 0&&conti==1)
        {
            GameData.Boss = 0;
            GameData.IceFloor = 0;
            GameData.Star = false;
            GameData.TP = 0;
            GameData.Score = 0;
            GameData.WindSpeed = 0;
            GameData.VirusBugEffectLevel = 0;
            SceneManager.LoadScene("Game");
        }
        if (time <= 0&&end)
        {
            SceneManager.LoadScene("Clear");
        }


    }

    //Imput
    public void OnStart(InputAction.CallbackContext context)
    {
        if (context.performed && !car)
        {
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(reS);
            conti = 1;
            car = true;
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
        }
    }

    public void OnEnd(InputAction.CallbackContext context)
    {
        if (context.performed && !car)
        {
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(endS);
            end = true;
            car = true;
        }
    }

    private int StageRestart(int round)
    {
        round =(round- 1) / 5;
        round *= 5;
        round++;
        return round;
    }
}
