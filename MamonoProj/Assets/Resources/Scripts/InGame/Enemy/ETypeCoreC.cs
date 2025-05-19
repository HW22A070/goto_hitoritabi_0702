using UnityEngine;

/// <summary>
/// 敵キャラ動きベース
/// </summary>
public class ETypeCoreC : MonoBehaviour
{
    protected ECoreC _eCoreC;

    protected Vector3 _posOwn, _posPlayer;

    protected Quaternion _rotOwn;

    protected GameObject _goPlayer;

    protected Transform _tfOwnBody;

    protected SpriteRenderer _srOwnBody;

    protected AudioControlC _audioManager;

    /// <summary>
    /// カメラオブジェクト
    /// </summary>
    protected GameObject _goCamera;

    /// <summary>
    /// スピーカ
    /// </summary>
    protected AudioSource _audioGO;
    
    protected PlayersManagerC _scPlsM;

    // Start is called before the first frame update
    protected void Start()
    {
        _scPlsM = GameObject.Find("PlayersManager").GetComponent<PlayersManagerC>();
        ChangeTarget();
        _posOwn = transform.position;
        _posPlayer = _goPlayer.transform.position;

        DoGetComponent();
        //if(!_scECore.CheckIsBoss)
    }

    protected void DoGetComponent()
    {
        _audioManager = GameObject.Find("BGMManager").GetComponent<AudioControlC>();
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();

        _eCoreC = GetComponent<ECoreC>();
        _tfOwnBody = transform.Find("Body");

        _srOwnBody = _tfOwnBody.GetComponent<SpriteRenderer>();

        _goCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    protected void Update()
    {
        _posOwn = transform.position;
        _posPlayer = _goPlayer.transform.position;
        _rotOwn = transform.rotation;
    }

    protected void FixedUpdate()
    {
        CheckIsAliveAndChange();
    }

    /// <summary>
    /// ターゲットの生存確認＆死んでいたら変更
    /// </summary>
    protected void CheckIsAliveAndChange()
    {
        if (!_goPlayer.GetComponent<PlayerC>().CheckIsAlive())
        {
            ChangeTarget();
        }
    }

    /// <summary>
    /// ターゲット変更
    /// </summary>
    protected void ChangeTarget() => _goPlayer = _scPlsM.GetRandomAlivePlayer();

    /// <summary>
    /// 射程圏内にプレイヤーがいるか確認
    /// </summary>
    /// <returns></returns>
    protected bool CheckIsTargetingAnyPlayers(float range)
    {
        foreach (GameObject player in _scPlsM.GetAlivePlayers())
        {
            if (player.transform.position.y >= _posOwn.y - range && player.transform.position.y <= _posOwn.y + range)
            {
                //見つけたターゲットに上書き
                _goPlayer = player;
                return true;
            }
        }
        return false;
    }

    public virtual void GetDamageValue(int damage) { }
}
