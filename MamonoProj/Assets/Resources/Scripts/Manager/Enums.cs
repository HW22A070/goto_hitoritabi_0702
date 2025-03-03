using EnumDic.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnumDic
{
    namespace System
    {
        public enum MODE_DIFFICULTY
        {
            Safety,
            General,
            Assault,
            Berserker
        }
    }

    namespace Player
    {
        public enum MODE_GUN
        {
            /// <summary>
            /// 光、エネルギー
            /// </summary>
            Shining,

            /// <summary>
            /// 物理攻撃
            /// </summary>
            Physical,

            /// <summary>
            /// 衝撃
            /// </summary>
            Crash,

            /// <summary>
            /// 熱、高温
            /// </summary>
            Heat
        }
    }

    namespace Floor
    {
        /// <summary>
        /// 0=通常
        /// 1=氷
        /// 2=可燃
        /// 3=炎上
        /// 4=理不尽棘準備
        /// 5=激やば理不尽棘
        /// </summary>
        public enum MODE_FLOOR
        {
            Normal,
            IceFloor,
            PreBurning,
            Burning,
            PreNeedle,
            Needle
        }
    }

    namespace Enemy
    {
        /// <summary>
        /// 通常的の種類
        /// </summary>
        public enum KIND_ENEMY
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

        /// <summary>
        /// ボス敵の種類
        /// </summary>
        public enum KIND_BOSS
        {
            _NULL,
            InsectBoss,
            UFO,
            Vane,
            IceClione,
            Ifrit,
            MechaZombie,
            MailVirus
        }

        public enum MODE_LIFE
        {
            Arrival,
            Fight,
            Dead
        }

        namespace Virus
        {
            public enum MODE_VIRUS {
                None,
                Little,
                Medium,
                Large,
                FullThrottle1,
                FullThrottle2,

            }
        }
    }
}

[System.Serializable]
public struct GunStates
{
    /// <summary>
    /// 武器モード
    /// </summary>
    public EnumDic.Player.MODE_GUN mode;

    /// <summary>
    /// 武器スプライト
    /// </summary>
    public Sprite[] spritesWeapon;

    /// <summary>
    /// 武器ごとのティック毎チャージ量
    /// </summary>
    public float energyChargeTick;

    /// <summary>
    /// 短距離攻撃のクールタイム
    /// </summary>
    public float cooltimeDefault;

    /// <summary>
    /// 現在のクールタイム
    /// </summary>
    public float cooltimeNow;

    /// <summary>
    /// 武器のエネルギー
    /// </summary>
    public float energy;

    /// <summary>
    /// 消費エネルギー
    /// </summary>
    public float enegyConsumption;

    /// <summary>
    /// 武器が使えるか
    /// </summary>
    public bool isLoaded;
}

[System.Serializable]
public struct StageStates
{
    /// <summary>
    /// 登場する敵
    /// </summary>
    public List<KIND_ENEMY> listEnemys;

    /// <summary>
    /// ボス敵
    /// </summary>
    public KIND_BOSS boss;

    /// <summary>
    /// クールタイム上限
    /// </summary>
    public float delayMin;

    /// <summary>
    /// クールタイム下限
    /// </summary>
    public float delayMax;

}
