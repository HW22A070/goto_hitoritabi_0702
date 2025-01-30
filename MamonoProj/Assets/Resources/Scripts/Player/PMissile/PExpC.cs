using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PExpC : MonoBehaviour
{
    Vector3 velocity, pos;
    float sspeed, kkaso, aang;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void EShot1(float angle, float speed,float delete)
    {
        var direction = GameData.GetDirection(angle);
        velocity = direction * speed;
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;
        Destroy(gameObject,delete);

        StartCoroutine("Game");

    }

    // Update is called once per frame
    private IEnumerator Game()
    {
        for (; ; )
        {
            pos = transform.position;
            transform.localPosition += velocity;

            if (GetComponent<PMCoreC>().DeleteMissileCheck())
            {
                Destroy(gameObject);
            }

            yield return new WaitForSeconds(0.03f);
        }
    }

}
