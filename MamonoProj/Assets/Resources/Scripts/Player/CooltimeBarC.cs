using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumDic.Player;

public class CooltimeBarC : PlateC
{
    private bool _isCooldown;

    [SerializeField, Header("クールダウンバー伸びるとこ")]
    private GameObject _coolDownBarFill;

    private Image _imgCoolDownBarFill;

    private Color32 _defaultColor;

    /// <summary>
    /// バーが警告モードになる時間;
    /// </summary>
    private float _ratioWarning = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        _playerGO = GameObject.Find("Player");
        _cooltimeBar = GetComponent<Slider>();
        _imgCoolDownBarFill = _coolDownBarFill.GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        MODE_GUN weapon = _playerGO.GetComponent<PlayerC>().CheckWeaponValue();
        float cooltime = 1.0f-_playerGO.GetComponent<PlayerC>().GetCoolTime(weapon);
        //クールタイム中
        if (cooltime < 1)
        {
            _cooltimeBar.value = cooltime;
            transform.localPosition = Vector3.zero;
        }
        else transform.localPosition = new Vector3(0,GameData.WindowSize.y*100,0);

    }

    private void FixedUpdate()
    {
        MODE_GUN weapon = _playerGO.GetComponent<PlayerC>().CheckWeaponValue();
        float cooltime = 1.0f - _playerGO.GetComponent<PlayerC>().GetCoolTime(weapon);
        //クールタイム中

        _defaultColor = GameData.WeaponColor[(int)weapon];

        if (cooltime < _ratioWarning || !_playerGO.GetComponent<PlayerC>().CheckIsLoaded(weapon))
        {
            _cooltime+=10;
            if (_cooltime >= 180) _cooltime = 0;
            float lightSin = 1.0f-(Mathf.Sin(_cooltime * Mathf.Deg2Rad));
            _imgCoolDownBarFill.color = new Color32((byte)(_defaultColor.r+(255- _defaultColor.r) * (1.0f-lightSin)), (byte)(_defaultColor.g* lightSin), (byte)(_defaultColor.b * lightSin), 255);
        }
        else
        {
            _cooltime = 0;
                _imgCoolDownBarFill.color = _defaultColor;
        }
    }
}
