using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaffRollC : MonoBehaviour
{
    private int i, j;

    private Vector3 pos;

    private GameObject playerGO;

    public ExpC ExpPrefab;
    public short BeamD = 2, BulletD = 1, FireD = 1, BombD = 5, ExpD = 2, RifleD = 4, MagicD = 3;

    private bool _isBug;

    [SerializeField]
    private AudioClip _sickS, _effectS;

    [SerializeField]
    private ExpC _virusEf;




    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.Find("Player");
        StartCoroutine(StaffrollUpper());
        //GameData.StageMovingAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;

        if (pos.y >= 200 && GameData.Difficulty >= 2&&!_isBug)
        {
            _isBug = true;
            StartCoroutine(VirusStart());
        }
    }

    void FixedUpdate()
    {
    }

    public void Summon(int judge)
    {
        GameData.Star =true;
    }

    private IEnumerator StaffrollUpper()
    {
        while (pos.y <500)
        {
            transform.localPosition += new Vector3(0, 2, 0);
            if (Random.Range(0, 40) ==0)
            {
                Firework();
            }
            yield return new WaitForSeconds(0.03f);
        }
        playerGO.GetComponent<PlayerC>().StageMoveAction();
        Destroy(gameObject);

    }

    private void Firework()
    {
        float angle2 = 0;
        Vector3 direction2 = new Vector3(Random.Range(10, 630), Random.Range(10, 470), 0);
        for (i = 0; i < 36; i++)
        {
            angle2 += 10;
            Quaternion rot2 = transform.localRotation;
            ExpC shot2 = Instantiate(ExpPrefab, direction2, rot2);
            shot2.EShot1(angle2, 10, 0.5f);
        }
        for (i = 0; i < 36; i++)
        {
            angle2 += 10;
            Quaternion rot2 = transform.localRotation;
            ExpC shot2 = Instantiate(ExpPrefab, direction2, rot2);
            shot2.EShot1(angle2, 4, 0.5f);
        }
        for (i = 0; i < 36; i++)
        {
            angle2 += 10;
            Quaternion rot2 = transform.localRotation;
            ExpC shot2 = Instantiate(ExpPrefab, direction2, rot2);
            shot2.EShot1(angle2, 1, 0.5f);
        }
    }

    private void VirusEf()
    {
        float angle = Random.Range(0, 360);
        Quaternion rot = transform.localRotation;
        ExpC shot = Instantiate(_virusEf, pos+new Vector3(Random.Range(-300, 300 + 1), Random.Range(-100, 100 + 1),0), rot);
        shot.EShot1(angle, 0.1f, 1.0f);
        transform.position = new Vector3(pos.x, 200, 0) + transform.up * Random.Range(-10, 10);
    }

    private IEnumerator VirusStart() {
        GameData.GoalRound = 35;
        GameObject.FindObjectOfType<AudioSource>().PlayOneShot(_sickS);
        StopCoroutine(StaffrollUpper());
        GameData.Star = false;
        GameData.EX = 1;
        GameData.VirusBugEffectLevel = 3;
        GameData.Score += 100000;
        for (j = 0; j < 33; j++)
        {
            for (int k = 0; k < 10; k++) VirusEf();
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(_effectS);
            yield return new WaitForSeconds(0.03f);
        }
        GameData.VirusBugEffectLevel = 0;
        Destroy(gameObject);
    }

    
}
