using EnumDic.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TargetC : MonoBehaviour
{

    [SerializeField]
    private int Hp = 0;
    private Quaternion rot;

    private bool death, deathStarted;

    [SerializeField]
    [Tooltip("被ダメージ")]
    private int BeamD, BulletD, FireD, BombD, ExpD;

    [SerializeField]
    [Tooltip("クリティカル発生")]
    private bool _isBeamCritical, _isBulletCritical, _isFireCritical, _isBombCritical;

    private GameObject[] _tutorialGO;

    private PMCoreC playerMissileP;

    [SerializeField]
    [Tooltip("エフェクト")]
    private ExpC _invalidEP, _damageEP, _criticalEP, _criticalEP2;

    [SerializeField]
    [Tooltip("効果音")]
    private AudioClip criticalS, damageS, invalidS;

    private PlayersManagerC _scPlsM;

    private AudioSource _audioGO;

    private void Start()
    {
        _scPlsM = GameObject.Find("PlayersManager").GetComponent<PlayersManagerC>();
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        rot = transform.localRotation;

        if (death && !deathStarted)
        {
            _tutorialGO = GameObject.FindGameObjectsWithTag("Tutorial");
            for (int i = 0; i < _tutorialGO.Length; i++) _tutorialGO[i].GetComponent<TutorialC>().GoToTutorial();
            Destroy(gameObject);

        }

         //_scPlsM.GetRandomAlivePlayer().transform.position;
    }

    public void Changeritical(bool beamC, bool bulletC, bool fireC, bool bombC)
    {
        if (beamC)
        {
            GetComponent<SpriteRenderer>().color = new Color32(247, 139, 131, 255);
            _isBeamCritical = true;
        }
        else BeamD = 0;
        if (bulletC)
        {
            GetComponent<SpriteRenderer>().color = new Color32(169, 171, 94, 255);
            _isBulletCritical = true;
        }
        else BulletD = 0;
        if (fireC)
        {
            GetComponent<SpriteRenderer>().color = new Color32(82, 183, 174, 255);
            _isFireCritical = true;
        }
        else FireD = 0;
        if (bombC)
        {

            GetComponent<SpriteRenderer>().color = new Color32(139, 144, 189, 255);
            _isBombCritical = true;
        }
        else
        {
            BombD = 0;
            ExpD = 0;
        }
    }

    private bool Damage(int hit, bool isCritical, Vector3 attackPos)
    {
        bool damage = hit > 0;
        Hp -= hit;
        //エフェクト発生
        if (!damage)
        {
            InvalidEffect(attackPos);
        }
        else
        {
            DamageEffect(isCritical, attackPos);
        }
        if (damage)
        {
            if (Hp <= 0)
            {
                death = true;
            }

        }

        return damage;
    }

    public bool HitTarget(MODE_GUN weapon, Vector3 hitPos)
    {
        Debug.Log("Hit!");
        bool isDamage = false;

        switch (weapon)
        {
            case MODE_GUN.Shining:
                isDamage = Damage(BeamD, _isBeamCritical, hitPos);
                break;
            case MODE_GUN.Physical:
                isDamage = Damage(BulletD, _isBulletCritical, hitPos);
                break;
            case MODE_GUN.Crash:
                isDamage = Damage(FireD, _isFireCritical, hitPos);
                break;
            case MODE_GUN.Heat:
                isDamage = Damage(BombD, _isBombCritical, hitPos);
                break;
        }

        return isDamage;
    }

    private void DamageEffect(bool isCritical, Vector3 effectPos)
    {
        if (isCritical)
        {
            for (int ddd = 20; ddd <= 60; ddd += 20)
            {
                Quaternion rot2 = transform.localRotation;
                ExpC shot2 = Instantiate(_criticalEP2, effectPos + new Vector3(Random.Range(-ddd, ddd), Random.Range(-ddd, ddd), 0), rot2);
                shot2.ShotEXP(Random.Range(0, 360), 0.2f, 0.4f);
            }
            _audioGO.PlayOneShot(criticalS);
            EffectSummon(_criticalEP, effectPos);
           // GameObject.Find("Player").GetComponent<PlayerC>().VibrationCritical();
        }
        else
        {
            _audioGO.PlayOneShot(damageS);
            EffectSummon(_damageEP, effectPos);
        }
    }

    public void InvalidEffect(Vector3 effectPos)
    {
        _audioGO.PlayOneShot(invalidS);
        Vector3 direction2 = new Vector3(transform.position.x, transform.position.y, 0);
        for (int ddd = 0; ddd < 4; ddd++)
        {
            Quaternion rot2 = transform.localRotation;
            ExpC shot2 = Instantiate(_invalidEP, effectPos + new Vector3(Random.Range(-32, 32), Random.Range(-32, 32), 0), rot2);
            shot2.ShotEXP(0, 0, 0.2f);
        }
    }

    /// <summary>
    /// エフェクト発生
    /// </summary>
    /// <param name="damageE"></param>
    /// <param name="effectGenten"></param>
    private void EffectSummon(ExpC damageE, Vector3 effectGenten)
    {
        float angle2 = 0;
        Vector3 direction2 = new Vector3(transform.position.x, transform.position.y, 0);
        for (int ddd = 0; ddd < 10; ddd++)
        {
            angle2 += 36;
            Quaternion rot2 = transform.localRotation;
            ExpC shot2 = Instantiate(damageE, effectGenten, rot2);
            shot2.ShotEXP(angle2, 10, 0.2f);
        }
    }
}
