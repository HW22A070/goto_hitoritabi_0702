using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorC : MonoBehaviour
{
    [SerializeField]
    private int hp = 390;

    public short BeamD = 2, BulletD = 1, FireD = 1, BombD = 5, ExpD = 2, RifleD = 4, MagicD = 3;
    int i;
    int texture = 0;

    float movex = 0;
    float movey = 0;

    int mode = 0;
    float time = 0;
    float shotdown;
    Vector3 pos, ppos;

    public SpriteRenderer spriteRenderer;
    public Sprite a, b, c, fire, ice, thunder, meteor;

    public EMissile1C EMissile1Prefab;
    public BombC BombPrefab, MeteorPrefab;
    public ExpC ExpPrefab, LEPrefab;

    public StaffRollC StaffPrefab;

    Transform Player;

    GameObject GM;

    [SerializeField]
    [Tooltip("PlayerGameObject")]
    private GameObject playerGO;

    //Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameManager");
        GM.GetComponent<GameManagement>()._bossNowHp = hp;
        GM.GetComponent<GameManagement>()._bossMaxHp = hp;
        playerGO = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;

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
                spriteRenderer.sprite = fire;
                break;
            case 4:
                spriteRenderer.sprite = ice;
                break;
            case 5:
                spriteRenderer.sprite = thunder;
                break;
            case 6:
                spriteRenderer.sprite = meteor;
                break;
        }

        if (mode == 1)
        {
            texture = 3;
            if (shotdown != 0) shotdown -= Time.deltaTime;
            if (shotdown <= 0)
            {
                Vector3 direction = Player.position - transform.position;
                float angle = Random.Range(45, 135);
                Quaternion rot = transform.localRotation;
                BombC shot = Instantiate(BombPrefab, pos, rot);
                shot.EShot1(angle, 10, 0, 100, 50, 0.5f);
                shotdown = 0.2f;
            }
        }
        else if (mode == 2)
        {
            FloorManagerC.StageGimic(100,1);
            texture = 4;
            if (shotdown != 0) shotdown -= Time.deltaTime;
            if (shotdown <= 0)
            {
                Vector3 direction2 = GameData.RandomWindowPosition();
                Quaternion rot2 = transform.localRotation;
                ExpC shot2 = Instantiate(ExpPrefab, direction2, rot2);
                shot2.EShot1(0, 0, 1.5f);
                shotdown = 0.2f;
            }
        }
        else if (mode == 3)
        {
            texture = 5;
            if (shotdown != 0) shotdown -= Time.deltaTime;
            if (shotdown <= 0)
            {
                for (i = 0; i < Random.Range(1, 3); i++)
                {
                    Vector3 direction2 = new Vector3(Random.Range(0, 640), 480, 0);
                    float angle2 = 270;
                    Quaternion rot2 = transform.localRotation;
                    EMissile1C shot2 = Instantiate(EMissile1Prefab, direction2, rot2);
                    shot2.EShot1(angle2, 240, 1000);
                    shotdown = Random.Range(0.3f, 0.7f);
                }
            }
        }
        else if (mode == 4)
        {
            texture = 6;
            if (shotdown != 0) shotdown -= Time.deltaTime;
            if (shotdown <= 0)
            {
                float angle = Random.Range(260, 280);
                Quaternion rot = transform.localRotation;
                BombC shot = Instantiate(MeteorPrefab, new Vector3(360, 550, 0), rot);
                shot.EShot1(angle, 5, 0, 100, 200, 3.0f);
                shotdown = 7;
            }
        }

    }

    public void Summon(int judge)
    {
        pos = transform.position;
        movex = Random.Range(50, 590);
        movey = Random.Range(50, 430);
        movex = (movex - pos.x) / 30;
        movey = (movey - pos.y) / 30;
    }

    void FixedUpdate()
    {
        if (time == 0)
        {
            FloorManagerC.StageGimic(100,0);
            movex = Random.Range(50, 590);
            movey = Random.Range(50, 430);
            movex = (movex - pos.x) / 50;
            movey = (movey - pos.y) / 50;
            mode = 0;
        }
        else if (time == 50)
        {
            mode = Random.Range(1, 5);
        }
        else if (time >= 220)
        {
            time = -1;
        }
        time++;

        if (mode == 0)
        {
            transform.localPosition += new Vector3(movex, movey, 0);
            texture = Random.Range(0, 3);
        }

        //Effect
        switch (texture)
        {

            case 3:
                spriteRenderer.sprite = fire;
                break;
            case 4:
                spriteRenderer.sprite = ice;
                break;
            case 5:
                pos.x += Random.Range(-128, 128);
                pos.y += Random.Range(-128, 128);
                float angle = Random.Range(0, 360);
                Quaternion rot = transform.localRotation;
                ExpC shot = Instantiate(LEPrefab, pos, rot);
                shot.EShot1(angle, 0, 0.1f);
                break;
            case 6:
                spriteRenderer.sprite = meteor;
                break;
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
            /*
            pos = new Vector3(360, -100, 0);
            StaffRollC staff = Instantiate(StaffPrefab, pos, transform.localRotation);
            staff.Summon(0);
            FloorManagerC.StageIce(100) = 0;
            Destroy(gameObject);
            */

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
