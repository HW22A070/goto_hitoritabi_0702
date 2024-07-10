using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenC : MonoBehaviour
{
    Vector3 velocity, pos;
    float sspeed, kkaso, aang,kkait;

    public bool geigeki,bombbarrier = true, bombsosai;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void EShot1(float angle, float speed, float kasoku,float kaiten)
    {
        var direction = GetDirection(angle);
        velocity = direction * speed;
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        sspeed = speed;
        kkaso = kasoku;
        aang = angle;
        kkait = kaiten;

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

        transform.localEulerAngles += new Vector3(0, 0, kkait);

        transform.localPosition += velocity;
        sspeed += kkaso;
        var direction = GetDirection(aang);
        velocity = direction * sspeed;

        if (pos.y <= -50 || pos.y >= 700 || pos.x > 700 || pos.x < -50)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "PBeam"
            || collision.gameObject.tag == "PBullet"
            || collision.gameObject.tag == "PFire"
            || collision.gameObject.tag == "PExp")
            && geigeki)
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
                bomb.EXPEffect(10);
            }
            Destroy(gameObject);
        }
    }
}
