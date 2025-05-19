using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGraC : MonoBehaviour
{
    [SerializeField]
    int _ofsetChangeTick = 0;

    int _tick = 0;

    [SerializeField]
    Sprite _spChanged;

    // Update is called once per frame
    void Update()
    {
        if (_ofsetChangeTick <= _tick)
        {
            GetComponent<SpriteRenderer>().sprite = _spChanged;
            enabled = false;
        }
        _tick++;
    }
}
