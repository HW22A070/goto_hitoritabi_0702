using UnityEngine;

/// <summary>
/// 敵キャラ動きベース
/// </summary>
public class ETypeCoreC : MonoBehaviour
{
    protected Vector3 _posOwn, _posPlayer;

    protected GameObject _goPlayer;

    protected SpriteRenderer spriteRenderer;

    /// <summary>
    /// スピーカ
    /// </summary>
    protected AudioSource _audioGO;

    // Start is called before the first frame update
    protected void Start()
    {
        _goPlayer = GameObject.Find("Player");
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    protected void Update()
    {
        _posOwn = transform.position;
        _posPlayer = _goPlayer.transform.position;
    }
}
