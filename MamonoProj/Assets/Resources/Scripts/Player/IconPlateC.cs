using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumDic.Player;

public class IconPlateC : MonoBehaviour
{
    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    protected GameObject _goPlayer;

    [SerializeField]
    private MODE_GUN _modeWeapon;

    protected float _cooltime;

    /// <summary>
    /// クールタイムバー
    /// </summary>
    [SerializeField]
    protected Image _cooltimeBar;

    // Start is called before the first frame update
    private void Start()
    {
        _goPlayer = GameObject.Find("Player");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _cooltimeBar.fillAmount = _goPlayer.GetComponent<PlayerC>().GetCoolTime(_modeWeapon);
    }
}
