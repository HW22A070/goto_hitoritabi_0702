using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LamiaC : ETypeArmyC
{
    [SerializeField]
    private Sprite noemalS, gachiS;

    private bool gachi;

    /// <summary>
    /// バリア
    /// </summary>
    [SerializeField]
    private GameObject _barrier;

    private new void FixedUpdate()
    {
        base.FixedUpdate();

        ECoreC core = GetComponent<ECoreC>();
        if (core.EvoltionMode == 1)
        {
            _barrier.SetActive(false);
            gachi = true;
            _srOwnBody.sprite = gachiS;

            _moveSpeedBound = 1.0f;
            _moveSpeedMax = 0.1f;
        }

    }
}
