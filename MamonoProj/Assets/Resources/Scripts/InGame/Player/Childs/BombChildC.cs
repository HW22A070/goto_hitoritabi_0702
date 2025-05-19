using Move2DSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombChildC : ChildsCoreC
{
    private Vector3  _posOfset;

    [SerializeField, Tooltip("弾アタッチ")]
    private PBombC _prfbBomb;
    
    [SerializeField]
    private Sprite _spNormal, _spSpesial;

    // Update is called once per frame
    void Update()
    {
        _pos = transform.position;
        _posPlayer = _playerGO.transform.position + _posOfset;
        _posDelta = Moving2DSystems.GetSneaking(_pos, _posPlayer, 4);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        transform.position += _posDelta;

        if (_scPlayer.CheckPlayerAngleIsRight())
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        SetSpriteBySpecial();
    }

    public void DoAttackDrop()
    {
        _pos = transform.position;
        Instantiate(_prfbBomb, _pos-(transform.right*24), transform.rotation).ShotBomb(270, 0, 1.0f, 100, 64);
        Instantiate(_prfbBomb, _pos, transform.rotation).ShotBomb(270, 0, 1.1f, 100, 64);
        Instantiate(_prfbBomb, _pos +(transform.right * 24), transform.rotation).ShotBomb(270, 0, 0.9f, 100, 64);
    }

    public void SetOfset(Vector3 ofset)
    {
        _posOfset = ofset;
    }

    private void SetSpriteBySpecial()
    {
        spriteRenderer.sprite = _scPlayer.CheckIsSpecial() ? _spSpesial : _spNormal;
    }

}
