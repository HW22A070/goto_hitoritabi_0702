using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HenchmanC : MonoBehaviour
{
    Vector3 velocity, pos;
    float aang, ttime,ddns;
    new bool audio;

    Transform Player;

    public EMissile1C EMissile1Prefab;

    public AudioClip shotS;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").transform;
    }

    public void EShot1(float angle, float time,float dnsk)
    {
        var direction = GetDirection(angle);
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        aang = angle;
        ttime = time;
        ddns = dnsk;

    }

    public Vector3 GetDirection(float angle)
    {
        Vector3 direction = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0);
        return direction;
    }

    private void Update()
    {
        ttime -= Time.deltaTime;

        if (ttime <= 0&&ttime>-0.1f)
        {
            pos = transform.position;
            Quaternion rot = transform.localRotation;
            EMissile1C shot = Instantiate(EMissile1Prefab, pos, rot);
            if (!audio)
            {
                GameObject.FindObjectOfType<AudioSource>().PlayOneShot(shotS);
                audio = true;
            }

            shot.EShot1(aang, ddns, 0);
        }
        else if (ttime < -1.3f)
        {
            Destroy(gameObject);
        }
    }

    public float GetAngle(Vector2 direction)
    {
        float rad = Mathf.Atan2(direction.y, direction.x);
        return rad * Mathf.Rad2Deg;
    }
}
