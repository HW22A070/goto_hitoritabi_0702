using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseC : ETypeEelC
{
    private int deltaspeed=0;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;
    public AudioClip runS;

    // Start is called before the first frame update
    void Start()
    {
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();

        deltaspeed++;
        if (deltaspeed >= 30)
        {
            _moveSpeed = deltaspeed * 2;
            _audioGO.PlayOneShot(runS);
        }
    }
}
