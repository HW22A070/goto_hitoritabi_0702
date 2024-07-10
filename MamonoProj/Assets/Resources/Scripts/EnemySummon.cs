using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENEMY_SHU
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

    FIRE,
    FLAMER,
    GOLEM,

    BEAMEYE,
    DEVIL,
    DOUBLESNAKE,

    LIGHT,
    THUNDERDRAGON,
    PULSE,

    UPPER,
    ANGEL,
    FAIRY,

    CUBE,

}

public class EnemySummon : MonoBehaviour
{
    public ArmyC ArmyPrefab;
    public DrawnC DrawnPrefab;
    public TankC TankPrefab;
    public FlyCannonC FlyCannonPrefab;
    public TurretC TurretPrefab;
    public EelC EelPrefab;
    public LamiaC LamiaPrefab;
    public FaceC FacePrefab;
    public SnowC SnowPrefab;
    public BeastC BeastPrefab;
    public LampreyC LampreyPrefab;
    public FishC FishPrefab;
    public CubeC CubePrefab;
    [SerializeField]
    private CannonC CannonP;

    public FireC FirePrefab;
    public FlamerC FlamerPrefab;
    public GolemC GolemPrefab;

    public BeameyeC BeameyePrefab;
    public DevilC DevilPrefab;
    public DoubleSnakeC DoubleSnakePrefab;

    public LightC LightPrefab;
    public ThunderDragonC ThunderDragonPrefab;
    public PulseC PulsePrefab;

    public UpperC UpperPrefab;
    public AngelC AngelPrefab;
    public FairyC FairyPrefab;

    public InsectBossC InsectBossPrefab;
    public UfoC UfoPrefab;
    public IcequeenC IcequeenPrefab;
    public IfritC IfritPrefab;
    public MechaZombieC MechaZombiePrefab;
    public MailC MailPrefab;

    public StaffRollC StaffPrefab;

    public TutorialC TutorialP;

    //GameMode
    public VaneC VanePrefab;

    float delay, delaycont = 1.0f;
    public float delayhosei;
    float timer = 0.0f;
    int muki;
    ENEMY_SHU eshu;

    Vector3 pos, ppos;

    [SerializeField]
    [Tooltip("PlayerGameObject")]
    private GameObject playerGO;

