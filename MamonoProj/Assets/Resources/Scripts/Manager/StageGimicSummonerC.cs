using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGimicSummonerC : MonoBehaviour
{
    //ステージレベルアップ時に特別な処置を行う
    public void SpecialStageCreate()
    {
        GameData.WindSpeed = 0;
        FloorManagerC.StageGimic(100, 0);

        switch (GameData.Round)
        {
            case 14:
                GameData.WindSpeed = 20;
                break;

            case 17:
                FloorManagerC.StageGimic(30, 1);
                break;
            case 18:
                FloorManagerC.StageGimic(60, 1);
                break;
            case 19:
                FloorManagerC.StageGimic(100, 1);
                break;
            case 22:
                FloorManagerC.StageGimic(10, 2);
                break;
            case 23:
                FloorManagerC.StageGimic(7, 2);
                break;
            case 24:
                FloorManagerC.StageGimic(100, 0);
                FloorManagerC.SetGimicBedRock(2);
                break;

            case 28:
                FloorManagerC.StageGimic(80, 1);
                break;

            case 34:
                FloorManagerC.StageGimic(3, 4);
                FloorManagerC.StageGimic(10, 2);
                break;
        }
    }
}
