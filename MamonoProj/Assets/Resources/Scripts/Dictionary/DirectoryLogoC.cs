using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using EnumDic.System;
using EnumDic.Enemy;

public class DirectoryLogoC : MonoBehaviour
{
    private AudioSource _asOwn;

    [SerializeField]
    protected AudioClip startS, faildS, selectS;

    private float _prologueTimer = 0;

    [SerializeField]
    private float _prologueFireTimeS = 20;

    [SerializeField]
    private WarningWindowC _goWindow;

    public bool _isStart;

    [SerializeField]
    protected SpriteRenderer[] _iconSprites;

    [SerializeField]
    protected Color _on;

    protected Color[] _original = new Color[10];

    /// <summary>
    /// ÉçÉSÇëIÇÒÇ≈Ç¢ÇÈèÍèä
    /// </summary>
    private int _posLogoX = 0, _posLogoY = 4;

    /// <summary>
    /// ÉçÉSÇëIÇÒÇ≈Ç¢ÇÈèÍèä
    /// </summary>
    private int _posLogoMaxX = 4, _posLogoMaxY = 8;

    [SerializeField]
    private Transform _tsLogoStart;

    [SerializeField]
    private DicLogoC _prfbDicLogoC;


    private void Start()
    {
        for (int i = 0; i < Enum.GetNames(typeof(KIND_ENEMY)).Length + Enum.GetNames(typeof(KIND_BOSS)).Length-2; i++)
        {
            if(i< Enum.GetNames(typeof(KIND_ENEMY)).Length)
            {
                Instantiate(_prfbDicLogoC
    , _tsLogoStart.position + new Vector3(i % _posLogoMaxX, -(i / _posLogoMaxX), 0) * 48
    , Quaternion.Euler(0, 0, 0)).SetSprite((KIND_ENEMY)i, i % _posLogoMaxX, i / _posLogoMaxY);
            }

            else
            {
                Instantiate(_prfbDicLogoC
    , _tsLogoStart.position + new Vector3(i % _posLogoMaxX, -(i / _posLogoMaxX), 0) * 48
    , Quaternion.Euler(0, 0, 0)).SetSprite((KIND_BOSS)i+1, i % _posLogoMaxX, i / _posLogoMaxY);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnUp(InputAction.CallbackContext context)
    {
        if (context.started && !_isStart)
        {
            if(_posLogoY<=0) _posLogoY = _posLogoMaxY;
            else _posLogoY--;

            MoveFlash();
        }
    }

    public void OnDown(InputAction.CallbackContext context)
    {
        if (context.started && !_isStart)
        {
            if (_posLogoY >= _posLogoMaxY) _posLogoY =0;
            else _posLogoY ++;

            MoveFlash();
        }
    }

    public void OnRight(InputAction.CallbackContext context)
    {
        if (context.started && !_isStart)
        {
            if (_posLogoX <= 0) _posLogoX = _posLogoMaxX;
            else _posLogoX--;
            MoveFlash();
        }
    }
    public void OnLeft(InputAction.CallbackContext context)
    {
        if (context.started && !_isStart)
        {
            if (_posLogoX >= _posLogoMaxX) _posLogoX = 0;
            else _posLogoX++;

            MoveFlash();
        }
    }

    protected void MoveFlash()
    {
        _asOwn.PlayOneShot(selectS);
        for (int i = 0; i < _iconSprites.Length; i++)
        {

            if (i == _posLogoX % _posLogoMaxX && i == _posLogoY % _posLogoMaxY) _iconSprites[i].color = _on;
            else _iconSprites[i].color = _original[i];

        }
    }

}
