using EnumDic.Enemy.Virus;
using EnumDic.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// クリア演出制御
/// </summary>
public class ClearEffectC : MonoBehaviour
{
    private Vector3 _posOwn;

    private GameObject _goPlayer;

    [SerializeField]
    private ExpC ExpPrefab;

    private bool _isBug;

    [SerializeField]
    private AudioClip _sickS, _effectS;

    [SerializeField]
    private ExpC _virusEf;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;


    // Start is called before the first frame update
    void Start()
    {
        GameData.IsInvincible = true;
        GetComponents();
        StartCoroutine(MoveUpper());
    }

    private void GetComponents()
    {
        _goPlayer = GameObject.Find("Player");
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        _posOwn = transform.position;

        //エクストラステージ開始
        if (_posOwn.y >= 200 
            && GameData.CheckIsUpperDifficulty(MODE_DIFFICULTY.Assault)
            &&GameData.StartRound==1
            && GameData.GoalRound== 30 
            && !_isBug)
        {
            _isBug = true;
            StartCoroutine(VirusStart());
        }
    }

    /// <summary>
    /// 画面外まで上昇
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveUpper()
    {
        while (_posOwn.y <500)
        {
            transform.localPosition += new Vector3(0, 4, 0);
            if (Random.Range(0, 40) ==0)
            {
                Firework();
            }
            yield return new WaitForSeconds(0.03f);
        }
        _goPlayer.GetComponent<PlayerC>().StageMoveAction();
        Destroy(gameObject);

    }

    /// <summary>
    /// 花火演出
    /// </summary>
    private void Firework()
    {
        float angle2 = 0;
        Vector3 direction2 = new Vector3(Random.Range(10, 630), Random.Range(10, 470), 0);
        for (int i = 0; i < 36; i++)
        {
            angle2 += 10;
            Quaternion rot2 = transform.localRotation;
            ExpC shot2 = Instantiate(ExpPrefab, direction2, rot2);
            shot2.EShot1(angle2, 10, 0.5f);
        }
        for (int i = 0; i < 36; i++)
        {
            angle2 += 10;
            Quaternion rot2 = transform.localRotation;
            ExpC shot2 = Instantiate(ExpPrefab, direction2, rot2);
            shot2.EShot1(angle2, 4, 0.5f);
        }
        for (int i = 0; i < 36; i++)
        {
            angle2 += 10;
            Quaternion rot2 = transform.localRotation;
            ExpC shot2 = Instantiate(ExpPrefab, direction2, rot2);
            shot2.EShot1(angle2, 1, 0.5f);
        }
    }

    /// <summary>
    /// ウィルス演出
    /// </summary>
    private void VirusEf()
    {
        float angle = Random.Range(0, 360);
        Quaternion rot = transform.localRotation;
        Instantiate(_virusEf, _posOwn+new Vector3(Random.Range(-300, 300 + 1), Random.Range(-100, 100 + 1),0), rot).EShot1(angle, 0.1f, 1.0f);

        //痙攣
        transform.position = new Vector3(_posOwn.x, 200, 0) + transform.up * Random.Range(-10, 10);
    }

    /// <summary>
    /// EXステージ開始
    /// </summary>
    /// <returns></returns>
    private IEnumerator VirusStart() {
        GameData.GoalRound = 35;
        _audioGO.PlayOneShot(_sickS);
        StopCoroutine(MoveUpper());

        GameData.IsInvincible = false;
        GameData.EX = 1;
        GameData.VirusBugEffectLevel = MODE_VIRUS.Medium;
        GameData.Point += 100000;
        for (int j = 0; j < 33; j++)
        {
            for (int k = 0; k < 10; k++) VirusEf();
            _audioGO.PlayOneShot(_effectS);
            yield return new WaitForSeconds(0.03f);
        }
        GameData.VirusBugEffectLevel = MODE_VIRUS.None;
        Destroy(gameObject);
    }

    
}
