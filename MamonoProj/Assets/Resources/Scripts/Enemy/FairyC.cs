using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyC : MonoBehaviour
{
    int judge;
    float down = 0;
    int pull, tate;

    int i;

    private bool _isCharging;

    float shotdown;

    int muki;
    float angle = 0;

    int move = 0;
    Vector3 pos, ppos;

    public SpriteRenderer spriteRenderer;

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
        pos = transform.position;
        ppos = playerGO.transform.position;

        if (pos.x > 632)
        {
            move = -5;
            spriteRenderer.flipX = false;
        }
        if (pos.x < 8)
        {
            move = 5;
            spriteRenderer.flipX = true;
        }

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

        if (shotdown != 0) shotdown -= Time.deltaTime;
        if (ppos.y >= pos.y - 16 && ppos.y <= pos.y + 16 && shotdown <= 0)
        {
            StartCoroutine("Shoot");
            shotdown = 3;
        }

        
    }

    public float GetAngle(Vector2 direction)
    {
        float rad = Mathf.Atan2(direction.y, direction.x);
        return rad * Mathf.Rad2Deg;
    }

    void FixedUpdate()
    {
            if (down == 0 && tate == 0&&!_isCharging)
            {
                pull = 1;
                tate = 1;
            }
            if (pos.y <= -50)
            {
                Destroy(gameObject);
            }
            down = Random.Range(0, 100);

            transform.localPosition += new Vector3(move, 0, 0);

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


    private IEnumerator Shoot()
    {
        _isCharging = true;
        if (pos.x > ppos.x)spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true;

        int movetemp = move;
        move = 0;
        yield return new WaitForSeconds(1.00f);
        for (i = 0; i < 4; i++)
        {
            if (pos.x > ppos.x)
            {
                angle = 180 + (-30 + (i * 20));
                spriteRenderer.flipX = false;
            }
            else
            {
                angle = -30 + (i * 20);
                spriteRenderer.flipX = true;
            }
            Quaternion rot = transform.localRotation;
            EMissile1C shot = Instantiate(EMissile1Prefab, pos, rot);
            shot.EShot1(angle, 20, 0);
            yield return new WaitForSeconds(0.03f);
        }
        GameObject.FindObjectOfType<AudioSource>().PlayOneShot(shotS);
        yield return new WaitForSeconds(0.5f);
        move = movetemp;
        _isCharging=false;
    }

    public void Summon(int judge)
    {
        if (judge == 0)
        {
            move = -5;
            spriteRenderer.flipX = false;
        }
        else
        {
            move = 5;
            spriteRenderer.flipX = true;
        }
    }

   
}
