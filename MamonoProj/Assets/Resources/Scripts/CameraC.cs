using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraC : MonoBehaviour
{
    Quaternion rot;
    int kka = 0;
    public int i;
    Vector3 pos;

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

        if (IsCriticalShake)
        {
            StartCoroutine("CriticalShake");
            transform.localPosition = pos;
            IsCriticalShake = false;
        }

        if (IsDamageShake)
        {
            StartCoroutine("DamageShake");
            transform.localPosition = pos;
            IsDamageShake = false;
        }
    }

    private IEnumerator CriticalShake()
    {
        for (i = 0; i < 10; i++)
        {
            transform.localPosition = pos + new Vector3(0, Random.Range(-3,4), 0);
            yield return new WaitForSeconds(0.03f);
        }
        transform.localPosition =pos;
    }

    private IEnumerator DamageShake()
    {
        for (i = 0; i < 15; i++)
        {
            transform.localPosition =pos+ new Vector3(Random.Range(-3, 4),0 , 0);
            yield return new WaitForSeconds(0.03f);
        }
        transform.localPosition = pos;
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
