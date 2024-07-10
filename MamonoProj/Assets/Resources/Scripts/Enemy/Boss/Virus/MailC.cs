using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailC : MonoBehaviour
{
    GameObject GM;
    Vector3 pos, ppos, muki, velocity;
    Vector3 Aa;
    Quaternion rot;
    public ExpC Fire;
    public VirusC VirusPrefab;

    public SpriteRenderer spriteRenderer;
    public Sprite normal;

    public AudioClip mailS;

    [SerializeField]
    [Tooltip("PlayerGameObject")]
    private GameObject playerGO;

    /// <summary>
    /// 動作中のコルーチン
    /// </summary>
    private Coroutine _movingCoroutine;

    /// <summary>
    /// ECoreCのコンポーネント
    /// </summary>
    private ECoreC _eCoreC;


    // Start is called before the first frame update
    void Start()
    {

    }


    public void Summon(int judge)
    {
        _eCoreC = GetComponent<ECoreC>();
        pos = transform.position;
        VirusC.VirusMode = 0;
        GM = GameObject.Find("GameManager");
        GM.GetComponent<GameManagement>()._bossNowHp = _eCoreC.hp[0];
        GM.GetComponent<GameManagement>()._bossMaxHp = _eCoreC.hp[0];
        GameObject.FindObjectOfType<AudioSource>().PlayOneShot(mailS);
        _eCoreC.IsBoss = true;
        playerGO = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (_eCoreC.BossLifeMode != 2) GM.GetComponent<GameManagement>()._bossNowHp = _eCoreC.hp[0];

        pos = transform.position;
        ppos = playerGO.transform.position;

        //follow
        pos = transform.position;
        ppos = GameObject.Find("Player").transform.position;

    }


    void FixedUpdate()
    {

        //SummonAction
        if (_eCoreC.BossLifeMode == 0)
        {
            _eCoreC.BossLifeMode = 1;
        }


        //DeathAction
        if (_eCoreC.BossLifeMode == 1)
        {
            if (pos.y > 120)
            {
                transform.localPosition += new Vector3(0, -1, 0);
            }
        }


        //DeathAction
        if (_eCoreC.BossLifeMode == 2)
        {

            pos = new Vector3(Random.Range(100, 540), Random.Range(50, 430), 0);
            VirusC virus = Instantiate(VirusPrefab, pos, rot);
            virus.Summon(0);

            Destroy(gameObject);
        }
    }
}
