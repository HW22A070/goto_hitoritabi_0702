using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PBombC : MonoBehaviour
{
    private Vector3 _velocity, _posOwn;

    private float _speed, _speedDelta, _angle, _expCount, _expCounttime=99;
    private int i, hunj;

    [SerializeField]
    [Tooltip("爆発トリガー")]
    private bool sosai;

    /// <summary>
    /// 0=未設定状態
    /// 1=カウントスタート
    /// 2=起爆
    /// </summary>
    private int _expCountMode;

    [SerializeField]
    [Tooltip("爆発物")]
    private PExpC ExpPrefab;

    [SerializeField]
    [Tooltip("爆発物")]
    private ExpC _prhbExpShining;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    public AudioClip expS;
    private int ddd;

    private Coroutine _movingCoroutine;

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

    /// <summary>
    /// 爆発
    /// </summary>
    /// <param name="angle">発射向き</param>
    /// <param name="speed">発射速度</param>
    /// <param name="kasoku">発射加速度</param>
    /// <param name="exp">　起爆カウントダウン</param>
    /// <param name="hunjin">爆発粉塵数</param>
    /// <param name="exptime">粉塵が消えるまで</param>
    public void EShot1(float angle, float speed, float kasoku, float exp, int hunjin, float exptime)
    {
        
        var direction = GameData.GetDirection(angle);
        _velocity = direction * speed;
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        _speed = speed;
        _speedDelta = kasoku;
        _angle = angle;
        _expCount = exp;
        _expCounttime = exptime;
        hunj = hunjin;
        _expCountMode = 1;
    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        if (_expCountMode == 1)
        {
            _posOwn = transform.position;

            transform.localPosition += _velocity;
            _speed += _speedDelta;
            var direction = GameData.GetDirection(_angle);
            _velocity = direction * _speed;

            //time_ex
            _expCount--;
            if (_expCount <= 0) EXPEffect(hunj);

            if (GetComponent<PMCoreC>().DeleteMissileCheck())
            {
                EXPEffect(hunj);
            }
        }
    }

    private void EXPEffect(int hun)
    {
        if (_expCountMode == 1)
        {
            _expCountMode = 2;
            StartCoroutine(Explosion(hun));
        }
    }

    private IEnumerator Explosion(int hunj)
    {
        _posOwn = transform.position;
        _audioGO.PlayOneShot(expS);
        _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(3, 6);

        ExpEffect(4);

        for (int j = 0; j < hunj; j++)
        {
            for(int k = 0; k < 2; k++)
            {
                Vector3 direction2 = new Vector3(Random.Range(10, 630), Random.Range(10, 470), 0);
                Instantiate(ExpPrefab, _posOwn, transform.localRotation).EShot1(Random.Range(0,360)/*((360 / hunj) * j)+(k*180)*/, 10, _expCounttime);
            }

            yield return new WaitForSeconds(0.03f);
        }
        Destroy(gameObject);
    }

    private void ExpEffect(int shiningValue)
    {
        Instantiate(_prhbExpShining, _posOwn, Quaternion.Euler(0, 0, 0)).EShot1(0, 0, 0.3f);
        for (int i = 0; i < shiningValue; i++)
        {
            Instantiate(_prhbExpShining, _posOwn + new Vector3(Random.Range(-48, 48), Random.Range(-48, 48), 0), Quaternion.Euler(0, 0, 0))
                .EShot1(0, 0, 0.3f);
        }
    }


}
