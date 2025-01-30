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
            SceneManager.LoadSceneAsync("Demo");
        }
    }

    protected override void Option1() { StartCoroutine(DoOption(0)); }
    protected override void Option2() { StartCoroutine(DoOption(1)); }
    protected override void Option3() { StartCoroutine(DoOption(2)); }
    protected override void Option4() { StartCoroutine(DoOption(3)); }
    protected override void Option5() { StartCoroutine(DoOption(4)); }

    private IEnumerator DoOption(int mode)
    {
        AsyncOperation asyncLoad;
        if (mode == 0)
        {
            asyncLoad = SceneManager.LoadSceneAsync("Level");
            asyncLoad.allowSceneActivation = false;
            yield return new WaitForSeconds(1.0f);
            asyncLoad.allowSceneActivation = true;
            GameData.GoalRound = 30;
            GameData.StartRound = 1;

        }
        else if (mode == 1)
        {
            asyncLoad = SceneManager.LoadSceneAsync("StageSelect");
            asyncLoad.allowSceneActivation = false;
            yield return new WaitForSeconds(1.0f);
            asyncLoad.allowSceneActivation = true;

        }
        else if (mode == 2)
        {
            asyncLoad = SceneManager.LoadSceneAsync("Game");
            asyncLoad.allowSceneActivation = false;
            yield return new WaitForSeconds(1.0f);
            GameData.Difficulty = 0;
            GameData.Round = 0;
            GameData.GoalRound = 0;
            GameData.StartRound = 0;
            asyncLoad.allowSceneActivation = true;


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