    // Start is called before the first frame update
    void Start()
    {
        delayhosei = delaycont * ((3.5f-GameData.Difficulty)* 0.3f);
        timer = 3.0f;
        delay = 3.0f;
        playerGO = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.Boss == 0) timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Quaternion rot = transform.localRotation;
            switch (GameData.Round)
            {
                case 0:
                    eshu = (ENEMY_SHU)9999;
                    pos = new Vector2(320,225);
                    Instantiate(TutorialP, pos, rot).Summon();
                    GameData.Boss = 1;
                    break;

                case 1:
                    eshu = (ENEMY_SHU)0;
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    delay = Random.Range(2.0f, 4.0f)*delaycont;
                    break;

                case 2:
                    eshu = (ENEMY_SHU)1;
                    delay = Random.Range(2.0f, 4.0f) * delaycont;
                    break;

                case 3:
                    eshu = (ENEMY_SHU)Random.Range(0, 2);
                    delay = Random.Range(2.0f, 4.0f) * delaycont;
                    break;

                case 4:
                    eshu = (ENEMY_SHU)2;
                    delay = Random.Range(2.5f, 5.0f) * delaycont;
                    break;

                case 5:
                    eshu = (ENEMY_SHU)9999;
                    pos = new Vector3(64, Random.Range(2, 6) * 90, 0);
                    InsectBossC insectboss = Instantiate(InsectBossPrefab, pos, rot);
                    insectboss.Summon(0);
                    GameData.Boss = 1;
                    break;

                case 6:
                    eshu = (ENEMY_SHU)Random.Range(0, 4);
                    delay = Random.Range(1.5f, 3.5f) * delaycont;
                    break;

                case 7:
                    eshu = (ENEMY_SHU)Random.Range(2, 4);
                    delay = Random.Range(1.5f, 3.5f) * delaycont;
                    break;

                case 8:
                    eshu = (ENEMY_SHU)4;
                    delay = Random.Range(1.5f, 3.5f) * delaycont;
                    break;

                case 9:
                    eshu = (ENEMY_SHU)Random.Range(2, 5);
                    delay = Random.Range(1.5f, 4.0f) * delaycont;
                    break;

                case 10:
                    eshu = (ENEMY_SHU)9999;
                    pos = new Vector3(Random.Range(100, 540), 470, 0);
                    UfoC ufo = Instantiate(UfoPrefab, pos, rot);
                    ufo.Summon(0);
                    GameData.Boss = 1;
                    break;

                case 11:
                    //Ell
                    eshu = (ENEMY_SHU)5;
                    delay = Random.Range(1.0f, 3.0f) * delaycont;
                    break;

                case 12:
                    //Lamia
                    eshu = (ENEMY_SHU)6;
                    delay = Random.Range(1.0f, 3.0f) * delaycont;
                    break;

                case 13:
                    //Face
                    eshu = (ENEMY_SHU)7;
                    delay = Random.Range(1.0f, 3.0f) * delaycont;
                    break;

                case 14:
                    GameData.WindSpeed = 20;
                    //Ell
                    eshu = (ENEMY_SHU)Random.Range(5, 6);
                    delay = Random.Range(1.0f, 3.0f) * delaycont;
                    break;

                case 15:
                    eshu = (ENEMY_SHU)9999;
                    pos = new Vector3(100, (Random.Range(0, 3) * 90) + 110, 0);
                    VaneC vane = Instantiate(VanePrefab, pos, rot);
                    vane.Summon(0);
                    GameData.Boss = 1;
                    break;

                case 16:
                    eshu = (ENEMY_SHU)8;
                    delay = Random.Range(1.0f, 2.5f) * delaycont;
                    break;

                case 17:
                    eshu = (ENEMY_SHU)Random.Range(7, 9);
                    delay = Random.Range(1.0f, 2.5f) * delaycont;
                    break;

                case 18:
                    if (Random.Range(0, 2) == 0) eshu = (ENEMY_SHU)3;
                    else eshu = (ENEMY_SHU)7;
                    delay = Random.Range(1.0f, 2.5f) * delaycont;
                    break;

                case 19:
                    if (Random.Range(0, 2) == 0) eshu = (ENEMY_SHU)5;
                    else eshu = (ENEMY_SHU)7;
                    delay = Random.Range(1.0f, 2.5f) * delaycont;
                    break;

                case 20:
                    
                    eshu = (ENEMY_SHU)9999;
                    pos = new Vector3(Random.Range(100, 540), Random.Range(50, 430), 0);
                    IcequeenC icequeen = Instantiate(IcequeenPrefab, pos, rot);
                    icequeen.Summon(0);
                    GameData.Boss = 1;
                    break;

                case 21:
                    eshu = (ENEMY_SHU)9;
                    delay = Random.Range(1.5f, 3.5f) * delaycont;
                    break;

                case 22:
                    eshu = (ENEMY_SHU)10;
                    delay = Random.Range(1.5f, 3.5f) * delaycont;
                    break;

                case 23:
                    eshu = (ENEMY_SHU)11;
                    delay = Random.Range(1.5f, 3.5f) * delaycont;
                    break;

                case 24:
                    eshu = (ENEMY_SHU)Random.Range(10,12);
                    delay = 1.5f * delaycont;
                    break;

                case 25:

                    eshu = (ENEMY_SHU)9999;
                    pos = new Vector3(Random.Range(100, 540), Random.Range(50, 430), 0);
                    IfritC ifrit = Instantiate(IfritPrefab, pos, rot);
                    ifrit.Summon(0);
                    GameData.Boss = 1;
                    break;

                case 26:
                    eshu = (ENEMY_SHU)12;
                    delay = Random.Range(1.5f, 3.5f) * delaycont;
                    break;

                case 27:
                    if (Random.Range(0, 4) == 0) eshu = (ENEMY_SHU)12;
                    else eshu = (ENEMY_SHU)2;
                    delay = 1f * delaycont;
                    break;

                case 28:
                    GameData.IceFloor = 1;
                    if (Random.Range(0, 3) == 0) eshu = (ENEMY_SHU)5;
                    else eshu = (ENEMY_SHU)10;
                    delay = Random.Range(1.0f, 1.5f) * delaycont;
                    break;

                case 29:
                    eshu = (ENEMY_SHU)Random.Range(1, 13);
                    delay = Random.Range(0.5f, 2.0f) * delaycont;
                    break;

                case 30:
                    eshu = (ENEMY_SHU)9999;
                    pos = new Vector3(700, 165, 0);
                    MechaZombieC meczom = Instantiate(MechaZombiePrefab, pos, rot);
                    meczom.Summon(0);
                    GameData.Boss = 1;
                    break;

                case 31:
                    if (Random.Range(0, 4) == 0) eshu = (ENEMY_SHU)25;
                    else eshu = (ENEMY_SHU)Random.Range(13, 16);
                    delay = Random.Range(1.0f, 2.0f) * delaycont;
                    break;

                case 32:
                    if (Random.Range(0, 4) == 0) eshu = (ENEMY_SHU)25;
                    else eshu = (ENEMY_SHU)Random.Range(16, 19);
                    delay = Random.Range(1.0f, 2.0f) * delaycont;
                    break;

                case 33:
                    if (Random.Range(0, 4) == 0) eshu = (ENEMY_SHU)25;
                    else eshu = (ENEMY_SHU)Random.Range(19, 22);
                    delay = Random.Range(1.0f, 2.0f) * delaycont;
                    break;

                case 34:
                    if (Random.Range(0, 4) == 0) eshu = (ENEMY_SHU)25;
                    else eshu = (ENEMY_SHU)Random.Range(22, 25);
                    delay = Random.Range(1.0f, 2.0f) * delaycont;
                    break;

                case 35:
                    eshu = (ENEMY_SHU)9999;
                    pos = new Vector3(320, 550, 0);
                    MailC mail = Instantiate(MailPrefab, pos, rot);
                    mail.Summon(0);
                    GameData.Boss = 1;
                    break;

                    
                case 107:
                    eshu = (ENEMY_SHU)9999;
                    pos = new Vector3(360, -100, 0);
                    StaffRollC staff = Instantiate(StaffPrefab, pos, rot);
                    staff.Summon(0);
                    GameData.Boss = 1;
                    break;




                    /*
                    eshu = (ENEMY_SHU)9999;
                    pos = new Vector3(360, -100, 0);
                    staff = Instantiate(StaffPrefab, pos, rot);
                    staff.Summon(0);
                    GameData.Boss = 1;
                    break;*/

            }



