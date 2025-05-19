using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishC : ETypeDevilC
{
    public Sprite noemalS, gachiS;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (_eCoreC.EvoltionMode == 1)
        {
            _srOwnBody.sprite = gachiS;

            _moveSpeedBound = 7;
        }
    }
}
