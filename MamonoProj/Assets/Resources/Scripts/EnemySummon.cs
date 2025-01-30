using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_kind
{
    ARMY,
    DRAWN,
    TANK,
    FLYCANNON,
    TURRET,
    EEL,
    LAMIA,
    FACE,
    SNOW,
    BEAST,
    LAMPREY,
    FISH,
    CANNON,

    CUBE,

    ARMY_EX,
    Tank_Danger,
    LAMPREY_Mecha,

}

public class EnemySummon : MonoBehaviour
{
    [SerializeField]
    private ECoreC
        ArmyPrefab, DrawnPrefab, TankPrefab, FlyCannonPrefab, TurretPrefab, EelPrefab, LamiaPrefab, FacePrefab, SnowPrefab, BeastPrefab
        , LampreyPrefab, FishPrefab, CubePrefab, CannonP;

    [SerializeField]
    private ECoreC ArmyEXPrefab, TankDangerPrefab, LampreyMechaPrefab;

    [SerializeField]
    private ECoreC BeameyePrefab, DevilPrefab, DoubleSnakePrefab;

    [SerializeField]
    private ECoreC LightPrefab, ThunderDragonPrefab, PulsePrefab;

    [SerializeField]
    private ECoreC UpperPrefab, AngelPrefab, FairyPrefab;

    [SerializeField]
    private BossAlarmC _bossAlarmPrhb;

    public StaffRollC StaffPrefab;

    public TutorialC TutorialP;

    [SerializeField]
    private GameObject _prfbTutoBal;

    private List<E_kind>[] _summonList1 = new List<E_kind>[36]
    {
        new List<E_kind>{},

        new List<E_kind>{E_kind.ARMY},
        new List<E_kind>{E_kind.DRAWN},
        new List<E_kind>{E_kind.LAMIA},
        new List<E_kind>{E_kind.ARMY,E_kind.DRAWN},
        new List<E_kind>{},

        new List<E_kind>{E_kind.ARMY,E_kind.DRAWN,E_kind.TANK,E_kind.FLYCANNON},
        new List<E_kind>{E_kind.TANK,E_kind.FLYCANNON},
        new List<E_kind>{E_kind.TURRET},
        new List<E_kind>{E_kind.TURRET,E_kind.TANK,E_kind.FLYCANNON},
        new List<E_kind>{},


        new List<E_kind>{E_kind.EEL},
        new List<E_kind>{E_kind.LAMIA},
        new List<E_kind>{E_kind.FACE},
        new List<E_kind>{E_kind.EEL},
        new List<E_kind>{},

        new List<E_kind>{E_kind.SNOW},
        new List<E_kind>{E_kind.SNOW,E_kind.FACE},
        new List<E_kind>{E_kind.FACE,E_kind.FLYCANNON},
        new List<E_kind>{E_kind.EEL,E_kind.FACE},
        new List<E_kind>{},

        new List<E_kind>{E_kind.BEAST},
        new List<E_kind>{E_kind.LAMPREY},
        new List<E_kind>{E_kind.FISH},
        new List<E_kind>{E_kind.FISH,E_kind.LAMPREY},
        new List<E_kind>{},

        new List<E_kind>{E_kind.CANNON},
        new List<E_kind>{E_kind.TANK,E_kind.CANNON},
        new List<E_kind>{E_kind.LAMPREY,E_kind.EEL},
        new List<E_kind>{E_kind.ARMY,E_kind.DRAWN,E_kind.TANK,E_kind.FLYCANNON,E_kind.TURRET,E_kind.EEL
            ,E_kind.LAMIA,E_kind.FACE,E_kind.SNOW,E_kind.BEAST,E_kind.LAMPREY,E_kind.FISH,E_kind.CANNON},
        new List<E_kind>{},

        new List<E_kind>{E_kind.BEAST,E_kind.ARMY_EX},
        new List<E_kind>{E_kind.LAMIA,E_kind.LAMPREY_Mecha},
        new List<E_kind>{E_kind.CANNON,E_kind.Tank_Danger},
        new List<E_kind>{E_kind.ARMY_EX,E_kind.LAMPREY_Mecha,E_kind.Tank_Danger},
        new List<E_kind>{},
    };

