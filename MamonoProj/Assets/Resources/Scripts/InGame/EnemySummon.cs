using EnumDic.Enemy;
using EnumDic.System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵キャラ生成管理
/// </summary>
public class EnemySummon : MonoBehaviour
{
    /// <summary>
    /// 通常敵
    /// </summary>
    [SerializeField]
    private ECoreC
        ArmyPrefab, DrawnPrefab, TankPrefab, FlyCannonPrefab, TurretPrefab, BeamTurretPrefab
        , EelPrefab, LamiaPrefab, FacePrefab, SnowPrefab, BeastPrefab, FireBeastPrefab, LampreyPrefab
        , FishPrefab, CubePrefab, CannonP;

    /// <summary>
    /// EX敵
    /// </summary>
    [SerializeField]
    private ECoreC ArmyEXPrefab, TankDangerPrefab, LampreyMechaPrefab;

    /// <summary>
    /// 追加敵
    /// </summary>
    [SerializeField]
    private ECoreC ArmorFishPrefab;

    [SerializeField]
    private BossAlarmC _bossAlarmPrhb;

    [SerializeField]
    private ClearEffectC StaffPrefab;

    [SerializeField]
    private TutorialC TutorialP;

    [SerializeField]
    private GameObject _prfbTutoBal;

    /// <summary>
    /// 敵生成間隔
    /// </summary>
    private float delay;

    /// <summary>
    /// 難易度ごとの敵生成間隔補正
    /// </summary>
    private float _delayDifficultTuning = 1.0f;

    private float _timer = 0.0f;

    private KIND_ENEMY _kindEnemy;

    /// <summary>
    /// チュートリアル終了
    /// </summary>
    private bool _isFinishedTutorial;

    private Vector3 _posPlayer;

    private List<StageStates> _stagesStages = new List<StageStates> { };

