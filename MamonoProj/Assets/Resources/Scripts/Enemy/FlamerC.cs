using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamerC : MonoBehaviour
{
    int judge;
    float down = 0;
    int pull, tate;

    float shotdown = 2;

    int muki;

    float move = 0;
    Vector3 pos, ppos;

    public SpriteRenderer spriteRenderer;

    public BombC BombPrefab;
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

        if (pos.x > 632)
        {
            move = -5f;
            spriteRenderer.flipX = false;
        }
        if (pos.x < 8)
        {
            move = 5f;
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

        if (shotdown != 0) shotdown -= Time.deltaTime; ;
        if (shotdown <= 0)
        {
            Vector3 direction = ppos - transform.position;
            float angle = GetAngle(direction);
            Quaternion rot = transform.localRotation;
            BombC shot = Instantiate(BombPrefab, pos, rot);
            shot.EShot1(angle, 3, 0, 100, 100, 0.5f);
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(shotS);
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

    public void Summon(int judge)
    {
        if (judge == 0)
        {
            move = -5f;
            spriteRenderer.flipX = false;
        }
        else
        {
            move = 5f;
            spriteRenderer.flipX =true;
        }
    }

    
}
