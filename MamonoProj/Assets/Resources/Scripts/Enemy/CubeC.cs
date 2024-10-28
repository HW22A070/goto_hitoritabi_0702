using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeC : MonoBehaviour
{
    int mode = 0;
   
    Vector3 scare;
    float size = 0;

    BoxCollider2D Col;

    // Start is called before the first frame update
    void Start()
    {
        Col = GetComponent<BoxCollider2D>();
        scare = gameObject.transform.localScale;
        scare = new Vector3(0.01f, 0.01f, 0);
        gameObject.transform.localScale = scare;
        Col.size = new Vector3(size, size, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
            if (mode == 0)
            {
                scare = gameObject.transform.localScale;
                scare += new Vector3(0.02f, 0.02f, 0);
                size += 2.56f;
                Col.size = new Vector3(size, size, 0);
                gameObject.transform.localScale = scare;
            }
            if (scare.x >= 1)
            {
                mode = 1;
            }
            if (mode == 1)
            {
                scare = gameObject.transform.localScale;
                scare -= new Vector3(0.02f, 0.02f, 0);
                size -= 2.56f;
                Col.size = new Vector3(size, size, 0);
                gameObject.transform.localScale = scare;
            }
            if (scare.x <= 0)
            {
                Destroy(gameObject);
            }
    }

    
}