    // Start is called before the first frame update
    void Start()
    {
        _delayDifficultTuning = 0.8f + (0.6f - (float)GameData.Difficulty * 0.2f);
        _timer = 3.0f;
        delay = _timer;

        switch (GameData.GameMode)
        {
            case MODE_GAMEMODE.Normal:
                for (int i = 0; i < StageData.DataNormalStages.Length; i++)
                {
                    _stagesStages.Add(StageData.DataNormalStages[i]);
                }
                break;

            case MODE_GAMEMODE.MultiTower:
                for (int i = 0; i < StageData.DataMultiTowerStages.Length; i++)
                {
                    _stagesStages.Add(StageData.DataMultiTowerStages[i]);
                }
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム内チュートリアル出現
        if (GameData.EX == 0
            && GameData.Round == 1
            && GameData.Difficulty == MODE_DIFFICULTY.Safety
             && GameData.GameMode == MODE_GAMEMODE.Normal
            && GameData.ClearTime <= 1.0f)
        {
            _isFinishedTutorial = true;
            _prfbTutoBal.SetActive(true);
        }

        if (!GameData.IsBossFight)
        {
            _timer -= Time.deltaTime * (GameData.GetCountEnemys() <= 0 ? 2 : 1);
        }

        //敵生成
        if (_timer <= 0)
        {
            //敵上限
            if (GetCountEnemys() < 8)
            {
                Quaternion rot = transform.localRotation;

                StageStates stage = _stagesStages[GameData.Round];

                //チュートリアル
                if (GameData.Round == 0)
                {
                    _kindEnemy = (KIND_ENEMY)9999;
                    Instantiate(TutorialP, new Vector2(320, 225), rot);
                    GameData.IsBossFight = true;
                }

                else
                {
                    //通常敵
                    if (stage.boss == KIND_BOSS._NULL)
                    {
                        _kindEnemy = stage.listEnemys[Random.Range(0, stage.listEnemys.Count)];
                        delay = Random.Range(stage.delayMin, stage.delayMax) * _delayDifficultTuning / GameData.MultiPlayerCount;
                    }

                    //ボス
                    else
                    {
                        _kindEnemy = (KIND_ENEMY)9999;
                        GameData.DeleteAllEnemys();
                        Instantiate(_bossAlarmPrhb, new Vector3(640, 480, 0) / 2, rot).BossAlarmSummon(stage.boss);
                        delay = Random.Range(stage.delayMin, stage.delayMax) * _delayDifficultTuning;
                    }
                }

                switch (GameData.Round)
                {
                    case 31:
                    case 32:
                    case 33:
                    case 34:
                        if (Random.Range(0, 4) == 0) _kindEnemy = KIND_ENEMY.CUBE;
                        break;

                    case 107:
                        _kindEnemy = (KIND_ENEMY)9999;
                        Instantiate(StaffPrefab, new Vector3(360, -100, 0), rot);
                        delay = 5.0f;
                        //GameData.Boss = 1;
                        break;
                }

                switch (_kindEnemy)
                {
                    case KIND_ENEMY.ARMY:
                        SummonEnemyTypeArmy(ArmyPrefab, 32);
                        break;

                    case KIND_ENEMY.DRAWN:
                        SummonEnemyTypeDrawn(DrawnPrefab);
                        break;

                    case KIND_ENEMY.TANK:
                        SummonEnemyTypeArmy(TankPrefab, 32);
                        break;

                    case KIND_ENEMY.FLYCANNON:
                        SummonEnemyTypeDrawn(DrawnPrefab);
                        break;

                    case KIND_ENEMY.TURRET:
                        SummonEnemyTypeTurret(TurretPrefab);
                        break;

                    case KIND_ENEMY.EEL:
                        SummonEnemyTypeEel(EelPrefab, 240);
                        break;

                    case KIND_ENEMY.BEAMTURRET:
                        SummonEnemyTypeTurret(BeamTurretPrefab);
                        break;

                    case KIND_ENEMY.LAMIA:
                        SummonEnemyTypeArmy(LamiaPrefab, 32);
                        break;

                    case KIND_ENEMY.FACE:
                        SummonEnemyTypeTurret(FacePrefab);
                        break;

                    case KIND_ENEMY.SNOW:
                        SummonEnemyTypeSnow(SnowPrefab);
                        break;

                    case KIND_ENEMY.BEAST:
                        SummonEnemyTypeArmy(BeastPrefab, 64);
                        break;

                    case KIND_ENEMY.FIREBEAST:
                        SummonEnemyTypeArmy(FireBeastPrefab, 64);
                        break;

                    case KIND_ENEMY.LAMPREY:
                        SummonEnemyTypeEel(LamiaPrefab, 512);
                        break;

                    case KIND_ENEMY.FISH:
                        SummonEnemyTypeArmy(FishPrefab, 32);
                        break;

                    case KIND_ENEMY.CANNON:
                        SummonEnemyTypeArmy(CannonP, 64);
                        break;

                    case KIND_ENEMY.CUBE:
                        SummonEnemyTypeCube(CubePrefab);
                        break;

                    case KIND_ENEMY.ARMY_EX:
                        SummonEnemyTypeArmy(ArmyEXPrefab, 32);
                        break;

                    case KIND_ENEMY.Tank_Danger:
                        SummonEnemyTypeTurret(TankDangerPrefab);
                        break;

                    case KIND_ENEMY.LAMPREY_Mecha:
                        SummonEnemyTypeEel(LampreyMechaPrefab, 512);
                        break;

                    case KIND_ENEMY.ARMORFISH:
                        SummonEnemyTypeArmy(ArmorFishPrefab, 32);
                        break;

                }
            }

            _timer += delay;

        }
    }

    /// <summary>
    /// 敵召喚（タレットタイプ）
    /// </summary>
    /// <param name="enemy"></param>
    private void SummonEnemyTypeTurret(ECoreC enemy)
    {
        Vector3 posSummon = Vector3.zero;
        for (int i = 0; i < 32; i++)
        {
            posSummon = new Vector3(Random.Range(8, 632), 480, 0);
            if (CheckIsAvoidPlayerPos(posSummon)) break;
        }

        Instantiate(enemy, posSummon, Quaternion.Euler(0, 0, 0));
    }

    /// <summary>
    /// 敵召喚（歩兵タイプ）
    /// </summary>
    /// <param name="enemy"></param>
    private void SummonEnemyTypeArmy(ECoreC enemy, int size)
    {
        Vector3 posSummon = Vector3.zero;
        if (Randomtf() == 0)
        {
            for (int i = 0; i < 32; i++)
            {
                posSummon = new Vector3(Random.Range(8, 632), 480, 0);
                if (CheckIsAvoidPlayerPos(posSummon)) break;
            }
        }
        else
        {
            for (int i = 0; i < 32; i++)
            {
                posSummon = new Vector3(Randomtf() * 640, GameData.GetGroundPutY(Random.Range(2, 5), size), 0);
                if (CheckIsAvoidPlayerPos(posSummon)) break;
            }
        }
        Instantiate(enemy, posSummon, Quaternion.Euler(0, 0, 0));
    }

    /// <summary>
    /// 敵召喚（ドローンタイプ）
    /// </summary>
    /// <param name="enemy"></param>
    private void SummonEnemyTypeDrawn(ECoreC enemy)
    {
        Vector3 posSummon = Vector3.zero;
        for (int i = 0; i < 32; i++)
        {
            posSummon = new Vector3(Random.Range(8, 632), 480, 0);
            if (CheckIsAvoidPlayerPos(posSummon)) break;
        }
        Instantiate(enemy, posSummon, Quaternion.Euler(0, 0, 0));
    }

    /// <summary>
    /// 敵召喚（ノボリリュウタイプ）
    /// </summary>
    /// <param name="enemy"></param>
    private void SummonEnemyTypeEel(ECoreC enemy,int size)
    {
        Vector3 posSummon = new Vector3(Random.Range(8, 632), -size, 0);
        Instantiate(enemy, posSummon, Quaternion.Euler(0, 0, 0));
    }

    /// <summary>
    /// 敵召喚（スノータイプ）
    /// </summary>
    /// <param name="enemy"></param>
    private void SummonEnemyTypeSnow(ECoreC enemy)
    {
        Vector3 posSummon = Vector3.zero;

        if (Random.Range(0, 2) == 0)
        {
            for (int i = 0; i < 32; i++)
            {
                posSummon = new Vector3(Random.Range(8, 632), 480, 0);
                if (CheckIsAvoidPlayerPos(posSummon)) break;
            }
        }
        else
        {
            for (int i = 0; i < 32; i++)
            {
                if (Random.Range(0, 2) == 0)posSummon  = new Vector3(0, Random.Range(8, 462), 0);
                else posSummon = new Vector3(640, Random.Range(8, 462), 0);

                if (CheckIsAvoidPlayerPos(posSummon)) break;
            }
        }
        Instantiate(enemy, posSummon, Quaternion.Euler(0, 0, 0));
    }

    /// <summary>
    /// 敵召喚（異空間タイプ）
    /// </summary>
    /// <param name="enemy"></param>
    private void SummonEnemyTypeCube(ECoreC enemy)
    {
        Vector3 posSummon = Vector3.zero;
        for (int i = 0; i < 32; i++)
        {
            posSummon = GameData.GetRandomWindowPosition();
            if (CheckIsAvoidPlayerPos(posSummon)) break;
        }
        Instantiate(enemy, posSummon, Quaternion.Euler(0, 0, 0));
    }


    /// <summary>
    /// 敵召喚位置がプレイヤーと被ってないか確認
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool CheckIsAvoidPlayerPos(Vector3 pos)
    {
        return! (_posPlayer.x - 128 < pos.x && pos.x < _posPlayer.x + 128
            && _posPlayer.y - 128 < pos.y && pos.y < _posPlayer.y + 256);
    }

    /// <summary>
    /// 0か1のいずれかを返す
    /// </summary>
    /// <returns></returns>
    public int Randomtf()
    {
        return Random.Range(0, 2);
    }

    /// <summary>
    /// 敵キャラの数を返す
    /// </summary>
    /// <returns></returns>
    public int GetCountEnemys() => GameObject.FindGameObjectsWithTag("Enemy").Length;
}
