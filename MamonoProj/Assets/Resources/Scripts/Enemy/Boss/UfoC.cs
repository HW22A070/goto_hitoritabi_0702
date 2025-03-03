using EnumDic.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoC : MonoBehaviour
{
    /// <summary>
    /// 加速度
    /// </summary>
    private float _xMove=0,_yMove=0;

    private int i, j, k, texture;
        
    private MODE_ATTACK _attackMode;

    private Vector3 _posOwn,_posPlayer;

    private float _xPosMove = 5;
    private float _yPosMove = 5;
    private float movexx, moveyy;

    private bool _isFirstAttack;

    [SerializeField]
    private GuardC GuardPrefab;

    [SerializeField]
    private EMissile1C EMissile1Prefab;

    [SerializeField]
    private ExpC LEPrefab;

    [SerializeField]
    private ExpC _prfbTarget;

    [SerializeField]
    private ClearEffectC StaffPrefab;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite a, b;

    private GameObject GM;

    /// <summary>
    /// アニメーション
    /// </summary>
    private Animator _animAttackEffect;

    [SerializeField]
    [Tooltip("PlayerGameObject")]
    private GameObject playerGO;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    [SerializeField]
    private AudioClip magicS, moveS, chargeS;

    /// <summary>
    /// ECoreCのコンポーネント
    /// </summary>
    private ECoreC _eCoreC;


    /// <summary>
    /// 動作中のコルーチン
    /// </summary>
    private Coroutine _movingCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        GetComponents();

        _eCoreC.IsBoss = true;
        GM.GetComponent<GameManagement>()._bossNowHp = _eCoreC.hp[0];
        GM.GetComponent<GameManagement>()._bossMaxHp = _eCoreC.hp[0];
        _movingCoroutine=StartCoroutine(Moving());
    }

    private void GetComponents()
    {
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        playerGO = GameObject.Find("Player");
        GM = GameObject.Find("GameManager");
        //アニメーション
        _animAttackEffect = GetComponent<Animator>();
        _eCoreC = GetComponent<ECoreC>();
    }

    // Update is called once per frame
    void Update()
    {
        _posOwn = transform.position;
        _posPlayer = playerGO.transform.position;

        if (_eCoreC.BossLifeMode != MODE_LIFE.Dead) GM.GetComponent<GameManagement>()._bossNowHp = _eCoreC.hp[0];

        if (_posOwn.x > 632)_xMove = -14f;
        if (_posOwn.x < 8)_xMove = 14f;
        if (_posOwn.y > 462) _yMove = -14f;
        if (_posOwn.y < 8) _yMove = 14f;
    }

    void FixedUpdate()
    {
        //死
        if (_eCoreC.BossLifeMode == MODE_LIFE.Dead)
        {
            GameData.IsTimerMoving = false;
            GameData.IsInvincible = true;
            AllCoroutineStop();

            GM.GetComponent<GameManagement>()._bossNowHp = 0;
            transform.localPosition += new Vector3(Random.Range(-10, 10), -5, 0);
            transform.localEulerAngles += new Vector3(0, 0, 10);
            if (_posOwn.y < -64)
            {
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
    }

    //移動
    private IEnumerator Moving()
    {

        _xPosMove = Random.Range(50, 590);
        _yPosMove = Random.Range(50, 430);
        for (j = 0; j < 30; j++)
        {
            float angle = Random.Range(0, 360);
            Quaternion rot = transform.localRotation;
            Instantiate(LEPrefab, new Vector3(_xPosMove, _yPosMove, 0), rot).EShot1(angle, 0, 0.05f);
            yield return new WaitForSeconds(0.03f);
        }
        movexx = (_xPosMove - _posOwn.x) / 10;
        moveyy = (_yPosMove - _posOwn.y) / 10;
        _audioGO.PlayOneShot(moveS);
        //攻撃アニメーションスタート
        _animAttackEffect.SetBool("Attack", true);

        for (j = 0; j < 10; j++)
        {
            transform.localPosition += new Vector3(movexx, moveyy, 0);
            yield return new WaitForSeconds(0.03f);
        }
        //攻撃アニメーションおわり
        _animAttackEffect.SetBool("Attack", false);
        yield return new WaitForSeconds(0.03f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    private enum MODE_ATTACK
    {
        GuardianBeam,
        MachineGun,
        MachineGun_Horming,
        Horming
    }

    //行動変わるヤツ
    private IEnumerator ActionBranch()
    {
        yield return new WaitForSeconds(0.3f);

        _attackMode = (MODE_ATTACK)Random.Range(0, 4);
        switch (_attackMode)
        {
            case MODE_ATTACK.GuardianBeam:
                _movingCoroutine = StartCoroutine(GuardianBeam());
                break;

            case MODE_ATTACK.MachineGun:
                if (_posPlayer.y < 240)
                {
                    _movingCoroutine = StartCoroutine(MachineGun_Under());
                }
                else
                {
                    _movingCoroutine = StartCoroutine(MachineGun_Random());
                }
                break;

            case MODE_ATTACK.MachineGun_Horming:
                _movingCoroutine = StartCoroutine(MachineGun_Horming());
                break;

            case MODE_ATTACK.Horming:
                _movingCoroutine = StartCoroutine(Horming());
                break;

        }
        _audioGO.PlayOneShot(magicS);
        //GetAttackCount();
    }

    //回転エネルギー弾
    private IEnumerator GuardianBeam()
    {
        _animAttackEffect.SetBool("Beam", true);

        _audioGO.PlayOneShot(chargeS);
        yield return new WaitForSeconds(0.3f);
        _animAttackEffect.SetBool("Beam", false);
        for (k = 0; k < 5; k++)
        {
            for (j = 0; j < 6; j++)
            {
                Quaternion rot = transform.localRotation;
                GuardC shot = Instantiate(GuardPrefab, _posOwn, rot);
                shot.EShot1(10, j * 60, 1.5f, 10, _posOwn);
            }
            for (j = 0; j < 6; j++)
            {
                Quaternion rot = transform.localRotation;
                GuardC shot = Instantiate(GuardPrefab, _posOwn, rot);
                shot.EShot1(10, j * 60, -1.5f, 10, _posOwn);
            }
            yield return new WaitForSeconds(0.3f);
            _audioGO.PlayOneShot(magicS);
        }

        GetAttackCount();
    }

    /// <summary>
    /// マシンガン下方向
    /// </summary>
    /// <returns></returns>
    private IEnumerator MachineGun_Under()
    {
        _animAttackEffect.SetBool("Beam", true);

        _audioGO.PlayOneShot(chargeS);
        yield return new WaitForSeconds(0.3f);
        _animAttackEffect.SetBool("Beam", false);



        for (j = 0; j < 20; j++)
        {
            _audioGO.PlayOneShot(magicS);
            Vector3 direction = _posPlayer - _posOwn;
            float angle = GameData.GetAngle(_posOwn, _posPlayer);
            angle = Random.Range(260, 280);
            for (k = 10; k < 40; k += 2)
            {
                Quaternion rot = transform.localRotation;
                EMissile1C shot = Instantiate(EMissile1Prefab, _posOwn, rot);
                shot.EShot1(angle, 0, k + 1);
            }
            yield return new WaitForSeconds(0.06f);
        }

        GetAttackCount();
    }

    /// <summary>
    /// マシンガン乱射（プレイヤー避ける）
    /// </summary>
    /// <returns></returns>
    private IEnumerator MachineGun_Random()
    {
        _animAttackEffect.SetBool("Beam", true);

        _audioGO.PlayOneShot(chargeS);
        yield return new WaitForSeconds(0.3f);
        _animAttackEffect.SetBool("Beam", false);

        for (j = 0; j < 20; j++)
        {
            _audioGO.PlayOneShot(magicS);
            Vector3 direction = _posPlayer - _posOwn;
            for (int i = 0; i < 3; i++)
            {
                float angle = GameData.GetAngle(_posOwn, _posPlayer);
                angle += Random.Range(20, 340);
                for (k = 10; k < 40; k += 2)
                {
                    Quaternion rot = transform.localRotation;
                    EMissile1C shot = Instantiate(EMissile1Prefab, _posOwn, rot);
                    shot.EShot1(angle, 0, k + 1);
                }
            }
            yield return new WaitForSeconds(0.06f);
        }

        GetAttackCount();
    }

    /// <summary>
    /// マシンガン狙撃
    /// </summary>
    /// <returns></returns>
    private IEnumerator MachineGun_Horming()
    {
        _animAttackEffect.SetBool("Beam", true);

        Vector3 targetPos = _posPlayer;
        float angle = GameData.GetAngle(_posOwn, targetPos);
        Vector3 direction = GameData.GetDirection(angle).normalized;

        Quaternion rot = transform.localRotation;

        float distance = GameData.GetDistance(_posOwn, _posPlayer);
        if (distance < 0) distance *= -1;

        for (int k = 0; k < 7; k++)
        {
            _audioGO.PlayOneShot(moveS);
            Instantiate(_prfbTarget, _posOwn+direction*(distance/7)*(7-k), rot).EShot1(0, 0, 0.3f+(7-k)*0.3f);

            for(int j = 0; j < 10; j++)
            {
                Instantiate(LEPrefab, _posOwn, rot).EShot1(Random.Range(0, 360), 0, 0.07f);
                yield return new WaitForSeconds(0.03f);
            }
        }

        _audioGO.PlayOneShot(chargeS);
        yield return new WaitForSeconds(0.3f);

        _animAttackEffect.SetBool("Beam", false);

        for (j = 0; j < 20; j++)
        {
            _audioGO.PlayOneShot(magicS);

            for (int i = 0; i < 3; i++)
            {

                float SettedAngle =angle+ Random.Range(-10,10);
                for (k = 10; k < 40; k += 2)
                {
                    EMissile1C shot = Instantiate(EMissile1Prefab, _posOwn, rot);
                    shot.EShot1(SettedAngle, 0, k + 1);
                }
            }
            yield return new WaitForSeconds(0.06f);
        }

        GetAttackCount();
    }

    //追尾突進
    private IEnumerator Horming()
    {
        //攻撃アニメーションスタート
        _animAttackEffect.SetBool("Attack", true);
        _audioGO.PlayOneShot(chargeS);
        yield return new WaitForSeconds(0.3f);

        for (j = 0; j < 150; j++)
        {
            if (_posOwn.x > _posPlayer.x - 64)
            {
                if (_xMove > -7) _xMove -= 0.4f;
            }
            if (_posOwn.x < _posPlayer.x + 64)
            {
                if (_xMove < 7) _xMove += 0.4f;
            }
            if (_posOwn.y > _posPlayer.y - 32)
            {
                if (_yMove > -7) _yMove -= 0.4f;
            }
            if (_posOwn.y < _posPlayer.y + 32)
            {
                if (_yMove < 7) _yMove += 0.4f;
            }
            transform.localPosition += new Vector3(_xMove, _yMove, 0);

            yield return new WaitForSeconds(0.03f);
        }
        _animAttackEffect.SetBool("Attack", false);
        GetAttackCount();
    }

    //攻撃何回目？１ならもっかい２なら終わり
    private void GetAttackCount()
    {
        if (!_isFirstAttack)
        {
            _isFirstAttack = true;
            _movingCoroutine = StartCoroutine(ActionBranch());
        }
        else
        {
            _isFirstAttack = false;
            _movingCoroutine = StartCoroutine(Moving());
        }
    }

    private void AllCoroutineStop()
    {
        if (_movingCoroutine != null)
        {
            StopCoroutine(_movingCoroutine);
            _movingCoroutine = null;
        }
    }
}
