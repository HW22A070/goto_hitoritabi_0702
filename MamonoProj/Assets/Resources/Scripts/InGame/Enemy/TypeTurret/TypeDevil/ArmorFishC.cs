using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorFishC : ETypeDevilC
{
    public Sprite armorS, noemalS, gachiS;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        switch (_eCoreC.EvoltionMode)
        {
            case 0:
                _srOwnBody.sprite = armorS;
                _moveSpeedBound = 3;
                break;

            case 1:
                _srOwnBody.sprite = noemalS;
                _moveSpeedBound = 5;
                break;

            case 2:
                _srOwnBody.sprite = gachiS;
                _moveSpeedBound = 7;
                break;
        }
    }
}
