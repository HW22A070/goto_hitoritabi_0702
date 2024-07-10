using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingC : MonoBehaviour
{
    Vector3 velocity, pos, ppos;
    float sspeed, kkaso, aang, eexp, eexptim, hms;
    int i, hunj;

    int mode;

    public ExpC ExpPrefab;

    public AudioClip expS;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void EShot1(float angle, float speed, float exp, int hunjin, float exptime, float hormingstart)
    {
        var direction = GetDirection(angle);
        velocity = direction * speed;
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        sspeed = speed;
        aang = angle;
        eexp = exp;
        eexptim = exptime;
        hunj = hunjin;
        hms = hormingstart;
    }

    public Vector3 GetDirection(float angle)
    {
        Vector3 direction = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0);
        return direction;
    }

    public float GetAngle(Vector2 direction)
    {
        float rad = Mathf.Atan2(direction.y, direction.x);
        return rad * Mathf.Rad2Deg;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
            pos = transform.position;
            ppos = GameObject.Find("Player").transform.position;

            transform.localPosition += velocity;

            hms--;
            if ((hms < 0||pos.y>450) && mode == 0)
            {
                Vector3 directio = ppos - pos;
                float angle2 = GetAngle(directio);
                var direction2 = GetDirection(angle2);
                velocity = direction2 * sspeed;
                var angles = transform.localEulerAngles;
                angles.z = angle2 - 90;
                transform.localEulerAngles = angles;
                mode = 1;
            }

            eexp--;
            if (eexp <= 0 || pos.y <= 0 || pos.x <= 0 || pos.x >= 640)
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
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
