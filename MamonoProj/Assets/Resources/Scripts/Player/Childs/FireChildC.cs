using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireChildC : ChildsCoreC
{

    [SerializeField, Tooltip("弾アタッチ")]
    private PBombC _prfbBomb;

    [SerializeField]
    private GameObject _goUnit01, _goUnit02, _goUnit03;

    private float _radius = 0;

    private float _unitSpeedMax=1,_unitSpeedDelta;

    [SerializeField, Tooltip("バーン必殺")]
    private PFireBallC PFireBallP;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _unitSpeedDelta = _unitSpeedMax;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerGO = GameObject.Find("Player");
        _scPlayer = playerGO.GetComponent<PlayerC>();
    }

    // Update is called once per frame
    void Update()
    {
        _radius+=0.3f;


        _pos = transform.position;
        _posPlayer = playerGO.transform.position +new Vector3(_scPlayer.CheckPlayerAngleIsRight() ? -24 : 24, Mathf.Cos(_radius * Mathf.Deg2Rad)*4, 0);
        _posDelta = GameData.GetSneaking(_pos, _posPlayer, 3);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        while (_radius >= 360.0f)
        {
            _radius -= 360.0f;
        }

        transform.position += _posDelta;

        if (_scPlayer.CheckPlayerAngleIsRight())
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        if (_unitSpeedDelta > _unitSpeedMax) _unitSpeedDelta -= 0.5f;

        _goUnit01.transform.eulerAngles += transform.forward* _unitSpeedDelta;
        _goUnit02.transform.eulerAngles -= transform.forward * _unitSpeedDelta*1.5f;
        _goUnit03.transform.eulerAngles -= transform.forward * (((int)_radius%120)/115)*12;
    }

    public void DoAttackRocket()
    {
        _pos = transform.position;
        _unitSpeedDelta = 30.0f;
        Instantiate(_prfbBomb, _pos, transform.rotation).EShot1((_scPlayer.CheckPlayerAngleIsRight() ? 0 : 180) + Random.Range(-5, 5), 20, -0.3f, 660, 20, 2.0f);
    }

    public void DoAttackBress()
    {
       if(_unitSpeedDelta<15) _unitSpeedDelta += 1.5f;
    }

    public void DoAttackSpecial() => StartCoroutine(PlayFireBall());

    private IEnumerator PlayFireBall()
    {
        for (int j = 0; j < 360; j+=36)
        {
            Vector3 posOfset = new Vector3(Mathf.Sin(j * Mathf.Deg2Rad), Mathf.Cos(j * Mathf.Deg2Rad), 0) * 64;
            Instantiate(PFireBallP, _pos, transform.rotation).SetFirstPos(_pos + posOfset);
            yield return new WaitForSeconds(0.7f);
        }

    }
}
