using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSnakeC : MonoBehaviour
{
    int judge;
    int mode = 0;

    float shotdown;
    float angle = 0;
    int i;

    float move = 0;


    public SpriteRenderer spriteRenderer;

    public EMissile1C EMissile1Prefab;

    Vector3 pos, ppos;

    public AudioClip shotS;

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
        pos = transform.position;
        ppos = playerGO.transform.position;

        if (ppos.y >= pos.y && mode == 0)
        {
            mode = 1;
            if (pos.x > ppos.x)
            {
                move = -3f;
            }
            else
            {
                move = 3f;
            }
        }

        if (shotdown != 0) shotdown -= Time.deltaTime;
        if (ppos.y >= pos.y - 16 && ppos.y <= pos.y + 16 && shotdown <= 0)
        {
            if (pos.x > ppos.x)
            {
                angle = 180;
                spriteRenderer.flipX = false;
            }
            else
            {
                angle = 0;
                spriteRenderer.flipX = true;
            }
            Quaternion rot = transform.localRotation;
            pos.y += 8;
            for (i = 0; i <= 1; i++)
            {
                pos.y -= i * 16;
                EMissile1C shot = Instantiate(EMissile1Prefab, pos, rot);
                shot.EShot1(angle, 10, 0);
            }
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(shotS);
            shotdown = 0.5f;
        }


        if (pos.y <= -50 || pos.x > 660 || pos.x < -20)
        {
            Destroy(gameObject);
        }

    }

    public float GetAngle(Vector2 direction)
    {
        float rad = Mathf.Atan2(direction.y, direction.x);
        return rad * Mathf.Rad2Deg;
    }

    void FixedUpdate()
    {
        //ˆÚ“®
        if (mode == 0)
        {
            transform.localPosition += new Vector3(0, move, 0);
        }
        else if (mode == 1)
        {
            transform.localPosition += new Vector3(move, 0, 0);
        }
    }

    public void Summon(int judge)
    {
        move = -4f;
    }
}
