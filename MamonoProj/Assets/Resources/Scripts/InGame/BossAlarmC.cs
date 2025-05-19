using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDic.Enemy;

public class BossAlarmC : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _alarmSR;

    [SerializeField]
    private Sprite _alarmSp;

    [SerializeField]
    private GameObject _prfbInsectBoss,_prfbUFO,_prfbVane,_prfbIceClione,_prfbIfrit,_prfbMecgaZombie,_prfbMail,_prfbNeonDragon;

    private string _bossName;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    [SerializeField]
    private AudioClip _alarmS;

    private AudioControlC _bgmManager;

    /// <summary>
    /// insect,ufo,vane,icequeen,ifrit,zombie,virus
    /// </summary>
    public void BossAlarmSummon(KIND_BOSS kind)
    {
        _bgmManager = GameObject.Find("BGMManager").GetComponent<AudioControlC>();
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        GameData.IsTimerMoving = false;
        StartCoroutine(Alarm(kind));
    }

    private IEnumerator Alarm(KIND_BOSS kind)
    {
        //アラート演出
        _bgmManager.VolumefeedInOut(3.0f, 0.0f);
        for(int hoge = 0; hoge < 3; hoge++)
        {
            _alarmSR.sprite = _alarmSp;
            _audioGO.PlayOneShot(_alarmS);
            yield return new WaitForSeconds(0.25f);
            yield return new WaitForSeconds(0.25f);
            _alarmSR.sprite = null;
            yield return new WaitForSeconds(0.25f);
        }
        _alarmSR.sprite = _alarmSp;
        _audioGO.PlayOneShot(_alarmS);

        yield return new WaitForSeconds(1.0f);
        _bgmManager.ChangeAudio((int)GameData.StageMode, true, 1.0f);
        SummonAndDestroy(kind);
    }


    private void SummonAndDestroy(KIND_BOSS kind)
    {
        GameData.IsBossFight = true;
        Quaternion rot = Quaternion.Euler(0, 0, 0);

        switch (kind)
        {
            case KIND_BOSS.InsectBoss:
                Instantiate(_prfbInsectBoss, new Vector3(0, 10000, 0), rot);
                break;

            case KIND_BOSS.UFO:
                Instantiate(_prfbUFO, new Vector3(Random.Range(100, 540), 470, 0), rot);
                break;

            case KIND_BOSS.Vane:
                Instantiate(_prfbVane, new Vector3(100, (Random.Range(0, 3) * 90) + 110, 0), rot);
                break;

            case KIND_BOSS.IceClione:
                Instantiate(_prfbIceClione, new Vector3(100, (Random.Range(0, 3) * 90) + 110, 0), rot);
                break;

            case KIND_BOSS.Ifrit:
                Instantiate(_prfbIfrit, new Vector3(Random.Range(100, 540), 640, 0), rot);
                break;

            case KIND_BOSS.MechaZombie:
                Instantiate(_prfbMecgaZombie, new Vector3(640, 165, 0), rot);
                break;

            case KIND_BOSS.MailVirus:
                _bgmManager.ChangeAudio(-5100, false, 1.0f);
                Instantiate(_prfbMail, new Vector3(320, 550, 0), rot);
                break;

            case KIND_BOSS.NeonDragon:
                Instantiate(_prfbNeonDragon, new Vector3(480, 550, 0), rot);
                break;
        }
        GameData.IsTimerMoving = true;
        Destroy(gameObject);
    }
}
