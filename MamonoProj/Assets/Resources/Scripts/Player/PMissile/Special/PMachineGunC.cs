using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMachineGunC : MonoBehaviour
{
    private Vector3 velocity, pos, fpos;

    public PMissile PBulletP;
    public PExpC PExpC;

    [SerializeField, Tooltip("エフェクトアタッチ")]
    private ExpC _prhbBulletShot;

    private float angle,_ownAngle=0;

    private float time = 7.0f;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    public AudioClip shotS,breakS;

    public SpriteRenderer spriteRenderer;

    /// <summary>
    /// PlayerGameObject
    /// </summary>
    private GameObject _playerGO;

    // Start is called before the first frame update
    void Start()
    {
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        _playerGO = GameObject.Find("Player");
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            for (int k = 1; k < 4; k++) {
                for (int j = 0; j < 360; j+=10)
                {
                    Quaternion rot = transform.localRotation;
                    PExpC shot2 = Instantiate(PExpC, fpos + pos, rot);
                    shot2.EShot1(j, 5*k, 3);
                }
            }
            _audioGO.PlayOneShot(breakS);
            Destroy(gameObject);
        }
    }

    /*//input
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started) Fireing = true;
        else if (context.canceled) Fireing = false;
    }*/

    // Update is called once per frame
    void FixedUpdate()
    {
        pos = _playerGO.transform.position;

        Quaternion rot = transform.localRotation;
        angle = GetTagPosition();


        if (_ownAngle!= angle)
        {
            _ownAngle = angle;
            /*
            _ownAngle++;
            if (_ownAngle >= 360) _ownAngle = 0;
            */
        }

        fpos = new Vector3(Mathf.Cos(_ownAngle++ * Mathf.Deg2Rad) * 64, Mathf.Sin(_ownAngle++ * Mathf.Deg2Rad) * 64, 0);
        transform.eulerAngles = transform.forward * _ownAngle++;

        for (int i = 0; i < 1; i++)
        {
            PMissile shot = Instantiate(PBulletP, fpos+pos+(transform.right*64), rot);
            shot.Shot(angle + Random.Range(-3, 3), Random.Range(70, 90), 0);
        }

        //発射エフェクト
        ExpC bulletEf = Instantiate(_prhbBulletShot, fpos + pos + (transform.right * 32), rot);
        bulletEf.transform.parent = transform;
        bulletEf.EShot1(angle, 0, 0.02f);
        //bulletEf.transform.position += bulletEf.transform.up * 24;


        if (90<angle&&angle<270)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }

        _audioGO.PlayOneShot(shotS);
        transform.localPosition = fpos;
    }



    private float GetTagPosition()
    {
        GameObject[] myObjects;
        myObjects = GameObject.FindGameObjectsWithTag("Enemy");
        if (myObjects.Length > 0) return GameData.GetAngle(pos, myObjects[Random.Range(0, myObjects.Length)].transform.position);
        else
        {
            if (GameData.Round == 0)
            {
                myObjects = GameObject.FindGameObjectsWithTag("Target");
                if (myObjects.Length > 0) return GameData.GetAngle(pos, myObjects[Random.Range(0, myObjects.Length)].transform.position);
            }
            return 180 - (PlayerC.muki * 180);
        }
    }
}