    private string[] _summonBossNameArray = new string[7]
    {
        "insect","ufo","vane","icequeen","ifrit","zombie","virus"
    };

    private float[,] _delayTimes = new float[36, 2]
    {
        {5.0f ,5.1f},
        {2.0f,3.0f },{2.0f,3.0f },{2.0f,3.0f },{2.5f,3.5f },{5.0f ,5.1f},
        {1.5f,3.5f },{1.5f,3.5f },{1.5f,3.5f },{1.5f,4.0f },{5.0f ,5.1f},
        {1.0f,3.0f },{0.8f,2.6f },{1.0f,3.0f },{1.0f,3.0f },{5.0f ,5.1f},
        {1.0f,2.5f },{1.0f,2.5f },{1.0f,2.5f },{1.0f,2.5f },{5.0f ,5.1f},
        {1.5f,3.5f },{1.5f,3.5f },{1.5f,3.5f },{1.5f,1.7f },{5.0f ,5.1f},
        {1.5f,3.5f },{1.0f,1.2f },{1.0f,1.5f },{0.5f,2.0f },{5.0f ,5.1f},
        {1.0f,2.0f },{1.0f,2.0f },{1.0f,2.0f },{1.0f,2.0f },{5.0f,5.1f }
    };

    private float delay, delaycont = 1.0f;
    public float delayhosei;
    private float timer = 0.0f;
    private int muki;
    private E_kind eshu;

    private bool _isFinishedTutorial;

    private Vector3 pos, ppos;

    [SerializeField]
    [Tooltip("PlayerGameObject")]
    private GameObject playerGO;

    // Start is called before the first frame update
    void Start()
    {
        delaycont = 0.8f + (0.6f - GameData.Difficulty * 0.2f);
        timer = 3.0f;
        delay = 3.0f;
        playerGO = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.EX == 0 && GameData.Round % 5 == 1 && GameData.Difficulty <= 0 && GameData.ClearTime <= 1.0f)
        {
            _isFinishedTutorial = true;
            _prfbTutoBal.SetActive(true);
        }

        if (!GameData.IsBossFight) timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Quaternion rot = transform.localRotation;

            //通常敵
            if (GameData.Round % 5 != 0)
            {
                eshu = _summonList1[GameData.Round][Random.Range(0, _summonList1[GameData.Round].Count)];
                delay = Random.Range(_delayTimes[GameData.Round,0], _delayTimes[GameData.Round, 1]) * delaycont;
            }
            //チュートリアル
            else if (GameData.Round == 0)
            {
                eshu = (E_kind)9999;
                pos = new Vector2(320, 225);
                Instantiate(TutorialP, pos, rot);
                GameData.IsBossFight = true;
            }
            //ボス
            else
            {
                eshu = (E_kind)9999;
                GameData.AllDeleteEnemy();
                Instantiate(_bossAlarmPrhb, new Vector3(640, 480, 0) / 2, rot).BossAlarmSummon(_summonBossNameArray[(GameData.Round / 5) - 1]);
                delay = Random.Range(_delayTimes[GameData.Round, 0], _delayTimes[GameData.Round, 1]) * delaycont;
            }

            switch (GameData.Round)
            {
                case 1:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    break;

                case 31:
                case 32:
                case 33:
                case 34:
                    if (Random.Range(0, 4) == 0) eshu = E_kind.CUBE;
                    break;

                case 107:
                    eshu = (E_kind)9999;
                    pos = new Vector3(360, -100, 0);
                    StaffRollC staff = Instantiate(StaffPrefab, pos, rot);
                    staff.Summon(0);
                    delay = 5.0f;
                    //GameData.Boss = 1;
                    break;
            }



            ppos = playerGO.transform.position;
            muki = 0;

