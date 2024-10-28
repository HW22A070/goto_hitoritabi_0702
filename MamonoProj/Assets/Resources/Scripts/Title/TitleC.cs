using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TitleC : MenuSystemC
{
    private AudioSource audioSource;

    private float _prologueTimer = 0;

    [SerializeField]
    private float _prologueFireTimeS = 20;

    private new void Start()
    {
        base.Start();
        _optionMax = 4;

        GameData.ClearTime = 0;

        GameData.Difficulty = 1;
    }

    // Update is called once per frame
    void Update()
    {
        _prologueTimer += Time.deltaTime;
        if (_prologueTimer >= _prologueFireTimeS)
        {
            SceneManager.LoadScene("Demo");
        }
    }

    protected override void Option1() { StartCoroutine(DoOption(0)); }
    protected override void Option2() { StartCoroutine(DoOption(1)); }
    protected override void Option3() { StartCoroutine(DoOption(2)); }
    protected override void Option4() { StartCoroutine(DoOption(3)); }
    protected override void Option5() { StartCoroutine(DoOption(4)); }

    private IEnumerator DoOption(int mode)
    {
        if (mode == 0)
        {
            yield return new WaitForSeconds(1.0f);
            GameData.GoalRound = 30;
            GameData.StartRound = 1;
            SceneManager.LoadScene("Level");
        }
        else if (mode == 1)
        {
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("StageSelect");
        }
        else if (mode == 2)
        {
            yield return new WaitForSeconds(1.0f);
            GameData.Difficulty = 0;
            GameData.Round = 0;
            GameData.GoalRound = 0;
            GameData.StartRound = 0;
            SceneManager.LoadScene("Game");
        }
        else if (mode == 3)
        {
            yield return new WaitForSeconds(0.5f);
            GameData.Language++;
            if (GameData.Language > 2) GameData.Language = 0;
            start = false;
        }
        else if (mode == 4)
        {
            yield return new WaitForSeconds(1.0f);
            Application.Quit();
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
}
