using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletChildC : MonoBehaviour
{
    private Vector3 _ppos,_pos, _posDelta,_posOfset,_shotPos;

    private GameObject playerGO;

    private PlayerC _scPlayer;

    private float angle, _ownAngle;

    /// <summary>
    /// ターゲット
    /// </summary>
    public Vector3 GOTarget;

    [SerializeField, Tooltip("弾アタッチ")]
    private PMissile PBulletP, PRifleP;

    [SerializeField, Tooltip("エフェクトアタッチ")]
    private ExpC _prhbBulletShot;

    private SpriteRenderer spriteRenderer;

    private bool _isTarget=false;

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
        _shotPos = _pos + transform.right * 32;
        _ppos = playerGO.transform.position + _posOfset+new Vector3(24-PlayerC.muki*48,0,0);
        _posDelta = GameData.GetSneaking(_pos, _ppos, 4);
        _isTarget = _scPlayer.GetFlontEnemy()!= null;
        if (_isTarget) GOTarget = _scPlayer.GetFlontEnemy().transform.position;
    }

    private void FixedUpdate()
    {
        transform.position += _posDelta;

        if (_isTarget) angle = GameData.GetAngle(_pos, GOTarget);
        else angle= 180 - (PlayerC.muki * 180);


        if (_ownAngle != angle)
        {
            _ownAngle = angle;
        }
        
        transform.eulerAngles = transform.forward * _ownAngle++;

        if (90 < angle && angle < 270)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }
    }

    public void SetOfset(Vector3 ofset)
    {
        _posOfset = ofset;
    }

    public void DoAttackSniper()
    {
        float angle;
        if (_isTarget) angle = GameData.GetAngle(_pos, GOTarget);
        else angle = 180 - (PlayerC.muki * 180);
        PMissile shot = Instantiate(PRifleP, _shotPos, transform.rotation);
        shot.Shot(angle + Random.Range(-1, 2), 0, 32);
        shot.transform.position += shot.transform.up * 128;
        BulletEffect();
        transform.position -= transform.right * 64;

    }

    /// <summary>
    /// 銃弾発射光エフェクト
    /// </summary>
    private void BulletEffect()
    {
        //発射エフェクト
        ExpC bulletEf = Instantiate(_prhbBulletShot, _shotPos+(transform.right*64), transform.rotation);
        bulletEf.transform.parent = transform;
        bulletEf.EShot1(angle, 0, 0.12f);
    }
}
