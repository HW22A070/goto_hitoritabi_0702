using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PouseMenuC : MonoBehaviour
{
    [SerializeField]
    protected AudioClip startS, selectS;

    protected AudioSource _audioSource;

    protected bool _isStart = false;

    [SerializeField]
    protected short _defaltMode = 0;


    protected short _optionMax = 4;

    /// <summary>
    /// 0=プレイ
    /// 1=チュートリアル
    /// 2=おわる
    /// </summary>
    protected short _titleMode = 0;

    [SerializeField]
    protected SpriteRenderer[] _iconSprites;

    [SerializeField]
    protected Color _on;

    protected Color[] _original = new Color[10];


    [SerializeField]
    [Header("ポーズ効果音")]
    private AudioClip pouseS;

    private float _timeBeforePouse;

    [SerializeField]
    private GameObject _goMenuPouse, _goGameMan;

    [SerializeField]
    private PlayersManagerC _scPlsM;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _titleMode = _defaltMode;

        for (int i = 0; i < _iconSprites.Length; i++)
        {
            _original[i] = _iconSprites[i].color;
        }
        MoveFlash();

        var gamepad = Gamepad.current;
        if (gamepad != null) gamepad.SetMotorSpeeds(0.0f, 0.0f);

        _optionMax = 2;
    }

    protected void MoveFlash()
    {
        for (int i = 0; i < _iconSprites.Length; i++)
        {
            if (i == _titleMode) _iconSprites[i].color = _on;
            else _iconSprites[i].color = _original[i];
        }
    }

    protected void Load()
    {
        Debug.Log("Loaded");
        if (_titleMode == 0) Option1();
        else if (_titleMode == 1) Option2();
        else if (_titleMode == 2) Option3();
    }

    protected void Option1() => StartCoroutine(DoOption(0));
    protected void Option2() => StartCoroutine(DoOption(1));
    protected void Option3() => StartCoroutine(DoOption(2));

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

    public void OnPouse()
    {
        _isStart = false;
        _titleMode = 0;
        SetPouseTF(true);
    }

    //Imput
    public void OnStart()
    {
        if (!_isStart)
        {
            _isStart = true;
            Load();
            _audioSource.PlayOneShot(startS);
        }
    }

    public void OnUp()
    {
        if (!_isStart)
        {
            _titleMode--;
            if (_titleMode < 0) _titleMode = _optionMax;
            MoveFlash();
            _audioSource.PlayOneShot(selectS);
        }
    }
    public void OnDown()
    {
        if (!_isStart)
        {
            _titleMode++;
            if (_titleMode > _optionMax) _titleMode = 0;
            MoveFlash();
            _audioSource.PlayOneShot(selectS);
        }
    }

    private void SetPouseTF(bool pouse)
    {
        _audioSource.PlayOneShot(pouseS);
        if (pouse)
        {
            GameData.IsPouse = true;
            _goMenuPouse.SetActive(true);
            foreach(GameObject player in _scPlsM.GetAllPlayers())
            {
                player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Main");
            }
            GameData.PlayerMoveAble = 0;
            _timeBeforePouse = Time.timeScale;
            TimeManager.ChangeTimeValue(0.0f);
        }
        else
        {
            GameData.IsPouse = false;
            GameData.PlayerMoveAble = 6;
            foreach (GameObject player in _scPlsM.GetAllPlayers())
            {
                player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            }
            TimeManager.ChangeTimeValue(_timeBeforePouse);
            _goMenuPouse.SetActive(false);
        }
    }
}
