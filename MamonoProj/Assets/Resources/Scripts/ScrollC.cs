using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollC : MonoBehaviour
{
    [SerializeField]
    private Vector3 _resetPosition;

    [SerializeField]
    private float _triggerPosX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position -= transform.right;
        if(transform.position.x<=_triggerPosX)transform.position=_resetPosition;
    }
}
