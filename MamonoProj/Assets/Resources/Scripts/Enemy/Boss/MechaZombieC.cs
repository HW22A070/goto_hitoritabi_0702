using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaZombieC : MonoBehaviour
{
    private int i = 0;
    private int j = 0;
    private int _attackMode = 0;
    private float _damagePar = 100;
    private int firstHP = 0;

    private float angle, movex, movey;
    private GameObject GM;
    private Vector3 pos, ppos, muki, velocity, zyuko;
    private Quaternion rot;
    public SpriteRenderer spriteRenderer;
    public Sprite normal, Gun, Rifle, swordn, swordr, swordf;

    public EMissile1C Ballet1P;
    public HomingC RocketPrefab;
    public GuardC Guard;
    public ExpC jet, SwingP;
    public HenchmanC GCP;
    public FireworkC FireworkP;
    private Vector3 kabeRR, kabeLL;
    private float kaber, kabel;

    private bool _isPositionLeft;

    public StaffRollC StaffPrefab;

    public AudioClip  missileS, gunS, fireS;

    private Vector3 hitPos;

    [SerializeField]
    [Tooltip("PlayerGameObject")]
    private GameObject playerGO;

    /// <summary>
    /// 動作中のコルーチン
    /// </summary>
    private Coroutine _movingCoroutine;

    /// <summary>
    /// ECoreCのコンポーネント
    /// </summary>
    private ECoreC _eCoreC;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Summon(int judge)
    {
        playerGO = GameObject.Find("Player");
        _eCoreC = GetComponent<ECoreC>();
        for (int j = 0; j < _eCoreC.hp.Length; j++)
        {
            firstHP += _eCoreC.hp[j];
        }
        _eCoreC.IsBoss = true;
        pos = transform.position;
        GM = GameObject.Find("GameManager");
        GM.GetComponent<GameManagement>()._bossNowHp = firstHP;
        GM.GetComponent<GameManagement>()._bossMaxHp = firstHP;
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        ppos = playerGO.transform.position;
        if (_eCoreC.BossLifeMode != 2) GM.GetComponent<GameManagement>()._bossNowHp = _eCoreC.TotalHp;
        _damagePar = _eCoreC.TotalHp * 100 / firstHP;

    }

    

    void FixedUpdate()
    {

        //SummonAction
        if (_eCoreC.BossLifeMode == 0)
        {
            transform.localPosition += new Vector3(-1, 0, 0);
            if (pos.x < 550)
            {
                _movingCoroutine = StartCoroutine(ActionBranch());
                _eCoreC.BossLifeMode = 1;
            }
        }

        //DeathAction
        if (_eCoreC.BossLifeMode == 2)
        {
            GM.GetComponent<GameManagement>()._bossNowHp = 0;
            StaffRollC staff = Instantiate(StaffPrefab, new Vector3(320, -100, 0), transform.localRotation);
            staff.Summon(0);
            GameData.IceFloor = 0;
            Destroy(gameObject);
            /*
            GameData.Score += 100000;
            Destroy(gameObject);
            */
        }
    }

    //行動変わるヤツ
    private IEnumerator ActionBranch()
    {
        yield return new WaitForSeconds(1.0f);
        if (!_isPositionLeft)
        {
            _attackMode = Random.Range(0, 4);
            if (_attackMode == 0) _movingCoroutine= StartCoroutine(Rocket());
            else if (_attackMode == 1) _movingCoroutine = StartCoroutine(MachineGun());
            else if (_attackMode == 2) _movingCoroutine = StartCoroutine(Guardian());
            else if (_attackMode == 3) _movingCoroutine = StartCoroutine(MoveRtoL());
        }
        else
        {
            _attackMode = Random.Range(0, 3);
            if (_attackMode == 0) _movingCoroutine = StartCoroutine(Rocket());
            else if (_attackMode == 1) _movingCoroutine = StartCoroutine(FireWork());
            else if (_attackMode == 2) _movingCoroutine = StartCoroutine(SwordSlash());
        }

    }

    /// <summary>
    /// ロケット
    /// </summary>
    /// <returns></returns>
    private IEnumerator Rocket()
    {
        zyuko = pos + new Vector3(5, 32, 0);
        kabeRR = new Vector3(50, 460, 0) - zyuko;
        kaber = GetAngle(kabeRR);
        kabeLL = new Vector3(590, 460, 0) - zyuko;
        kabel = GetAngle(kabeLL);

        for (j = 0; j < 20; j++)
        {
            float angle = Random.Range(kabel, kaber);
            Quaternion rot = transform.localRotation;
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(missileS);
            Instantiate(RocketPrefab, zyuko, rot).EShot1(angle, 15, 300, 5, 0.5f, Random.Range(20, 30));
            yield return new WaitForSeconds(0.03f);
        }
        _movingCoroutine = StartCoroutine("ActionBranch");
    }

    /// <summary>
    /// マシンガン
    /// </summary>
    /// <returns></returns>
    private IEnumerator MachineGun()
    {
        spriteRenderer.sprite = Rifle;
        yield return new WaitForSeconds(1.0f);
        for(j=0;j<10;j++)
        {
            if (pos.x >= ppos.x)
            {
                if (pos.x - 100 >= ppos.x)zyuko = pos + new Vector3(-32, -20, 0);
                else zyuko = pos + new Vector3(5, 32, 0);
                Vector3 direction = ppos - zyuko;
                float angle = GetAngle(direction) + Random.Range(-1, 1);
                Quaternion rot = transform.localRotation;
                GameObject.FindObjectOfType<AudioSource>().PlayOneShot(gunS);
                Instantiate(Ballet1P, zyuko, rot).EShot1(angle, 30, 0);
                yield return new WaitForSeconds(0.03f);
            }
            else
            {
                yield return new WaitForSeconds(0.03f);
            }
        }
        spriteRenderer.sprite = normal;
        _movingCoroutine = StartCoroutine("ActionBranch");
    }

    /// <summary>
    /// 衛生
    /// </summary>
    /// <returns></returns>
    private IEnumerator Guardian()
    {
        for (i = 0; i < 8; i++)
        {
            angle = 0;
            Quaternion rot = transform.localRotation;
            GuardC shot = Instantiate(Guard, pos, rot);
            shot.EShot1(10, i * 45, 1, 1, pos);
        }
        GameObject.FindObjectOfType<AudioSource>().PlayOneShot(fireS);

        yield return new WaitForSeconds(3.0f);
        spriteRenderer.sprite = normal;
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 移動RtoL
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveRtoL()
    {
        Debug.Log("MoveRtoL");
        for (j = 0; j < 50; j++)
        {
            transform.position += transform.up * 10;
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(gunS);
            JetPush();
            yield return new WaitForSeconds(0.03f);
        }

        if (_damagePar < 50) SkyFireWorkDown();

        transform.position = new Vector3(100, pos.y, 0);
        spriteRenderer.flipX = true;
        while (pos.y > 165)
        {
            transform.position -= transform.up * 10;
            JetPush();
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(0.03f);
        spriteRenderer.sprite = normal;
        _isPositionLeft = true;
        _movingCoroutine = StartCoroutine("ActionBranch");
    }

    /// <summary>
    /// 花火狙撃
    /// </summary>
    /// <returns></returns>
    private IEnumerator FireWork()
    {
        for ( j = 0;j<3;j++)
        {
            zyuko = pos + new Vector3(32, -20, 0);
            spriteRenderer.sprite = Gun;
            Vector3 direction = ppos - zyuko;
            float angle = GetAngle(direction) + Random.Range(-10, 10);
            Quaternion rot = transform.localRotation;
            FireworkC shot = Instantiate(FireworkP, pos, rot);
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(fireS);
            shot.EShot1(angle, 24, -0.2f, Random.Range(18, 25), 20, 0.5f);
            yield return new WaitForSeconds(1.5f);
        }
        spriteRenderer.sprite = normal;
        _movingCoroutine = StartCoroutine("ActionBranch");
    }

    /// <summary>
    /// 大剣攻撃
    /// </summary>
    /// <returns></returns>
    private IEnumerator SwordSlash()
    {

        spriteRenderer.sprite = swordn;
        movex = (ppos.x - pos.x - 91) / 20;
        movey = (ppos.y - pos.y) / 20;

        for (j = 0; j < 20; j++)
        {
            transform.localPosition += new Vector3(movex, movey, 0);
            spriteRenderer.sprite = swordn;
            if (pos.y < 0) transform.localPosition = new Vector3(pos.x, 0, 0);
            JetPush();
            yield return new WaitForSeconds(0.03f);
        }
        spriteRenderer.sprite = swordr;
        for (j = 0; j < 20; j++)
        {
            JetPush();
            yield return new WaitForSeconds(0.03f);
        }
        Vector3 swingpos = pos + new Vector3(12, 12, 0);
        Quaternion rot = transform.localRotation;
        ExpC shot = Instantiate(SwingP, swingpos, rot);
        shot.EShot1(0, 0, 0.3f);
        spriteRenderer.sprite = swordf;

        float angle2 = Random.Range(250, 290);
        Quaternion rot2 = transform.localRotation;
        ExpC shot2 = Instantiate(jet, new Vector3(pos.x + Random.Range(-32, 32), pos.y - 50, 0), rot);
        shot2.EShot1(angle2, 10, 0.3f);

        for (j = 0; j < 50; j++)
        {
            JetPush();
            yield return new WaitForSeconds(0.03f);
        }

        spriteRenderer.sprite = normal;
        _movingCoroutine = StartCoroutine(MoveLtoR());
    }

    /// <summary>
    /// 移動LtoR
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveLtoR()
    {
        for (j = 0; j < 50; j++)
        {
            transform.position += transform.up * 10;
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(gunS);
            JetPush();
            yield return new WaitForSeconds(0.03f);
        }

        if (_damagePar < 50) SkyFireWorkDown();

        transform.position = new Vector3(540, pos.y, 0);
        spriteRenderer.flipX = false;
        while(pos.y>165)
        {
            transform.position -= transform.up * 10;
            JetPush();
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(0.03f);
        spriteRenderer.sprite = normal;
        _isPositionLeft = false;
        _movingCoroutine = StartCoroutine("ActionBranch");
    }


    /// <summary>
    /// 足ジェット噴射
    /// </summary>
    private void JetPush()
    {
        for (i = 0; i < 3; i++)
        {
            float angle = Random.Range(250, 290);
            Quaternion rot = transform.localRotation;
            ExpC shot = Instantiate(jet, new Vector3(pos.x + Random.Range(-32, 32), pos.y - 50, 0), rot);
            shot.EShot1(angle, 10, 0.3f);
        }
    }

    /// <summary>
    /// 移動時の急襲花火攻撃
    /// </summary>
    private void SkyFireWorkDown()
    {
        for (i = 0; i < 3; i++)
        {
            zyuko = pos + new Vector3(32, -20, 0);
            spriteRenderer.sprite = Gun;
            Vector3 direction = ppos - zyuko;
            float angle = GetAngle(direction) + Random.Range(-10, 10);
            Quaternion rot = transform.localRotation;
            Instantiate(FireworkP, pos, rot).EShot1(angle, 36, -0.2f, Random.Range(18, 25), 20, 0.5f);
        }
        GameObject.FindObjectOfType<AudioSource>().PlayOneShot(fireS);
    }

    //Kakudo
    public float GetAngle(Vector2 direction)
    {
        float rad = Mathf.Atan2(direction.y, direction.x);
        return rad * Mathf.Rad2Deg;
    }

    //Muki
    public Vector3 GetDirection(float angle)
    {
        Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0);
        return direction;
    }
}
