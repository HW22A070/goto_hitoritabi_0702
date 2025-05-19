using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenC : EMissile1C
{
    protected float _rollValue=0;

    [SerializeField]
    protected bool geigeki,bombbarrier = true, bombsosai;

    public void ShotShuriken(float angle, float speed, float kasoku,float kaiten)
    {
        ShotMissile(angle, speed, kasoku);
        _rollValue = kaiten;
    }

    // Update is called once per frame
    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        transform.eulerAngles += Vector3.forward*_rollValue;
    }
}
