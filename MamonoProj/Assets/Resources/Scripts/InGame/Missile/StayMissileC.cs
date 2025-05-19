using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StayMissileC : EMissile1C
{
    private float _timeStay = 0f, _timeStayCount = 0f;

    private bool _isStaying = true;

    // Start is called before the first frame update
    public void SetStayMissile(float angle, float speed, float kasoku,float stayTime)
    {
        ShotMissile(angle, speed, kasoku);
        _timeStay = stayTime;
    }

    /// <summary>
    /// ステイ状態を解除し、発進
    /// </summary>
    public void SetDeparture()
    {
        _isStaying = false;
    }

    private new void FixedUpdate()
    {
        //ステイ状態であれば、FixedUpdateを動かさない。
        if (!_isStaying)
        {
            base.FixedUpdate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        _timeStayCount += Time.deltaTime;
        if (_timeStayCount >= _timeStay)
        {
            if (_isStaying)
            {
                SetDeparture();
            }
        }
    }
}
