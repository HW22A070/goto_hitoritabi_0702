using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonC : ETypeDevilC
{
    private float shotdown = 1.0f;

    public HomingC RocketPrefab;
    
    public AudioClip shotS;

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (shotdown != 0) shotdown -= Time.deltaTime;
        if (shotdown <= 0 && tate == 0)
        {

            Quaternion rot = transform.localRotation;
            HomingC shot = Instantiate(RocketPrefab, pos, rot);
            _audioGO.PlayOneShot(shotS);
            shot.EShot1(90, 15, 300, 5, 0.5f, Random.Range(20, 30));
            shotdown = Random.Range(3, 7)*0.5f;
        }
    }
}
