using EnumDic.Enemy.Virus;
using EnumDic.Stage;
using EnumDic.System;
using UnityEngine;

/// <summary>
/// 背景設定
/// </summary>
public class BackgroundC : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite windy, blue, bug1, bug2, bug3, bug4, bug5, bug6;

    [SerializeField, Tooltip("A=Insect,B=UFO,C=Vane,D=Ice,E=Ifrirt,F=Zombie,G=Virus")]
    private Sprite[] normalTexture;

    [SerializeField, Tooltip("A=Insect,B=UFO,C=Vane,D=Ice,E=Ifrirt,F=Zombie,G=Virus")]
    private Sprite MultiTowerTexture;

    void FixedUpdate()
    {
        Sprite spriteSet = null;

        switch (GameData.VirusBugEffectLevel)
        {
            //通常時
            case MODE_VIRUS.None:
                spriteSet = SettingNormal();
                break;

            case MODE_VIRUS.Little:
                spriteSet = SettingVirusLittle();
                break;

            case MODE_VIRUS.Medium:
                spriteSet = SettingVirusMedium();
                break;

            case MODE_VIRUS.Large:
                spriteSet = SettingVirusCrazy();
                break;

            case MODE_VIRUS.FullThrottle1:
                spriteSet = blue;
                break;

            case MODE_VIRUS.FullThrottle2:
                spriteSet = SpriteSettingVirusCritical2();
                break;
        }

        if (spriteRenderer.sprite != spriteSet)
        {
            if (spriteSet != null)
            {
                spriteRenderer.sprite = spriteSet;
            }
            else
            {
                if (spriteRenderer.sprite == null)
                {
                    spriteRenderer.sprite = SettingNormal();
                }
            }
        }
    }

    /// <summary>
    /// 背景設定　通常時
    /// </summary>
    /// <returns></returns>
    private Sprite SettingNormal()
    {
        if (GameData.WindSpeed >= 100 || GameData.WindSpeed <= -100) return windy;

        switch (GameData.GameMode)
        {
            case MODE_GAMEMODE.Normal:
                if (GameData.StageMode == KIND_STAGE.Tutorial) return normalTexture[0];
                else return normalTexture[(int)GameData.StageMode];

            case MODE_GAMEMODE.MultiTower:
                return MultiTowerTexture;
        }
        return normalTexture[(int)GameData.StageMode];
    }

    /// <summary>
    /// 背景設定　裏ボス第一形態
    /// </summary>
    /// <returns></returns>
    private Sprite SettingVirusLittle()
    {
        switch (Random.Range(0, 30))
        {
            case 2:
                return bug1;

            case 3:
                return bug2;

            default:
                return normalTexture[(int)GameData.StageMode];
        }
    }

    /// <summary>
    /// 背景設定　裏ボス第二形態
    /// </summary>
    /// <returns></returns>
    private Sprite SettingVirusMedium()
    {
        switch (Random.Range(0, 10))
        {
            case 0:
                return normalTexture[(int)GameData.StageMode];

            case 1:
                return normalTexture[Random.Range(0, 6)];

            case 2:
                return bug1;

            case 3:
                return bug2;

        }
        return null;
    }

    /// <summary>
    /// 背景設定　裏ボス最終形態
    /// </summary>
    /// <returns></returns>
    private Sprite SettingVirusCrazy()
    {
        switch (Random.Range(0, 20))
        {
            case 0:
                return normalTexture[(int)GameData.StageMode];

            case 1:
                return normalTexture[Random.Range(0, 6)];

            case 2:
                return bug1;

            case 3:
                return bug2;

            case 4:
                return bug3;

            case 5:
                return bug4;

            case 6:
                return bug5;

            case 7:
                return bug6;
        }
        return null;
    }

    /// <summary>
    /// 背景設定　裏ボス必殺技
    /// </summary>
    /// <returns></returns>
    private Sprite SettingSettingVirusCritical1()
    {
        return blue;
    }

    /// <summary>
    /// 背景設定　裏ボス必殺技２
    /// </summary>
    /// <returns></returns>
    private Sprite SpriteSettingVirusCritical2()
    {
        switch (Random.Range(0, 10))
        {
            case 0:
                return bug3;

            case 1:
                return bug4;

            case 2:
                return normalTexture[(int)GameData.StageMode];

        }
        return null;
    }
}
