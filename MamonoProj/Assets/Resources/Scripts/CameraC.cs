using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraC : MonoBehaviour
{
    private Quaternion rot;
    private int kka = 0;
    public int i;
    private Vector3 pos;

    public static bool IsCriticalShake,IsDamageShake;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        //StartCoroutine("Game");
    }

    /*public void Kaiten(int kaiten)
    {
        kka = kaiten;
    }*/

    // Update is called once per frame
    void Update()
    {
        if (i == 0)
        {
            transform.localEulerAngles = new Vector3(0, 0, GameData.Camera);
        }
        else if (i == 1)
        {
            transform.localEulerAngles = new Vector3(0, 0, GameData.Camera);
        }
    }

    /// <summary>
    /// 縦シェイク発動
    /// </summary>
    /// <param name="power">最大振動</param>
    /// <param name="loop">振動回数（loop*0.03=秒）</param>
    public void StartShakeVertical(int power, int loop)
    {
        StopAllCoroutines();
        transform.position = pos;
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
        transform.position = pos;
        StartCoroutine(ShakeBeside(power, loop));
    }

    private IEnumerator ShakeVertical(int power, int loop)
    {
        for (i = 0; i < loop; i++)
        {
            transform.localPosition = pos + new Vector3(0, Random.Range(-power,power+1), 0);
            yield return new WaitForSeconds(0.03f);
        }
        transform.position =pos;
    }

    private IEnumerator ShakeBeside(int power, int loop)
    {
        for (i=0;i<loop;i++)
        {
            transform.localPosition = pos + new Vector3(Random.Range(-power, power + 1), 0, 0);
            yield return new WaitForSeconds(0.03f);
        }
        transform.position = pos;
    }

    /*private IEnumerator Game()
    {

        transform.localEulerAngles = new Vector3(0, 0, GameData.Camera);
        for (; ; )
        {

            if (GameData.Camera > rot.z)
            {
                transform.localEulerAngles = new Vector3(0, 0, 2);
            }
            else if (GameData.Camera < rot.z)
            {
                transform.localEulerAngles -= new Vector3(0, 0, 2);
            }

            yield return new WaitForSeconds(0.01f);
        }
    }*/
}
