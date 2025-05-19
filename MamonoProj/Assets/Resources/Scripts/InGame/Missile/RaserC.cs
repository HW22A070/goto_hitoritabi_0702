using Move2DSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaserC : EMissile1C
{
    public void ShotRaser(float angle,float size)
    {
        transform.position += Moving2DSystems.GetDirection(angle) * size;
        ShotMissile(angle, 0, 1000);
    }
}
