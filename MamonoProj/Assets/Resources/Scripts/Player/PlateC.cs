using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateC : MonoBehaviour
{
    /// <summary>
    /// プレイヤーオブジェクト
    /// </summary>
    protected GameObject _playerGO;

    [SerializeField]
    private int weapon = 0;

    protected float _cooltime;

    /// <summary>
    /// クールタイムバー
    /// </summary>
    protected Slider _cooltimeBar;

    // Start is called before the first frame update
    private void Start()
    {
        _playerGO = GameObject.Find("Player");
        _cooltimeBar = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _cooltimeBar.value=_playerGO.GetComponent<PlayerC>().CheckCoolTime(weapon);
    }
}
