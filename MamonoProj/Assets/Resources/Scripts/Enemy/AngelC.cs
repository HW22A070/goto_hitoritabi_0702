using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelC : MonoBehaviour
{
    int i;

    Vector3 pos, ppos;
    Vector3 muki, velocity;

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

        muki = ppos - pos;
        float angle = GetAngle(muki);
        var direction = GetDirection(angle);
        velocity = direction * 3;
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
        playerGO = GameObject.Find("Player");
        pos = transform.position;
        ppos = playerGO.transform.position;

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
