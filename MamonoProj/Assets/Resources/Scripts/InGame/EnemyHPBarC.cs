using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 敵のHPバー制御
/// </summary>
public class EnemyHPBarC : MonoBehaviour
{
    private ECoreC _scEnemy;

    private int _beforeHP;

    private byte _byteUnBlue = 0;

    private Slider _sliderHP;

    [SerializeField]
    private Image _imageHP;

    private bool _isBoss;

    private int _ofsetX;
    
    private void Start()
    {
        GetComponets();
        if (_scEnemy.CheckIsBoss()) _ofsetX = 100;
        else _ofsetX = 20;
    }

    private void GetComponets()
    {
        _scEnemy = transform.parent.parent.gameObject.GetComponent<ECoreC>();
        _sliderHP = GetComponent<Slider>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        int nowHP = _scEnemy.TotalHp;

        //変化を検知しダメージを観測
        if (_beforeHP> nowHP)
        {
            //バーの色変化
            _byteUnBlue= 255;
        }
        else
        {
            //
            if (_byteUnBlue > 0)
            {
                _byteUnBlue -= 255/15;
                if (_byteUnBlue < 0) _byteUnBlue = 0;
            }
        }
        _imageHP.color = new Color32(_byteUnBlue, _byteUnBlue,255 , 255);
        _beforeHP = nowHP;

        //バーに反映
        _sliderHP.value = (float)nowHP / (float)_scEnemy.TotalHpFirst;

        //角度を場所を固定
        transform.eulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;

        SetPositionInWindow();
    }

    /// <summary>
    /// 画面外に飛び出すのを防ぐ
    /// </summary>
    private void SetPositionInWindow()
    {
        if (transform.position.y < 10) transform.position = new Vector3(transform.position.x, 10, 0);
        if (transform.position.y > GameData.WindowSize.x - 10) transform.position = new Vector3(transform.position.x, GameData.WindowSize.x - 10, 0);
        if (transform.position.x < _ofsetX) transform.position = new Vector3(_ofsetX, transform.position.y, 0);
        if (transform.position.x > GameData.WindowSize.x - _ofsetX) transform.position = new Vector3(GameData.WindowSize.x - _ofsetX, transform.position.y, 0);
    }
}
