using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Move2DSystem;

/// <summary>
/// クリオネの弾
/// </summary>
public class LoveC : MonoBehaviour
{
    private float _time = 1.0f;

    [SerializeField]
    private RaserC EMissile1Prefab;

    [SerializeField]
    private ExpC ExpPrefab;
    private Vector3 _posOwn;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;


    [SerializeField]
    private AudioClip loveS;
    private bool _isAudio;

    // Start is called before the first frame update
    void Start()
    {
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        _posOwn = transform.position;

        _time -= Time.deltaTime;

    }

    void FixedUpdate()
    {
        float angle = 270;
        Quaternion rot = transform.localRotation;

        if (_time > 0)
        {
            angle = Random.Range(0, 360);
            Instantiate(ExpPrefab, _posOwn - new Vector3(0, Random.Range(0, 480), 0), rot).ShotEXP(angle, 1, 0.3f);
        }
        else
        {
            Instantiate(EMissile1Prefab, _posOwn, rot).ShotRaser(angle,320);
            if (!_isAudio)
            {
                _audioGO.PlayOneShot(loveS);
                _isAudio = true;
            }

            Destroy(gameObject, 0.2f);

        }

    }
}
