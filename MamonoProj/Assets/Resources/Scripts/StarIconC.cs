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


    // Start is called before the first frame update
    void Start()
    {
        _firstPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = GameData.TP >= _starPoint ? _firstPosition : new Vector3(0, 10000, 0);

    }
}
