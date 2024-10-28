using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBeamC : MonoBehaviour
{
    private float shotdown = 0.5f;

    [SerializeField]
    private EMissile1C EMissile1Prefab;

    private Vector3 pos;

    /// <summary>
    /// スピーカ
    /// </summary>
    protected AudioSource _audioGO;

    [SerializeField]
    private AudioClip shotS;

    // Start is called before the first frame update
    protected void Start()
    {
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        pos = transform.position;

        if (0< pos.x && pos.x < GameData.WindowSize.x && 0< pos.y && pos.y < GameData.WindowSize.y)
        {

            if (shotdown != 0) shotdown -= Time.deltaTime;
            if (shotdown <= 0)
            {
                float angle = Random.Range(0.0f, 360.0f);
                Quaternion rot = transform.localRotation;
                pos.y += 14;

                EMissile1C shot = Instantiate(EMissile1Prefab, pos, rot);
                shot.EShot1(angle, 10, 0);
                shot.transform.position += shot.transform.up * 64;

                _audioGO.PlayOneShot(shotS);
                shotdown = Random.Range(0.5f, 2.0f);
            }
        }
    }
}
