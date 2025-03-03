using EnumDic.Enemy;
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
        ArmyPrefab, DrawnPrefab, TankPrefab, FlyCannonPrefab, TurretPrefab, EelPrefab, LamiaPrefab, FacePrefab, SnowPrefab, BeastPrefab
        , LampreyPrefab, FishPrefab, CubePrefab, CannonP;

    /// <summary>
    /// EX敵
    /// </summary>
    [SerializeField]
    private ECoreC ArmyEXPrefab, TankDangerPrefab, LampreyMechaPrefab;

    [SerializeField]
    private BossAlarmC _bossAlarmPrhb;

    [SerializeField]
    private ClearEffectC StaffPrefab;

    [SerializeField]
    private TutorialC TutorialP;

    [SerializeField]
    private GameObject _prfbTutoBal;

    private StageStates[] _stageStates = new StageStates[36]
    {
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{},
            delayMin=5.0f,
            delayMax=5.1f
        },

        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.ARMY},
            delayMin=5.0f,
            delayMax=5.1f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.DRAWN},
            delayMin=2.0f,
            delayMax=3.0f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.LAMIA},
            delayMin=2.0f,
            delayMax=3.0f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.DRAWN,KIND_ENEMY.LAMIA},
            delayMin=2.0f,
            delayMax=3.0f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{},
            boss=KIND_BOSS.InsectBoss,
            delayMin=5.0f,
            delayMax=5.1f
        },

        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.ARMY,KIND_ENEMY.DRAWN,KIND_ENEMY.TANK,KIND_ENEMY.FLYCANNON},
            delayMin=1.5f,
            delayMax=3.5f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.TANK,KIND_ENEMY.FLYCANNON},
            delayMin=1.5f,
            delayMax=3.5f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.TURRET},
            delayMin=1.5f,
            delayMax=3.5f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.TURRET,KIND_ENEMY.TANK,KIND_ENEMY.FLYCANNON},
            delayMin=1.5f,
            delayMax=4.5f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{},
            boss=KIND_BOSS.UFO,
            delayMin=5.0f,
            delayMax=5.1f
        },

        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.EEL},
            delayMin=1.0f,
            delayMax=3.0f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.LAMIA},
            delayMin=0.8f,
            delayMax=2.6f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.FACE},
            delayMin=1.0f,
            delayMax=3.0f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.EEL},
            delayMin=1.0f,
            delayMax=3.0f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{},
            boss=KIND_BOSS.Vane,
            delayMin=5.0f,
            delayMax=5.1f
        },

        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.SNOW},
            delayMin=1.0f,
            delayMax=2.5f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.SNOW,KIND_ENEMY.FACE},
            delayMin=1.0f,
            delayMax=2.5f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.FACE,KIND_ENEMY.FLYCANNON},
            delayMin=1.0f,
            delayMax=2.5f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.EEL,KIND_ENEMY.FACE },
            delayMin=1.0f,
            delayMax=2.5f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{},
            boss=KIND_BOSS.IceClione,
            delayMin=5.0f,
            delayMax=5.1f
        },

        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.BEAST},
            delayMin=1.5f,
            delayMax=3.5f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.LAMPREY},
            delayMin=1.5f,
            delayMax=3.5f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.FISH},
            delayMin=1.5f,
            delayMax=3.5f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.FISH,KIND_ENEMY.LAMPREY},
            delayMin=1.5f,
            delayMax=1.7f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{},
            boss=KIND_BOSS.Ifrit,
            delayMin=5.0f,
            delayMax=5.1f
        },

        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.CANNON},
            delayMin=1.5f,
            delayMax=3.5f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.CANNON,KIND_ENEMY.TANK},
            delayMin=1.0f,
            delayMax=1.2f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.LAMPREY,KIND_ENEMY.EEL},
            delayMin=1.0f,
            delayMax=1.5f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.ARMY,KIND_ENEMY.DRAWN,KIND_ENEMY.TANK,KIND_ENEMY.FLYCANNON,KIND_ENEMY.TURRET,KIND_ENEMY.EEL
            ,KIND_ENEMY.LAMIA,KIND_ENEMY.FACE,KIND_ENEMY.SNOW,KIND_ENEMY.BEAST,KIND_ENEMY.LAMPREY,KIND_ENEMY.FISH,KIND_ENEMY.CANNON},
            delayMin=0.5f,
            delayMax=2.0f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{},
            boss=KIND_BOSS.MechaZombie,
            delayMin=5.0f,
            delayMax=5.1f
        },

        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.BEAST,KIND_ENEMY.ARMY_EX},
            delayMin=1.0f,
            delayMax=2.0f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.LAMIA,KIND_ENEMY.LAMPREY_Mecha},
            delayMin=1.0f,
            delayMax=2.0f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.CANNON,KIND_ENEMY.Tank_Danger},
            delayMin=1.0f,
            delayMax=2.0f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.ARMY_EX,KIND_ENEMY.LAMPREY_Mecha,KIND_ENEMY.Tank_Danger},
            delayMin=1.0f,
            delayMax=2.0f
        },
        new StageStates
        {
            listEnemys=new List<KIND_ENEMY>{},
            boss=KIND_BOSS.MailVirus,
            delayMin=5.0f,
            delayMax=5.1f
        }
    };

    /// <summary>
    /// 敵生成間隔
    /// </summary>
    private float delay;

    /// <summary>
    /// 難易度ごとの敵生成間隔補正
    /// </summary>
    private float _delayDifficultTuning = 1.0f;

    private float _timer = 0.0f;

    /// <summary>
    /// 向き補正（右=0,左=1）
    /// </summary>
    private int _angleValue;

    private KIND_ENEMY _kindEnemy;

    /// <summary>
    /// チュートリアル終了
    /// </summary>
    private bool _isFinishedTutorial;

    private Vector3 _posOwn, _posPlayer;

    [SerializeField]
    [Tooltip("PlayerGameObject")]
    private GameObject playerGO;

    // Start is called before the first frame update
    void Start()
    {
        _delayDifficultTuning = 0.8f + (0.6f - (float)GameData.Difficulty * 0.2f);
        _timer = 3.0f;
        delay = _timer;
        playerGO = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム内チュートリアル出現
        if (GameData.EX == 0 && GameData.Round % 5 == 1 && GameData.Difficulty <= 0 && GameData.ClearTime <= 1.0f)
        {
            _isFinishedTutorial = true;
            _prfbTutoBal.SetActive(true);
        }

        if (!GameData.IsBossFight) _timer -= Time.deltaTime;

        //敵生成
        if (_timer <= 0)
        {
            Quaternion rot = transform.localRotation;

            StageStates stage = _stageStates[GameData.Round];

            //チュートリアル
            if (GameData.Round == 0)
            {
                _kindEnemy = (KIND_ENEMY)9999;
                _posOwn = new Vector2(320, 225);
                Instantiate(TutorialP, _posOwn, rot);
                GameData.IsBossFight = true;
            }
            
            else
            {
                //通常敵
                if (stage.boss == KIND_BOSS._NULL)
                {
                    _kindEnemy = stage.listEnemys[Random.Range(0, stage.listEnemys.Count)];
                    delay = Random.Range(stage.delayMin, stage.delayMax) * _delayDifficultTuning;
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
                case 1:
                    _posOwn = new Vector3(Random.Range(8, 632), 480, 0);
                    break;

                case 31:
                case 32:
                case 33:
                case 34:
                    if (Random.Range(0, 4) == 0) _kindEnemy = KIND_ENEMY.CUBE;
                    break;

                case 107:
                    _kindEnemy = (KIND_ENEMY)9999;
                    _posOwn = new Vector3(360, -100, 0);
                    Instantiate(StaffPrefab, _posOwn, rot);
                    delay = 5.0f;
                    //GameData.Boss = 1;
                    break;
            }



            _posPlayer = playerGO.transform.position;
            _angleValue = 0;

            switch (_kindEnemy)
            {
                case KIND_ENEMY.ARMY:
                    if (Random.Range(0, 2) == 0) _posOwn = new Vector3(Random.Range(8, 632), 480, 0);
                    else _posOwn = new Vector3(_angleValue * 640, GameData.GetGroundPutY(Random.Range(2, 5), 32), 0);
                    Instantiate(ArmyPrefab, _posOwn, rot);
                    break;

                case KIND_ENEMY.DRAWN:
                    _posOwn = new Vector3(Random.Range(8, 632), 480, 0);
                    Instantiate(DrawnPrefab, _posOwn, rot);
                    break;

                case KIND_ENEMY.TANK:
                    if (Random.Range(0, 2) == 0) _posOwn = new Vector3(Random.Range(8, 632), 480, 0);
                    else _posOwn = new Vector3(_angleValue * 640, GameData.GetGroundPutY(Random.Range(2, 5), 32), 0);
                    Instantiate(TankPrefab, _posOwn, rot);
                    break;

                case KIND_ENEMY.FLYCANNON:
                    _posOwn = new Vector3(Random.Range(8, 632), 480, 0);
                    Instantiate(FlyCannonPrefab, _posOwn, rot);
                    break;

                case KIND_ENEMY.TURRET:
                    _posOwn = new Vector3(Random.Range(8, 632), 480, 0);
                    Instantiate(TurretPrefab, _posOwn, rot);
                    break;

                case KIND_ENEMY.EEL:
                    _posOwn = new Vector3(Random.Range(8, 632), -320, 0);
                    Instantiate(EelPrefab, _posOwn, rot);
                    break;

                case KIND_ENEMY.LAMIA:
                    if (Random.Range(0, 2) == 0) _posOwn = new Vector3(Random.Range(8, 632), 480, 0);
                    else _posOwn = new Vector3(_angleValue * 640, GameData.GetGroundPutY(Random.Range(2, 5), 32), 0);
                    Instantiate(LamiaPrefab, _posOwn, rot);
                    break;

                case KIND_ENEMY.FACE:
                    _posOwn = new Vector3(Random.Range(8, 632), 480, 0);
                    Instantiate(FacePrefab, _posOwn, rot);
                    break;

                case KIND_ENEMY.SNOW:
                    if (Random.Range(0, 2) == 0) _posOwn = new Vector3(Random.Range(8, 632), 480, 0);
                    else
                    {
                        if (Random.Range(0, 2) == 0) _posOwn = new Vector3(0, Random.Range(8, 462), 0);
                        else _posOwn = new Vector3(640, Random.Range(8, 462), 0);
                    }

                    Instantiate(SnowPrefab, _posOwn, rot);
                    break;

                case KIND_ENEMY.BEAST:
                    if (Random.Range(0, 2) == 0) _posOwn = new Vector3(Random.Range(8, 632), 496, 0);
                    else _posOwn = new Vector3(_angleValue * 640, GameData.GetGroundPutY(Random.Range(2, 5), 48), 0);
                    Instantiate(BeastPrefab, _posOwn, rot);
                    break;

                case KIND_ENEMY.LAMPREY:
                    _posOwn = new Vector3(Random.Range(32, 608), -600, 0);
                    Instantiate(LampreyPrefab, _posOwn, rot);
                    break;

                case KIND_ENEMY.FISH:
                    if (Random.Range(0, 2) == 0) _posOwn = new Vector3(Random.Range(16, 628), 480, 0);
                    else _posOwn = new Vector3(_angleValue * 640, GameData.GetGroundPutY(Random.Range(2, 5), 32), 0);
                    Instantiate(FishPrefab, _posOwn, rot);
                    break;

                case KIND_ENEMY.CANNON:
                    if (Random.Range(0, 2) == 0) _posOwn = new Vector3(Random.Range(8, 632), 496, 0);
                    else _posOwn = new Vector3(_angleValue * 640, GameData.GetGroundPutY(Random.Range(2, 5), 48), 0);
                    Instantiate(CannonP, _posOwn, rot);
                    break;

                case KIND_ENEMY.CUBE:
                    _posOwn = GameData.GetRandomWindowPosition();
                    Instantiate(CubePrefab, _posOwn, rot);
                    break;

                case KIND_ENEMY.ARMY_EX:
                    if (Random.Range(0, 2) == 0) _posOwn = new Vector3(Random.Range(8, 632), 480, 0);
                    else _posOwn = new Vector3(_angleValue * 640, GameData.GetGroundPutY(Random.Range(2, 5), 32), 0);
                    Instantiate(ArmyEXPrefab, _posOwn, rot);
                    break;

                case KIND_ENEMY.Tank_Danger:
                    _posOwn = new Vector3(Random.Range(8, 632), 480, 0);
                    Instantiate(TankDangerPrefab, _posOwn, rot);
                    break;

                case KIND_ENEMY.LAMPREY_Mecha:
                    _posOwn = new Vector3(Random.Range(32, 608), -600, 0);
                    Instantiate(LampreyMechaPrefab, _posOwn, rot);
                    break;

            }

            _timer += delay;
        }
    }

    /// <summary>
    /// 0か1のいずれかを返す
    /// </summary>
    /// <returns></returns>
    public int Randomtf()
    {
        return Random.Range(0, 2);
    }
}
