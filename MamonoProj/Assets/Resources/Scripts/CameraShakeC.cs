using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラ振動
/// </summary>
public class CameraShakeC : MonoBehaviour
{
    private Quaternion _rotOwn;
    private Vector3 _posOwn;

    // Start is called before the first frame update
    void Start()
    {
        _posOwn = transform.position;
    }

    /// <summary>
    /// 縦シェイク発動
    /// </summary>
    /// <param name="power">最大振動</param>
    /// <param name="loop">振動回数（loop*0.03=秒）</param>
    public void StartShakeVertical(int power, int loop)
    {
        StopAllCoroutines();
        transform.position = _posOwn;
        StartCoroutine(ShakeVertical(power,loop));
    }

    /// <summary>
    /// 横シェイク発動
    /// </summary>
    /// <param name="power">最大振動</param>
    /// <param name="loop">振動回数（loop*0.03=秒）</param>
    public void StartShakeBeside(int power, int loop)
    {
        StopAllCoroutines();
        transform.position = _posOwn;
        StartCoroutine(ShakeBeside(power, loop));
    }

    /// <summary>
    /// 縦振動
    /// </summary>
    /// <param name="power"></param>
    /// <param name="loop"></param>
    /// <returns></returns>
    private IEnumerator ShakeVertical(int power, int loop)
    {
        for (int i = 0; i < loop; i++)
        {
            transform.localPosition = _posOwn + new Vector3(0, Random.Range(-power,power+1), 0);
            yield return new WaitForSeconds(0.03f);
        }
        transform.position =_posOwn;
    }

    /// <summary>
    /// 横振動
    /// </summary>
    /// <param name="power"></param>
    /// <param name="loop"></param>
    /// <returns></returns>
    private IEnumerator ShakeBeside(int power, int loop)
    {
        for (int i = 0;i<loop;i++)
        {
            transform.localPosition = _posOwn + new Vector3(Random.Range(-power, power + 1), 0, 0);
            yield return new WaitForSeconds(0.03f);
        }
        transform.position = _posOwn;
    }
}
