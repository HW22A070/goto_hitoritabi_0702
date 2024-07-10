using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TargetC : MonoBehaviour
{
    
    [SerializeField]
    private  int Hp = 0;
    private Vector3 pos;
    private Quaternion rot;

    private bool death, deathStarted;

    [SerializeField]
    [Tooltip("被ダメージ")]
    private int BeamD, BulletD , FireD, BombD, ExpD ;

    [SerializeField]
    [Tooltip("クリティカル発生")]
    private bool _isBeamCritical , _isBulletCritical, _isFireCritical , _isBombCritical ;

    private GameObject[] _tutorialGO;

    private PMCoreC playerMissileP;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        rot = transform.localRotation;

        if (death && !deathStarted)
        {
            _tutorialGO = GameObject.FindGameObjectsWithTag("Tutorial");
            for (int i = 0; i < _tutorialGO.Length; i++) _tutorialGO[i].GetComponent<TutorialC>().GoTutorial();
            Destroy(gameObject);

        }
    }

    public void Changeritical(bool beamC, bool bulletC, bool fireC,bool bombC)
    {
        if (beamC)
        {
            GetComponent<SpriteRenderer>().color = new Color32(247, 139, 131,255);
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

    private void Damage(int hit, GameObject PMissile, bool isCritical, Vector3 attackPos)
    {
        Hp -= hit;
        if (hit > 0)
        {
            if (Hp <= 0)
            {

                    death = true;
            }

        }
        playerMissileP = PMissile.GetComponent<PMCoreC>();
        if (hit > 0)
        {
            playerMissileP.DamageEffect(isCritical, attackPos);
        }
        else
        {
            playerMissileP.InvalidEffect(attackPos);
        }
    }

    

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 hitPos = collision.ClosestPoint(this.transform.position);
        if (collision.gameObject.tag == "PBeam")
        {
            Damage(BeamD , collision.gameObject, _isBeamCritical , hitPos);
        }
        if (collision.gameObject.tag == "PBullet")
        {
            Damage(BulletD , collision.gameObject, _isBulletCritical , hitPos);
        }
        if (collision.gameObject.tag == "PFire")
        {
            Damage(FireD , collision.gameObject, _isFireCritical , hitPos);
        }
        if (collision.gameObject.tag == "PBomb")
        {
            Damage(BombD , collision.gameObject, _isBombCritical , hitPos);
        }
        if (collision.gameObject.tag == "PExp")
        {
            Damage(ExpD , collision.gameObject, _isBombCritical , hitPos);
        }
    }
}
