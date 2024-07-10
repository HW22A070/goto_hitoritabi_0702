using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightC : MonoBehaviour
{

    int i;

    Vector3 pos, ppos, velocity,muki;

    float angle = 270;

    float movex = 5;
    float movey = 5;

    [SerializeField]
    [Tooltip("PlayerGameObject")]
    private GameObject playerGO;

    void Start()
    {
        playerGO = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        ppos = playerGO.transform.position;



        if (pos.x >= 624)
        {
            muki = ppos - pos;
            float angle = GetAngle(muki);
            var direction = GetDirection(angle);
            velocity = direction * 5;
            transform.localPosition += new Vector3(-15, 0, 0);
        }
        if (pos.x <= 16)
        {
            muki = ppos - pos;
            float angle = GetAngle(muki);
            var direction = GetDirection(angle);
            velocity = direction * 5;
            transform.localPosition += new Vector3(15, 0, 0);
        }
        if (pos.y >= 464)
        {
            muki = ppos - pos;
            float angle = GetAngle(muki);
            var direction = GetDirection(angle);
            velocity = direction * 5;
            transform.localPosition += new Vector3(0, -15, 0);
        }
        if (pos.y <= 16)
        {
            muki = ppos - pos;
            float angle = GetAngle(muki);
            var direction = GetDirection(angle);
            velocity = direction * 5;
            transform.localPosition += new Vector3(0, 15, 0);
        }
    }

    public void Summon(int judge)
    {
        pos = transform.position;
        ppos = GameObject.Find("Player").transform.position;
        muki = ppos - pos;
        float angle = GetAngle(muki);
        var direction = GetDirection(angle);
        velocity = direction * 5;
    }

    public float GetAngle(Vector2 direction)
    {
        float rad = Mathf.Atan2(direction.y, direction.x);
        return rad * Mathf.Rad2Deg;
    }

    public Vector3 GetDirection(float angle)
    {
        Vector3 direction = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0);
        return direction;
    }

    void FixedUpdate()
    {
            transform.localPosition += velocity;


    }
}
