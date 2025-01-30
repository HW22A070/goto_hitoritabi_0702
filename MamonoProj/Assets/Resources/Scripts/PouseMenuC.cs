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


    protected override void Option1() { StartCoroutine(DoOption(0)); }
    protected override void Option2() { StartCoroutine(DoOption(1)); }
    protected override void Option3() { StartCoroutine(DoOption(2)); }

    private IEnumerator DoOption(int mode)
    {
        if (GameData.Pouse)
        {
            if (mode == 0)
            {
                PouseOnAndOff(false);
            }
            else if (mode == 1)
            {
                yield return new WaitForSecondsRealtime(0.5f);
                GameData.Language++;
                if (GameData.Language > 2) GameData.Language = 0;
                start = false;
            }
            else if (mode == 2)
            {
                yield return new WaitForSecondsRealtime(1.0f);
                GameData.PlayerMoveAble = 6;
                TimeManager.ChangeTimeValue(1.0f);
                GameData.Pouse = false;

                if (GameData.Round <= 0)
                {
                    GameData.IsBossFight = false;
                    SceneManager.LoadScene("Title");
                }
                else
                {
                    CrearC._isGiveUp = true;
                    SceneManager.LoadScene("Clear");
                }
            }
        }
    }

    //Imput
    public void OnPouse(InputAction.CallbackContext context)
    {
        start = false;
        //ポーズ処理
        if (context.started)
        {
            _titleMode = 0;
            PouseOnAndOff(true);
        }
    }

    private void PouseOnAndOff(bool pouse)
    {
        _audioSource.PlayOneShot(pouseS);
        if (pouse)
        {
            GameData.Pouse = true;
            _goMenuPouse.SetActive(true);
            _goGameMan.GetComponent<PlayerInput>().SwitchCurrentActionMap("Main");
            GameData.PlayerMoveAble = 0;
            _timeBeforePouse = Time.timeScale;
            TimeManager.ChangeTimeValue(0.0f);
        }
        else
        {
            GameData.Pouse = false;
            GameData.PlayerMoveAble = 6;
            _goGameMan.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            TimeManager.ChangeTimeValue(_timeBeforePouse);
            _goMenuPouse.SetActive(false);
        }
    }
}
