using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMachineGunC : MonoBehaviour
{
    Vector3 velocity, pos, fpos;

    public PMissile PBulletP;
    public PExpC PExpC;

    float time = 10.0f;

    public AudioClip shotS,breakS;

    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            for (int k = 1; k < 4; k++) {
                for (int j = 0; j < 360; j+=10)
                {
                    Quaternion rot = transform.localRotation;
                    PExpC shot2 = Instantiate(PExpC, fpos, rot);
                    shot2.EShot1(j, 5*k, 3);
                }
            }
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(breakS);
            Destroy(gameObject);
        }
    }

    /*//input
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started) Fireing = true;
        else if (context.canceled) Fireing = false;
    }*/

    public Vector3 GetDirection(float angle)
    {
        Vector3 direction = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0);
        return direction;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos = GameObject.Find("Player").transform.position;
        if (PlayerC.muki == 0)
        {
            spriteRenderer.flipX = false;
            fpos = pos + new Vector3(48, 16, 0);
        }
        else
        {
            fpos = pos + new Vector3(-48, 16, 0);
            spriteRenderer.flipX = true;
        }


        Quaternion rot = transform.localRotation;
        float angle = PlayerC.muki * 180 + Random.Range(-3, 3);
        for (int i = 0; i < 3; i++)
        {
            PMissile shot = Instantiate(PBulletP, fpos, rot);
            shot.Shot(angle, Random.Range(70, 90), 0);
        }

        GameObject.FindObjectOfType<AudioSource>().PlayOneShot(shotS);
        transform.localPosition = fpos;
    }
}
