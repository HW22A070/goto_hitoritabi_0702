using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDic.System;
using EnumDic.Stage;

public class FloorManagerC : MonoBehaviour
{
    [SerializeField]
    private GameObject _goFloor;

    private float _scrapTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        MakeStage(100);
    }

    private void Update()
    {
        /*
        _scrapTime += Time.deltaTime;
        if (_scrapTime >=3f)
        {
            _scrapTime = 0;
            MakeStage();
        }
        */
    }


    public void MakeStage(int createPercent)
    {
        //破壊
        GameObject[] stageTiles= GameObject.FindGameObjectsWithTag("Floor");
        for (int i = 0; i < stageTiles.Length; i++)Destroy(stageTiles[i]);

        //創造
        switch (GameData.GameMode)
        {
            case MODE_GAMEMODE.Normal:
            case MODE_GAMEMODE.MultiTower:
                for (int y = 0; y < GameData.WindowSize.y - 32; y += 90)
                {
                    for (int x = 32; x < GameData.WindowSize.x; x += 64)
                    {
                        if (Random.Range(0, 101) <= createPercent) Instantiate(_goFloor, new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
                    }
                }
                break;
        }
    }

    /// <summary>
    /// ギミック作動
    /// </summary>
    /// <param name="createPercent"></param>
    /// <param name="value"></param>
    public static void SetStageGimic(int createPercent,MODE_FLOOR value)
    {
        GameObject[] stageTiles = GameObject.FindGameObjectsWithTag("Floor");
        for (int i = 0; i < stageTiles.Length; i++)
        {
            if (Random.Range(1, 101) <= createPercent)
            {
                stageTiles[i].GetComponent<FloorC>().SetFloorMode(value);
            }
        }
    }

    /// <summary>
    /// 最下層だけ発動
    /// </summary>
    public static void SetGimicBedRock(MODE_FLOOR value)
    {
        GameObject[] stageTiles = GameObject.FindGameObjectsWithTag("Floor");
        //着火
        for (int i = 0; i < stageTiles.Length; i++)
        {
            if (stageTiles[i].GetComponent<FloorC>()._isBedRock)
            {
                stageTiles[i].GetComponent<FloorC>().SetFloorMode(value);
            }
        }
    }
}
