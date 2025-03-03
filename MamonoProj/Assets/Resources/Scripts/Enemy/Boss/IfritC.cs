using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDic.Floor;
using EnumDic.Enemy;

public class IfritC : MonoBehaviour
{
    private int _hpDefault;
    private float _ratioHP = 100;

    private int i, j;
    private MODE_ATTACK _attackVariation = 0;
    private MODE_ATTACK _attackBefore;
    private int _lookAngle = 0;

    private float _angle;

    private float _moveX = 0, _modeDelta = 0,_moveY = 0;

    private Vector3 _posOwn, _posPlayer, target, face, fireworklockon;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// 軸固定
    /// </summary>
    private bool _isLockLookAngle = false;

    private Quaternion rot;

    [SerializeField]
    private BombC BombPrefab;

    [SerializeField]
    private ExpC ExpPrefab, PowderPrefab;

    [SerializeField]
    private ClearEffectC StaffPrefab;

    private GameObject GM;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    [SerializeField]
    private AudioClip fireS, volS, wingS;

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
        GetComponents();

        GM.GetComponent<GameManagement>()._bossNowHp = _eCoreC.hp[0];
        GM.GetComponent<GameManagement>()._bossMaxHp = _eCoreC.hp[0];

        FloorManagerC.SetStageGimic(100, MODE_FLOOR.Normal);
        _eCoreC.IsBoss = true;
        _hpDefault = _eCoreC.hp[0];

    }

    private void GetComponents()
    {
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        _eCoreC = GetComponent<ECoreC>();
        GM = GameObject.Find("GameManager");
        playerGO = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        _ratioHP = _eCoreC.hp[0] * 100 / _hpDefault;
        rot = transform.localRotation;
        _posOwn = transform.position;
        _posPlayer = playerGO.transform.position;

        GM.GetComponent<GameManagement>()._bossNowHp = _eCoreC.hp[0];

        if (!_isLockLookAngle)
        {
            if (_posOwn.x > _posPlayer.x)
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
        _posOwn = transform.position;
        if (_modeDelta != 0) transform.localPosition += GameData.GetSneaking(_posOwn, _posPlayer, _modeDelta);

        //登場
        if (_eCoreC.BossLifeMode == 0)
        {
            _movingCoroutine = StartCoroutine(ActionBranch());
            _eCoreC.BossLifeMode = MODE_LIFE.Fight;
        }

        //死
        if (_eCoreC.BossLifeMode == MODE_LIFE.Dead)
        {
            FloorManagerC.SetStageGimic(100, 0);
            GameData.IsInvincible = true;
            GameData.IsTimerMoving = false;
            if (!_isDeathActionStarted)
            {
                AllCoroutineStop();
                _movingCoroutine = StartCoroutine(DeathAction());
                _isDeathActionStarted = true;
            }
            GM.GetComponent<GameManagement>()._bossNowHp = 0;
        }
    }

    private enum MODE_ATTACK
    {
        Firework,
        Volcano,
        FireAttack
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
            _attackVariation = (MODE_ATTACK)Random.Range(0, 3);
        } while (_attackVariation == _attackBefore);
        _attackBefore = _attackVariation;
        FloorManagerC.SetStageGimic(100, 0);
        FloorManagerC.SetStageGimic((int)(100 - _ratioHP) / 2, MODE_FLOOR.PreBurning);
        switch (_attackVariation)
        {
            case MODE_ATTACK.Firework:
                _movingCoroutine = StartCoroutine(FireWork());
                break;

            case MODE_ATTACK.Volcano:
                Volcano();
                break;

            case MODE_ATTACK.FireAttack:
                _movingCoroutine = StartCoroutine(FireAttack());
                break;
        }
    }

    /// <summary>
    /// 火球放射
    /// </summary>
    /// <returns></returns>
    private IEnumerator FireWork()
    {
        _modeDelta = 100;
        float targetAngle = GameData.GetAngle(_posOwn, _posPlayer);
        for (int j = 0; j < 50; j++)
        {
            for (i = 0; i < 3; i++)
            {
                Vector3 tar = _posOwn + new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
                Vector3 direction2 = _posOwn - tar + new Vector3(0, 30, 0);
                Instantiate(ExpPrefab, tar, rot).EShot1(GameData.GetAngle(tar + new Vector3(0, 30, 0), _posOwn), 10, 0.1f);
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
        if (100 >= _ratioHP && _ratioHP > 60)
        {
            _movingCoroutine = StartCoroutine(VolcanoBreath());
        }

        else if (60 >= _ratioHP && _ratioHP > 30)
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
                _angle = Random.Range(0, 360);
                Quaternion rot3 = transform.localRotation;
                ExpC shot3 = Instantiate(ExpPrefab, _posOwn, rot3);
                shot3.EShot1(_angle, 1 + ((100 - _ratioHP) / 30), 0.8f);
                yield return new WaitForSeconds(0.03f);
            }
        }
        _modeDelta = 50;
        _audioGO.PlayOneShot(volS);
        for (int j = 0; j < 40; j++)
        {
            for (i = 0; i < 3; i++)
            {
                _angle = Random.Range(0, 360);
                ExpC shot3 = Instantiate(ExpPrefab, _posOwn, rot);
                shot3.EShot1(_angle, 4 + ((100 - _ratioHP) / 30), 0.8f);
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
                BombC shot = Instantiate(BombPrefab, _posOwn, rot);
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
                if (_posOwn.x > _posPlayer.x)
                {
                    _lookAngle = 0;//L
                    face = _posOwn + new Vector3(-12, 9, 0);
                }
                else
                {
                    _lookAngle = 1;//R
                    face = _posOwn + new Vector3(12, 9, 0);
                }
            }
            else _isLockLookAngle = true;

            for (i = 0; i < 10; i++)
            {
                if (_lookAngle == 0)
                {
                    transform.localPosition += new Vector3(0.05f, 0, 0);
                    _angle = Random.Range(180 - (60 - _ratioHP / 2), 180 + (60 - _ratioHP / 2));
                    face = _posOwn + new Vector3(-12, 9, 0);
                }
                else
                {
                    transform.localPosition += new Vector3(-0.05f, 0, 0);
                    _angle = Random.Range(-60 + _ratioHP / 2, 61 - _ratioHP / 2);
                    face = _posOwn + new Vector3(12, 9, 0);
                }
                ExpC shot2 = Instantiate(PowderPrefab, face, rot);
                shot2.EShot1(_angle, Random.Range(15, 135), 0.4f);
            }
            yield return new WaitForSeconds(0.03f);

        }
        _audioGO.PlayOneShot(volS);

        for (int j = 0; j < 70; j++)
        {

            for (i = 0; i < 10; i++)
            {
                if (_lookAngle == 0)
                {
                    transform.localPosition += new Vector3(0.05f, 0, 0);
                    _angle = Random.Range(180 - (60 - _ratioHP / 2), 180 + (60 - _ratioHP / 2));
                    face = _posOwn + new Vector3(-12, 9, 0);
                }
                else
                {
                    transform.localPosition += new Vector3(-0.05f, 0, 0);
                    _angle = Random.Range(-60 + _ratioHP / 2, 61 - _ratioHP / 2);
                    face = _posOwn + new Vector3(12, 9, 0);
                }
                ExpC shot2 = Instantiate(ExpPrefab, face, rot);
                shot2.EShot1(_angle, Random.Range(15, 135), 0.4f);
            }

            yield return new WaitForSeconds(0.03f);

        }

        _isLockLookAngle = false;
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
                target = _posPlayer;
                if (_posOwn.x > _posPlayer.x)
                {
                    _lookAngle = 0;//L
                    face = _posOwn + new Vector3(-12, 9, 0);
                }
                else
                {
                    _lookAngle = 1;//R
                    face = _posOwn + new Vector3(12, 9, 0);
                }
            }
            else _isLockLookAngle = true;

            for (i = 0; i < 10; i++)
            {

                Vector3 direction = target - _posOwn;
                float angle = GameData.GetAngle(_posOwn, target) + Random.Range(-20, 20);
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

                Vector3 direction = target - _posOwn;
                float angle = GameData.GetAngle(_posOwn, target) + Random.Range(-20, 20);
                ExpC shot2 = Instantiate(ExpPrefab, face, rot);
                shot2.EShot1(angle, Random.Range(15, 135), 0.4f);
            }

            yield return new WaitForSeconds(0.03f);

        }

        _isLockLookAngle = false;
        yield return new WaitForSeconds(0.3f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// なぎはらい火炎放射2
    /// </summary>
    /// <returns></returns>
    private IEnumerator VolcanoSlash()
    {
        target = _posPlayer;
        _isLockLookAngle = true;
        for (int j = 0; j < 30; j++)
        {

            for (i = 0; i < 10; i++)
            {
                if (_lookAngle == 0)
                {
                    transform.localPosition += new Vector3(0.05f, 0, 0);
                    _angle = 270 + ((-30 + j) * (-30 + j) * 0.03f);
                    face = _posOwn + new Vector3(-12, 9, 0);
                }
                else
                {
                    transform.localPosition += new Vector3(-0.05f, 0, 0);
                    _angle = 270 - ((-30 + j) * (-30 + j) * 0.03f);
                    face = _posOwn + new Vector3(12, 9, 0);
                }
                ExpC shot2 = Instantiate(PowderPrefab, face, rot);
                shot2.EShot1(_angle + Random.Range(-10, 10), Random.Range(15, 135), 0.4f);
            }
            yield return new WaitForSeconds(0.03f);

        }
        _audioGO.PlayOneShot(volS);

        for (int j = 0; j < 70; j++)
        {

            for (i = 0; i < 10; i++)
            {
                if (_lookAngle == 0)
                {
                    transform.localPosition += new Vector3(0.05f, 0, 0);
                    _angle = 270 - (j * j * 0.03f);
                    face = _posOwn + new Vector3(-12, 9, 0);
                }
                else
                {
                    transform.localPosition += new Vector3(-0.05f, 0, 0);
                    _angle = 270 + (j * j * 0.03f);
                    face = _posOwn + new Vector3(12, 9, 0);
                }
                ExpC shot2 = Instantiate(ExpPrefab, face, rot);
                shot2.EShot1(_angle + Random.Range(-10, 10), Random.Range(15, 135), 0.4f);
            }

            yield return new WaitForSeconds(0.03f);

        }

        _isLockLookAngle = false;
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

                    Instantiate(PowderPrefab, _posOwn, rot).EShot1(Random.Range(0, 360), 10 + (j * 10), 0.4f);
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
            Instantiate(PowderPrefab, _posOwn, rot).EShot1(Random.Range(0, 360), 20, 0.4f);
        }
        if (GameData.Round == GameData.GoalRound)
        {
            Instantiate(StaffPrefab, new Vector3(320, -100, 0), Quaternion.Euler(0, 0, 0));
        }
        else
        {
            playerGO.GetComponent<PlayerC>().StageMoveAction();
        }
        Destroy(gameObject);
    }
}
