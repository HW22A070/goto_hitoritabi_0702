using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantC : MonoBehaviour
{
    Vector3 velocity, pos;
    float aang,down,down2,delete,hb;
    int i, hirr;
    int tri = 0;

    public PlantC PlantPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void EShot1(float angle,float time,float deti,int hiro,float haba)
    {
        var direction = GetDirection(angle);
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        aang = angle;
        hb = haba;
        hirr = hiro;
        down = time;
        down2 = time;
        delete = deti;
    }

    public Vector3 GetDirection(float angle)
    {
        Vector3 direction = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0);
        return direction;
    }

    private void Update()
    {
        down -= Time.deltaTime;
        if (down <= 0 && tri == 0)
        {
            for (i = 0; i < hirr; i++)
            {
                pos = transform.position;
                Vector3 direction = pos + new Vector3(60, 60, 0);
                float angle = aang + Random.Range(-hb, hb);
                Quaternion rot = transform.localRotation;
                PlantC shot = Instantiate(PlantPrefab, direction, rot);
                shot.EShot1(angle, down2, delete, hirr, hb);
            }
            tri = 1;
        }
        if (down <= -delete)
        {
            Destroy(gameObject);
        }


        if (pos.y <= -50 || pos.y >= 700 || pos.x > 700 || pos.x < -50)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    private IEnumerator Game()
    {
        for (; ; )
        {
            

            yield return new WaitForSeconds(0.03f);
        }
    }
}
