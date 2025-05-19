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
        if (shotdown <= 0 && _vertical == 0)
        {

            Quaternion rot = transform.localRotation;
            HomingC shot = Instantiate(RocketPrefab, _posOwn, rot);
            _audioGO.PlayOneShot(shotS);
            shot.ShotHoming(90, 15, 300, 64, Random.Range(20, 30),_goPlayer);
            shotdown = Random.Range(3, 7)*0.5f;
        }
    }
}
