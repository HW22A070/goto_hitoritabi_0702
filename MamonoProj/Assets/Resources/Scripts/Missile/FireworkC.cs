using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkC : MonoBehaviour
{
    private Vector3 _velocity, _posOwn;
    private float _speed, _speedDelta, _angle, _expCount, _expCountTime;
    private int i,j, hunj;

    [SerializeField]
    private bool up, down, right, left;

    [SerializeField]
    private ExpC ExpPrefab;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    [SerializeField]
    private AudioClip expS;

    // Start is called before the first frame update
    void Start()
    {
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

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
        _expCountTime = exptime;
        hunj = hunjin;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _posOwn = transform.position;

        transform.localPosition += _velocity;
        _speed += _speedDelta;
        var direction = GameData.GetDirection(_angle);
        _velocity = direction * _speed;

        //time_ex
        _expCount--;
        if (_expCount <= 0) Explosion();

        if (GetComponent<EMCoreC>().DeleteMissileCheck()) Explosion();
    }

    private void Explosion()
    {
        if (Random.Range(0, 2) == 0)
        {
            for (j = 1; j < 4; j++)
            {
                for (i = 0; i < 20; i++)
                {
                    Quaternion rot2 = transform.localRotation;
                    ExpC shot2 = Instantiate(ExpPrefab, _posOwn, rot2);
                    shot2.EShot1(i * 18, j * 3, _expCountTime);
                }
            }
        }
        else
        {
            for (i = 0; i < 36; i++)
            {
                Quaternion rot2 = transform.localRotation;
                ExpC shot2 = Instantiate(ExpPrefab, _posOwn, rot2);
                shot2.EShot1((i * 10) + Random.Range(-1, 2), 13, _expCountTime);
            }
        }
        _audioGO.PlayOneShot(expS);
        Destroy(gameObject);
    }

}
