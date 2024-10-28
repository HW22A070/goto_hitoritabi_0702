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

    private float delay, delaycont = 1.0f;
    public float delayhosei;
    private float timer = 0.0f;
    private int muki;
    private ENEMY_SHU eshu;

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

        if (GameData.Boss == 0) timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Quaternion rot = transform.localRotation;

            switch (GameData.Round)
            {
                case 0:
                    eshu = (ENEMY_SHU)9999;
                    pos = new Vector2(320, 225);
                    Instantiate(TutorialP, pos, rot);
                    GameData.Boss = 1;
                    break;

                case 1:
                    eshu = (ENEMY_SHU)0;
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    delay = Random.Range(2.0f, 3.0f) * delaycont;
                    break;

                case 2:
                    eshu = (ENEMY_SHU)1;
                    delay = Random.Range(2.0f, 3.0f) * delaycont;
                    break;

                case 3:
                    eshu = (ENEMY_SHU)6;
                    delay = Random.Range(2.0f, 3.0f) * delaycont;
                    break;

                case 4:
                    eshu = (ENEMY_SHU)Random.Range(0, 2);
                    delay = Random.Range(2.5f, 3.5f) * delaycont;
                    break;

                case 5:
                    eshu = (ENEMY_SHU)9999;
                    GameData.AllDeleteEnemy();
                    Instantiate(_bossAlarmPrhb, new Vector3(640, 480, 0) / 2, rot).BossAlarmSummon("insect");
                    delay = 5.0f;
                    //GameData.Boss = 1;
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
                    GameData.AllDeleteEnemy();
                    Instantiate(_bossAlarmPrhb, new Vector3(640, 480, 0) / 2, rot).BossAlarmSummon("ufo");
                    delay = 5.0f;
                    //GameData.Boss = 1;
                    break;

                case 11:
                    //Ell
                    eshu = (ENEMY_SHU)5;
                    delay = Random.Range(1.0f, 3.0f) * delaycont;
                    break;

                case 12:
                    //Lamia
                    eshu = (ENEMY_SHU)6;
                    delay = Random.Range(0.8f, 2.6f) * delaycont;
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
                    GameData.AllDeleteEnemy();
                    Instantiate(_bossAlarmPrhb, new Vector3(640, 480, 0) / 2, rot).BossAlarmSummon("vane");
                    delay = 5.0f;
                    //GameData.Boss = 1;
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
                    GameData.AllDeleteEnemy();
                    Instantiate(_bossAlarmPrhb, new Vector3(640, 480, 0) / 2, rot).BossAlarmSummon("icequeen");
                    delay = 5.0f;
                    //GameData.Boss = 1;
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
                    eshu = (ENEMY_SHU)Random.Range(10, 12);
                    delay = 1.5f * delaycont;
                    break;

                case 25:
                    eshu = (ENEMY_SHU)9999;
                    GameData.AllDeleteEnemy();
                    Instantiate(_bossAlarmPrhb, new Vector3(640, 480, 0) / 2, rot).BossAlarmSummon("ifrit");
                    delay = 5.0f;
                    //GameData.Boss = 1;
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
                    GameData.AllDeleteEnemy();
                    Instantiate(_bossAlarmPrhb, new Vector3(640, 480, 0) / 2, rot).BossAlarmSummon("zombie");
                    delay = 5.0f;
                    //GameData.Boss = 1;
                    break;

                case 31:
                    if (Random.Range(0, 4) == 0) eshu = (ENEMY_SHU)13;
                    else
                    {
                        switch (Random.Range(0, 2))
                        {
                            case 0:
                                eshu = (ENEMY_SHU)9;
                                break;
                            case 1:
                                eshu = (ENEMY_SHU)14;
                                break;
                        }
                    }
                    delay = Random.Range(1.0f, 2.0f) * delaycont;
                    break;

                case 32:
                    if (Random.Range(0, 4) == 0) eshu = (ENEMY_SHU)13;
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            eshu = (ENEMY_SHU)6;
                            break;
                        case 1:
                            eshu = (ENEMY_SHU)16;
                            break;
                    }
                    delay = Random.Range(1.0f, 2.0f) * delaycont;
                    break;

                case 33:
                    if (Random.Range(0, 4) == 0) eshu = (ENEMY_SHU)13;
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            eshu = (ENEMY_SHU)12;
                            break;
                        case 1:
                            eshu = (ENEMY_SHU)15;
                            break;
                    }
                    delay = Random.Range(1.0f, 2.0f) * delaycont;
                    break;

                case 34:
                    if (Random.Range(0, 4) == 0) eshu = (ENEMY_SHU)13;
                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            eshu = (ENEMY_SHU)14;
                            break;
                        case 1:
                            eshu = (ENEMY_SHU)15;
                            break;
                        case 2:
                            eshu = (ENEMY_SHU)16;
                            break;
                    }
                    delay = Random.Range(1.0f, 2.0f) * delaycont;
                    break;

                case 35:
                    eshu = (ENEMY_SHU)9999;
                    Instantiate(_bossAlarmPrhb, new Vector3(640, 480, 0) / 2, rot).BossAlarmSummon("virus");
                    delay = 5.0f;
                    //GameData.Boss = 1;
                    break;


                case 107:
                    eshu = (ENEMY_SHU)9999;
                    pos = new Vector3(360, -100, 0);
                    StaffRollC staff = Instantiate(StaffPrefab, pos, rot);
                    staff.Summon(0);
                    delay = 5.0f;
                    //GameData.Boss = 1;
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
                    if (Random.Range(0, 2) == 0) pos = new Vector3(Random.Range(8, 632), 480, 0);
                    else pos = new Vector3(muki * 640, GameData.GroundPutY(Random.Range(2, 5), 32), 0);
                    Instantiate(ArmyPrefab, pos, rot);
                    break;

                case ENEMY_SHU.DRAWN:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    Instantiate(DrawnPrefab, pos, rot);
                    break;

                case ENEMY_SHU.TANK:
                    if (Random.Range(0, 2) == 0) pos = new Vector3(Random.Range(8, 632), 480, 0);
                    else pos = new Vector3(muki * 640, GameData.GroundPutY(Random.Range(2, 5), 32), 0);
                    Instantiate(TankPrefab, pos, rot);
                    break;

                case ENEMY_SHU.FLYCANNON:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    Instantiate(FlyCannonPrefab, pos, rot);
                    break;

                case ENEMY_SHU.TURRET:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    Instantiate(TurretPrefab, pos, rot);
                    break;

                case ENEMY_SHU.EEL:
                    pos = new Vector3(Random.Range(8, 632), -320, 0);
                    Instantiate(EelPrefab, pos, rot);
                    break;

                case ENEMY_SHU.LAMIA:
                    if (Random.Range(0, 2) == 0) pos = new Vector3(Random.Range(8, 632), 480, 0);
                    else pos = new Vector3(muki * 640, GameData.GroundPutY(Random.Range(2, 5), 32), 0);
                    Instantiate(LamiaPrefab, pos, rot);
                    break;

                case ENEMY_SHU.FACE:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    Instantiate(FacePrefab, pos, rot);
                    break;

                case ENEMY_SHU.SNOW:
                    if (Random.Range(0, 2) == 0) pos = new Vector3(Random.Range(8, 632), 480, 0);
                    else
                    {
                        if (Random.Range(0, 2) == 0) pos = new Vector3(0, Random.Range(8, 462), 0);
                        else pos = new Vector3(640, Random.Range(8, 462), 0);
                    }

                    Instantiate(SnowPrefab, pos, rot);
                    break;

                case ENEMY_SHU.BEAST:
                    if (Random.Range(0, 2) == 0) pos = new Vector3(Random.Range(8, 632), 496, 0);
                    else pos = new Vector3(muki * 640, GameData.GroundPutY(Random.Range(2, 5), 48), 0);
                    Instantiate(BeastPrefab, pos, rot);
                    break;

                case ENEMY_SHU.LAMPREY:
                    pos = new Vector3(Random.Range(32, 608), -600, 0);
                    Instantiate(LampreyPrefab, pos, rot);
                    break;

                case ENEMY_SHU.FISH:
                    if (Random.Range(0, 2) == 0) pos = new Vector3(Random.Range(16, 628), 480, 0);
                    else pos = new Vector3(muki * 640, GameData.GroundPutY(Random.Range(2, 5), 32), 0);
                    Instantiate(FishPrefab, pos, rot);
                    break;

                case ENEMY_SHU.CANNON:
                    if (Random.Range(0, 2) == 0) pos = new Vector3(Random.Range(8, 632), 496, 0);
                    else pos = new Vector3(muki * 640, GameData.GroundPutY(Random.Range(2, 5), 48), 0);
                    Instantiate(CannonP, pos, rot);
                    break;

                case ENEMY_SHU.CUBE:
                    pos = GameData.RandomWindowPosition();
                    Instantiate(CubePrefab, pos, rot);
                    break;

                case ENEMY_SHU.ARMY_EX:
                    if (Random.Range(0, 2) == 0) pos = new Vector3(Random.Range(8, 632), 480, 0);
                    else pos = new Vector3(muki * 640, GameData.GroundPutY(Random.Range(2, 5), 32), 0);
                    Instantiate(ArmyEXPrefab, pos, rot);
                    break;

                case ENEMY_SHU.Tank_Danger:
                    pos = new Vector3(Random.Range(8, 632), 480, 0);
                    Instantiate(TankDangerPrefab, pos, rot);
                    break;

                case ENEMY_SHU.LAMPREY_Mecha:
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
