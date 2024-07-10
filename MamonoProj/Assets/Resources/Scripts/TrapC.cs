using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapC : MonoBehaviour
{
    Vector3 velocity, pos;
    int i;

    public ExpC ExpPrefab;

    public AudioClip expS;
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PBeam"
            || collision.gameObject.tag == "PBullet"
            || collision.gameObject.tag == "PBomb"
            || collision.gameObject.tag == "PRifle"
            || collision.gameObject.tag == "PFire"
            || collision.gameObject.tag == "PMagic")
        {
            pos = transform.position;
            for (i = 0; i < 5; i++)
            {
                float angle2 = Random.Range(0, 360);
                Quaternion rot2 = transform.localRotation;
                ExpC shot2 = Instantiate(ExpPrefab, pos, rot2);
                shot2.EShot1(angle2, Random.Range(1, 10.0f), 0.5f);
            }
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(expS);
            Destroy(gameObject);
        }
    }

}
