using EnumDic.Enemy;
using EnumDic.Enemy.Virus;
using Move2DSystem;
using System.Collections;
using UnityEngine;

public class MailC : BossCoreC
{

    [SerializeField]
    private VirusC VirusPrefab;

    [SerializeField]
    private Sprite normal,_open;

    [SerializeField]
    private AudioClip mailS;

    [SerializeField]
    private ExpC _prfbVirusEf;

    protected override void FxUpFight()
    {
        if (transform.position.y > 120)
        {
            transform.localPosition += new Vector3(0, -1, 0);
        }
    }

    protected override void FxUpDead()
    {
        if (_eCoreC.CheckIsAlive())
        {
            _eCoreC.SetIsAlive(false);
            StartCoroutine(DeadAction());
        }
    }

    protected override IEnumerator ArrivalAction()
    {
        _audioGO.PlayOneShot(mailS);
        return base.ArrivalAction();
    }

    protected override IEnumerator DeadAction()
    {
        _srOwnBody.sprite = _open;

        Vector3 posVirus =new Vector3(Random.Range(100, GameData.WindowSize.x - 100), Random.Range(50, GameData.WindowSize.y - 50), 0);
        GameData.VirusBugEffectLevel = MODE_VIRUS.Large;

        float distance = Moving2DSystems.GetDistance(transform.position, posVirus);
        Vector3 angles = Moving2DSystems.GetDirection(Moving2DSystems.GetAngle(transform.position,posVirus)).normalized;

        for (int i = 0; i < 90; i++)
        {
            for(int j=0; j < 20; j++)
            {
                Instantiate(_prfbVirusEf, posVirus, _rotOwn).ShotEXP(Random.Range(0, 360), 10, 0.3f);
                Instantiate(_prfbVirusEf, transform.position + angles * Random.Range(0, distance), _rotOwn).ShotEXP(Random.Range(0, 360), 1, 0.3f);
            }

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.3f);

        GameData.VirusBugEffectLevel = MODE_VIRUS.None;

        Instantiate(VirusPrefab, posVirus, _rotOwn);
        Destroy(gameObject);
    }

}
