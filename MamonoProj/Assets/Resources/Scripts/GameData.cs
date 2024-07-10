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
    /// 0=�ʏ�
    /// </summary>
    public static int GameMode = 0;

    /// <summary>
    /// 0=�ʏ� 1=�H�ʓ���
    /// </summary>
    public static int IceFloor = 0;

    /// <summary>
    /// 0=�ʏ� 1=�G�N�X�g���X�e�[�W
    /// </summary>
    public static int EX = 0;

    /// <summary>
    /// ���E���h
    /// </summary>
    public static int Round = 1;

    /// <summary>
    /// ���v�X�R�A
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
    /// �X�^�[�gRound
    /// </summary>
    public static int StartRound = 1;

    /// <summary>
    /// �S�[��Round
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
    /// �ړ��A�N�V���������ǂ���
    /// </summary>
    public static bool StageMovingAction;

    /// <summary>
    /// ���ԓ���
    /// </summary>
    public static bool TimerMoving=true;

    /// <summary>
    /// �v���C���[�̍s������
    /// 0=�����Ȃ�
    /// 1=�����\
    /// 2=�W�����v�\
    /// 3=�~���\
    /// 4=�U���\
    /// 5=�Ԃ��ւ��\
    /// 6=���ׂĉ\
    /// </summary>
    public static int PlayerMoveAble = 6;

    /// <summary>
    /// ��ʂ̒��̂ǂ���(X:0-640,Y:0-480,Z:0)��Vector3�Ŏw��
    /// </summary>
    /// <returns></returns>
    public static Vector3 RandomWindowPosition()
    {
        return new Vector3(Random.Range(0, 640), Random.Range(0, 480), 0);
    }

    /// <summary>
    /// ���u��Y���W�v�Z
    /// </summary>
    /// <param name="targetFloor">�u��������0~4</param>
    /// <param name="offsetY">Y�I�t�Z�b�g</param>
    /// <returns></returns>
    public static float GroundPutY(int targetFloor, float offsetY)
    {
        return (targetFloor * 90) + offsetY;
    }

    /// <summary>
    /// Own���W���猩��Target���W�ւ̌���������o��
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static float GetAngle(Vector3 ownPos,Vector3 targetPos)
    {
        Vector2 direction = targetPos-ownPos;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
