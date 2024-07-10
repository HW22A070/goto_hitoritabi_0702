using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using unityroom.Api;
using UnityEngine.InputSystem;

public class CrearC : MonoBehaviour
{
    public Text roundText,LevelText,TimeText,ScoreText,RankText;

    float sc;

    float lastscore;
    // Start is called before the first frame update
    void Start()
    {
        GameData.LastCrearLound -= GameData.StartRound;

        lastscore = (float)((GameData.LastCrearLound * ((GameData.Difficulty * 0.3) + 0.7)) - (GameData.ClearTime / 60));

        if (lastscore < 0) lastscore = 0;

        TimeText.text= GameData.ClearTime.ToString("N2");

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

        roundText.text = "" + GameData.LastCrearLound.ToString(); 
        if (GameData.LastCrearLound>30)
        {
            roundText.text = "D" + (GameData.LastCrearLound - 30).ToString();
        }
        else if (GameData.LastCrearLound <= 0)
        {
            roundText.text = "Non";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sc < lastscore) sc += 0.05f;
            if (sc >= 33) RankText.text = "S+";
            else if (sc >= 25) RankText.text = "S";
            else if (sc >= 20) RankText.text = "A";
            else if (sc >= 10) RankText.text = "B";
            else RankText.text = "C";
        
        ScoreText.text = sc.ToString("N2");
        

        if (GameData.GameMode == 0)
        {
            UnityroomApiClient.Instance.SendScore(1, lastscore, ScoreboardWriteMode.Always);
        }
        else
        {
            UnityroomApiClient.Instance.SendScore(2, GameData.Round - 100, ScoreboardWriteMode.Always);
        }

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
    }

    void FixedUpdate()
    {
        
    }

    //Imput
    public void OnTitle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
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
    }
}



/*
score>33=S+
score>25=S
score>20=A
score>10=B
>C
*/