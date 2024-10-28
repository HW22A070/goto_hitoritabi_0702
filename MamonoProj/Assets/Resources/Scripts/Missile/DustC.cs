using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustC : MonoBehaviour
{
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void EShot1()
    {
        Destroy(gameObject,0.5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos = transform.position;
        transform.localEulerAngles = new Vector3(0, 0, -GameData.WindSpeed);
        transform.position += new Vector3(GameData.WindSpeed / 5, 0, 0);
        if (pos.y < 0 || pos.y > 480 || pos.x > 790 || pos.x < -150)
        {
            Destroy(gameObject);
        }
    }
}
