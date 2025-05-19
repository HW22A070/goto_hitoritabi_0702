using EnumDic.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCoreC : ETypeCoreC
{
    protected int _firstHP;

    /// <summary>
    /// HP����
    /// </summary>
    protected float _damagePar;


    [SerializeField]
    protected ClearEffectC StaffPrefab;

    protected GameManagement _gameManaC;

    /// <summary>
    /// ���쒆�̃R���[�`��
    /// </summary>
    protected Coroutine _movingCoroutine;

    /// <summary>
    /// �P�ނ��邩
    /// </summary>
    protected bool _isLefted;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        _gameManaC = GameObject.Find("GameManager").GetComponent<GameManagement>();
        for (int j = 0; j < _eCoreC.hp.Length; j++)
        {
            _firstHP += _eCoreC.hp[j];
        }

        _gameManaC._bossNowHp = _firstHP;
        _gameManaC._bossMaxHp = _firstHP;

        StartCoroutine(ArrivalAction());
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
        _damagePar = _eCoreC.hp[0] * 100 / _firstHP;
        if (_eCoreC.GetModeBossLife() != MODE_LIFE.Dead) _gameManaC._bossNowHp = _eCoreC.hp[0];
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        switch (_eCoreC.GetModeBossLife())
        {
            case MODE_LIFE.Arrival:
                FxUpArrival();
                break;

            case MODE_LIFE.Fight:
                FxUpFight();
                break;

            case MODE_LIFE.Dead:
                FxUpDead();
                break;

            case MODE_LIFE.Leave:
                FxUpLeave();
                break;
        }
    }

    protected virtual void FxUpArrival() { }
    protected virtual void FxUpFight() { }
    protected virtual void FxUpDead() { }

    protected virtual void FxUpLeave() {
        if (!_isLefted)
        {
            AllCoroutineStop();
            StartCoroutine(LeaveAction());
            _isLefted = true;
        }
    }

    protected virtual IEnumerator ActionBranch()
    {
        yield return new WaitForFixedUpdate();
    }

    //�S��~
    protected virtual void AllCoroutineStop()
    {
        if (_movingCoroutine != null)
        {
            StopCoroutine(_movingCoroutine);
            _movingCoroutine = null;
        }
    }

    /// <summary>
    /// �o��A�N�V����
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator ArrivalAction() {
        yield return new WaitForFixedUpdate();
        _eCoreC.SetModeBossLife(MODE_LIFE.Fight);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// ���j�A�N�V����
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator DeadAction()
    {
        yield return new WaitForFixedUpdate();
        DoCollapuse();
    }

    /// <summary>
    /// �v���C���[�S�ŃA�N�V����
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator LeaveAction()
    {
        for (int i= 0; i < 1028; i++){
            transform.position += transform.up*i;
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// ���Ł��X�e�[�W�ڍs�܂��̓N���A
    /// </summary>
    protected void DoCollapuse()
    {
        if (GameData.Round == GameData.GoalRound)
        {
            Instantiate(StaffPrefab, new Vector3(320, -100, 0), Quaternion.Euler(0, 0, 0));
        }
        else
        {
            foreach(GameObject player in _scPlsM.GetAllPlayers()) {
                player.GetComponent<PlayerC>().DoAllPlayerStageMoveAction();
            }
            GameData.IsTimerMoving = false;
            GameData.IsStageMovingAction = true;


        }
        Destroy(gameObject);
    }
}
