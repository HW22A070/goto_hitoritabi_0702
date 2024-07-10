using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombC : MonoBehaviour
{
    Vector3 velocity, pos;
    float sspeed, kkaso, aang, eexp, eexptim;
    int i, hunj;

    public bool up, down, right, left, player,PM;
    private bool exp;

    public ExpC ExpPrefab;

    public AudioClip expS;

    public bool bombbarrier = true, bombsosai;

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
        if (eexp <= 0) exp = true;

        //down_ex
        if (pos.y <= 0 && down == true) exp = true;

        //up_ex
        if (pos.y >= 480 && up == true) exp = true;

        //left_ex
        if (pos.x <= 0 && left == true) exp = true;

        //right_ex
        if (pos.x >= 640 && right == true) exp = true;

        if (exp) Explosion();
    }

    public void Explosion()
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "PBeam"
            || collision.gameObject.tag == "PBullet"
            || collision.gameObject.tag == "PFire"
            || collision.gameObject.tag == "PExp")
            && PM)
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "PBomb" && bombbarrier)
        {
            if (bombsosai)
            {
                PBombC bomb;
                GameObject obj = collision.gameObject;
                bomb = obj.GetComponent<PBombC>();
                bomb.EXPEffect(7);
                exp = true;

            }
            Destroy(gameObject);
        }
    }

}
