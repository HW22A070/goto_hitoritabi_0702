using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerC : MonoBehaviour
{
    [SerializeField,Tooltip("画像元")]
    private SpriteRenderer _spriteRPlayer, _spriteRWeaponPlate, _spriteRWeaponPlate2;

    [SerializeField, Tooltip("画像データ")]
    private Sprite[] _playerGurNormal, _playerGurWark,_playerGurNormalAttack,_playerGurWarkAttack;

    [SerializeField, Tooltip("プレートGUI")]
    private Sprite[] platesp;


    private bool moveL, moveR, Fireing, Fireing2, Jumping, Downing, SP1ing, SP2ing;

    /// <summary>
    /// 00=left,01=right;
    /// </summary>
    public static int muki = 0;

    /// <summary>
    /// プレイヤーの速さ
    /// </summary>
    private float pfast = 0;
    private float pjump = 0;
    private bool warking = false;
    private int warktime = 0;
    private int jump, pull, tate, dan;
    private Vector3 pos,_shotPos;

    private Quaternion rot;

    private float pmspeed = 0;
    private  float tpwdown = 0;

    private  float[] shotdown = { 0, 0, 0, 0, 0, 0, 0, 0 };
    private  float warkcount = 0;
    private float startime = 0;

    private float wh = 0;
    private float GunMode = 0.1f;
    private int plnum = 0;

    /// <summary>
    /// 遠距離武器がロードされているか
    /// </summary>
    private bool[] _isLoaded= { true, true, true, true};

    /// <summary>
    /// 横移動最大値
    /// </summary>
    private float _moveMax = 8;

    [SerializeField,Tooltip("弾アタッチ")]
    private PMissile PBeamP, PRaserP, PBulletP, PRifleP;

    [SerializeField, Tooltip("弾アタッチ、")]
    private PBombC PBombP, PMineP, PMinecristalP;

    [SerializeField, Tooltip("弾アタッチ")]
    private PExpC PFireP, PSlashP, PBulletExpP;

    [SerializeField, Tooltip("弾アタッチ")]
    private EMissile1C TPwazaPrefab;

    [SerializeField, Tooltip("エフェクトアタッチ")]
    private ExpC ExpPrefab, DamagePrefab, HealEffectPrefab, BDE;

    [SerializeField, Tooltip("弾アタッチ")]
    private BeamMC BeamMP;

    [SerializeField, Tooltip("弾アタッチ")]
    private PMachineGunC PMachinegunP;

    [SerializeField, Tooltip("弾アタッチ")]
    private PMeteorC PMeteorP;

    public object gameobject { get; private set; }

    [SerializeField, Tooltip("サウンド")]
    private AudioClip shotS, healS, damageS, magicgetS, magicuseS, exprosionS, bulletS, putS, fireS, ChangeS, SlashS;

    [SerializeField, Tooltip("ロードサウンド")]
    private AudioClip[] _loadS;

    private int i;


    [SerializeField,Tooltip("スピーカ")]
    private AudioSource _audioGO;

    [SerializeField, Header("Effect")]
    private SpriteRenderer _spriteREffect;

    [SerializeField, Tooltip("プレートGUI")]
    private Sprite[] _effectGur;


    // Start is called before the first frame update
    void Start()
    {
        _audioGO = FindObjectOfType<AudioSource>();
        var gamepad = Gamepad.current;
        jump = 0;
        pull = 0;
        dan = 1;
        _spriteRPlayer = GetComponent<SpriteRenderer>();
        _spriteRWeaponPlate.sprite = platesp[plnum];
        StartCoroutine(EffectAnim());
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        _shotPos = pos - (transform.up * 4);
        rot = transform.localRotation;

        if (GameData.PlayerMoveAble>=1)
        {
            //Bound
            if (pos.x > 624)
            {
                MoveBound(new Vector3(624, pos.y, 0));
            }
            if (pos.x < 16)
            {
                MoveBound(new Vector3(16, pos.y, 0));
            }
        }
        for (i = 0; i < 8; i++)
        {
            if (shotdown[i] != 0) shotdown[i] -= Time.deltaTime;
        }


        //GunModeChange
        GunMode += Input.GetAxis("Mouse ScrollWheel") * 5;
        //GunMode = wh;
        if (GunMode <= 0.0f) GunMode = platesp.Length-0.1f;
        if (GunMode >= platesp.Length) GunMode = 0.1f;
        _spriteRWeaponPlate.sprite = platesp[(int)GunMode];
        _spriteRWeaponPlate2.sprite = platesp[(int)GunMode];

        //if (wh < 0) wh = 15.0f;
        //if (wh > 15) wh = 0.0f;

        //Magic
        if (tpwdown > 0)
        {
            tpwdown -= Time.deltaTime;
        }

        //StarTime
        if (startime != 0) startime -= Time.deltaTime;

        //moveauto
        if (moveL) MoveLeft(2);
        else if (moveR) MoveRight(2);
        else MoveStop();
        if (Jumping) MoveJump();
        if (Downing) MoveDown();
        if (Fireing) Fire();
        if (Fireing2) Fire2();

        if (SP2ing && SP2ing && tpwdown <= 0 && GameData.TP > 0) Magic();

        //ロードされたらSE
        for (i = 0; i < 4; i++)
        {
            if (shotdown[i*2] <= 0&&!_isLoaded[i])
            {
                _audioGO.PlayOneShot(_loadS[i]);
                _isLoaded[i] = true;
            }
        }

    }

    //Imput
    public void OnMoveL(InputAction.CallbackContext context)
    {
        if (context.started) moveL = true;
        else if (context.canceled) moveL = false;
    }
    public void OnMoveR(InputAction.CallbackContext context)
    {
        if (context.started) moveR = true;
        else if (context.canceled) moveR = false;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started) Jumping = true;
        else if (context.canceled) Jumping = false;
    }
    public void OnDown(InputAction.CallbackContext context)
    {
        if (context.started) Downing = true;
        else if (context.canceled) Downing = false;
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started) Fireing = true;
        else if (context.canceled) Fireing = false;
    }
    public void OnFire2(InputAction.CallbackContext context)
    {
        if (context.started) Fireing2 = true;
        else if (context.canceled) Fireing2 = false;
    }
    public void OnSuperFire(InputAction.CallbackContext context)
    {
        if (context.performed && tpwdown <= 0 && GameData.TP > 0) Magic();
    }
    public void GunModeChangeUp(InputAction.CallbackContext context)
    {
        if (context.performed&&GameData.PlayerMoveAble>=5)
        {
            GunMode++;
            _audioGO.PlayOneShot(ChangeS);
            _spriteRPlayer.sprite = _playerGurNormal[(int)GunMode];
        }
    }
    public void GunModeChangeDown(InputAction.CallbackContext context)
    {
        if (context.performed && GameData.PlayerMoveAble >= 5)
        {
            GunMode--;
            _audioGO.PlayOneShot(ChangeS);
            _spriteRPlayer.sprite = _playerGurNormal[(int)GunMode];
        }
    }
    public void GunModeChangeBeam(InputAction.CallbackContext context)
    {
        if (context.performed && GameData.PlayerMoveAble >= 5) GunModeChange(0.5f);
    }
    public void GunModeChangeBullet(InputAction.CallbackContext context)
    {
        if (context.performed && GameData.PlayerMoveAble >= 5) GunModeChange(1.5f);
    }
    public void GunModeChangeRocket(InputAction.CallbackContext context)
    {
        if (context.performed && GameData.PlayerMoveAble >= 5) GunModeChange(2.5f);
    }
    public void GunModeChangeMine(InputAction.CallbackContext context)
    {
        if (context.performed && GameData.PlayerMoveAble >= 5) GunModeChange(3.5f);
    }
    public void GunModeChange(float modevalue)
    {
        GunMode = modevalue;
        _audioGO.PlayOneShot(ChangeS);
        if(_isLoaded[(int)GunMode])_audioGO.PlayOneShot(_loadS[(int)GunMode]);
        _spriteRPlayer.sprite = _playerGurNormal[(int)GunMode];
    }
    public void SP1(InputAction.CallbackContext context)
    {
        if (context.started) SP1ing = true;
        else if (context.canceled) SP1ing = false;
    }
    public void SP2(InputAction.CallbackContext context)
    {
        if (context.started) SP2ing = true;
        else if (context.canceled) SP2ing = false;
    }



    private void FixedUpdate()
    {

        //MoveAction
        transform.localPosition += new Vector3(pfast, 0, 0);

        //JumpAction
        if (jump > 0)
        {
            if (dan < 5)
            {
                jump++;
                pjump -= 4f;
                transform.localPosition += new Vector3(0, 9 + pjump, 0);
                if (jump > 10)
                {
                    jump = 0;
                    tate = 0;
                    dan += 1;
                }
            }
            else if (dan == 5)
            {
                jump++;
                pjump -= 4f;
                transform.localPosition += new Vector3(0, pjump, 0);
                if (jump > 10)
                {
                    jump = 0;
                    tate = 0;
                }
            }
        }
        //JumpCharge
        if (pull > 0)
        {
            pull++;
            transform.localPosition += new Vector3(0, -10, 0);
            if (pull > 9)
            {
                pull = 0;
                tate = 0;
            }
        }

        //TPEffect
        //if (GameData.TP >= 1) TPEffect();
        //else Gui.sprite = gn;

        //WindowAttackJump
        if (tate == 0) transform.position += new Vector3(GameData.WindSpeed / 10, 0, 0);
        else if (tate == 1) transform.position += new Vector3(GameData.WindSpeed / 5, 0, 0);


        //graphic
        //ジャンプ中は歩きGur
        if (warktime < 4) warktime++;
        if (tate == 1)
        {
            _spriteRPlayer.sprite = _playerGurWark[(int)GunMode];
        }
        else if (warking == true)
        {
            if (warktime > 3)
            {
                if (_spriteRPlayer.sprite == _playerGurNormal[(int)GunMode]) _spriteRPlayer.sprite = _playerGurWark[(int)GunMode];
                else if (_spriteRPlayer.sprite == _playerGurWark[(int)GunMode]) _spriteRPlayer.sprite = _playerGurNormal[(int)GunMode];
                warktime = 0;
            }
        }
        else
        {
            _spriteRPlayer.sprite = _playerGurNormal[(int)GunMode];
        }

        /*
        //攻撃時
        else
        {
            if (tate == 1)
            {
                _spriteRPlayer.sprite =_playerGurWarkAttack[(int)GunMode];
            }
            else if (warking == true)
            {
                if (warktime > 3)
                {
                    if (_spriteRPlayer.sprite ==_playerGurNormalAttack[(int)GunMode])
                    {
                        _spriteRPlayer.sprite =_playerGurWarkAttack[(int)GunMode];
                    }
                    else if (_spriteRPlayer.sprite ==_playerGurWarkAttack[(int)GunMode])
                    {
                        _spriteRPlayer.sprite =_playerGurNormalAttack[(int)GunMode];
                    }
                    warktime = 0;
                }

                warktime++;
            }
            else if (tate == 0)
            {
                _spriteRPlayer.sprite =_playerGurNormalAttack[(int)GunMode];
            }
        }
        */

    }


    private IEnumerator EffectAnim()
    {
        
        for(int hoge = 0; hoge < 4; hoge++)
        {
            if (shotdown[(int)GunMode*2] <= 0/* &&shotdown[((int)GunMode*2)+1] <= 0*/)
            {
                _spriteREffect.sprite = _effectGur[hoge + ((int)GunMode * 4)];
            }
            else _spriteREffect.sprite = _effectGur[16];

            yield return new WaitForSeconds(0.03f);
            if(shotdown[((int)GunMode * 2) + 1] <= 0) yield return new WaitForSeconds(0.07f);
        }
        StartCoroutine(EffectAnim());
    }

    //Collision
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Heal
        if (collision.gameObject.tag == "Heal" && (GameData.HP < GameManagement.maxhp||GameData.StageMovingAction))
        {
            Heal(3);
            if (GameData.GameMode == 1) Heal(4);
            Destroy(collision.gameObject);
        }

        //Magic
        if (collision.gameObject.tag == "Magic"&&( GameData.TP < 3 || GameData.StageMovingAction))
        {
            _audioGO.PlayOneShot(magicgetS);
            if(GameData.TP<3)GameData.TP += 1;
            Destroy(collision.gameObject);
        }

        if (startime <= 0 && !GameData.Star)
        {
            if (collision.gameObject.tag == "Enemy1" || collision.gameObject.tag == "EK1")
            {
                EDamage(1);
                startime = 0.5f;
            }
            if (collision.gameObject.tag == "Enemy2" || collision.gameObject.tag == "EK2")
            {
                EDamage(2);
                startime = 0.5f;
            }
            if (collision.gameObject.tag == "Enemy3" || collision.gameObject.tag == "EK3")
            {
                EDamage(3);
                startime = 0.5f;
            }
            if (collision.gameObject.tag == "Enemy4")
            {
                EDamage(4);
                startime = 0.5f;
            }
            if (collision.gameObject.tag == "Enemy6")
            {
                EDamage(6);
                startime = 0.5f;
            }
            if (collision.gameObject.tag == "EK10")
            {
                EDamage(10);
                startime = 0.5f;
            }
            if (collision.gameObject.tag == "EM1")
            {
                MDamage(1, collision.gameObject);
                startime = 0.5f;
            }
            if (collision.gameObject.tag == "EM2")
            {
                MDamage(2, collision.gameObject);
                startime = 0.5f;
            }
            if (collision.gameObject.tag == "EM3")
            {
                MDamage(3, collision.gameObject);
                startime = 0.5f;
            }
            if (collision.gameObject.tag == "EM10")
            {
                MDamage(10, collision.gameObject);
                startime = 0.5f;
            }
            if (collision.gameObject.tag == "Bullet1")
            {
                MDamage(1, collision.gameObject);
            }
            if (collision.gameObject.tag == "Barrier")
            {
                MDamage(1, collision.gameObject);
                startime = 0.1f;
            }
        }
    }

    public void CriticalVibration()
    {
        if(GameData.IsVibration)StartCoroutine("CriticalControllerVibration");
    }

    private IEnumerator CriticalControllerVibration()
    {
        var gamepad = Gamepad.current;

        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(1.0f, 1.0f);
            yield return new WaitForSeconds(0.1f);
            gamepad.SetMotorSpeeds(0.0f, 0.0f);
        }
    }

    private IEnumerator DamageControllerVibration()
    {
        var gamepad = Gamepad.current;

        if (GameData.IsVibration)
        {
            if (gamepad != null)
            {
                gamepad.SetMotorSpeeds(0.8f, 0.8f);
                yield return new WaitForSeconds(0.05f);
                gamepad.SetMotorSpeeds(0.0f, 0.0f);
            }
        }
    }

    private void MoveRight(float mo)
    {
        if (GameData.PlayerMoveAble>=1)
        {
            warking = true;
            muki = 1;
            _spriteREffect.flipX = true;
            _spriteRPlayer.flipX = true;
            pfast += mo;
            if (pfast >= _moveMax) pfast = _moveMax;
        }
    }

    private void MoveLeft(float mo)
    {
        if (GameData.PlayerMoveAble>=1)
        {
            warking = true;
            muki = 0;
            _spriteREffect.flipX = false;
            _spriteRPlayer.flipX = false;
            pfast -= mo;
            if (pfast <= -_moveMax) pfast = -_moveMax;
        }
    }

    private void MoveStop()
    {
        if (GameData.IceFloor == 1)
        {
            //IceFloor
            warking = false;
            if (muki == 0 && pfast != 0)
            {
                //pfast -= 2;
                if (pfast <= -12) pfast = -12;
            }
            else if (muki == 1 && pfast != 0)
            {
                //pfast += 2;
                if (pfast >= 12) pfast = 12;
            }
        }
        else
        {
            //Stop
            warking = false;
            if (pfast > 1) pfast--;
            else if (pfast < -1) pfast++;
            else if (pfast <= 1 && pfast >= -1) pfast = 0;
        }
    }

    private void MoveBound(Vector3 mpos)
    {
        transform.localPosition = mpos;
        if (GameData.IceFloor == 1)
        {
            pfast = -pfast;
        }
    }

    private void MoveJump()
    {
        if (GameData.PlayerMoveAble>=2)
        {
            if (jump == 0 && tate == 0)
            {
                if (dan < 5)
                {
                    tate = 1;

                    jump = 1;
                    pjump = 22f;
                }
                else if (dan == 5)
                {
                    tate = 1;
                    jump = 1;
                    pjump = 22f;
                }
            }
        }
    }

    private void MoveDown()
    {
        if (GameData.PlayerMoveAble>=3)
        {
            if (pull == 0 && dan > 1 && tate == 0)
            {
                dan -= 1;
                pull = 1;
                tate = 1;
            }
        }
    }

    /// <summary>
    /// 遠距離
    /// </summary>
    private void Fire()
    {
        if (GameData.PlayerMoveAble>=4)
        {
            if (GunMode < 1 && shotdown[0] <= 0) Shot_Raser();
            else if (GunMode >= 1 && GunMode < 2 && shotdown[2] <= 0) Shot_Bullet();
            else if (GunMode >= 2 && GunMode < 3 && shotdown[4] <= 0) Shot_Drop();
            else if (GunMode >= 3 && GunMode < 4 && shotdown[6] <= 0) Shot_Rocket();
        }
    }

    /// <summary>
    /// 近距離
    /// </summary>
    private void Fire2()
    {
        if (GameData.PlayerMoveAble>=4)
        {
            if (GunMode < 1 && shotdown[1] <= 0) Shot_Slash();
            else if (GunMode >= 1 && GunMode < 2 && shotdown[3] <= 0) Shot_ShotGun();
            else if (GunMode >= 2 && GunMode < 3 && shotdown[5] <= 0) Shot_MineSield();
            else if (GunMode >= 3 && GunMode < 4 && shotdown[7] <= 0) Shot_Fire();
        }
    }


    private void Heal(int dhp)
    {
        _audioGO.PlayOneShot(healS);
        GameData.HP += dhp;
        for (i = 0; i < 7; i++)
        {
            ExpC shot = Instantiate(HealEffectPrefab, _shotPos, rot);
            shot.EShot1(Random.Range(80, 100), Random.Range(10, 15), 0.3f);
        }
    }

    /// <summary>
    /// 0Beam_beam
    /// </summary>
    private void Shot_Raser()
    {
        PMissile shot = Instantiate(PRaserP, _shotPos, rot);
        shot.Shot(180 + (muki * 180), 320, 1000);
        _audioGO.PlayOneShot(shotS);
        _isLoaded[0] = false;
        shotdown[0] = 1.0f;
    }
    
    /// <summary>
    /// 1Beam_Slash
    /// </summary>
    private void Shot_Slash()
    {
        Instantiate(PSlashP, new Vector3(pos.x - 64 + (128 * muki), _shotPos.y, 0), rot).EShot1(muki * 180, 0, 0.08f);
        _audioGO.PlayOneShot(SlashS);
        shotdown[1] = 0.1f;
    }

    /// <summary>
    /// 2Bullet_bullet
    /// </summary>
    private void Shot_Bullet()
    {
        for (i = -4; i < 4; i++)
        {
            //Instantiate(PBulletP, _shotPos, rot).Shot(180 + (muki * 180), 100+(i*10), 0);
            Instantiate(PBulletP, _shotPos, rot).Shot(180 + (muki * 180) + (i * 3) + Random.Range(-1, 1), 70, 0);
            //Instantiate(PBulletExpP, _shotPos, rot).EShot1(180 + (muki * 180) + (i * 5) + Random.Range(-1, 1), 70, 0.1f);
        }
        _audioGO.PlayOneShot(bulletS);
        _isLoaded[1] = false;
        shotdown[2] = 0.8f;
    }

    /// <summary>
    /// 3Bullet_ShotGun
    /// </summary>
    private void Shot_ShotGun()
    {
        Instantiate(PBulletP, _shotPos, rot).Shot(180 + (muki * 180) + Random.Range(-4, 4), 70, 0);
        //Instantiate(PBulletExpP, _shotPos, rot).EShot1(180 + (muki * 180) + Random.Range(-4, 4), 70, 0.1f);
        _audioGO.PlayOneShot(bulletS);
        shotdown[3] = 0.09f;
    }

    /// <summary>
    /// 4Bomb_drop
    /// </summary>
    private void Shot_Drop()
    {
        PBombC shot = Instantiate(PMineP, _shotPos, rot);
        shot.EShot1(270, 0, 1.0f, 100, 8, 0.5f);
        _audioGO.PlayOneShot(putS);
        _isLoaded[2] = false;
        shotdown[4] = 1.0f;
    }

    /// <summary>
    /// 5Bomb_Sield
    /// </summary>
    private void Shot_MineSield()
    {
        PBombC shot = Instantiate(PMineP, new Vector3(_shotPos.x-48+(96*muki), GameData.GroundPutY((int)_shotPos.y / 90, 32), 0), rot);
        shot.EShot1(270, 0, 0, 100, 3, 0.5f);
        _audioGO.PlayOneShot(putS);
        shotdown[5] = 0.6f;
    }

    /// <summary>
    /// 6rocket
    /// </summary>
    private void Shot_Rocket()
    {
        PBombC shot = Instantiate(PBombP, _shotPos, rot);
        shot.EShot1(180 + (muki * 180) + Random.Range(-5, 5), 20, -0.3f, 660, 20, 2.0f);
        _audioGO.PlayOneShot(bulletS);
        _isLoaded[3] = false;
        shotdown[6] = 4.0f;
    }

    /// <summary>
    /// 7Fire
    /// </summary>
    private void Shot_Fire()
    {
        PExpC shot4 = Instantiate(PFireP, _shotPos, rot);
        shot4.EShot1(180 + (muki * 180) + Random.Range(-20, 20), Random.Range(2, 20), 0.2f);
        _audioGO.PlayOneShot(fireS);
        shotdown[7] = 0.03f;
    }

    private void Magic()
    {
        if (GameData.PlayerMoveAble>=6)
        {
            tpwdown = 10;
            _audioGO.PlayOneShot(magicuseS);
            _audioGO.PlayOneShot(exprosionS);
            GameData.TP -= 1;
            if (GunMode < 1 && shotdown[1] <= 0) Magic_SuperRaser();
            else if (GunMode >= 1 && GunMode < 2 && shotdown[3] <= 0) Magic_BackMachinegun();
            else if (GunMode >= 2 && GunMode < 3 && shotdown[5] <= 0) Magic_MineCristal();
            else if (GunMode >= 3 && GunMode < 4 && shotdown[7] <= 0) Magic_Meteor();
        }
    }

    private void Magic_SuperRaser()
    {
        Quaternion rot2 = transform.localRotation;

        for (i = 0; i < 3; i++)
        {
            BeamMC shot = Instantiate(BeamMP, _shotPos, rot);
            shot.EShot1(i*120);
        }
    }

    private void Magic_BackMachinegun()
    {
        Instantiate(PMachinegunP, _shotPos, rot);
    }

    private void Magic_MineCristal()
    {
        Quaternion rot = transform.localRotation;

        for (i = 10; i < 640; i+=20)
        {
            PBombC shot = Instantiate(PMinecristalP, new Vector3(i,_shotPos.y,0), rot);
            shot.EShot1(Random.Range(0,360), 0, 0.001f, 300-(i/10), 10, 1.0f);
        }
        for (i = 10; i < 480; i += 20)
        {
            PBombC shot = Instantiate(PMinecristalP, new Vector3(_shotPos.x, i, 0), rot);
            shot.EShot1(Random.Range(0, 360), 0, 0.001f, 300 - (i / 10), 10, 1.0f);
        }
        for (i = 10; i < 360; i += 20)
        {
            PBombC shot = Instantiate(PMinecristalP, _shotPos + (new Vector3(Mathf.Sin(i* Mathf.Deg2Rad),Mathf.Cos(i * Mathf.Deg2Rad), 0)*100), rot);
            shot.EShot1(Random.Range(0, 360), 0, 0.001f,350, 10, 1.0f);
        }
    }

    private void Magic_Meteor()
    {
        for (i = 0; i < 10; i++)
        {
            PMeteorC shot = Instantiate(PMeteorP, new Vector3(32+i*64, ((int)_shotPos.y / 90 * 90) + 64, 0), rot);
            shot.EShot1(Random.Range(50,100+(i*15)));
        }
    }

    /// <summary>
    /// 接触ダメージ
    /// </summary>
    /// <param name="dhp"></param>
    public void EDamage(int dhp)
    {
        StartCoroutine("DamageControllerVibration");
        CameraC.IsDamageShake = true;
        GameData.HP -= dhp;
        if (GameData.HP < 0)
        {
            GameData.HP = 0;
        }
        _audioGO.PlayOneShot(damageS);
        ExpC shot = Instantiate(BDE, new Vector3(320, 240, 0), rot);
        shot.EShot1(0, 0, 0.1f);
        for (i = 0; i < 10; i++)
        {
            rot = transform.localRotation;
            shot = Instantiate(DamagePrefab, pos, rot);
            shot.EShot1(Random.Range(0, 360), Random.Range(5, 20), 0.3f);
        }
    }

    /// <summary>
    /// 弾ダメージ
    /// </summary>
    /// <param name="dhp"></param>
    /// <param name="destroy"></param>
    public void MDamage(int dhp,GameObject destroy)
    {
        CameraC.IsDamageShake = true;
        GameData.HP -= dhp;
        if (GameData.HP < 0)
        {
            GameData.HP = 0;
        }
        _audioGO.PlayOneShot(damageS);
        ExpC shot = Instantiate(BDE, new Vector3(320, 240, 0), rot);
        shot.EShot1(0, 0, 0.1f);
        for (i = 0; i < 10; i++)
        {
            rot = transform.localRotation;
            shot = Instantiate(DamagePrefab, pos, rot);
            shot.EShot1(Random.Range(0, 360), Random.Range(5, 20), 0.3f);
        }
        StartCoroutine("DamageControllerVibration");
        Destroy(destroy);
    }

    /// <summary>
    /// ステージ移動アクションスタート
    /// </summary>
    public void StageMoveAction()
    {
        if (!GameData.StageMovingAction)
        {
            StopCoroutine(StageMove());
            StartCoroutine(StageMove());
        }
        
    }

    private IEnumerator StageMove()
    {
        GameData.TimerMoving = false;
        GameData.Star = true;
        GameData.StageMovingAction = true;
        muki = 1;
        _spriteRPlayer.flipX = true;
        pfast = 0;
        while (tate==1)
        {
            yield return new WaitForSeconds(0.03f);
        }
        GameData.PlayerMoveAble = 0;
        yield return new WaitForSeconds(2.0f);
        while (transform.position.x <= 670)
        {
            transform.position += transform.right * 7;
            yield return new WaitForSeconds(0.03f);
        }
        transform.position = new Vector3(32, pos.y, 0);
        AllEnemyDelete();
        GameData.Star = false;
        GameData.Score += 100000;
        GameData.PlayerMoveAble = 6;
        GameData.TimerMoving = true;
        GameData.StageMovingAction = false;
    }

    private void TPEffect()
    {
        float angle = Random.Range(0, 360);
        ExpC shot = Instantiate(ExpPrefab, pos, rot);
        shot.EShot1(angle, 1.0f, 0.5f);
        //Gui.sprite = gm;
    }

    public void OnEnd(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Time.timeScale = 1.0f;
            GameData.Round = 1;
            GameData.HP = 20;
            GameData.Boss = 0;
            GameData.IceFloor = 0;
            GameData.Star = false;
            GameData.TP = 0;
            GameData.Score = 0;
            GameData.GameMode = 0;
            GameData.ClearTime = 0;
            SceneManager.LoadScene("Title");
        }
    }

    private void AllEnemyDelete()
    {
        TagDelete("Enemy0");
        TagDelete("Enemy1");
        TagDelete("Enemy2");
        TagDelete("Enemy3");
        TagDelete("Enemy4");
        TagDelete("Enemy6");
        TagDelete("EK1");
        TagDelete("EK2");
        TagDelete("EK3");
        TagDelete("EK10");
        TagDelete("EM1");
        TagDelete("EM2");
        TagDelete("EM3");
        TagDelete("EM10");
        TagDelete("Bullet1");
        TagDelete("Barrier");
        TagDelete("Effect");
    }

    private void TagDelete(string tagName)
    {
        GameObject[] myObjects = GameObject.FindGameObjectsWithTag(tagName);
        for(int zz=0; zz < myObjects.Length; zz++)
        {
            Destroy(myObjects[zz]);
        }
    }

}
