using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PouseMenuC : MenuSystemC
{
    [SerializeField]
    [Header("ポーズ効果音")]
    private AudioClip pouseS;

    private float _timeBeforePouse;

    [SerializeField]
    private GameObject _goMenuPouse, _goGameMan;

    private new void Start()
    {
        base.Start();
        _optionMax = 2;
    }


    protected override void Option1() => StartCoroutine(DoOption(0));
    protected override void Option2() => StartCoroutine(DoOption(1));
    protected override void Option3() => StartCoroutine(DoOption(2));

    private IEnumerator DoOption(int mode)
    {
        if (GameData.IsPouse)
        {
            if (mode == 0)
            {
                SetPouseTF(false);
            }
            else if (mode == 1)
            {
                yield return new WaitForSecondsRealtime(0.5f);
                GameData.Language++;
                if (GameData.Language > 2) GameData.Language = 0;
                _isStart = false;
            }
            else if (mode == 2)
            {
                yield return new WaitForSecondsRealtime(1.0f);
                GameData.PlayerMoveAble = 6;
                TimeManager.ChangeTimeValue(1.0f);
                GameData.IsPouse = false;

                if (GameData.Round <= 0)
                {
                    GameData.IsBossFight = false;
                    SceneManager.LoadScene("Title");
                }
                else
                {
                    ClearC._isGiveUp = true;
                    SceneManager.LoadScene("Clear");
                }
            }
        }
    }

    //Imput
    public void OnPouse(InputAction.CallbackContext context)
    {
        _isStart = false;
        //ポーズ処理
        if (context.started)
        {
            _titleMode = 0;
            SetPouseTF(true);
        }
    }

    private void SetPouseTF(bool pouse)
    {
        _audioSource.PlayOneShot(pouseS);
        if (pouse)
        {
            GameData.IsPouse = true;
            _goMenuPouse.SetActive(true);
            _goGameMan.GetComponent<PlayerInput>().SwitchCurrentActionMap("Main");
            GameData.PlayerMoveAble = 0;
            _timeBeforePouse = Time.timeScale;
            TimeManager.ChangeTimeValue(0.0f);
        }
        else
        {
            GameData.IsPouse = false;
            GameData.PlayerMoveAble = 6;
            _goGameMan.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            TimeManager.ChangeTimeValue(_timeBeforePouse);
            _goMenuPouse.SetActive(false);
        }
    }
}
