using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeameyeC : MonoBehaviour
{
    int judge;
    float down = 0;
    int pull, tate, texture,eye;

    float shotdown = 0.5f;

    Vector3 pos,ppos;

    public EMissile1C EMissile1Prefab;

    public AudioClip shotS;

    private GameObject playerGO;

    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        ppos = playerGO.transform.position;
        pos = transform.position;

        if (down == 0 && tate == 0)
        {
            pull = 1;
            tate = 1;
        }
        if (pos.y <= -50)
        {
            Destroy(gameObject);
        }
        down = Random.Range(0, 1000);

        if (shotdown != 0) shotdown -= Time.deltaTime; ;
        if (shotdown <= 0)
        {
            for (eye = 0; eye < 8; eye++)
            {
                pos = transform.position;
                if (eye==1) pos -= new Vector3(-22, 23, 0);
                else if (eye == 2) pos -= new Vector3(21, 25, 0);
                else if (eye == 3) pos -= new Vector3(28, 12, 0);
                else if (eye == 4) pos -= new Vector3(25, -3, 0);
                else if (eye == 5) pos -= new Vector3(22, -24, 0);
                else if (eye == 6) pos -= new Vector3(-23, -22, 0);
                else if (eye == 7) pos -= new Vector3(-28, 7, 0);
                Vector3 direction = ppos - pos;
                float angle = GetAngle(direction);
                Quaternion rot = transform.localRotation;
                EMissile1C shot = Instantiate(EMissile1Prefab, pos, rot);
                shot.EShot1(angle, 20, 1);
                pos -= new Vector3(-24, 0, 0);
            }
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(shotS);
            shotdown = 2;
        }

        pos = transform.position;
    }

    public float GetAngle(Vector2 direction)
    {
        float rad = Mathf.Atan2(direction.y, direction.x);
        return rad * Mathf.Rad2Deg;
    }

    void FixedUpdate()
    {
            if (down == 0 && tate == 0)
            {
                pull = 1;
                tate = 1;
            }
            if (pos.y <= -50)
            {
                Destroy(gameObject);
            }
            down = Random.Range(0, 100);

            if (pull > 0)
            {
                pull++;
                transform.localPosition += new Vector3(0, -10, 0);
                if (pull > 9)
                {
                    pull = 0;
                    tate = 0;
                }
            }
    }

    public void Summon(int judge)
    {

    }

    
}