            ppos = playerGO.transform.position;
            muki = 0;

            switch (eshu)
            {
                case ENEMY_SHU.ARMY:
                    if (Random.Range(0, 2) == 0)
                    {
                        pos = new Vector3(Random.Range(8, 632), 480, 0);
                        Instantiate(ArmyPrefab, pos, rot).Summon(Random.Range(0, 2),0);
                    }
                    else 
                    {
                        pos = new Vector3(muki*640, GameData.GroundPutY(Random.Range(0,5),32), 0);
                        Instantiate(ArmyPrefab, pos, rot).Summon(muki, 1);
                    }
                    break;

                case ENEMY_SHU.DRAWN:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    Instantiate(DrawnPrefab, pos, rot).Summon(0);
                    break;

                case ENEMY_SHU.TANK:
                    if (Random.Range(0, 2) == 0)
                    {
                        pos = new Vector3(Random.Range(8, 632), 480, 0);
                        TankC tank = Instantiate(TankPrefab, pos, rot);
                        tank.Summon(Random.Range(0, 2), 0);
                    }
                    else
                    {

                        pos = new Vector3(muki * 640, GameData.GroundPutY(Random.Range(0,5),32), 0);
                        TankC tank = Instantiate(TankPrefab, pos, rot);
                        tank.Summon(muki, 1);
                    }
                    break;

                case ENEMY_SHU.FLYCANNON:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    FlyCannonC flycannon = Instantiate(FlyCannonPrefab, pos, rot);
                    flycannon.Summon(0);
                    break;

                case ENEMY_SHU.TURRET:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    TurretC turret = Instantiate(TurretPrefab, pos, rot);
                    turret.Summon(0);
                    break;

                case ENEMY_SHU.EEL:
                    pos = new Vector3(Random.Range(8, 632), -320, 0);
                    EelC eel = Instantiate(EelPrefab, pos, rot);
                    eel.Summon(0);
                    break;

                case ENEMY_SHU.LAMIA:
                    if (Random.Range(0, 2) == 0)
                    {
                        pos = new Vector3(Random.Range(8, 632), 480, 0);
                        LamiaC lamia = Instantiate(LamiaPrefab, pos, rot);
                        lamia.Summon(0,0);
                    }
                    else
                    {
                        pos = new Vector3(muki * 640, GameData.GroundPutY(Random.Range(0,5),32), 0);
                        LamiaC lamia = Instantiate(LamiaPrefab, pos, rot);
                        lamia.Summon(0,1);
                    }
                    break;
                    

                case ENEMY_SHU.FACE:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    FaceC face = Instantiate(FacePrefab, pos, rot);
                    face.Summon(0);
                    break;

                case ENEMY_SHU.SNOW:
                    if (Random.Range(0, 2)==0)pos = new Vector3(Random.Range(8, 632), 480, 0);
                    else 
                    {
                        if (Random.Range(0, 2) == 0)pos = new Vector3(0, Random.Range(8, 462), 0);
                        else pos = new Vector3(640, Random.Range(8, 462), 0);
                    }

                    SnowC snow = Instantiate(SnowPrefab, pos, rot);
                    snow.Summon(0);
                    break;

                case ENEMY_SHU.BEAST:
                    if (Random.Range(0, 2) == 0)
                    {
                        pos = new Vector3(Random.Range(8, 632), 496, 0);
                        BeastC beast = Instantiate(BeastPrefab, pos, rot);
                        beast.Summon(Random.Range(0, 2),0);
                    }
                    else
                    {
                        pos = new Vector3(muki * 640, GameData.GroundPutY(Random.Range(0,5),48), 0);
                        BeastC beast = Instantiate(BeastPrefab, pos, rot);
                        beast.Summon(muki,1);
                    }
                    break;

                case ENEMY_SHU.LAMPREY:
                    pos = new Vector3(Random.Range(32, 608), -600, 0);
                    LampreyC lamprey = Instantiate(LampreyPrefab, pos, rot);
                    lamprey.Summon(0);
                    break;

                case ENEMY_SHU.FISH:
                    if (Random.Range(0, 2) == 0)
                    {
                        pos = new Vector3(Random.Range(16, 628), 480, 0);
                        FishC fish = Instantiate(FishPrefab, pos, rot);
                        fish.Summon(Random.Range(0, 2), 0);
                    }
                    else
                    {
                        pos = new Vector3(muki * 640, GameData.GroundPutY(Random.Range(0,5),32), 0);
                        FishC fish = Instantiate(FishPrefab, pos, rot);
                        fish.Summon(muki, 1);
                    }
                    break;

                case ENEMY_SHU.CANNON:
                    if (Random.Range(0, 2) == 0)
                    {
                        pos = new Vector3(Random.Range(8, 632), 496, 0);
                        Instantiate(CannonP, pos, rot).Summon(Random.Range(0, 2), 0);
                    }
                    else
                    {
                        pos = new Vector3(muki * 640, GameData.GroundPutY(Random.Range(0,5),48), 0);
                        Instantiate(CannonP, pos, rot).Summon(muki, 1);
                    }
                    break;

                case ENEMY_SHU.FIRE:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    FireC fire = Instantiate(FirePrefab, pos, rot);
                    fire.Summon(0);
                    break;

                case ENEMY_SHU.FLAMER:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    FlamerC flamer = Instantiate(FlamerPrefab, pos, rot);
                    flamer.Summon(0);
                    break;

                case ENEMY_SHU.GOLEM:
                    if (Random.Range(0, 2) == 0)
                    {
                        pos = new Vector3(Random.Range(8, 632), 480, 0);
                        GolemC golem = Instantiate(GolemPrefab, pos, rot);
                        golem.Summon(0,0);
                    }
                    else
                    {
                        pos = new Vector3(Random.Range(0, 1) * 640, GameData.GroundPutY(Random.Range(0,5),32), 0);
                        GolemC golem = Instantiate(GolemPrefab, pos, rot);
                        golem.Summon(0,1);
                    }

                    break;

                case ENEMY_SHU.BEAMEYE:
                    pos = new Vector3(Random.Range(32, 608), 496, 0);
                    BeameyeC beameye = Instantiate(BeameyePrefab, pos, rot);
                    beameye.Summon(Random.Range(0, 2));
                    break;

                case ENEMY_SHU.DEVIL:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    DevilC devil = Instantiate(DevilPrefab, pos, rot);
                    devil.Summon(0);
                    break;

                case ENEMY_SHU.DOUBLESNAKE:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    DoubleSnakeC doublesnake = Instantiate(DoubleSnakePrefab, pos, rot);
                    doublesnake.Summon(0);
                    break;

                case ENEMY_SHU.LIGHT:
                    if (Random.Range(0, 2) == 0) pos = new Vector3(Random.Range(8, 632), 480, 0);
                    else
                    {
                        if (Random.Range(0, 2) == 0) pos = new Vector3(0, Random.Range(8, 462), 0);
                        else pos = new Vector3(640, Random.Range(8, 462), 0);
                    }
                    Instantiate(LightPrefab, pos, rot).Summon(0);
                    break;

                case ENEMY_SHU.THUNDERDRAGON:
                    pos = new Vector3(Random.Range(32, 608), 496, 0);
                    Instantiate(ThunderDragonPrefab, pos, rot).Summon(Random.Range(0, 2));
                    break;

                case ENEMY_SHU.PULSE:
                    pos = new Vector3(ppos.x, -240, 0);
                    Instantiate(PulsePrefab, pos, rot).Summon(0);
                    break;

                case ENEMY_SHU.UPPER:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    Instantiate(UpperPrefab, pos, rot).Summon(0);
                    break;

                case ENEMY_SHU.ANGEL:
                    if (Random.Range(0, 2) == 0) pos = new Vector3(Random.Range(8, 632), 480, 0);
                    else
                    {
                        if (Random.Range(0, 2) == 0) pos = new Vector3(0, Random.Range(8, 462), 0);
                        else pos = new Vector3(640, Random.Range(8, 462), 0);
                    }
                    Instantiate(AngelPrefab, pos, rot).Summon(0);
                    break;

                case ENEMY_SHU.FAIRY:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    Instantiate(FairyPrefab, pos, rot).Summon(0);
                    break;
                    
                case ENEMY_SHU.CUBE:
                    pos = GameData.RandomWindowPosition();
                    Instantiate(CubePrefab, pos, rot).Summon(0);
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
