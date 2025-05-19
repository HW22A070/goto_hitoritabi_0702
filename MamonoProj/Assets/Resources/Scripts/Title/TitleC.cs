using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using EnumDic.System;

public class TitleC : MenuSystemC
{
    private AudioSource _asOwn;

    private float _prologueTimer = 0;

    [SerializeField]
    private float _prologueFireTimeS = 20;

    [SerializeField]
    private WarningWindowC _goWindow;

    private new void Start()
    {
        base.Start();
        _optionMax = 5;

        GameData.ClearTime = 0;

        GameData.Difficulty = MODE_DIFFICULTY.General;
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
    protected override void Option6() { StartCoroutine(DoOption(5)); }

    private IEnumerator DoOption(int mode)
    {
        AsyncOperation asyncLoad;
        switch (mode)
        {
            case 0:
                //キーボード数識別ができるまで強制true
                if (CheckIsConnectController()||true)
                {
                    _audioSource.PlayOneShot(startS);

                    asyncLoad = SceneManager.LoadSceneAsync("Level");
                    asyncLoad.allowSceneActivation = false;
                    yield return new WaitForSeconds(1.0f);

                    GameData.GameMode = MODE_GAMEMODE.Normal;
                    GameData.GoalRound = 30;
                    GameData.StartRound = 1;

                    //GameData.MultiPlayerCount = 1;

                    asyncLoad.allowSceneActivation = true;
                }
                else
                {
                    _audioSource.PlayOneShot(faildS);
                    _isStart = false;
                    _goWindow.DoOpen(1.0f);
                }
                break;

            case 1:
                    _audioSource.PlayOneShot(startS);

                    asyncLoad = SceneManager.LoadSceneAsync("StageSelect");
                    GameData.GameMode = MODE_GAMEMODE.Normal;
                //GameData.MultiPlayerCount = 1;
                asyncLoad.allowSceneActivation = false;
                    yield return new WaitForSeconds(1.0f);
                    asyncLoad.allowSceneActivation = true;
                break;

            case 2:
                _audioSource.PlayOneShot(startS);

                /*
                GameData.MultiPlayerCount = GetActiveControllerCount();
                asyncLoad = SceneManager.LoadSceneAsync("Level");
                asyncLoad.allowSceneActivation = false;
                yield return new WaitForSeconds(1.0f);
                asyncLoad.allowSceneActivation = true;
                GameData.GameMode = MODE_GAMEMODE.MultiTower;
                GameData.GoalRound = 30;
                GameData.StartRound = 1;
                */

                yield return new WaitForSeconds(0.5f);
                switch (GameData.MultiPlayerCount)
                {
                    case 1:
                        GameData.MultiPlayerCount = 2;
                        break;

                    case 2:
                        GameData.MultiPlayerCount = 1;
                        break;
                }
                _isStart = false;
                break;

            case 3:
                _audioSource.PlayOneShot(startS);

                asyncLoad = SceneManager.LoadSceneAsync("Setumei");
                asyncLoad.allowSceneActivation = false;
                GameData.MultiPlayerCount = 1;
                yield return new WaitForSeconds(1.0f);
                GameData.GameMode = MODE_GAMEMODE.Normal;
                GameData.Difficulty = 0;
                GameData.GoalRound = 0;
                GameData.StartRound = 0;
                
                asyncLoad.allowSceneActivation = true;
                break;
            
            case 4:
                _audioSource.PlayOneShot(startS);

                yield return new WaitForSeconds(0.5f);
                GameData.Language++;
                if (GameData.Language > 2) GameData.Language = 0;
                _isStart = false;
                break;

            case 5:
                _audioSource.PlayOneShot(startS);

                yield return new WaitForSeconds(1.0f);
                Application.Quit();
                break;

        }
    }
    
    public void OnEnd(InputAction.CallbackContext context)
    {
        if (context.performed && !_isStart)
        {
            _titleMode = _optionMax;
            MoveFlash();
        }
    }

    /// <summary>
    /// 人数にコントローラーが足りているかチェック
    /// </summary>
    /// <returns></returns>
    private bool CheckIsConnectController()
    {
        return GameData.GetActiveControllerCount() >= GameData.MultiPlayerCount;
    }
}
