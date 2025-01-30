using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControlC : MonoBehaviour
{
    private AudioSource BGM;

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
        BGM = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        BGM.pitch = Time.timeScale;
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
            if (GameData.Difficulty == 3) bgmValue = 201;
            if (isBoss) bgmValue += 10;

            switch (bgmValue)
            {
                case -1:
                    BGM.clip = _tuto;
                    break;

                case 1:
                    BGM.clip = _bgmRazer;
                    break;
                case 2:
                    BGM.clip = _bgmRuin;
                    break;
                case 3:
                    BGM.clip = _bgmHurricane;
                    break;
                case 4:
                    BGM.clip = _bgmSnow;
                    break;
                case 5:
                    BGM.clip = _bgmVolcano;
                    break;
                case 6:
                    BGM.clip = _bgmMetal;
                    break;
                case 7:
                    BGM.clip = _bgmBug;
                    break;

                case 11:
                    BGM.clip = _bgmBossRazer;
                    break;
                case 12:
                    BGM.clip = _bgmBossRuin;
                    break;
                case 13:
                    BGM.clip = _bgmBossHurricane;
                    break;
                case 14:
                    BGM.clip = _bgmBossSnow;
                    break;
                case 15:
                    BGM.clip = _bgmBossVolcano;
                    break;
                case 16:
                    BGM.clip = _bgmBossMetal;
                    break;

                case 101:
                    BGM.clip = V1;
                    break;
                case 102:
                    BGM.clip = V2;
                    break;
                case 103:
                    BGM.clip = V3;
                    break;

                case 200:
                    BGM.clip = V1;
                    break;
                case 210:
                    BGM.clip = V2;
                    break;


                case 201:
                    BGM.clip = _satsuriku;
                    break;
                case 211:
                    BGM.clip = _satsurikuBoss;
                    break;

                default:
                    BGM.clip = null;
                    break;
            }
            BGM.Play();
        }
        if (volume >= 0)
        {
            BGM.volume = volume;
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
        float volmeValue = goal - BGM.volume;
        int volmeDelta = (int)(second / 0.1f);
        for (int hoge=0;hoge< volmeDelta; hoge++)
        {
            BGM.volume += volmeValue / volmeDelta;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
