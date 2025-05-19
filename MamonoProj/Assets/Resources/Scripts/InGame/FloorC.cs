using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDic.System;
using EnumDic.Stage;
using EnumDic.Enemy.Virus;

/// <summary>
/// 床ギミック管理
/// </summary>
public class FloorC : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField, Tooltip("A=Insect,B=UFO,C=Vane,D=Ice,E=Ifrirt,F=Zombie,G=Virus")]
    private Sprite[] normalTexture;

    [SerializeField]
    private Sprite ice,blue,red,bug1,_fire,_spike,_spike2;

    [SerializeField]
    private Sprite _tower;


    private MODE_FLOOR _floorMode = MODE_FLOOR.Normal;

    /// <summary>
    /// 最下層判定
    /// </summary>
    public bool _isBedRock;

    [SerializeField]
    private GameObject _goFire;

    void FixedUpdate()
    {
        _isBedRock = !Physics2D.Raycast(transform.position- new Vector3(0, 24, 0), new Vector3(0, -1, 0), GameData.WindowSize.y, 8);

        switch (GameData.VirusBugEffectLevel)
        {
            case MODE_VIRUS.None:
                if (_floorMode == MODE_FLOOR.IceFloor) spriteRenderer.sprite = ice;
                else if (GameData.StageMode == KIND_STAGE.Tutorial) spriteRenderer.sprite =normalTexture[0];
                else
                {
                    switch (GameData.GameMode)
                    {
                        case MODE_GAMEMODE.Normal:
                            spriteRenderer.sprite = normalTexture[(int)GameData.StageMode];
                            break;

                        case MODE_GAMEMODE.MultiTower:
                            spriteRenderer.sprite = _tower;
                            break;
                    }
                }
                break;

            case MODE_VIRUS.Little:
                switch(Random.Range(0, 20))
                {
                    case 1:
                        spriteRenderer.sprite = bug1;
                        break;

                    default:
                        spriteRenderer.sprite = normalTexture[(int)GameData.StageMode];
                        break;
                }
                break;

            case MODE_VIRUS.Medium:
                switch(Random.Range(0, 10))
                {
                    case 0:
                        spriteRenderer.sprite = normalTexture[(int)GameData.StageMode];
                        break;

                    case 1:
                        spriteRenderer.sprite = bug1;
                        break;

                    case 2:
                        spriteRenderer.sprite = red;
                        break;
                }
                break;

            case MODE_VIRUS.Large:
                switch (Random.Range(0, 10))
                {
                    case 0:
                        spriteRenderer.sprite = normalTexture[(int)GameData.StageMode];
                        break;

                    case 1:
                        spriteRenderer.sprite = bug1;
                        break;

                    case 2:
                        spriteRenderer.sprite = red;
                        break;

                    case 3:
                        spriteRenderer.sprite = blue;
                        break;
                }
                break;

            case MODE_VIRUS.FullThrottle1:
                spriteRenderer.sprite = blue;
                break;

            case MODE_VIRUS.FullThrottle2:
                switch (Random.Range(0, 20))
                {
                    case 0:
                        spriteRenderer.sprite = bug1;
                        break;

                    default:
                        spriteRenderer.sprite = normalTexture[(int)GameData.StageMode];
                        break;
                }
                break;

        }

        switch (_floorMode)
        {
            case MODE_FLOOR.PreBurning:
                StartCoroutine(OnFire());
                spriteRenderer.sprite = _fire;
                break;

            case MODE_FLOOR.Burning:
                StartCoroutine(OnFire());
                spriteRenderer.sprite = _fire;
                _goFire.SetActive(true);
                break;

            case MODE_FLOOR.PreNeedle:
                StartCoroutine(OnSpike());
                spriteRenderer.sprite = _spike;
                _goFire.SetActive(false);
                break;

            case MODE_FLOOR.Needle:
                StartCoroutine(OnSpike());
                spriteRenderer.sprite = _spike2;
                _goFire.SetActive(false);
                break;

            default:
                _goFire.SetActive(false);
                break;

        }
    }

    private IEnumerator OnFire()
    {
        yield return new WaitForSeconds(1.0f);
        if(_floorMode==MODE_FLOOR.PreBurning)
        {
            _floorMode = MODE_FLOOR.Burning;
        }
    }

    private IEnumerator OnSpike()
    {
        yield return new WaitForSeconds(2.0f);
        if (_floorMode == MODE_FLOOR.PreNeedle)
        {
            _floorMode = MODE_FLOOR.Needle;
        }
    }

    public MODE_FLOOR GetFloorMode() => _floorMode;

    public void SetFloorMode(MODE_FLOOR mode) => _floorMode = mode;
}
