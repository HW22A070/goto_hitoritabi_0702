using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustC : MonoBehaviour
{
    Vector3 pos;
    public ExpC ExpPrefab;

    [SerializeField]
    [Tooltip("‰ñ“]‚·‚é‚©")]
    private bool _isKaitenLock;

    public float sizex, sizey, speed, delete;

    float angle;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
    }

    void FixedUpdate()
    {
        pos.x += Random.Range(-sizex, sizex + 1);
        pos.y += Random.Range(-sizey, sizey + 1);
        if (_isKaitenLock) angle = 90;
        else angle = Random.Range(0, 360);
        Quaternion rot = transform.localRotation;
        ExpC shot = Instantiate(ExpPrefab, pos, rot);
        shot.EShot1(angle, speed, delete);
    }
}
