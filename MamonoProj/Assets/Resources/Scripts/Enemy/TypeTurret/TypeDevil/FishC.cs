using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishC : ETypeDevilC
{
    public Sprite noemalS, gachiS;

    bool gachi;

    private ECoreC core;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        ECoreC core = GetComponent<ECoreC>();
        if (core.EvoltionMode == 1)
        {
            gachi = true;
            spriteRenderer.sprite = gachiS;

            _moveSpeedBound = 7;
        }
    }
}
