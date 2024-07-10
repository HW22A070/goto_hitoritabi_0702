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

    public AudioClip expS;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void EShot1(float angle, float speed, float kasoku, float exp, int hunjin, float exptime)
    {
        var direction = GetDirection(angle);
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

    public Vector3 GetDirection(float angle)
    {
        Vector3 direction = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0);
        return direction;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
            pos = transform.position;

            transform.localPosition += velocity;
            sspeed += kkaso;
            var direction = GetDirection(aang);
            velocity = direction * sspeed;

            //time_ex
            eexp--;
            if (eexp <= 0)
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
                        shot2.EShot1((i * 10)+Random.Range(-1,2), 13, eexptim);
                    }
                }
                GameObject.FindObjectOfType<AudioSource>().PlayOneShot(expS);
                Destroy(gameObject);
            }

            //down_ex
            if (pos.y <= 0)
            {
                if (down == true)
                {
                    for (i = 0; i < hunj; i++)
                    {
                        Vector3 direction2 = new Vector3(Random.Range(10, 630), Random.Range(10, 470), 0);
                        float angle2 = Random.Range(0, 360);
                        Quaternion rot2 = transform.localRotation;
                        ExpC shot2 = Instantiate(ExpPrefab, pos, rot2);
                        shot2.EShot1(angle2, Random.Range(1, 10.0f), eexptim);
                    }
                    GameObject.FindObjectOfType<AudioSource>().PlayOneShot(expS);
                    Destroy(gameObject);
                }
            }

            //up_ex
            if (pos.y >= 480)
            {
                if (up == true)
                {
                    for (i = 0; i < hunj; i++)
                    {
                        Vector3 direction2 = new Vector3(Random.Range(10, 630), Random.Range(10, 470), 0);
                        float angle2 = Random.Range(0, 360);
                        Quaternion rot2 = transform.localRotation;
                        ExpC shot2 = Instantiate(ExpPrefab, pos, rot2);
                        shot2.EShot1(angle2, Random.Range(1, 10.0f), eexptim);
                    }
                    GameObject.FindObjectOfType<AudioSource>().PlayOneShot(expS);
                    Destroy(gameObject);
                }
            }

            //left_ex
            if (pos.x <= 0)
            {
                if (left == true)
                {
                    for (i = 0; i < hunj; i++)
                    {
                        Vector3 direction2 = new Vector3(Random.Range(10, 630), Random.Range(10, 470), 0);
                        float angle2 = Random.Range(0, 360);
                        Quaternion rot2 = transform.localRotation;
                        ExpC shot2 = Instantiate(ExpPrefab, pos, rot2);
                        shot2.EShot1(angle2, Random.Range(1, 10.0f), eexptim);
                    }
                    GameObject.FindObjectOfType<AudioSource>().PlayOneShot(expS);
                    Destroy(gameObject);
                }
            }

            //right_ex
            if (pos.x >= 640)
            {
                if (right == true)
                {
                    for (i = 0; i < hunj; i++)
                    {
                        Vector3 direction2 = new Vector3(Random.Range(10, 630), Random.Range(10, 470), 0);
                        float angle2 = Random.Range(0, 360);
                        Quaternion rot2 = transform.localRotation;
                        ExpC shot2 = Instantiate(ExpPrefab, pos, rot2);
                        shot2.EShot1(angle2, Random.Range(1, 10.0f), eexptim);
                    }
                    GameObject.FindObjectOfType<AudioSource>().PlayOneShot(expS);
                    Destroy(gameObject);
                }
            }
    }

}
