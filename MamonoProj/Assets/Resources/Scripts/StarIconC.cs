using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarIconC : MonoBehaviour
{
    private Vector3 _firstPosition;

    [SerializeField]
    private int _starPoint;


    // Start is called before the first frame update
    void Start()
    {
        _firstPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameData.TP>=_starPoint)transform.position = _firstPosition;
        else transform.position = new Vector3(0, 10000, 0);

    }
}
