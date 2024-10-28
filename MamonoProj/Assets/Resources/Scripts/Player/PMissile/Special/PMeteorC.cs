using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMeteorC : MonoBehaviour
{
    private Vector3 velocity, pos;
    private int i,j;

    private int mode = 0;

    public PExpC ExpPrefab;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    public AudioClip expS;

    public PBombC PDustP;
    public ExpC ExhaustP;
    private int Xday;

    private bool _isExp;

    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    public void EShot1(int xday)
    {
        Xday = xday;
        StartCoroutine("Game");

    }

    // Update is called once per frame
    private IEnumerator Game()
    {
        for (; ; )
        {
            pos = transform.position;

            if (mode == 0)
            {
                transform.localPosition += new Vector3(0, 10, 0);
                Quaternion rot2 = transform.localRotation;
                ExpC shot2 = Instantiate(ExhaustP, pos+new Vector3(0,-64,0), rot2);
                shot2.EShot1(Random.Range(260, 280), 10, 1.0f);
                if (pos.y > 600) mode = 1;
            }
            else if (1<=mode&&mode<Xday)
            {

                /*if (mode % 5 == 0)
                {
                    Quaternion rot = transform.localRotation;
                    PBombC shot = Instantiate(PDustP, new Vector3(Random.Range(10, 630), 479, 0), rot);
                    shot.EShot1(270 + Random.Range(-20, 20), 50, 0.02f, 50, 5, 1.0f);
                }*/
                mode++;
            }
            else if (mode>= Xday)
            {
                if (!_isExp)
                {
                    Quaternion rot2 = transform.localRotation;
                    PExpC shot2 = Instantiate(ExpPrefab, pos, rot2);
                    shot2.EShot1(Random.Range(70, 110), 10, 1.0f);
                    spriteRenderer.flipY = true;
                    transform.localPosition += new Vector3(0, -20, 0);
                    if (GetComponent<PMCoreC>().DeleteMissileCheck())
                    {
                        EXPEffect();
                    }
                }
            }

            
            yield return new WaitForSeconds(0.03f);
        }
    }

    private void EXPEffect()
    {
        if (!_isExp)
        {
            _isExp = true;
            StartCoroutine(Explosion(15));
        }
    }

    private IEnumerator Explosion(int hunj)
    {
        pos = transform.position;
        _audioGO.PlayOneShot(expS);
        for (int j = 0; j < hunj; j++)
        {
            for (int k = 0; k < 4; k++)
            {
                Vector3 direction2 = new Vector3(Random.Range(10, 630), Random.Range(10, 470), 0);
                Instantiate(ExpPrefab, pos, transform.localRotation).EShot1(Random.Range(0, 360), 10,3.0f);
            }

            yield return new WaitForSeconds(0.03f);
        }
        Destroy(gameObject);
    }
}
