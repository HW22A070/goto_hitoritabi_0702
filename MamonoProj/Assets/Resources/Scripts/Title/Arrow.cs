using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Vector3 _firstPos;
    [SerializeField]
    private float _posDelta;
    // Start is called before the first frame update
    void Start()
    {
        _firstPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveArrow(int titleMode)
    {
        transform.position = _firstPos - (transform.up * _posDelta * titleMode);
    }
}
