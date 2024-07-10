using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSHPBarC : MonoBehaviour
{
    private Vector3 _firstBarPos;

    private void Start()
    {
        _firstBarPos = transform.localPosition;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameData.Boss == 0)
        {
            transform.localPosition = new Vector3(1000,10000,0);
        }
        else if (GameData.Boss == 1)
        {
            transform.localPosition = _firstBarPos;
        }
    }
}
