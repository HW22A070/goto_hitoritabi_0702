using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalC : MonoBehaviour
{
    [SerializeField]
    private int hp = 300;
    int i,j,k;

    int mode = 0;

    float angle;

    Vector3 movepo;

    float down = 2;
    Vector3 pos, ppos;
    Quaternion rot;

    public EMissile1C MagicBallP;
    public BeamC BeamPrefab;
    public ExpC EffectPrefab;
    public PlantC PlantPrefab;
    public ShurikenC ShurikenPrefab;

    public SpriteRenderer spriteRenderer;
    public Sprite normal, magical;

    GameObject GM;
    public short BeamD = 2, BulletD = 1, FireD = 1, BombD = 5, ExpD = 2, RifleD = 4, MagicD = 3;

    [SerializeField]
    [Tooltip("PlayerGameObject")]
    private GameObject playerGO;

    // Start is called before the first frame update
    void Start()
    {
        //Camera = GameObject.Find("Main Camera");
        GM = GameObject.Find("GameManager");
        GM.GetComponent<GameManagement>()._bossNowHp = hp;
        GM.GetComponent<GameManagement>()._bossMaxHp = hp;
        i = 0;
        playerGO = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        ppos = playerGO.transform.position;

        if (down > 0 && mode == 0) down -= Time.deltaTime;
        if (down <= 1)
        {
            Vector3 tar = pos + new Vector3(Random.Range(-300, 300), Random.Range(0, 300), 0);
            float angle2 = GameData.GetAngle(tar + new Vector3(0, 30, 0), pos);
            ExpC shot2 = Instantiate(EffectPrefab, tar, rot);
            shot2.EShot1(angle2, 10, 0.1f);
            spriteRenderer.sprite = magical;
        }
        if (down <= 0)
        {
            mode = Random.Range(1, 5);
            if (hp >= 50)
            {
                down = 5;
            }
            else if (hp < 50)
            {
                down = 1.1f;
            }

        }

        if (mode > 0)
        {
            spriteRenderer.sprite = magical;
        }

    }

    public void Summon(int judge)
    {

    }

    void FixedUpdate()
    {

        //Beam
        if (mode == 1)
        {
            if (i == 0)
            {
                angle = GameData.GetAngle(pos,ppos);
            }

            if (i < 50 + hp)
            {
                Quaternion rot = transform.localRotation;
                BeamC shot = Instantiate(BeamPrefab, pos, rot);
                shot.EShot1(angle, 10, 0, 1, 2.0f);
                i++;
            }
            else
            {
                spriteRenderer.sprite = normal;
                mode = 0;
                i = 0
;
            }
        }

        //Kaiten
        if (mode == 2)
        {
            if (i < 90)
            {
                GameData.Camera += i;
                i += 2;
                transform.position = new Vector3(Random.Range(16, 624), GameData.GroundPutY(Random.Range(0,5),32), 0);
            }
            else
            {
                spriteRenderer.sprite = normal;
                mode = 0;
                i = 0
;
            }
        }

        //Shuriken
        if (mode == 3)
        {
            i++;
            if (i % 10 == 0)
            {
                angle = Random.Range(250, 290);
                Quaternion rot = transform.localRotation;
                ShurikenC shot = Instantiate(ShurikenPrefab, new Vector3(Random.Range(0, 640), 500, 0), rot);
                shot.EShot1(angle, 14, -0.2f, 20);
            }
            else if (i > 50 + hp)
            {
                spriteRenderer.sprite = normal;
                mode = 0;
                i = 0
;
            }

        }

        //Ball
        if (mode == 4)
        {
            i++;
            if (i % 20 - (hp / 10) == 0)
            {
                movepo = new Vector3(Random.Range(16, 624), GameData.GroundPutY(Random.Range(0,5), 32), 0);
                movepo = (movepo - pos) / 30;
                for (j = 0; j < 30; j++)
                {
                    transform.position += movepo;
                    pos = transform.position;

                    float angle2 = Random.Range(0, 360);
                    ExpC shot2 = Instantiate(EffectPrefab, pos, rot);
                    shot2.EShot1(angle2, 0.1f, 1.0f);
                    spriteRenderer.sprite = magical;
                }
                for (j = 0; j < 8; j++)
                {
                    angle += 45;
                    Quaternion rot2 = transform.localRotation;
                    EMissile1C shot = Instantiate(MagicBallP, pos, rot);
                    shot.EShot1(angle, 0, 0.5f);
                }
                k++;
            }
            else if (k >= 8)
            {
                spriteRenderer.sprite = normal;
                mode = 0;
                i = 0;
                k = 0;
                ;
            }

        }
    }

    private void Damage(int hit)
    {
        hp--;
        GM.GetComponent<GameManagement>()._bossNowHp = hp;
        if (hp <= 0)
        {
            playerGO.GetComponent<PlayerC>().StageMoveAction();
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "PBeam")
        {
            Damage(BeamD);
        }
        if (collision.gameObject.tag == "PBullet")
        {
            Damage(BulletD);
        }
        if (collision.gameObject.tag == "PFire")
        {
            Damage(FireD);
        }
        if (collision.gameObject.tag == "PBomb")
        {
            Damage(BombD);
        }
        if (collision.gameObject.tag == "PExp")
        {
            Damage(ExpD);
        }
        if (collision.gameObject.tag == "PRifle")
        {
            Damage(RifleD);
        }
        if (collision.gameObject.tag == "PMagic")
        {
            Damage(MagicD);
        }
    }

}
