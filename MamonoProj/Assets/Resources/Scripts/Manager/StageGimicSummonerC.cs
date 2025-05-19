using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDic.Stage;
using EnumDic.System;

public class StageGimicSummonerC : MonoBehaviour
{
    //ステージレベルアップ時に特別な処置を行う
    public void DoSpecialStageCreate()
    {
        GameData.WindSpeed = 0;
        FloorManagerC.SetStageGimic(100, 0);

        switch (GameData.GameMode)
        {
            case MODE_GAMEMODE.Normal:
                switch (GameData.Round)
                {
                    case 14:
                        GameData.WindSpeed = 20;
                        break;

                    case 17:
                        FloorManagerC.SetStageGimic(30, MODE_FLOOR.IceFloor);
                        break;
                    case 18:
                        FloorManagerC.SetStageGimic(60, MODE_FLOOR.IceFloor);
                        break;
                    case 19:
                        FloorManagerC.SetStageGimic(100, MODE_FLOOR.IceFloor);
                        break;
                    case 22:
                        FloorManagerC.SetStageGimic(10, MODE_FLOOR.PreBurning);
                        break;
                    case 23:
                        FloorManagerC.SetStageGimic(7, MODE_FLOOR.PreBurning);
                        break;
                    case 24:
                        FloorManagerC.SetStageGimic(100, 0);
                        FloorManagerC.SetGimicBedRock(MODE_FLOOR.PreBurning);
                        break;

                    case 27:
                        FloorManagerC.SetStageGimic(80, MODE_FLOOR.IceFloor);
                        break;

                    case 34:
                        FloorManagerC.SetStageGimic(3, MODE_FLOOR.PreNeedle);
                        FloorManagerC.SetStageGimic(10, MODE_FLOOR.PreBurning);
                        break;
                }
                break;

            case MODE_GAMEMODE.MultiTower:
                break;
        }


        
    }
}
