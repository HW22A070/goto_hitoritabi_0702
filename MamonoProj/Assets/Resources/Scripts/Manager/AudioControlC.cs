using EnumDic.Stage;
using EnumDic.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BGMのボリュームなど
/// </summary>
public class AudioControlC : MonoBehaviour
{
    [SerializeField]
    private AudioSource _asBGM;

    [SerializeField, Tooltip("つうじょうBGM")]
    private AudioClip _bgmRazer, _bgmRuin, _bgmHurricane, _bgmSnow, _bgmVolcano, _bgmMetal,_bgmBug;

    [SerializeField, Tooltip("ボスBGM")]
    private AudioClip _bgmBossRazer, _bgmBossRuin, _bgmBossHurricane, _bgmBossSnow, _bgmBossVolcano, _bgmBossMetal;

    [SerializeField, Header("マルチタワー")]
    private AudioClip _multi, _multiBoss;

    [SerializeField, Header("特殊音楽")]
    private AudioClip V1, V2, V3, _tuto,_satsuriku,_satsurikuBoss;

    private bool _isStarted;

    // Update is called once per frame
    void Update()
    {
        _asBGM.pitch = Time.timeScale;
    }


    /// <summary>
    /// BGM管理
    /// </summary>
    /// <param name="bgmValue">種類（-2で変更なし）</param>
    /// <param name="isBoss"></param>
    /// <param name="volume">ボリューム（-1で変更なし）</param>
    public void ChangeAudio(int bgmValue,bool isBoss,float volume)
    {
        if (bgmValue != -2)
        {
            if (GameData.StageMode == KIND_STAGE.Tutorial)
            {
                _asBGM.clip = _tuto;
            }
            else
            {
                //さつりくモードであれば強制上書き
                if (GameData.Difficulty == MODE_DIFFICULTY.Berserker)
                {
                    switch (bgmValue)
                    {
                        //ウィルスとチュートリアルは対象外
                        case -1:
                        case 101:
                        case 102:
                        case 103:
                            break;

                        default:
                            bgmValue = 201;
                            break;
                    }
                }

                switch (bgmValue)
                {
                    case 101:
                        _asBGM.clip = V1;
                        break;
                    case 102:
                        _asBGM.clip = V2;
                        break;
                    case 103:
                        _asBGM.clip = V3;
                        break;

                    case 201:
                        _asBGM.clip = isBoss ? _satsurikuBoss : _satsuriku;
                        break;

                    default:
                        switch (GameData.GameMode)
                        {
                            case MODE_GAMEMODE.Normal:
                                switch (bgmValue)
                                {
                                    case 0:
                                        _asBGM.clip = isBoss ? _bgmBossRazer : _bgmRazer;
                                        break;
                                    case 1:
                                        _asBGM.clip = isBoss ? _bgmBossRuin : _bgmRuin;
                                        break;
                                    case 2:
                                        _asBGM.clip = isBoss ? _bgmBossHurricane : _bgmHurricane;
                                        break;
                                    case 3:
                                        _asBGM.clip = isBoss ? _bgmBossSnow : _bgmSnow;
                                        break;
                                    case 4:
                                        _asBGM.clip = isBoss ? _bgmBossVolcano : _bgmVolcano;
                                        break;
                                    case 5:
                                        _asBGM.clip = isBoss ? _bgmBossMetal : _bgmMetal;
                                        break;
                                    case 6:
                                        _asBGM.clip = _bgmBug;
                                        break;

                                    default:
                                        _asBGM.clip = null;
                                        break;
                                }
                                break;

                            case MODE_GAMEMODE.MultiTower:
                                switch (bgmValue)
                                {
                                    case 5:
                                        _asBGM.clip = isBoss ? _bgmBossMetal : _multi;
                                        break;

                                    case 6:
                                        _asBGM.clip = _bgmBug;
                                        break;

                                    default:
                                        _asBGM.clip = isBoss ? _multiBoss : _multi;
                                        break;
                                }
                                break;
                        }
                        break;
                }
            }
            
            _asBGM.Play();
        }
        if (volume >= 0)
        {
            _asBGM.volume = volume;
        }
    }

    /// <summary>
    /// フェードイン、アウト実行
    /// second=かかる秒数
    /// goal=目標音量
    /// </summary>
    /// <param name="second"></param>
    /// <param name="isIn"></param>
    public void VolumefeedInOut(float second,float goal)
    {
        StartCoroutine(BGMFeedInOut(second, goal));
    }

    private IEnumerator BGMFeedInOut(float second, float goal)
    {
        float volmeValue = goal - _asBGM.volume;
        int volmeDelta = (int)(second / 0.1f);
        for (int hoge=0;hoge< volmeDelta; hoge++)
        {
            _asBGM.volume += volmeValue / volmeDelta;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
