using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawnC : MonoBehaviour
{
    int judge;

    float move = 0;


    public SpriteRenderer spriteRenderer;

    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        
        if (pos.y <= -50 || pos.x > 660 || pos.x < -20)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
            transform.localPosition += new Vector3(0, move, 0);
    }

    public void Summon(int judge)
    {
        move = -3f;
    }
}
