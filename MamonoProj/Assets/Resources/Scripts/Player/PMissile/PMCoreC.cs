using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMCoreC : MonoBehaviour
{
    [SerializeField]
    [Tooltip("敵弾と相殺するか")]
    private bool sosai;

    [SerializeField]
    [Tooltip("エフェクト")]
    private ExpC _invalidEP, _damageEP, _criticalEP,_criticalEP2;

    private Vector3 pos;

    private int ddd;

    private GameObject PlayerGO;

    [SerializeField]
    [Tooltip("効果音")]
    private AudioClip criticalS, damageS, invalidS;

    // Start is called before the first frame update
    void Start()
    {
        PlayerGO=GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageEffect(bool isCritical,Vector3 effectPos)
    {
        if (isCritical)
        {
            for (ddd = 20; ddd <= 60; ddd+=20)
            {
                Quaternion rot2 = transform.localRotation;
                ExpC shot2 = Instantiate(_criticalEP2, effectPos + new Vector3(Random.Range(-ddd, ddd), Random.Range(-ddd, ddd), 0), rot2);
                shot2.EShot1(Random.Range(0,360), 0.2f, 0.4f);
            }
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(criticalS);
            EffectSummon(_criticalEP, effectPos);
            CameraC.IsCriticalShake = true;
            PlayerGO.GetComponent<PlayerC>().CriticalVibration();
        }
        else
        {
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(damageS);
            EffectSummon(_damageEP, effectPos);
        }
    }

    public void InvalidEffect(Vector3 effectPos)
    {
        GameObject.FindObjectOfType<AudioSource>().PlayOneShot(invalidS);
        Vector3 direction2 = new Vector3(pos.x, pos.y, 0);
        for (ddd = 0; ddd < 4; ddd++)
        {
            Quaternion rot2 = transform.localRotation;
            ExpC shot2 = Instantiate(_invalidEP, effectPos+new Vector3(Random.Range(-32,32), Random.Range(-32, 32),0), rot2);
            shot2.EShot1(0, 0, 0.2f);
        }
        /*if (sosai)*/ Destroy(gameObject);
    }

    /// <summary>
    /// エフェクト発生
    /// </summary>
    /// <param name="damageE"></param>
    /// <param name="effectGenten"></param>
    private void EffectSummon(ExpC damageE, Vector3 effectGenten)
    {
        float angle2 = 0;
        Vector3 direction2 = new Vector3(pos.x, pos.y, 0);
        for (ddd = 0; ddd < 10; ddd++)
        {
            angle2 += 36;
            Quaternion rot2 = transform.localRotation;
            ExpC shot2 = Instantiate(damageE, effectGenten, rot2);
            shot2.EShot1(angle2, 10, 0.2f);
        }
        if (sosai) Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        pos = transform.position;
        if (collision.gameObject.tag == "Barrier")
        {
            Destroy(gameObject);
        }
    }
}
