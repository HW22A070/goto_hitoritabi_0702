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
        pos = transform.position;
        ppos = GameObject.Find("Player").transform.position;
        float angle = GameData.GetAngle(pos, ppos);
        var direction = GameData.GetDirection(angle);
        velocity = direction * 5;
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        ppos = playerGO.transform.position;



        if (pos.x >= 624)
        {
            float angle = GameData.GetAngle(pos,ppos);
            var direction = GameData.GetDirection(angle);
            velocity = direction * 5;
            transform.localPosition += new Vector3(-15, 0, 0);
        }
        if (pos.x <= 16)
        {
            float angle = GameData.GetAngle(pos, ppos);
            var direction = GameData.GetDirection(angle);
            velocity = direction * 5;
            transform.localPosition += new Vector3(15, 0, 0);
        }
        if (pos.y >= 464)
        {
            float angle = GameData.GetAngle(pos, ppos);
            var direction = GameData.GetDirection(angle);
            velocity = direction * 5;
            transform.localPosition += new Vector3(0, -15, 0);
        }
        if (pos.y <= 16)
        {
            float angle = GameData.GetAngle(pos, ppos);
            var direction = GameData.GetDirection(angle);
            velocity = direction * 5;
            transform.localPosition += new Vector3(0, 15, 0);
        }
    }

    void FixedUpdate()
    {
            transform.localPosition += velocity;
    }
}
