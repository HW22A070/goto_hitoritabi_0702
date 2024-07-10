using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampreyC : MonoBehaviour
{
    int i;

    Vector3 pos;

    public SpriteRenderer spriteRenderer;
    public Sprite a, b, c, d;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;

        if (pos.y >= 1200)
        {
            Destroy(gameObject);
        }

    }

    void FixedUpdate()
    {
            transform.localPosition += new Vector3(0, 10, 0);

            i = Random.Range(0, 10);
            if (i == 0) spriteRenderer.sprite = b;
            else if (i == 1) spriteRenderer.sprite = c;
            else if (i == 2) spriteRenderer.sprite = d;
            else spriteRenderer.sprite = a;
    }

    public void Summon(int judge)
    {

    }
}
