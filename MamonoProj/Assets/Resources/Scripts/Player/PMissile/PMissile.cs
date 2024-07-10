using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMissile : MonoBehaviour
{
    Vector3 velocity, pos;

    float sspeed, kkaso, aang;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void Shot(float angle, float speed, float kasoku)
    {
        var direction = GetDirection(angle);
        velocity = direction * speed;
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        sspeed = speed;
        kkaso = kasoku;
        aang = angle;

        StartCoroutine("Game");

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
    private IEnumerator Game()
    {
        for (; ; )
        {
            pos = transform.position;

            transform.localPosition += velocity;
            sspeed += kkaso;
            var direction = GetDirection(aang);
            velocity = direction * sspeed;

            if (pos.y <= -50 || pos.y >= 700 || pos.x > 700 || pos.x < -50)
            {
                Destroy(gameObject);
            }

            yield return new WaitForSeconds(0.03f);
        }
    }
}
