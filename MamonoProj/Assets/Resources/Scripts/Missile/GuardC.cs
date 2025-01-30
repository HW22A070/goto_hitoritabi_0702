using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GuardC : MonoBehaviour
{
    Vector3 velocity, pos, fpos;

    float r, zou, ssp, of;
    float sp = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void EShot1(float radius, float offset, float speed, float deltar, Vector3 defpos)
    {
        r = radius;
        of = offset;
        ssp = speed;
        zou = deltar;
        pos = defpos;
    }

    void FixedUpdate()
    {

        fpos = pos + new Vector3(Mathf.Sin((sp + of) * Mathf.Deg2Rad) * r, Mathf.Cos((sp + of) * Mathf.Deg2Rad) * r, 0);

        transform.localPosition = fpos;

        sp += ssp;
        r += zou;
        while (sp > 360)
        {
            sp -= 360;
        }

        if (GetComponent<EMCoreC>().DeleteMissileCheck())
        {
            Destroy(gameObject);
        }
    }
}
