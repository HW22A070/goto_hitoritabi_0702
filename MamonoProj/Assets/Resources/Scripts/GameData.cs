using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    /// <summary>
    /// 0=easy 1=normal 2=hard 3=nooooo!!!
    /// </summary>
    public static int Difficulty = 0;

    /// <summary>
    /// 0=通常
    /// </summary>
    public static int GameMode = 0;

    /// <summary>
    /// 0=通常 1=路面凍結
    /// </summary>
    public static int IceFloor = 0;

    /// <summary>
    /// 0=通常 1=エクストラステージ
    /// </summary>
    public static int EX = 0;

    /// <summary>
    /// ラウンド
    /// </summary>
    public static int Round = 1;

    /// <summary>
    /// 合計スコア
    /// </summary>
    public static float Score = 0;

    /// <summary>
    /// PlayerHP
    /// </summary>
    public static float HP = 20;

    /// <summary>
    /// 0=normal 1=NoDamage
    /// </summary>
    public static bool Star=false;

    /// <summary>
    /// 0=normal 1=boss fight
    /// </summary>
    public static float Boss = 0;

    /// <summary>
    /// Magicattack
    /// </summary>
    public static float TP = 0;

    /// <summary>
    /// 0=normal 1=pouse
    /// </summary>
    public static bool Pouse=false;

    /// <summary>
    /// Camera's Rotation
    /// </summary>
    public static int Camera = 0;

    /// <summary>
    /// ClearTime
    /// </summary>
    public static float ClearTime = 0;

    /// <summary>
    /// WindSpeed X
    /// </summary>
    public static float WindSpeed = 0;

    /// <summary>
    /// MaxClearLound
    /// </summary>
    public static int LastCrearLound = 0;

    /// <summary>
    /// スタートRound
    /// </summary>
    public static int StartRound = 1;

    /// <summary>
    /// ゴールRound
    /// </summary>
    public static int GoalRound = 30;

    /// <summary>
    /// VirusBoss Level
    /// 0=No Bug
    /// 1=Little
    /// 2=Crazy
    /// 100=Blue
    /// 200=City
    /// </summary>
    public static int VirusBugEffectLevel = 0;

    /// <summary>
    /// UraVirus
    /// </summary>
    public static bool Zuru;

    /// <summary>
    /// ControllerisVibration
    /// </summary>
    public static bool IsVibration;

    /// <summary>
    /// 移動アクション中かどうか
    /// </summary>
    public static bool StageMovingAction;

    /// <summary>
    /// 時間動き
    /// </summary>
    public static bool TimerMoving=true;

    /// <summary>
    /// プレイヤーの行動制限
    /// 0=動けない
    /// 1=歩き可能
    /// 2=ジャンプ可能
    /// 3=降下可能
    /// 4=攻撃可能
    /// 5=ぶき替え可能
    /// 6=すべて可能
    /// </summary>
    public static int PlayerMoveAble = 6;

    /// <summary>
    /// 画面の中のどこか(X:0-640,Y:0-480,Z:0)をVector3で指定
    /// </summary>
    /// <returns></returns>
    public static Vector3 RandomWindowPosition()
    {
        return new Vector3(Random.Range(0, 640), Random.Range(0, 480), 0);
    }

    /// <summary>
    /// 床置きY座標計算
    /// </summary>
    /// <param name="targetFloor">置きたい床0~4</param>
    /// <param name="offsetY">Yオフセット</param>
    /// <returns></returns>
    public static float GroundPutY(int targetFloor, float offsetY)
    {
        return (targetFloor * 90) + offsetY;
    }

    /// <summary>
    /// Own座標から見たTarget座標への向きを割り出す
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static float GetAngle(Vector3 ownPos,Vector3 targetPos)
    {
        Vector2 direction = targetPos-ownPos;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
