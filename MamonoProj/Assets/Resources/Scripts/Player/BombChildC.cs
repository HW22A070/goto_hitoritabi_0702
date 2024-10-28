using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombChildC : MonoBehaviour
{
    private Vector3 _ppos, _pos, _posDelta, _posOfset;

    private GameObject playerGO;

    private PlayerC _scPlayer;


    [SerializeField, Tooltip("弾アタッチ")]
    private PBombC _prfbBomb;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerGO = GameObject.Find("Player");
        _scPlayer = playerGO.GetComponent<PlayerC>();
    }

    // Update is called once per frame
    void Update()
    {
        _pos = transform.position;
        _ppos = playerGO.transform.position + _posOfset;
        _posDelta = GameData.GetSneaking(_pos, _ppos, 4);
    }

    private void FixedUpdate()
    {
        transform.position += _posDelta;

        if (PlayerC.muki==1)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    public void DoAttackDrop()
    {
        _pos = transform.position;
        Instantiate(_prfbBomb, _pos-(transform.right*24), transform.rotation).EShot1(270, 0, 1.0f, 100, 6, 0.5f);
        Instantiate(_prfbBomb, _pos, transform.rotation).EShot1(270, 0, 1.1f, 100, 6, 0.5f);
        Instantiate(_prfbBomb, _pos +(transform.right * 24), transform.rotation).EShot1(270, 0, 0.9f, 100, 6, 0.5f);
    }

    public void SetOfset(Vector3 ofset)
    {
        _posOfset = ofset;
    }
    
}
