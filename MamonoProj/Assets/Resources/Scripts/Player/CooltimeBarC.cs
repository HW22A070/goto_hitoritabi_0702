using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooltimeBarC : PlateC
{
    private bool _isCooldown;

    [SerializeField, Header("クールダウンバー伸びるとこ")]
    private GameObject _coolDownBarFill;

    // Start is called before the first frame update
    void Start()
    {
        _playerGO = GameObject.Find("Player");
        _cooltimeBar = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        int weapon = _playerGO.GetComponent<PlayerC>().CheckWeaponValue();
        float cooltime = 1.0f-_playerGO.GetComponent<PlayerC>().CheckCoolTime(weapon);
        //クールタイム中
        if (cooltime < 1)
        {
            _coolDownBarFill.GetComponent<Image>().color = GameData.WeaponColor[weapon];
            
            _cooltimeBar.value = cooltime;
            transform.position = _playerGO.transform.position + (-transform.up * 24);
        }
        else transform.position = new Vector3(0,10000,0);

    }
}
