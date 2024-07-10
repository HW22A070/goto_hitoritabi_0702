using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TitleC : MonoBehaviour
{
    [SerializeField]
    private int debuground=1;

    private int bosscharge = 0;

    [SerializeField]
    private Text LevelText;

    [SerializeField]
    private Text BossText;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite normal, bossr;

    [SerializeField]
    private AudioClip startS;

    private AudioSource audioSource;

    [SerializeField]
    private int _firstRound, _finishRound;

    private float time = 0;

    private bool start =false;

    [SerializeField]
    private GameObject _arrowUI;

    /// <summary>
    /// 0=プレイ
    /// 1=チュートリアル
    /// 2=おわる
    /// </summary>
    private short _titleMode = 0;

    private void Start()
    {
        GameData.Zuru = false;

        var gamepad = Gamepad.current;
        if (gamepad != null) gamepad.SetMotorSpeeds(0.0f, 0.0f);

        GameData.Round = debuground;
        GameData.ClearTime = 0;
        audioSource = GetComponent<AudioSource>();

        GameData.Difficulty = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Load()
    {
        yield return new WaitForSeconds(1.0f);
        if (_titleMode == 0)
        {
            GameData.GoalRound = 30;
            GameData.StartRound = 1;
            SceneManager.LoadScene("Level");
        }
        else if (_titleMode == 1)
        {
            GameData.GoalRound =_finishRound;
            GameData.StartRound = _firstRound ;
            SceneManager.LoadScene("Level");
        }
        else if (_titleMode == 2)
        {
            GameData.Difficulty = 0;
            GameData.Round = 0;
            SceneManager.LoadScene("Game");
        }
        else if (_titleMode == 3)
        {
            Application.Quit();
        }
    }

    //Imput
    public void OnStart(InputAction.CallbackContext context)
    {
        if (context.performed && !start)
        {
            start = true;
            Debug.Log("start!");
            StartCoroutine(Load());
            audioSource.PlayOneShot(startS);
        }
    }

    public void OnEnd(InputAction.CallbackContext context)
    {
        if (context.performed && !start)
        {
            _titleMode = 3;
            _arrowUI.GetComponent<Arrow>().MoveArrow(_titleMode);
        }
    }

    public void OnUp(InputAction.CallbackContext context)
    {
        if (context.started && !start)
        {
            _titleMode--;
            if (_titleMode < 0) _titleMode = 3;
            _arrowUI.GetComponent<Arrow>().MoveArrow(_titleMode);
        }
    }
    public void OnDown(InputAction.CallbackContext context)
    {
        if (context.started && !start)
        {
            _titleMode++;
            if (_titleMode >3) _titleMode = 0;
            _arrowUI.GetComponent<Arrow>().MoveArrow(_titleMode);
        }
    }
}
