using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowC : MonoBehaviour
{
    int i;

    Vector3 pos, ppos;
    Vector3 muki, velocity;


    //public SpriteRenderer spriteRenderer;
    //public Sprite a, b, c, d, e, f, g;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        ppos = GameObject.Find("Player").transform.position;

        muki = ppos - pos;
        float angle = GetAngle(muki);
        var direction = GetDirection(angle);
        velocity = direction * 2;
    }



    public Vector3 GetDirection(float angle)
    {
        Vector3 direction = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0);
        return direction;
    }

    public void Summon(int judge)
    {
        pos = transform.position;
        ppos = GameObject.Find("Player").transform.position;

    }

    void FixedUpdate()
    {
        transform.localPosition += velocity;
    }

    public float GetAngle(Vector2 direction)
    {
        float rad = Mathf.Atan2(direction.y, direction.x);
        return rad * Mathf.Rad2Deg;
    }
}
