using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustC : MonoBehaviour
{
    private Vector3 pos;

    [SerializeField]
    private ExpC ExpPrefab;

    [SerializeField]
    [Tooltip("回転するか")]
    private bool _isKaitenLock;

    [SerializeField]
    private float sizex, sizey, speed, delete;

    private  float angle;

    [SerializeField, Tooltip("1フレームにエフェクト出す回数")]
    private int _loop = 1;

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
        for (int hoge = 0; hoge < _loop; hoge++)
        {
            pos += transform.right* Random.Range(-sizex, sizex + 1);
            pos += transform.up * Random.Range(-sizey, sizey + 1);
            if (_isKaitenLock) angle = 90;
            else angle = Random.Range(0, 360);
            Quaternion rot = transform.localRotation;
            ExpC shot = Instantiate(ExpPrefab, pos, rot);
            shot.EShot1(angle, speed, delete);
        }
    }
}
