using EnumDic.Enemy;
using UnityEngine;

public class MailC : MonoBehaviour
{
    private GameObject _goGameManager;

    private Quaternion _rotOwn;

    [SerializeField]
    private VirusC VirusPrefab;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite normal;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;


    [SerializeField]
    private AudioClip mailS;

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
        GetComponents();
        
        _goGameManager.GetComponent<GameManagement>()._bossNowHp = _eCoreC.hp[0];
        _goGameManager.GetComponent<GameManagement>()._bossMaxHp = _eCoreC.hp[0];
        _audioGO.PlayOneShot(mailS);
        _eCoreC.IsBoss = true;
    }

    private void GetComponents()
    {
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        playerGO = GameObject.Find("Player");
        _eCoreC = GetComponent<ECoreC>();
        _goGameManager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (_eCoreC.BossLifeMode != MODE_LIFE.Dead) _goGameManager.GetComponent<GameManagement>()._bossNowHp = _eCoreC.hp[0];
    }


    void FixedUpdate()
    {
        //SummonAction
        if (_eCoreC.BossLifeMode == 0)
        {
            _eCoreC.BossLifeMode = MODE_LIFE.Fight;
        }


        //DeathAction
        if (_eCoreC.BossLifeMode == MODE_LIFE.Fight)
        {
            if (transform.position.y > 120)
            {
                transform.localPosition += new Vector3(0, -1, 0);
            }
        }


        //DeathAction
        if (_eCoreC.BossLifeMode == MODE_LIFE.Dead)
        {
            Instantiate(VirusPrefab, new Vector3(Random.Range(100, 540), Random.Range(50, 430), 0), _rotOwn);

            Destroy(gameObject);
        }
    }
}
