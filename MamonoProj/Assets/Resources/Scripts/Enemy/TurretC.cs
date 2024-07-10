using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretC : MonoBehaviour
{
    int judge;
    float down = 0;
    int pull, tate,texture; 

    float shotdown=0.5f;

    Vector3 pos;

    public SpriteRenderer spriteRenderer;
    public Sprite a,b,c,d,e,f,g,h;

    public EMissile1C EMissile1Prefab;

    public AudioClip shotS;

    // Start is called before the first frame update
    void Start()
    {
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
            float angle = Random.Range(0.0f, 360.0f);
            Quaternion rot = transform.localRotation;
            pos.y += 14;
            EMissile1C shot = Instantiate(EMissile1Prefab, pos, rot);
            shot.EShot1(angle, 20, 0);
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(shotS);
            shotdown = 1;
        }
        
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

        texture++;
        if (texture > 7) texture = 0;
        switch (texture)
        {
            case 0:
                spriteRenderer.sprite = a;
                break;
            case 1:
                spriteRenderer.sprite = b;
                break;
            case 2:
                spriteRenderer.sprite = c;
                break;
            case 3:
                spriteRenderer.sprite = d;
                break;
            case 4:
                spriteRenderer.sprite = e;
                break;
            case 5:
                spriteRenderer.sprite = f;
                break;
            case 6:
                spriteRenderer.sprite = g;
                break;
            case 7:
                spriteRenderer.sprite = h;
                break;
        }
    }

    public void Summon(int judge)
    {

    }

    
}
