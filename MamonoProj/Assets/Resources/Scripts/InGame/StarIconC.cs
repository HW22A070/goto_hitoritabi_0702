using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 必殺技ゲージ
/// </summary>
public class StarIconC : MonoBehaviour
{
    private Vector3 _firstPosition;

    [SerializeField]
    private int _starPoint;

    [SerializeField, Min(0)]
    private int _number = 0;

    private GameObject _playerGO;

    private PlayersManagerC _scPlsM;

    // Start is called before the first frame update
    void Start()
    {
        _firstPosition = transform.position;
        _scPlsM = GameObject.Find("PlayersManager").GetComponent<PlayersManagerC>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _playerGO = _scPlsM.GetPlayerByNumber(_number);

        transform.position = _playerGO.GetComponent<PlayerC>().GetTP() >= _starPoint ? _firstPosition : new Vector3(0, 10000, 0);

    }
}
