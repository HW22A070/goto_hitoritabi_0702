using EnumDic.System;
using UnityEngine;

/// <summary>
/// ステージ演出
/// </summary>
public class StageEffectC : MonoBehaviour
{
    private float _timecount = 0;

    /// <summary>
    /// 葉
    /// </summary>
    [SerializeField]
    private ExpC _leafEf;

    /// <summary>
    /// UFO
    /// </summary>
    [SerializeField]
    private ExpC _ufoEfL, _ufoEfR;

    /// <summary>
    /// 雪
    /// </summary>
    [SerializeField]
    private ExpC _snowEf;

    /// <summary>
    /// 火の粉
    /// </summary>
    [SerializeField]
    private ExpC _fireEf;

    /// <summary>
    /// ウイルス
    /// </summary>
    [SerializeField]
    private ExpC _virusEffect;

    // Update is called once per frame
    void FixedUpdate()
    {
        _timecount -= Time.deltaTime;
        if (_timecount <= 0)
        {
            switch (GameData.GameMode)
            {
                case MODE_GAMEMODE.Normal:
                    //葉
                    if (1 <= GameData.Round && GameData.Round <= 5)
                    {
                        RunEffectRain(_leafEf, Random.Range(2, 5), 10);
                        _timecount = 0.7f;
                    }
                    //UFO
                    if (6 <= GameData.Round && GameData.Round <= 10)
                    {

                        EffectUFO(_ufoEfL, _ufoEfR, Random.Range(3, 10), 10, Random.Range(0, 2));
                        _timecount = 1.2f;
                    }
                    //雪
                    if (16 <= GameData.Round && GameData.Round <= 20)
                    {
                        RunEffectRain(_snowEf, 2, 10);
                        _timecount = 0.4f;
                    }
                    //火の粉
                    else if (20 <= GameData.Round && GameData.Round <= 25)
                    {
                        EffectSmog(_fireEf, Random.Range(2, 6), 10);
                        _timecount = 0.4f;
                    }
                    //Virus
                    else if (31 <= GameData.Round && GameData.Round <= 34)
                    {
                        EffectDust(_virusEffect, 4);
                        _timecount = 0.1f;
                    }
                    break;
            }

        }
    }

    private void EffectDust(ExpC dust,float destroy)
    {
        Instantiate(dust, new Vector3(Random.Range(0, GameData.WindowSize.x), Random.Range(-50, GameData.WindowSize.y), 0), transform.localRotation).ShotEXP(Random.Range(0, 360), 0.3f, destroy);
    }

    private void EffectSmog(ExpC dust, int speed, float destroy)
    {
        Instantiate(dust, new Vector3(Random.Range(0, GameData.WindowSize.x), -5, 0), transform.localRotation).ShotEXP(Random.Range(85, 96), speed, destroy);
    }

    private void EffectUFO(ExpC dustL, ExpC dustR, int speed, float destroy,int LR)
    {
        if (LR == 0)
        {
            Instantiate(dustL, new Vector3(GameData.WindowSize.x+16, Random.Range(0, GameData.WindowSize.y), 0), transform.localRotation).ShotEXP(Random.Range(175, 186), speed, destroy);
         }
        else
        {
            Instantiate(dustR, new Vector3(-16, Random.Range(0, GameData.WindowSize.x), 0), transform.localRotation).ShotEXP(Random.Range(-10, 10), speed, destroy);
        }
    }

    private void RunEffectRain(ExpC dust,int speed,float destroy)
    {
        Instantiate(dust, new Vector3(Random.Range(0, GameData.WindowSize.x), 485, 0), transform.localRotation).ShotEXP(Random.Range(265, 276), speed, destroy);
    }
}
