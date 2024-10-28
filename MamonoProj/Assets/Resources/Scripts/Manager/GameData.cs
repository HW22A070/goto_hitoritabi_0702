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
    /// 0=通常 1=エクストラステージ
    /// </summary>
    public static int EX = 0;

    /// <summary>
    /// ラウンド
    /// </summary>
    public static int Round = 1;

    /// <summary>
    /// 合計ポイント
    /// </summary>
    public static float Point = 0;

    /// <summary>
    /// PlayerHP
    /// </summary>
    public static float HP = 20;

    /// <summary>
    /// 0=normal 1=NoDamage
    /// </summary>
    public static bool Star = false;

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
    public static bool Pouse = false;

    /// <summary>
    /// Camera's Rotation
    /// </summary>
    public static int Camera = 0;

    /// <summary>
    /// ClearTime
    /// </summary>
    public static float ClearTime = 0;

    /// <summary>
    /// 0＝無風
    /// 1＜右風
    /// -1＞左風
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
    /// ControllerisVibration
    /// </summary>
    public static bool IsVibration;

    /// <summary>
    /// 移動アクション中かどうか
    /// </summary>
    public static bool StageMovingAction;

    /// <summary>
    /// 0=日本語
    /// 1=にほんご
    /// 2=English
    /// </summary>
    public static int Language = 0;

    /// <summary>
    /// [難易度,言語]
    /// </summary>
    public static string[,] TextDifficulty = new string[4, 3] {
        {"あんぜん","あんぜん","Safety"},
        {"いっぱん","いっぱん","General" },
        {"かりょく","かりょく","Assault" },
        {"さつりく","さつりく","Berserker"}
    };


    /// <summary>
    /// クリア時間動き
    /// </summary>
    public static bool TimerMoving = true;

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
    /// 画面サイズ
    /// </summary>
    public static int FirstWidth = Screen.width, FirstHeight = Screen.height;

    /// <summary>
    /// 武器カラー
    /// </summary>
    public static Color32[] WeaponColor = {
        new Color32(247, 139, 131, 255) ,
        new Color32(169, 171, 94, 255),
        new Color32(139, 144, 189, 255),
        new Color32(82, 183, 174, 255)};

    public static Vector2 WindowSize = new Vector2(640, 480);

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
    public static float GetAngle(Vector3 ownPos, Vector3 targetPos)
    {
        Vector2 direction = targetPos - ownPos;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// angleに進む際のVector3の増分を割り出す
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector3 GetDirection(float angle)
    {
        Vector3 direction = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0);
        return direction;
    }

    /// <summary>
    /// 忍び寄り
    /// </summary>
    /// <param name="ownPos"></param>
    /// <param name="targetPos"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    public static Vector3 GetSneaking(Vector3 ownPos, Vector3 targetPos, float speed)
    {
        Vector3 sneaking = Vector3.zero;
        if (speed == 0.0f)
        {
            return sneaking;
        }
        sneaking.x = (targetPos.x - ownPos.x) / speed;
        sneaking.y = (targetPos.y - ownPos.y) / speed;
        return sneaking;
    }

    public static int GetRoundNumber()
    {
        if (Round == 0) return -1;
        if (GameMode == 0) return ((Round - 1) / 5) + 1;


        return 0;
    }

    /// <summary>
    /// 画面外にぶっ飛んでる分をもどす
    /// </summary>
    public static Vector3 FixPosition(Vector3 position, float ofsetX, float ofsetY)
    {
        if (position.x < ofsetX) position = new Vector3(ofsetX, position.y, 0);
        else if (position.x > 640 - ofsetX) position = new Vector3(640 - ofsetX, position.y, 0);

        if (position.y < ofsetY) position = new Vector3(position.x, ofsetY, 0);
        else if (position.y > 480 - ofsetY) position = new Vector3(position.x, 480 - ofsetY, 0);

        return position;
    }

    /// <summary>
    /// エネミー全部消す
    /// </summary>
    public static void AllDeleteEnemy()
    {
        GameObject[] myObjects;
        string[] _tagName = { "Enemy" };
        for (int huga = 0; huga < _tagName.Length; huga++)
        {
            myObjects = GameObject.FindGameObjectsWithTag(_tagName[huga]);
            for (int hoge = 0; hoge < myObjects.Length; hoge++)
            {
                Destroy(myObjects[hoge]);
            }
        }
    }

    /// <summary>
    /// 敵弾全部消す
    /// </summary>
    public static void AllDeleteEMissile()
    {
        GameObject[] myObjects;
        string[] _tagName = { "EM" };
        for (int huga = 0; huga < _tagName.Length; huga++)
        {
            myObjects = GameObject.FindGameObjectsWithTag(_tagName[huga]);
            for (int hoge = 0; hoge < myObjects.Length; hoge++)
            {
                Destroy(myObjects[hoge]);
            }
        }
    }
}
