using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFireBallC : MonoBehaviour
{
    private Vector3 _pos, _posPlayer, _firstPos, _posDelta;

    private float _waitSecond = 2;

    private bool _isAttack;

    /// <summary>
    /// 追跡中の敵
    /// </summary>
    private GameObject _goTarget;

    /// <summary>
    /// 0=待機中
    /// 1=カウントスタート
    /// 2=起爆
    /// </summary>
    private int _expMode;

    [SerializeField]
    [Tooltip("爆発物")]
    private PExpC ExpPrefab;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    public AudioClip expS;

    /// <summary>
    /// カメラオブジェクト
    /// </summary>
    private GameObject _goCamera;


    // Start is called before the first frame update
    void Start()
    {
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        _goCamera = GameObject.Find("Main Camera");
    }

    public void SetFirstPos(Vector3 firstPos)
    {
        _firstPos = firstPos;
    }

    // Update is called once per frame
    void Update()
    {
        _pos = transform.position;
        //待機中
        if (_expMode == 0)
        {
            _posDelta =GameData.GetSneaking(_pos, _firstPos, 20);
            _waitSecond -= Time.deltaTime;
            if (_waitSecond <= 0)
            {
                _goTarget = GetTagPosition();
                _expMode = 1;
            }
        }
        else if(_expMode==1)
        {
            //ターゲットがあれば追跡。なければ爆発しておわり
            if (_goTarget != null)
            {
                _posDelta = GameData.GetDirection(GameData.GetAngle(_pos, _goTarget.transform.position))*16;
            }
            else
            {
                EXPEffect();
            }

            if (GetComponent<PMCoreC>().DeleteMissileCheck())
            {
                EXPEffect();
            }
        }
    }

    void FixedUpdate() => transform.position += _posDelta;

    private void EXPEffect()
    {
        if (_expMode == 1)
        {
            _expMode = 2;
            StartCoroutine(Explosion());
        }
    }

    private IEnumerator Explosion()
    {
        _pos = transform.position;
        _audioGO.PlayOneShot(expS);
        _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(3, 6);
        for (int j = 0; j < 10; j++)
        {
            for (int k = 0; k < 2; k++)
            {
                Vector3 direction2 = new Vector3(Random.Range(10, 630), Random.Range(10, 470), 0);
                Instantiate(ExpPrefab, _pos, transform.localRotation).EShot1(Random.Range(0, 360)/*((360 / hunj) * j)+(k*180)*/, 10, 0.5f);
            }

            yield return new WaitForSeconds(0.03f);
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// 敵場所特定、ランダムに返す
    /// </summary>
    /// <returns></returns>
    private GameObject GetTagPosition()
    {
        GameObject[] myObjects;
        myObjects = GameObject.FindGameObjectsWithTag("Enemy");
        if (myObjects.Length > 0)
        {
            return myObjects[Random.Range(0,myObjects.Length)];
        }
        else
        {
            if (GameData.Round == 0)
            {
                myObjects = GameObject.FindGameObjectsWithTag("Target");
                if (myObjects.Length > 0)
                {
                    return myObjects[Random.Range(0, myObjects.Length)];
                }
            }
            return null;
        }
    }
}
