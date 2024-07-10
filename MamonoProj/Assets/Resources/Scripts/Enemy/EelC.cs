using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelC : MonoBehaviour
{

    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;

        if (pos.y >= 900)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        transform.localPosition += new Vector3(0, 15, 0);
    }

    public void Summon(int judge)
    {

    }

    
}
