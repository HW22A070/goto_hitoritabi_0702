using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Move2DSystem;

public class BeamTurretC : ETypeTurretC
{
    [SerializeField]
    private RaserC _prfbRaser;

    [SerializeField]
    private ExpC _prfRaserDust;

    [SerializeField]
    private AudioClip shotS;

    [SerializeField]
    private Transform _tfBeam;

    // Update is called once per frame
    private new void Start()
    {
        base.Start();
        _probabilityDown = -1;
        StartCoroutine(ShotBeam());
    }

    private IEnumerator ShotBeam()
    {
        _isDownFloor = true;
        yield return new WaitForSeconds(0.1f);
        _isDownFloor = false;
        yield return new WaitForSeconds(0.5f);

        float angle = Random.Range(0,5)!=0? Random.Range(0.0f, 360.0f):Moving2DSystems.GetAngle(_posOwn, _posPlayer);

        yield return StartCoroutine(ChargeRaser(angle, _tfBeam));
        Quaternion rot = transform.localRotation;

        _audioGO.PlayOneShot(shotS);

        for(int i = 0; i < 10; i++)
        {
            Instantiate(_prfbRaser, _tfBeam.position, transform.rotation).ShotRaser(angle,480);
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(ShotBeam());

    }

    //”­ŽË€”õ
    private IEnumerator ChargeRaser(float angle, Transform nozzle)
    {
        Vector3 angles = Moving2DSystems.GetDirection(angle).normalized;
        for (int j = 0; j < 40; j++)
        {
            Vector3 posEnergySummon = nozzle.position;
            for (int k = 0; k < 512; k++)
            {
                posEnergySummon += angles * 48;

                Instantiate(_prfRaserDust, posEnergySummon, transform.rotation)
                    .ShotEXP(angle + Random.Range(0, 2) * 180, 8, 0.1f);

                if (GameData.CheckIsInWindow(posEnergySummon,0,0)) break;
            }

            yield return new WaitForSeconds(0.03f);
        }
        _audioGO.PlayOneShot(shotS);
    }
}
