using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumDic.Player;

[DefaultExecutionOrder(1)]
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

    // Update is called once per frame
    private void FixedUpdate()
    {
        _goPlayer = GameObject.FindWithTag("Player");
        _cooltimeBar.fillAmount = _goPlayer.GetComponent<PlayerC>().GetCoolTime(_modeWeapon);
    }
}
