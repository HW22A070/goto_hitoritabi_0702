using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDic.Player;

public class CristalC : ETypeCoreC
{
    public void SetColor(MODE_GUN weapon)
    {
        DoGetComponent();

        _srOwnBody.color = GameData.WeaponColor[(int)weapon];
        switch (weapon)
        {
            case MODE_GUN.Shining:
                _eCoreC.SetDamageValue(3, 0, 0, 0);
                break;

            case MODE_GUN.Physical:
                _eCoreC.SetDamageValue(0,3, 0, 0);
                break;

            case MODE_GUN.Crash:
                _eCoreC.SetDamageValue(0, 0, 3, 0);
                break;

            case MODE_GUN.Heat:
                _eCoreC.SetDamageValue(0, 0, 0, 3);
                break;
        }
    }
}
