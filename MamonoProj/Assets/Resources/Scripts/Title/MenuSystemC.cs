using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuSystemC : MonoBehaviour
{
    [SerializeField]
    protected AudioClip startS,faildS, selectS;

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

    protected Color[] _original=new Color[10];

    protected void Start()
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
    }

    protected void Load()
    {
        if (_titleMode == 0) Option1();
        else if (_titleMode == 1)Option2();
        else if (_titleMode == 2)Option3();
        else if (_titleMode == 3)Option4();
        else if (_titleMode == 4) Option5();
        else if (_titleMode == 5) Option6();
        else if (_titleMode == 6) Option7();
        else if (_titleMode == 7) Option8();
        else if (_titleMode == 8) Option9();
        else if (_titleMode == 9) Option10();
    }

    //テンプレート群
    protected virtual void Option1() { }
    protected virtual void Option2() { }
    protected virtual void Option3() { }
    protected virtual void Option4() { }
    protected virtual void Option5() { }
    protected virtual void Option6() { }
    protected virtual void Option7() { }
    protected virtual void Option8() { }
    protected virtual void Option9() { }
    protected virtual void Option10() { }

    protected void MoveFlash()
    {
        for (int i = 0; i < _iconSprites.Length; i++)
        {
            if (i == _titleMode) _iconSprites[i].color = _on;
            else _iconSprites[i].color = _original[i];
        }
    }

    //Imput
    public virtual void OnStart(InputAction.CallbackContext context)
    {
        if (context.started && !_isStart)
        {
            _isStart = true;
            Load();
        }
    }

    public virtual void OnUp(InputAction.CallbackContext context)
    {
        if (context.started && !_isStart)
        {
            _titleMode--;
            if (_titleMode < 0) _titleMode = _optionMax;
            MoveFlash();
            _audioSource.PlayOneShot(selectS);
        }
    }

    public virtual void OnDown(InputAction.CallbackContext context)
    {
        if (context.started && !_isStart)
        {
            _titleMode++;
            if (_titleMode > _optionMax) _titleMode = 0;
            MoveFlash();
            _audioSource.PlayOneShot(selectS);
        }
    }
    public virtual void OnRight(InputAction.CallbackContext context)
    {
        if (context.started && !_isStart)
        {

        }
    }
    public virtual void OnLeft(InputAction.CallbackContext context)
    {
        if (context.started && !_isStart)
        {

        }
    }
}
