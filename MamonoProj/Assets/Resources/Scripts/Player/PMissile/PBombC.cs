using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PBombC : MonoBehaviour
{
    private Vector3 velocity, pos;

    private float sspeed, kkaso, aang, eexp, eexptim=99;
    private int i, hunj;

    [SerializeField]
    [Tooltip("爆発トリガー")]
    private bool sosai;

    /// <summary>
    /// 0=未設定状態
    /// 1=カウントスタート
    /// 2=起爆
    /// </summary>
    private int _expMode;

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
        velocity = direction * speed;
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        sspeed = speed;
        kkaso = kasoku;
        aang = angle;
        eexp = exp;
        eexptim = exptime;
        hunj = hunjin;
        _expMode = 1;
    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        if (_expMode == 1)
        {
            pos = transform.position;

            transform.localPosition += velocity;
            sspeed += kkaso;
            var direction = GameData.GetDirection(aang);
            velocity = direction * sspeed;

            //time_ex
            eexp--;
            if (eexp <= 0) EXPEffect(hunj);

            if (GetComponent<PMCoreC>().DeleteMissileCheck())
            {
                EXPEffect(hunj);
            }
        }
    }

    private void EXPEffect(int hun)
    {
        if (_expMode == 1)
        {
            _expMode = 2;
            StartCoroutine(Explosion(hun));
        }
    }

    private IEnumerator Explosion(int hunj)
    {
        pos = transform.position;
        _audioGO.PlayOneShot(expS);
        _goCamera.GetComponent<CameraC>().StartShakeVertical(3, 6);

        ExpEffect(4);

        for (int j = 0; j < hunj; j++)
        {
            for(int k = 0; k < 2; k++)
            {
                Vector3 direction2 = new Vector3(Random.Range(10, 630), Random.Range(10, 470), 0);
                Instantiate(ExpPrefab, pos, transform.localRotation).EShot1(Random.Range(0,360)/*((360 / hunj) * j)+(k*180)*/, 10, eexptim);
            }

            yield return new WaitForSeconds(0.03f);
        }
        Destroy(gameObject);
    }

    private void ExpEffect(int shiningValue)
    {
        Instantiate(_prhbExpShining, pos, Quaternion.Euler(0, 0, 0)).EShot1(0, 0, 0.3f);
        for (int i = 0; i < shiningValue; i++)
        {
            Instantiate(_prhbExpShining, pos + new Vector3(Random.Range(-48, 48), Random.Range(-48, 48), 0), Quaternion.Euler(0, 0, 0))
                .EShot1(0, 0, 0.3f);
        }
    }


}
