using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperC : MonoBehaviour
{
    int judge;
    float down = 0;
    int pull, tate;
    
    float shotdown = 0.5f;

    Vector3 pos;

    public EMissile1C EMissile1Prefab;

    public AudioClip shotS;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Game");
    }

    // Update is called once per frame
    void Update()
    {
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
            float angle = 90;
            Quaternion rot = transform.localRotation;
            pos.y += 14;
            EMissile1C shot = Instantiate(EMissile1Prefab, pos, rot);
            shot.EShot1(angle, 0.5f, 2);
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(shotS);
            shotdown = 1.5f;
        }

    }

    public float GetAngle(Vector2 direction)
    {
        float rad = Mathf.Atan2(direction.y, direction.x);
        return rad * Mathf.Rad2Deg;
    }

    private IEnumerator Game()
    {
        for (; ; )
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

            yield return new WaitForSeconds(0.03f);
        }

    }

    public void Summon(int judge)
    {

    }
}
