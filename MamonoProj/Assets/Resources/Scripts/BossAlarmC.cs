using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAlarmC : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _alarmSR;

    [SerializeField]
    private Sprite _alarmSp;

    [SerializeField]
    private InsectBossC InsectBossPrefab;
    [SerializeField]
    private UfoC UfoPrefab;
    [SerializeField]
    private VaneC VanePrefab;
    [SerializeField]
    private IcequeenC IcequeenPrefab;
    [SerializeField]
    private IfritC IfritPrefab;
    [SerializeField]
    private MechaZombieC MechaZombiePrefab;
    [SerializeField]
    private MailC MailPrefab;

    private string _bossName;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    [SerializeField]
    private AudioClip _alarmS;

    private AudioControlC _bgmManager;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// insect,ufo,vane,icequeen,ifrit,zombie,virus
    /// </summary>
    public void BossAlarmSummon(string bossName)
    {
        _bgmManager = GameObject.Find("BGMManager").GetComponent<AudioControlC>();
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        _bossName = bossName;
        GameData.TimerMoving = false;
        StartCoroutine(Alarm());
    }

    private IEnumerator Alarm()
    {
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
        _bgmManager.ChangeAudio(GameData.GetRoundNumber(), true, 1.0f);
        SummonAndDestroy();

    }


    private void SummonAndDestroy()
    {
        GameData.IsBossFight = true;
        Quaternion rot = Quaternion.Euler(0, 0, 0);
        switch (_bossName)
        {
            case "insect":
                Instantiate(InsectBossPrefab, new Vector3(64, Random.Range(2, 5) * 90, 0), rot);
                break;

            case "ufo":
                Instantiate(UfoPrefab, new Vector3(Random.Range(100, 540), 470, 0), rot);
                break;

            case "vane":
                Instantiate(VanePrefab, new Vector3(100, (Random.Range(0, 3) * 90) + 110, 0), rot);
                break;

            case "icequeen":
                Instantiate(IcequeenPrefab, new Vector3(100, (Random.Range(0, 3) * 90) + 110, 0), rot);
                break;

            case "ifrit":
                Instantiate(IfritPrefab, new Vector3(Random.Range(100, 540), Random.Range(50, 430), 0), rot);
                break;

            case "zombie":
                Instantiate(MechaZombiePrefab, new Vector3(640, 165, 0), rot);
                break;

            case "virus":
                Instantiate(MailPrefab, new Vector3(320, 550, 0), rot);
                break;
        }
        GameData.TimerMoving = true;
        Destroy(gameObject);
    }
}
