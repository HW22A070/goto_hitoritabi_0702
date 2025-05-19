using EnumDic.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData : MonoBehaviour
{
    public static string[,] NamesNormalStage = new string[3, 8]
    {
        {
            "チュートリアル","うっそう森林","未知のいせき","ぼうふう岩山","こおった平原"
            ,"ようがん連山","すたれた工場","�y���傩����Ă��Ȃ��A�N�Z�X�ł��z"
        },
                {
            "チュートリアル","うっそう　しんりん","みちの　いせき","ぼうふう　がんざん"
            ,"こおった　へいげん","ようがん　れんざん","すたれた　こうじょう","�y���傩����Ă��Ȃ��A�N�Z�X�ł��z"
        },
                        {
            "Tutorial","Deep Forest","Mysterious Ruins","Hurricane Mountain"
            ,"Frozen Plains","Hot Volcanos","Abandoned Factory","�y���傩����Ă��Ȃ��A�N�Z�X�ł��z"
        },
    };

    public static string[,] NamesMultiTowerStage = new string[3, 8]
{
        {
            "チュートリアル","1F","2F","3F"
            ,"4F","5F","6F","�y���傩����Ă��Ȃ��A�N�Z�X�ł��z"
        },
                {
            "チュートリアル","1F","2F","3F"
            ,"4F","5F","6F","�y���傩����Ă��Ȃ��A�N�Z�X�ł��z"
        },
                        {
            "Tutorial","1F","2F","3F"
            ,"4F","5F","6F","�y���傩����Ă��Ȃ��A�N�Z�X�ł��z"
        },
};



    public static StageStates[] DataNormalStages = new StageStates[36]
    {
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{},
                    delayMin=3.0f,
                    delayMax=3.1f,
                    score=10000
                },

                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.ARMY},
                    delayMin=3.0f,
                    delayMax=3.1f,
                    score=4
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.DRAWN},
                    delayMin=2.0f,
                    delayMax=3.0f,
                    score=6
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.LAMIA},
                    delayMin=2.0f,
                    delayMax=3.0f,
                    score=6
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.DRAWN,KIND_ENEMY.LAMIA},
                    delayMin=2.0f,
                    delayMax=3.0f,
                    score=6
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{},
                    boss=KIND_BOSS.InsectBoss,
                    delayMin=10.0f,
                    delayMax=10.1f,
                    score=10000
                },

                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.ARMY,KIND_ENEMY.DRAWN,KIND_ENEMY.TANK,KIND_ENEMY.FLYCANNON},
                    delayMin=1.5f,
                    delayMax=3.5f,
                    score=6
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.TANK,KIND_ENEMY.FLYCANNON},
                    delayMin=1.5f,
                    delayMax=3.5f,
                    score=8
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.TURRET},
                    delayMin=1.5f,
                    delayMax=3.5f,
                    score=8
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.TURRET,KIND_ENEMY.TANK,KIND_ENEMY.FLYCANNON},
                    delayMin=1.5f,
                    delayMax=4.5f,
                    score=9
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{},
                    boss=KIND_BOSS.UFO,
                    delayMin=10.0f,
                    delayMax=10.1f,
                    score=10000
                },

                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.EEL},
                    delayMin=1.0f,
                    delayMax=2.0f,
                    score=5
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.BEAMTURRET},
                    delayMin=0.8f,
                    delayMax=2.6f,
                    score=8
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.FACE},
                    delayMin=1.0f,
                    delayMax=3.0f,
                    score=7
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.EEL,KIND_ENEMY.BEAMTURRET},
                    delayMin=1.0f,
                    delayMax=3.0f,
                    score=8
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{},
                    boss=KIND_BOSS.Vane,
                    delayMin=10.0f,
                    delayMax=10.1f,
                    score=10000
                },

                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.SNOW},
                    delayMin=1.0f,
                    delayMax=2.5f,
                    score=6
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.SNOW,KIND_ENEMY.FACE},
                    delayMin=1.0f,
                    delayMax=2.5f,
                    score=6
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.FACE,KIND_ENEMY.FLYCANNON},
                    delayMin=1.0f,
                    delayMax=2.5f,
                    score=8
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.EEL,KIND_ENEMY.FACE },
                    delayMin=1.0f,
                    delayMax=2.5f,
                    score=10
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{},
                    boss=KIND_BOSS.IceClione,
                    delayMin=10.0f,
                    delayMax=10.1f,
                    score=10000
                },

                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.BEAST,KIND_ENEMY.LAMPREY},
                    delayMin=1.5f,
                    delayMax=2.5f,
                    score=12
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.FIREBEAST, KIND_ENEMY.BEAMTURRET},
                    delayMin=1.0f,
                    delayMax=2.5f,
                    score=10
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.FISH},
                    delayMin=1.5f,
                    delayMax=2.5f,
                    score=12
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.BEAST,KIND_ENEMY.FIREBEAST},
                    delayMin=1.3f,
                    delayMax=1.7f,
                    score=17
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{},
                    boss=KIND_BOSS.Ifrit,
                    delayMin=10.0f,
                    delayMax=10.1f,
                    score=10000
                },

                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.CANNON},
                    delayMin=1.5f,
                    delayMax=3.5f,
                    score=15
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.LAMPREY,KIND_ENEMY.BEAMTURRET},
                    delayMin=1.0f,
                    delayMax=1.5f,
                    score=20
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.FIREBEAST,KIND_ENEMY.TURRET,KIND_ENEMY.FACE},
                    delayMin=0.5f,
                    delayMax=1.5f,
                    score=13
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.CANNON,KIND_ENEMY.TANK,KIND_ENEMY.LAMPREY,KIND_ENEMY.FIREBEAST},
                    delayMin=0.5f,
                    delayMax=1.5f,
                    score=17
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{},
                    boss=KIND_BOSS.MechaZombie,
                    delayMin=10.0f,
                    delayMax=10.1f,
                    score=10000
                },

                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.BEAST,KIND_ENEMY.ARMY_EX},
                    delayMin=1.0f,
                    delayMax=2.0f,
                    score=25
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.TURRET,KIND_ENEMY.LAMPREY_Mecha},
                    delayMin=1.0f,
                    delayMax=2.0f,
                    score=23
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.CANNON,KIND_ENEMY.Tank_Danger},
                    delayMin=1.0f,
                    delayMax=2.0f,
                    score=27
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.ARMY_EX,KIND_ENEMY.LAMPREY_Mecha,KIND_ENEMY.Tank_Danger},
                    delayMin=1.0f,
                    delayMax=2.0f,
                    score=40
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{},
                    boss=KIND_BOSS.MailVirus,
                    delayMin=10.0f,
                    delayMax=10.1f,
                    score=10000

                }
    };

    public static StageStates[] DataMultiTowerStages = new StageStates[36]
    {
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{},
                    delayMin=3.0f,
                    delayMax=3.1f,
                    score=10000
                },

                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.ARMY,KIND_ENEMY.DRAWN},
                    delayMin=2.0f,
                    delayMax=3.5f,
                    score=6
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.LAMIA},
                    delayMin=2.0f,
                    delayMax=3.0f,
                    score=7
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{ KIND_ENEMY.ARMY, KIND_ENEMY.DRAWN,KIND_ENEMY.LAMIA},
                    delayMin=1.5f,
                    delayMax=3.0f,
                    score=7
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.DRAWN,KIND_ENEMY.TANK},
                    delayMin=2.0f,
                    delayMax=3.0f,
                    score=7
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{},
                    boss=KIND_BOSS.InsectBoss,
                    delayMin=10.0f,
                    delayMax=10.1f,
                    score=10000
                },

                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.TANK,KIND_ENEMY.FLYCANNON},
                    delayMin=1.5f,
                    delayMax=3.5f,
                    score=7
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.TANK,KIND_ENEMY.EEL},
                    delayMin=1.5f,
                    delayMax=3.5f,
                    score=8
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.TURRET,KIND_ENEMY.LAMIA},
                    delayMin=1.5f,
                    delayMax=3.5f,
                    score=8
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.TURRET,KIND_ENEMY.BEAMTURRET},
                    delayMin=1.5f,
                    delayMax=3.0f,
                    score=12
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{},
                    boss=KIND_BOSS.UFO,
                    delayMin=10.0f,
                    delayMax=10.1f,
                    score=10000
                },

                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.EEL},
                    delayMin=0.3f,
                    delayMax=1.2f,
                    score=20
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.FISH,KIND_ENEMY.SNOW},
                    delayMin=0.8f,
                    delayMax=2.6f,
                    score=12
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.FISH,KIND_ENEMY.LAMIA,KIND_ENEMY.SNOW},
                    delayMin=1.0f,
                    delayMax=3.0f,
                    score=14
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.FACE,KIND_ENEMY.ARMY},
                    delayMin=0.7f,
                    delayMax=2.4f,
                    score=20
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{},
                    boss=KIND_BOSS.Vane,
                    delayMin=10.0f,
                    delayMax=10.1f,
                    score=10000
                },

                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.BEAST},
                    delayMin=1.0f,
                    delayMax=2.5f,
                    score=10
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.EEL,KIND_ENEMY.SNOW},
                    delayMin=0.6f,
                    delayMax=1.3f,
                    score=22
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.FACE,KIND_ENEMY.BEAMTURRET},
                    delayMin=1.0f,
                    delayMax=2.0f,
                    score=14
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.ARMY,KIND_ENEMY.FIREBEAST },
                    delayMin=1.0f,
                    delayMax=2.5f,
                    score=10
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{},
                    boss=KIND_BOSS.IceClione,
                    delayMin=10.0f,
                    delayMax=10.1f,
                    score=10000
                },

                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.LAMPREY},
                    delayMin=1.0f,
                    delayMax=2.0f,
                    score=15
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{ KIND_ENEMY.CANNON, KIND_ENEMY.TANK},
                    delayMin=1.0f,
                    delayMax=2.5f,
                    score=14
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.ARMORFISH,KIND_ENEMY.FISH},
                    delayMin=1.5f,
                    delayMax=2.5f,
                    score=25
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.BEAST,KIND_ENEMY.FIREBEAST},
                    delayMin=1.3f,
                    delayMax=1.7f,
                    score=20
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{},
                    boss=KIND_BOSS.Ifrit,
                    delayMin=10.0f,
                    delayMax=10.1f,
                    score=10000
                },

                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.ARMORFISH},
                    delayMin=1.0f,
                    delayMax=2.5f,
                    score=20
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{ KIND_ENEMY.FIREBEAST, KIND_ENEMY.ARMORFISH},
                    delayMin=1.0f,
                    delayMax=1.5f,
                    score=20
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.FIREBEAST,KIND_ENEMY.CANNON},
                    delayMin=1.0f,
                    delayMax=1.5f,
                    score=20
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.CANNON,KIND_ENEMY.FACE,KIND_ENEMY.BEAMTURRET, KIND_ENEMY.FIREBEAST},
                    delayMin=0.5f,
                    delayMax=1.5f,
                    score=25
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{},
                    boss=KIND_BOSS.NeonDragon,
                    delayMin=10.0f,
                    delayMax=10.1f,
                    score=10000
                },

                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.FIREBEAST,KIND_ENEMY.ARMY_EX},
                    delayMin=1.0f,
                    delayMax=2.0f,
                    score=25
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.ARMORFISH,KIND_ENEMY.LAMPREY_Mecha},
                    delayMin=1.0f,
                    delayMax=2.0f,
                    score=23
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.BEAMTURRET,KIND_ENEMY.Tank_Danger},
                    delayMin=1.0f,
                    delayMax=2.0f,
                    score=27
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{KIND_ENEMY.ARMY_EX,KIND_ENEMY.LAMPREY_Mecha,KIND_ENEMY.Tank_Danger},
                    delayMin=1.0f,
                    delayMax=2.0f,
                    score=40
                },
                new StageStates
                {
                    listEnemys=new List<KIND_ENEMY>{},
                    boss=KIND_BOSS.MailVirus,
                    delayMin=10.0f,
                    delayMax=10.1f,
                    score=10000

                }
    };




}

