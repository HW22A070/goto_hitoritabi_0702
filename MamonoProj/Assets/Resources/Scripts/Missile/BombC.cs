using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombC : MonoBehaviour
{
    protected Vector3 velocity, pos;
    protected float sspeed, kkaso, aang, eexp, eexptim;
    protected int i, hunj;

    /// <summary>
    /// スピーカ
    /// </summary>
    protected private AudioSource _audioGO;

    [SerializeField]
    protected ExpC ExpPrefab;

    [SerializeField]
    protected AudioClip expS;

    [SerializeField]
    protected bool bombbarrier = true, bombsosai;

    [SerializeField]
    [Tooltip("爆発物")]
    protected ExpC _prhbExpShining;

    // Start is called before the first frame update
    protected void Start()
    {
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

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
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        pos = transform.position;

        transform.localPosition += velocity;
        sspeed += kkaso;
        var direction = GameData.GetDirection(aang);
        velocity = direction * sspeed;

        //time_ex
        eexp--;
        if (eexp <= 0) Explosion();

        if (GetComponent<EMCoreC>().DeleteMissileCheck()) Explosion();
    }

    public void Explosion()
    {
        if (_prhbExpShining != null) ExpEffect(4);

        for (i = 0; i < hunj; i++)
        {
            
            Vector3 direction2 = new Vector3(Random.Range(10, 630), Random.Range(10, 470), 0);
            float angle2 = Random.Range(0, 360);
            Quaternion rot2 = transform.localRotation;
            ExpC shot2 = Instantiate(ExpPrefab, pos, rot2);
            shot2.EShot1(angle2, Random.Range(1, 10.0f), eexptim);
        }
        _audioGO.PlayOneShot(expS);
        Destroy(gameObject);
    }

    protected void ExpEffect(int shiningValue)
    {
        Instantiate(_prhbExpShining, pos, Quaternion.Euler(0, 0, 0)).EShot1(0, 0, 0.3f);
        for (int i = 0; i < shiningValue; i++)
        {
            Instantiate(_prhbExpShining, pos + new Vector3(Random.Range(-48, 48), Random.Range(-48, 48), 0), Quaternion.Euler(0, 0, 0))
                .EShot1(0, 0, 0.3f);
        }
    }

}
