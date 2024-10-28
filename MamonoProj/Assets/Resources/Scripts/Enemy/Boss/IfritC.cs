using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfritC : MonoBehaviour
{
    private int firstHP;
    private float damagePar = 100;

    private int i, j;
    private int _attackVariation = 0;
    private int mae = 0;
    private int look = 0;

    private float angle;

    private float movex = 0, _modeDelta = 0;
    private float movey = 0;

    private Vector3 pos, ppos, target, face, fireworklockon;

    public SpriteRenderer spriteRenderer;
    private bool _lookLock = false;

    private Quaternion rot;

    public BombC BombPrefab;
    public ExpC ExpPrefab, PowderPrefab;
    public StaffRollC StaffPrefab;

    private GameObject GM;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    public AudioClip fireS, volS, wingS;

    private bool _isDeathActionStarted;

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
        FloorManagerC.StageGimic(100, 0);
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        _eCoreC = GetComponent<ECoreC>();
        _eCoreC.IsBoss = true;
        firstHP = _eCoreC.hp[0];
        GM = GameObject.Find("GameManager");
        GM.GetComponent<GameManagement>()._bossNowHp = _eCoreC.hp[0];
        GM.GetComponent<GameManagement>()._bossMaxHp = _eCoreC.hp[0];
        playerGO = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        damagePar = _eCoreC.hp[0] * 100 / firstHP;
        rot = transform.localRotation;
        pos = transform.position;
        ppos = playerGO.transform.position;

        GM.GetComponent<GameManagement>()._bossNowHp = _eCoreC.hp[0];

        if (!_lookLock)
        {
            if (pos.x > ppos.x)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }

    }



    void FixedUpdate()
    {
        pos = transform.position;
        if (_modeDelta != 0) transform.localPosition += GameData.GetSneaking(pos, ppos, _modeDelta);

        //登場
        if (_eCoreC.BossLifeMode == 0)
        {
            _movingCoroutine = StartCoroutine(ActionBranch());
            _eCoreC.BossLifeMode = 1;
        }

        //死
        if (_eCoreC.BossLifeMode == 2)
        {
            FloorManagerC.StageGimic(100, 0);
            GameData.Star = true;
            GameData.TimerMoving = false;
            if (!_isDeathActionStarted)
            {
                AllCoroutineStop();
                _movingCoroutine = StartCoroutine(DeathAction());
                _isDeathActionStarted = true;
            }
            GM.GetComponent<GameManagement>()._bossNowHp = 0;
        }
    }



    /// <summary>
    /// 行動分岐
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActionBranch()
    {
        yield return new WaitForSeconds(0.15f);
        do
        {
            _attackVariation = Random.Range(0, 3);
        } while (_attackVariation == mae);
        mae = _attackVariation;
        FloorManagerC.StageGimic(100, 0);
        FloorManagerC.StageGimic((int)(100 - damagePar) / 2, 2);
        if (_attackVariation == 0) _movingCoroutine = StartCoroutine(FireWork());
        else if (_attackVariation == 1) Volcano();
        else if (_attackVariation == 2) _movingCoroutine = StartCoroutine(FireAttack());
    }

    /// <summary>
    /// 火球放射
    /// </summary>
    /// <returns></returns>
    private IEnumerator FireWork()
    {
        _modeDelta = 100;
        float targetAngle = GameData.GetAngle(pos, ppos);
        for (int j = 0; j < 50; j++)
        {
            for (i = 0; i < 3; i++)
            {
                Vector3 tar = pos + new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
                Vector3 direction2 = pos - tar + new Vector3(0, 30, 0);
                Instantiate(ExpPrefab, tar, rot).EShot1(GameData.GetAngle(tar + new Vector3(0, 30, 0), pos), 10, 0.1f);
            }
            yield return new WaitForSeconds(0.03f);
        }
        StartCoroutine(FireWorks(targetAngle));
        _audioGO.PlayOneShot(fireS);
        yield return new WaitForSeconds(0.3f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }


    /// <summary>
    /// 火炎放射
    /// </summary>
    /// <returns></returns>
    private void Volcano()
    {
        _modeDelta = 0;
        if (100 >= damagePar && damagePar > 60)
        {
            _movingCoroutine = StartCoroutine(VolcanoBreath());
        }

        else if (60 >= damagePar && damagePar > 30)
        {
            _movingCoroutine = StartCoroutine(VolcanoHorming());
        }

        else
        {
            _movingCoroutine = StartCoroutine(VolcanoSlash());
        }

    }

    /// <summary>
    /// 火の粉突進
    /// </summary>
    /// <returns></returns>
    private IEnumerator FireAttack()
    {
        _modeDelta = 0;
        for (int j = 0; j < 20; j++)
        {
            for (i = 0; i < 3; i++)
            {
                angle = Random.Range(0, 360);
                Quaternion rot3 = transform.localRotation;
                ExpC shot3 = Instantiate(ExpPrefab, pos, rot3);
                shot3.EShot1(angle, 1 + ((100 - damagePar) / 30), 0.8f);
                yield return new WaitForSeconds(0.03f);
            }
        }
        _modeDelta = 50;
        _audioGO.PlayOneShot(volS);
        for (int j = 0; j < 40; j++)
        {
            for (i = 0; i < 3; i++)
            {
                angle = Random.Range(0, 360);
                ExpC shot3 = Instantiate(ExpPrefab, pos, rot);
                shot3.EShot1(angle, 4 + ((100 - damagePar) / 30), 0.8f);
                yield return new WaitForSeconds(0.03f);
            }
        }
        yield return new WaitForSeconds(0.3f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 爆発火球放射
    /// </summary>
    /// <returns></returns>
    private IEnumerator FireWorks(float targetAngle)
    {
        for (i = 0; i < 10; i++)
        {
            for (j = 0; j < 3; j++)
            {
                BombC shot = Instantiate(BombPrefab, pos, rot);
                shot.EShot1(targetAngle + Random.Range(-30, 30), 12 + (i * 1.2f), -0.6f, 20 + (i * 2), 5, 0.5f);
            }
            yield return new WaitForSeconds(0.03f);
        }
    }

    /// <summary>
    /// 広範囲火炎放射0
    /// </summary>
    /// <returns></returns>
    private IEnumerator VolcanoBreath()
    {
        for (int j = 0; j < 40; j++)
        {
            if (j < 20)
            {
                if (pos.x > ppos.x)
                {
                    look = 0;//L
                    face = pos + new Vector3(-12, 9, 0);
                }
                else
                {
                    look = 1;//R
                    face = pos + new Vector3(12, 9, 0);
                }
            }
            else _lookLock = true;

            for (i = 0; i < 10; i++)
            {
                if (look == 0)
                {
                    transform.localPosition += new Vector3(0.05f, 0, 0);
                    angle = Random.Range(180 - (60 - damagePar / 2), 180 + (60 - damagePar / 2));
                    face = pos + new Vector3(-12, 9, 0);
                }
                else
                {
                    transform.localPosition += new Vector3(-0.05f, 0, 0);
                    angle = Random.Range(-60 + damagePar / 2, 61 - damagePar / 2);
                    face = pos + new Vector3(12, 9, 0);
                }
                ExpC shot2 = Instantiate(PowderPrefab, face, rot);
                shot2.EShot1(angle, Random.Range(15, 135), 0.4f);
            }
            yield return new WaitForSeconds(0.03f);

        }
        _audioGO.PlayOneShot(volS);

        for (int j = 0; j < 70; j++)
        {

            for (i = 0; i < 10; i++)
            {
                if (look == 0)
                {
                    transform.localPosition += new Vector3(0.05f, 0, 0);
                    angle = Random.Range(180 - (60 - damagePar / 2), 180 + (60 - damagePar / 2));
                    face = pos + new Vector3(-12, 9, 0);
                }
                else
                {
                    transform.localPosition += new Vector3(-0.05f, 0, 0);
                    angle = Random.Range(-60 + damagePar / 2, 61 - damagePar / 2);
                    face = pos + new Vector3(12, 9, 0);
                }
                ExpC shot2 = Instantiate(ExpPrefab, face, rot);
                shot2.EShot1(angle, Random.Range(15, 135), 0.4f);
            }

            yield return new WaitForSeconds(0.03f);

        }

        _lookLock = false;
        yield return new WaitForSeconds(0.3f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 追跡火炎放射1
    /// </summary>
    /// <returns></returns>
    private IEnumerator VolcanoHorming()
    {

        for (int j = 0; j < 40; j++)
        {
            if (j < 20)
            {
                target = ppos;
                if (pos.x > ppos.x)
                {
                    look = 0;//L
                    face = pos + new Vector3(-12, 9, 0);
                }
                else
                {
                    look = 1;//R
                    face = pos + new Vector3(12, 9, 0);
                }
            }
            else _lookLock = true;

            for (i = 0; i < 10; i++)
            {

                Vector3 direction = target - pos;
                float angle = GameData.GetAngle(pos, target) + Random.Range(-20, 20);
                ExpC shot2 = Instantiate(PowderPrefab, face, rot);
                shot2.EShot1(angle, Random.Range(15, 135), 0.4f);
            }
            yield return new WaitForSeconds(0.03f);

        }
        _audioGO.PlayOneShot(volS);

        for (int j = 0; j < 70; j++)
        {
            for (i = 0; i < 10; i++)
            {

                Vector3 direction = target - pos;
                float angle = GameData.GetAngle(pos, target) + Random.Range(-20, 20);
                ExpC shot2 = Instantiate(ExpPrefab, face, rot);
                shot2.EShot1(angle, Random.Range(15, 135), 0.4f);
            }

            yield return new WaitForSeconds(0.03f);

        }

        _lookLock = false;
        yield return new WaitForSeconds(0.3f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// なぎはらい火炎放射2
    /// </summary>
    /// <returns></returns>
    private IEnumerator VolcanoSlash()
    {
        target = ppos;
        _lookLock = true;
        for (int j = 0; j < 30; j++)
        {

            for (i = 0; i < 10; i++)
            {
                if (look == 0)
                {
                    transform.localPosition += new Vector3(0.05f, 0, 0);
                    angle = 270 + ((-30 + j) * (-30 + j) * 0.03f);
                    face = pos + new Vector3(-12, 9, 0);
                }
                else
                {
                    transform.localPosition += new Vector3(-0.05f, 0, 0);
                    angle = 270 - ((-30 + j) * (-30 + j) * 0.03f);
                    face = pos + new Vector3(12, 9, 0);
                }
                ExpC shot2 = Instantiate(PowderPrefab, face, rot);
                shot2.EShot1(angle + Random.Range(-10, 10), Random.Range(15, 135), 0.4f);
            }
            yield return new WaitForSeconds(0.03f);

        }
        _audioGO.PlayOneShot(volS);

        for (int j = 0; j < 70; j++)
        {

            for (i = 0; i < 10; i++)
            {
                if (look == 0)
                {
                    transform.localPosition += new Vector3(0.05f, 0, 0);
                    angle = 270 - (j * j * 0.03f);
                    face = pos + new Vector3(-12, 9, 0);
                }
                else
                {
                    transform.localPosition += new Vector3(-0.05f, 0, 0);
                    angle = 270 + (j * j * 0.03f);
                    face = pos + new Vector3(12, 9, 0);
                }
                ExpC shot2 = Instantiate(ExpPrefab, face, rot);
                shot2.EShot1(angle + Random.Range(-10, 10), Random.Range(15, 135), 0.4f);
            }

            yield return new WaitForSeconds(0.03f);

        }

        _lookLock = false;
        yield return new WaitForSeconds(0.3f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    private void AllCoroutineStop()
    {
        StopAllCoroutines();
        _movingCoroutine = null;
    }

    /// <summary>
    /// 死アクション
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeathAction()
    {
        _modeDelta = 0;
        _eCoreC.SummonItems();
        for (int k = 0; k < 3; k++)
        {
            for (int j = 0; j < 5; j++)
            {
                for (int l = 0; l < 10; l++)
                {

                    Instantiate(PowderPrefab, pos, rot).EShot1(Random.Range(0, 360), 10 + (j * 10), 0.4f);
                }
            }

            for (int j = 0; j < 30; j++)
            {
                transform.localPosition += new Vector3(Random.Range(-10, 10), 0, 0);
                transform.localEulerAngles += new Vector3(0, 0, 10);
                yield return new WaitForSeconds(0.03f);
            }
        }
        for (int j = 0; j < 50; j++)
        {
            Instantiate(PowderPrefab, pos, rot).EShot1(Random.Range(0, 360), 20, 0.4f);
        }
        if (GameData.Round == GameData.GoalRound)
        {
            Instantiate(StaffPrefab, new Vector3(320, -100, 0), Quaternion.Euler(0, 0, 0)).Summon(0);
        }
        else
        {
            playerGO.GetComponent<PlayerC>().StageMoveAction();
        }
        Destroy(gameObject);
    }
}
