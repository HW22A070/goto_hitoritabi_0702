using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceC : MonoBehaviour
{
    int judge;
    float down = 0;
    int pull, tate, texture;

    float shotdown=2.0f;

    Vector3 pos,ppos;

    public SpriteRenderer spriteRenderer;
    public Sprite a, b;

    public EMissile1C EMissile1Prefab;

    public AudioClip damageS, deadS,shotS;

    [SerializeField]
    [Tooltip("PlayerGameObject")]
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
            Vector3 direction = ppos - transform.position;
            float angle = GetAngle(direction);
            Quaternion rot = transform.localRotation;
            EMissile1C shot = Instantiate(EMissile1Prefab, pos, rot);
            shot.EShot1(angle, 10, 0);
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(shotS);
            shotdown = 2;
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
            if (texture > 2) texture = 0;
            switch (texture)
            {
                case 0:
                    spriteRenderer.sprite = a;
                    break;
                case 1:
                    spriteRenderer.sprite = b;
                    break;
            }
    }

    public void Summon(int judge)
    {

    }

   
}
