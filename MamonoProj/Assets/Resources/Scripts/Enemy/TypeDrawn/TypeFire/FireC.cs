using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireC : MonoBehaviour
{
    int judge;
    float down = 0;
    int mode = 0;
    int i;

    float move = 0;


    public SpriteRenderer spriteRenderer;
    public ExpC ExpPrefab;

    Vector3 pos, ppos;

    [SerializeField]
    [Tooltip("PlayerGameObject")]
    private GameObject playerGO;

    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.Find("Player");
        move = -3f;
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
                move = -100f;
                spriteRenderer.flipX = false;
            }
            else
            {
                move = 100f;
                spriteRenderer.flipX = true;
            }
        }
                
        if (pos.y <= -50 || pos.x > 660 || pos.x < -20)
        {
            Destroy(gameObject);
        }

        
    }

    void FixedUpdate()
    {
            if (mode == 0)
            {
                transform.localPosition += new Vector3(0, move, 0);
            }
            else if (mode == 1)
            {
                transform.localPosition += new Vector3(move, 0, 0);
                float angle = Random.Range(0, 360);
                Quaternion rot = transform.localRotation;
                ExpC shot = Instantiate(ExpPrefab, pos, rot);
                shot.EShot1(angle, 1, 1);
            }
    }
}
