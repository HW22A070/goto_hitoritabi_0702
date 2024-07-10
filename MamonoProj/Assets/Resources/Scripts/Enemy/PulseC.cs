using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseC : MonoBehaviour
{
    int deltaspeed=0;

    Vector3 pos;

    public AudioClip runS;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;

        if (pos.y >= 720)
        {
            Destroy(gameObject);
        }

        
    }

    void FixedUpdate()
    {

            deltaspeed++;
            if (deltaspeed>=30)
            {
                transform.localPosition += new Vector3(0, deltaspeed*2, 0);
                GameObject.FindObjectOfType<AudioSource>().PlayOneShot(runS);
            }
            else
            {
                transform.localPosition += new Vector3(0,1, 0);
            }
    }

    public void Summon(int judge)
    {

    }

    
}
