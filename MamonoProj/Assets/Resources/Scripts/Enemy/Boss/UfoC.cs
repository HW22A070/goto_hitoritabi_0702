using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoC : MonoBehaviour
{

    /// <summary>
    /// 加速度
    /// </summary>
    private float _xMove=0,_yMove=0;

    private int i,j,k, texture, _attackMode;

    private Vector3 pos, shutu,ppos;

    private float movex = 5;
    private float movey = 5;
    private float movexx, moveyy;

    private bool _isFirstAttack;

    public GuardC GuardPrefab;
    public EMissile1C EMissile1Prefab;
    public ExpC LEPrefab;

    public SpriteRenderer spriteRenderer;
    public Sprite a, b;

    private GameObject GM;

    [SerializeField]
    [Tooltip("PlayerGameObject")]
    private GameObject playerGO;

    public AudioClip magicS, moveS, chargeS;

    private Vector3 hitPos;

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
        _eCoreC = GetComponent<ECoreC>();
        _eCoreC.IsBoss = true;
        playerGO = GameObject.Find("Player");
        GM = GameObject.Find("GameManager");
        GM.GetComponent<GameManagement>()._bossNowHp = _eCoreC.hp[0];
        GM.GetComponent<GameManagement>()._bossMaxHp = _eCoreC.hp[0];
        StartCoroutine("Moving");
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        ppos = playerGO.transform.position;

        if (_eCoreC.BossLifeMode != 2) GM.GetComponent<GameManagement>()._bossNowHp = _eCoreC.hp[0];

        if (pos.x > 632)_xMove = -14f;
        if (pos.x < 8)_xMove = 14f;
        if (pos.y > 462) _yMove = -14f;
        if (pos.y < 8) _yMove = 14f;
    }

    void FixedUpdate()
    {
        //死
        if (_eCoreC.BossLifeMode == 2)
        {
            AllCoroutineStop();

            GM.GetComponent<GameManagement>()._bossNowHp = 0;
            transform.localPosition += new Vector3(Random.Range(-10, 10), -5, 0);
            transform.localEulerAngles += new Vector3(0, 0, 10);
            if (pos.y < -64)
            {
                playerGO.GetComponent<PlayerC>().StageMoveAction();
                Destroy(gameObject);
            }
        }
    }

    //移動
    private IEnumerator Moving()
    {
        movex = Random.Range(50, 590);
        movey = Random.Range(50, 430);
        for (j = 0; j < 30; j++)
        {
            float angle = Random.Range(0, 360);
            Quaternion rot = transform.localRotation;
            ExpC shot = Instantiate(LEPrefab, new Vector3(movex, movey, 0), rot);
            shot.EShot1(angle, 0, 0.05f);
            yield return new WaitForSeconds(0.03f);
        }
        movexx = (movex - pos.x) / 10;
        moveyy = (movey - pos.y) / 10;
        GameObject.FindObjectOfType<AudioSource>().PlayOneShot(moveS);
        spriteRenderer.sprite = b;
        for (j = 0; j < 10; j++)
        {
            transform.localPosition += new Vector3(movexx, moveyy, 0);
            yield return new WaitForSeconds(0.03f);
        }
        spriteRenderer.sprite = a;
        yield return new WaitForSeconds(0.03f);
        StartCoroutine("ActionBranch");
    }

    //行動変わるヤツ
    private IEnumerator ActionBranch()
    {
        yield return new WaitForSeconds(0.03f);
        _attackMode = Random.Range(0, 4);
        if (_attackMode == 1) StartCoroutine("GuardianBeam");
        else if (_attackMode == 2) StartCoroutine("MachineGun");
        else if (_attackMode == 3) StartCoroutine("Horming");
        else
        {
            for (i = 0; i < 8; i++)
            {
                Quaternion rot = transform.localRotation;
                GuardC shot = Instantiate(GuardPrefab, pos, rot);
                shot.EShot1(10, i * 45, -1, 5, pos);
            }
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(magicS);
            HowManyAttack();
        }
    }

    //回転エネルギー弾
    private IEnumerator GuardianBeam()
    {
        GameObject.FindObjectOfType<AudioSource>().PlayOneShot(chargeS);
        yield return new WaitForSeconds(0.3f);
        for (k = 0; k < 5; k++)
        {
            for (j = 0; j < 6; j++)
            {
                Quaternion rot = transform.localRotation;
                GuardC shot = Instantiate(GuardPrefab, pos, rot);
                shot.EShot1(10, j * 60, 1.5f, 10, pos);
            }
            for (j = 0; j < 6; j++)
            {
                Quaternion rot = transform.localRotation;
                GuardC shot = Instantiate(GuardPrefab, pos, rot);
                shot.EShot1(10, j * 60, -1.5f, 10, pos);
            }
            spriteRenderer.sprite = b;
            yield return new WaitForSeconds(0.03f);
            spriteRenderer.sprite = a;
            yield return new WaitForSeconds(0.27f);
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(magicS);
        }
        HowManyAttack();
    }    
    
    //マシンガン
    private IEnumerator MachineGun()
    {
        GameObject.FindObjectOfType<AudioSource>().PlayOneShot(chargeS);
        yield return new WaitForSeconds(0.3f);
        for (j = 0; j < 10; j++)
        {
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(magicS);
            Vector3 direction = ppos - pos;
            float angle = GetAngle(direction);
            angle += Random.Range(20, 340);
            if (pos.y > 240) angle = Random.Range(260, 280);
            for (k = 10; k < 40; k+=2)
            {
                Quaternion rot = transform.localRotation;
                EMissile1C shot = Instantiate(EMissile1Prefab, pos, rot);
                shot.EShot1(angle, 0, k + 1);
            }
            spriteRenderer.sprite = b;
            yield return new WaitForSeconds(0.03f);
            spriteRenderer.sprite = a;
            yield return new WaitForSeconds(0.06f);
        }
        HowManyAttack();
    }    
    
    //追尾突進
    private IEnumerator Horming()
    {
        GameObject.FindObjectOfType<AudioSource>().PlayOneShot(chargeS);
        yield return new WaitForSeconds(0.3f);
        for (j = 0; j < 150; j++)
        {
            if (pos.x > ppos.x - 64)
            {
                if (_xMove > -7) _xMove -= 0.4f;
            }
            if (pos.x < ppos.x + 64)
            {
                if (_xMove < 7) _xMove += 0.4f;
            }
            if (pos.y > ppos.y - 32)
            {
                if (_yMove > -7) _yMove -= 0.4f;
            }
            if (pos.y < ppos.y + 32)
            {
                if (_yMove < 7) _yMove += 0.4f;
            }
            transform.localPosition += new Vector3(_xMove, _yMove, 0);
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

            yield return new WaitForSeconds(0.03f);
        }
        spriteRenderer.sprite = a;
        HowManyAttack();
    }

    //攻撃何回目？１ならもっかい２なら終わり
    private void HowManyAttack()
    {
        if (!_isFirstAttack)
        {
            _isFirstAttack = true;
            StartCoroutine("ActionBranch");
        }
        else
        {
            _isFirstAttack = false;
            StartCoroutine("Moving");
        }
    }

    private void AllCoroutineStop()
    {
        StopCoroutine("ActionBranch");
        StopCoroutine("Horming");
        StopCoroutine("MachineGun");
        StopCoroutine("GuardianBeam");
        StopCoroutine("Moving");
    }

    public float GetAngle(Vector2 direction)
    {
        float rad = Mathf.Atan2(direction.y, direction.x);
        return rad * Mathf.Rad2Deg;
    }    
}
