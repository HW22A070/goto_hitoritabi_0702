using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoveC : MonoBehaviour
{
    float time = 1.0f;
    public EMissile1C EMissile1Prefab;
    public ExpC ExpPrefab;
    Vector3 pos;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    public AudioClip loveS;
    new bool audio;

    // Start is called before the first frame update
    void Start()
    {
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    public void EShot1()
    {
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;

        time -= Time.deltaTime;

    }

    void FixedUpdate()
    {
            if (time > 0)
            {
                float angle = 270;
                Quaternion rot = transform.localRotation;
                angle = Random.Range(0, 360);
                ExpC shot = Instantiate(ExpPrefab, pos-new Vector3(0,Random.Range(0,480),0), rot);
                shot.EShot1(angle, 1, 0.3f);
            }
            if (time <= 0)
            {
                float angle = 270;
                Quaternion rot = transform.localRotation;
                EMissile1C shot = Instantiate(EMissile1Prefab, pos, rot);
                shot.EShot1(angle, 80, 0);
                if (!audio)
                {
                    _audioGO.PlayOneShot(loveS);
                    audio = true;
                }

                Destroy(gameObject,0.2f);

            }

    }
}
