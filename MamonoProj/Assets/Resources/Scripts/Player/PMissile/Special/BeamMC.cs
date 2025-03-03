using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeamMC : MonoBehaviour
{
    private Vector3 _posOwn, fpos;

    private float of,shotdown,sddelta=0.35f;
    private float sp = 0;

    public int geigeki = 0;

    public PMissile PBeamP,PRaserP;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    public AudioClip shotS;

    private GameObject Pl,_playerGO;

    private bool Fireing =true;

    [SerializeField, Tooltip("ビームballアタッチ")]
    private PFireBallC _prfbBeamBoll;

    // Start is called before the first frame update
    void Start()
    {
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        _playerGO = GameObject.Find("Player");
    }

    void Update()
    {
        fpos = new Vector3(Mathf.Sin((sp + of) * Mathf.Deg2Rad) * 32, Mathf.Cos((sp + of) * Mathf.Deg2Rad) * 32, 0);
        _posOwn = transform.position;

        if (shotdown != 0) shotdown -= Time.deltaTime;
        if (Fireing && shotdown <= 0) Shot_Beam();
    }

    /*//input
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started) Fireing = true;
        else if (context.canceled) Fireing = false;
    }*/

    public void EShot1(float offset)
    {
        of = offset;
        Destroy(gameObject,10);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localPosition = fpos;

        sp += 10;
        while (sp > 360)
        {
            sp -= 360;
        }
    }

    //beam
    private void Shot_Beam()
    {
        sddelta -= 0.01f;
        if (sddelta < 0.09f)
        {
            Instantiate(_prfbBeamBoll, _posOwn, transform.rotation).SetFirstPos(_posOwn + GameData.GetDirection(-(sp + of) % 360) * 64);
            /*
            Quaternion rot = transform.localRotation;
            PMissile shot = Instantiate(PRaserP, _posOwn, rot);
            shot.Shot(sddelta, 10, 1);
            */
            sddelta = 0.06f;
        }
        else
        {
            Instantiate(_prfbBeamBoll, _posOwn, transform.rotation).SetFirstPos(_posOwn+GameData.GetDirection(-(sp +of) % 360) * 64);
            /*
            Quaternion rot = transform.localRotation;
            PMissile shot = Instantiate(PRaserP, _posOwn, rot);
            shot.Shot(sddelta, 20, 1);
            */
        }
        if (of == 0)
        {
            _audioGO.PlayOneShot(shotS);
        }

        shotdown = sddelta;
    }
}
