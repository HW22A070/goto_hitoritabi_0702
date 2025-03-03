using EnumDic.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BGMのボリュームなど
/// </summary>
public class AudioControlC : MonoBehaviour
{
    private AudioSource _asBGM;

    [SerializeField, Tooltip("つうじょうBGM")]
    private AudioClip _bgmRazer, _bgmRuin, _bgmHurricane, _bgmSnow, _bgmVolcano, _bgmMetal,_bgmBug;

    [SerializeField, Tooltip("ボスBGM")]
    private AudioClip _bgmBossRazer, _bgmBossRuin, _bgmBossHurricane, _bgmBossSnow, _bgmBossVolcano, _bgmBossMetal;

    [SerializeField, Header("特殊音楽")]
    private AudioClip V1, V2, V3, _tuto,_satsuriku,_satsurikuBoss;

    private bool _isStarted;


    // Start is called before the first frame update
    void Start()
    {
        _asBGM = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        _asBGM.pitch = Time.timeScale;
        if (!_isStarted)
        {
            ChangeAudio(GameData.GetRoundNumber(), false, -1);
            _isStarted = true;
        }
    }
    

    /// <summary>
    /// BGM管理
    /// volume=ボリューム（-1で変更なし）
    /// </summary>
    /// <param name="bgmValue"></param>
    public void ChangeAudio(int bgmValue,bool isBoss,float volume)
    {
        if (bgmValue != 0)
        {
            if (GameData.Difficulty == MODE_DIFFICULTY.Berserker) bgmValue = 201;
            if (isBoss) bgmValue += 10;

            switch (bgmValue)
            {
                case -1:
                    _asBGM.clip = _tuto;
                    break;

                case 1:
                    _asBGM.clip = _bgmRazer;
                    break;
                case 2:
                    _asBGM.clip = _bgmRuin;
                    break;
                case 3:
                    _asBGM.clip = _bgmHurricane;
                    break;
                case 4:
                    _asBGM.clip = _bgmSnow;
                    break;
                case 5:
                    _asBGM.clip = _bgmVolcano;
                    break;
                case 6:
                    _asBGM.clip = _bgmMetal;
                    break;
                case 7:
                    _asBGM.clip = _bgmBug;
                    break;

                case 11:
                    _asBGM.clip = _bgmBossRazer;
                    break;
                case 12:
                    _asBGM.clip = _bgmBossRuin;
                    break;
                case 13:
                    _asBGM.clip = _bgmBossHurricane;
                    break;
                case 14:
                    _asBGM.clip = _bgmBossSnow;
                    break;
                case 15:
                    _asBGM.clip = _bgmBossVolcano;
                    break;
                case 16:
                    _asBGM.clip = _bgmBossMetal;
                    break;

                case 101:
                    _asBGM.clip = V1;
                    break;
                case 102:
                    _asBGM.clip = V2;
                    break;
                case 103:
                    _asBGM.clip = V3;
                    break;

                case 200:
                    _asBGM.clip = V1;
                    break;
                case 210:
                    _asBGM.clip = V2;
                    break;


                case 201:
                    _asBGM.clip = _satsuriku;
                    break;
                case 211:
                    _asBGM.clip = _satsurikuBoss;
                    break;

                default:
                    _asBGM.clip = null;
                    break;
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
