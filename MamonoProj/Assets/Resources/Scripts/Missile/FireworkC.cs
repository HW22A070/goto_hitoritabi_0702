using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkC : MonoBehaviour
{
    Vector3 velocity, pos;
    float sspeed, kkaso, aang, eexp, eexptim;
    int i,j, hunj;

    public bool up, down, right, left;

    public ExpC ExpPrefab;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    public AudioClip expS;

    // Start is called before the first frame update
    void Start()
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
    void FixedUpdate()
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

    private void Explosion()
    {
        if (Random.Range(0, 2) == 0)
        {
            for (j = 1; j < 4; j++)
            {
                for (i = 0; i < 20; i++)
                {
                    Quaternion rot2 = transform.localRotation;
                    ExpC shot2 = Instantiate(ExpPrefab, pos, rot2);
                    shot2.EShot1(i * 18, j * 3, eexptim);
                }
            }
        }
        else
        {
            for (i = 0; i < 36; i++)
            {
                Quaternion rot2 = transform.localRotation;
                ExpC shot2 = Instantiate(ExpPrefab, pos, rot2);
                shot2.EShot1((i * 10) + Random.Range(-1, 2), 13, eexptim);
            }
        }
        _audioGO.PlayOneShot(expS);
        Destroy(gameObject);
    }

}
