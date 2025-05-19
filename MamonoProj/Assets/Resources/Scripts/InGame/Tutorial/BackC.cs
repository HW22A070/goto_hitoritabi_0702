using Move2DSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackC : MonoBehaviour
{
    private Vector3 _posFirst;

    [SerializeField, Tooltip("エフェクトアタッチ")]
    private ExpC _prhbBulletShot;

    private float _time = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        _posFirst = transform.position;
    }

    private void Update()
    {
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            BulletEffect();
            transform.position += transform.right * 64;
            _time = 1.0f;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Moving2DSystems.GetSneaking(transform.position, _posFirst, 10);
    }

    /// <summary>
    /// 銃弾発射光エフェクト
    /// </summary>
    private void BulletEffect()
    {
        //発射エフェクト
        ExpC bulletEf = Instantiate(_prhbBulletShot, transform.position-(transform.right*96), transform.rotation);
        bulletEf.transform.parent = transform;
        bulletEf.ShotEXP(180, 0, 0.12f);
    }
}
