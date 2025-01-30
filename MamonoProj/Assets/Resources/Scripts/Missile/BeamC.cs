using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamC : MonoBehaviour
{
    Vector3 velocity, pos;
    float sspeed, kkaso, aang, eexptim;
    int i, hunj;

    public ExpC ExpPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void EShot1(float angle, float speed, float kasoku, int hunjin, float exptime)
    {
        var direction = GameData.GetDirection(angle);
        velocity = direction * speed;
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        sspeed = speed;
        kkaso = kasoku;
        aang = angle;
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

        if (pos.y < 0 || pos.y > 480 || pos.x < 0 || pos.x > 640)
        {
            for (i = 0; i < hunj; i++)
            {
                Vector3 direction2 = new Vector3(Random.Range(10, 630), Random.Range(10, 470), 0);
                float angle2 = Random.Range(0, 360);
                Quaternion rot2 = transform.localRotation;
                ExpC shot2 = Instantiate(ExpPrefab, pos, rot2);
                shot2.EShot1(angle2, Random.Range(1, 10.0f), eexptim);
            }
            Destroy(gameObject);
        }
    }
}
