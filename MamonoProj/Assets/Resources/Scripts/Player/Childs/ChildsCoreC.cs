using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDic.Player;

public class ChildsCoreC : MonoBehaviour
{
    protected Vector3 _posPlayer, _pos, _posDelta;

    protected GameObject playerGO;

    protected PlayerC _scPlayer;

    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    protected MODE_GUN _modeWeapon;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerGO = GameObject.Find("Player");
        _scPlayer = playerGO.GetComponent<PlayerC>();
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (_scPlayer.CheckIsAbleChargeShot(_modeWeapon)) spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        else spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 1f);

    }
}
