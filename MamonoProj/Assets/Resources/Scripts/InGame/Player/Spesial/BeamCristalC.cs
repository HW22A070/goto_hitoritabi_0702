using Move2DSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamCristalC : MonoBehaviour
{
    [SerializeField]
    private PMissile _prfbPRaserSP;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Shot());   
    }

    private IEnumerator Shot()
    {
        yield return new WaitForSeconds(1.2f);

        List<float> angles = GetAngleWithEnemy(transform.position);
        for (int i = 0; i < 10; i++)
        {

            foreach (float angle in angles)
            {
                PMissile shot2 = Instantiate(_prfbPRaserSP, transform.position, transform.rotation);
                shot2.ShotMissle(angle, 0, 2000);
                shot2.transform.position += shot2.transform.up * 640;
            }
            yield return new WaitForFixedUpdate();
        }
        for (int i = 0; i < 10; i++)
        {
            if (Random.Range(0, 3) == 0)
            {
                foreach (float angle in angles)
                {
                    PMissile shot2 = Instantiate(_prfbPRaserSP, transform.position, transform.rotation);
                    shot2.ShotMissle(angle, 0, 2000);
                    shot2.transform.position += shot2.transform.up * 640;
                }
            }

            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// ìGèÍèäì¡íËÅAé©ï™Ç∆ÇÃÉAÉìÉOÉãÇãÅÇﬂÇÈ
    /// </summary>
    /// <returns></returns>
    private List<float> GetAngleWithEnemy(Vector3 _posOwn)
    {
        GameObject[] myObjects;
        List<float> angles = new List<float> { };
        myObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obEnemy in myObjects)
        {
            Vector3 enemyPos = GameData.FixPosition(obEnemy.transform.position, 32, 32);
            angles.Add(Moving2DSystems.GetAngle(_posOwn, enemyPos));
        }

        myObjects = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject obEnemy in myObjects)
        {
            Vector3 enemyPos = GameData.FixPosition(obEnemy.transform.position, 32, 32);
            angles.Add(Moving2DSystems.GetAngle(_posOwn, enemyPos));
        }

        return angles;
    }
}