            switch (eshu)
            {
                case E_kind.ARMY:
                    if (Random.Range(0, 2) == 0) pos = new Vector3(Random.Range(8, 632), 480, 0);
                    else pos = new Vector3(muki * 640, GameData.GroundPutY(Random.Range(2, 5), 32), 0);
                    Instantiate(ArmyPrefab, pos, rot);
                    break;

                case E_kind.DRAWN:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    Instantiate(DrawnPrefab, pos, rot);
                    break;

                case E_kind.TANK:
                    if (Random.Range(0, 2) == 0) pos = new Vector3(Random.Range(8, 632), 480, 0);
                    else pos = new Vector3(muki * 640, GameData.GroundPutY(Random.Range(2, 5), 32), 0);
                    Instantiate(TankPrefab, pos, rot);
                    break;

                case E_kind.FLYCANNON:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    Instantiate(FlyCannonPrefab, pos, rot);
                    break;

                case E_kind.TURRET:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    Instantiate(TurretPrefab, pos, rot);
                    break;

                case E_kind.EEL:
                    pos = new Vector3(Random.Range(8, 632), -320, 0);
                    Instantiate(EelPrefab, pos, rot);
                    break;

                case E_kind.LAMIA:
                    if (Random.Range(0, 2) == 0) pos = new Vector3(Random.Range(8, 632), 480, 0);
                    else pos = new Vector3(muki * 640, GameData.GroundPutY(Random.Range(2, 5), 32), 0);
                    Instantiate(LamiaPrefab, pos, rot);
                    break;

                case E_kind.FACE:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    Instantiate(FacePrefab, pos, rot);
                    break;

                case E_kind.SNOW:
                    if (Random.Range(0, 2) == 0) pos = new Vector3(Random.Range(8, 632), 480, 0);
                    else
                    {
                        if (Random.Range(0, 2) == 0) pos = new Vector3(0, Random.Range(8, 462), 0);
                        else pos = new Vector3(640, Random.Range(8, 462), 0);
                    }

                    Instantiate(SnowPrefab, pos, rot);
                    break;

                case E_kind.BEAST:
                    if (Random.Range(0, 2) == 0) pos = new Vector3(Random.Range(8, 632), 496, 0);
                    else pos = new Vector3(muki * 640, GameData.GroundPutY(Random.Range(2, 5), 48), 0);
                    Instantiate(BeastPrefab, pos, rot);
                    break;

                case E_kind.LAMPREY:
                    pos = new Vector3(Random.Range(32, 608), -600, 0);
                    Instantiate(LampreyPrefab, pos, rot);
                    break;

                case E_kind.FISH:
                    if (Random.Range(0, 2) == 0) pos = new Vector3(Random.Range(16, 628), 480, 0);
                    else pos = new Vector3(muki * 640, GameData.GroundPutY(Random.Range(2, 5), 32), 0);
                    Instantiate(FishPrefab, pos, rot);
                    break;

                case E_kind.CANNON:
                    if (Random.Range(0, 2) == 0) pos = new Vector3(Random.Range(8, 632), 496, 0);
                    else pos = new Vector3(muki * 640, GameData.GroundPutY(Random.Range(2, 5), 48), 0);
                    Instantiate(CannonP, pos, rot);
                    break;

                case E_kind.CUBE:
                    pos = GameData.RandomWindowPosition();
                    Instantiate(CubePrefab, pos, rot);
                    break;

                case E_kind.ARMY_EX:
                    if (Random.Range(0, 2) == 0) pos = new Vector3(Random.Range(8, 632), 480, 0);
                    else pos = new Vector3(muki * 640, GameData.GroundPutY(Random.Range(2, 5), 32), 0);
                    Instantiate(ArmyEXPrefab, pos, rot);
                    break;

                case E_kind.Tank_Danger:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    Instantiate(TankDangerPrefab, pos, rot);
                    break;

                case E_kind.LAMPREY_Mecha:
                    pos = new Vector3(Random.Range(32, 608), -600, 0);
                    Instantiate(LampreyMechaPrefab, pos, rot);
                    break;

            }

            timer += delay;
        }
    }

    public int Randomtf()
    {
        return Random.Range(0, 2);
    }
}
