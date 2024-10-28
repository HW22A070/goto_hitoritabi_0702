using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerC : MonoBehaviour
{
    /// <summary>
    /// プレイヤースプライト
    /// </summary>
    private SpriteRenderer _spriteRPlayer;

    [SerializeField, Tooltip("プレイヤー画像データ")]
    private Sprite[] _playerGurNormal, _playerGurWark,_playerGurNormalAttack,_playerGurWarkAttack;

    [SerializeField, Tooltip("プレイヤー死")]
    private Sprite　_playerDeath;

    [SerializeField, Tooltip("プレイヤー変形中")]
    private Sprite _playerChanging;


    private bool _isMoveL, _isMoveR, _isFireing, _isFireing2, _isFireingChange2, Jumping, Downing, SP1ing, SP2ing;

    /// <summary>
    /// 00=left,01=right;
    /// </summary>
    public static int muki = 0;

    /// <summary>
    /// プレイヤーの速さ
    /// </summary>
    private float pfast = 0;
    private bool warking = false;
    private int warktime = 0;

    private Vector3 pos;

    /// <summary>
    /// プレイヤーのグラフィックかちょっとでかいぶん銃の場所を下げる値
    /// </summary>
    private Vector3 _shotPos;

    private Quaternion rot;

    private float pmspeed = 0;
    private  float tpwdown = 0;

    /// <summary>
    /// クールタイムセット
    /// </summary>
    private float[] _defaultCoolTime = { 1.8f, 0.1f, 2.0f, 0.08f, 1.5f, 0.6f, 4.0f, 0.03f };

    /// <summary>
    /// 現在のクールタイム
    /// </summary>
    private  float[] shotdown = { 0, 0, 0, 0, 0, 0, 0, 0 };
    private  float warkcount = 0;
    private float _invincibleTime = 0;

    private float wh = 0;

    private float _gunModeMouseValue = 1.1f;

    /// <summary>
    /// 0=ビーム
    /// 1=バレット
    /// 2=ボム
    /// 3=バーン
    /// </summary>
    private int _gunMode = 1;
    private int plnum = 0;

    /// <summary>
    /// 遠距離武器がロードされているか
    /// </summary>
    private bool[] _isLoaded= { true, true, true, true};

    /// <summary>
    /// 変形中
    /// </summary>
    private bool _isChanging;

    /// <summary>
    /// 横移動最大値
    /// </summary>
    private float _moveMax = 8;

    [SerializeField,Tooltip("弾アタッチ")]
    private PMissile PBulletP;

    [SerializeField, Tooltip("弾アタッチ、")]
    private PBombC PBombP, PMinePutP, PMineP, PMinecristalP;

    [SerializeField, Tooltip("弾アタッチ")]
    private PExpC PFireP, PSlashP, PBulletExpP;

    [SerializeField, Tooltip("弾アタッチ")]
    private EMissile1C TPwazaPrefab;

    [SerializeField, Tooltip("エフェクトアタッチ")]
    private ExpC ExpPrefab, DamagePrefab, HealEffectPrefab, BDE,_prhbBulletShot;

    [SerializeField, Tooltip("ビーム必殺")]
    private PSpecialBeamC BeamMP;

    [SerializeField, Tooltip("バレット必殺")]
    private PMachineGunC PMachinegunP;

    [SerializeField, Tooltip("バーン必殺")]
    private PMeteorC PMeteorP;

    [SerializeField, Tooltip("照準エフェクトアタッチ")]
    private ExpC _prfbTarget;

    [SerializeField,Header("ビームチャイルド")]
    private GameObject _prhbBeamChild;

    [SerializeField, Header("バレットチャイルド")]
    private GameObject _prhbBulletChild;

    [SerializeField, Header("ボムチャイルド")]
    private GameObject _prhbBombChild;

    [SerializeField, Header("バ―ンチャイルド")]
    private GameObject _prhbBurnChild;

    /// <summary>
    /// ロードされたやつ管理用
    /// </summary>
    private GameObject[] _goChild=new GameObject[3];

    public object gameobject { get; private set; }

    [SerializeField, Tooltip("サウンド")]
    private AudioClip shotS, healS, damageS, magicgetS, magicuseS, exprosionS, bulletS, putS, fireS, ChangeS, SlashS;

    [SerializeField, Tooltip("ロードサウンド")]
    private AudioClip[] _loadS;

    /// <summary>
    /// 繰り返し用とかそんなん
    /// </summary>
    private int i;

    /// <summary>
    /// 空中
    /// </summary>
    private bool _isGround;

    /// <summary>
    /// 最大重力
    /// </summary>
    private int _gravityMax = 10;

    /// <summary>
    /// 重力加速度
    /// </summary>
    private int _gravityDelta = 4;
    
    /// <summary>
    /// 現在重力
    /// </summary>
    private int _gravityNow = 0;

    /// <summary>
    /// ジャンプなう
    /// </summary>
    private bool _isJumping;

    /// <summary>
    /// 下降中なう
    /// </summary>
    private bool _isDowning;


    /// <summary>
    /// プレイヤーと床の判定
    /// </summary>
    private RaycastHit2D _hitPlayerToFloor;


    [SerializeField,Tooltip("スピーカ")]
    private AudioSource _audioGO;

    [SerializeField, Header("Effect")]
    private SpriteRenderer _spriteREffect;

    [SerializeField, Tooltip("プレートGUI")]
    private Sprite[] _effectGur;

    /// <summary>
    /// カメラオブジェクト
    /// </summary>
    [SerializeField,Tooltip("カメラオブジェクト")]
    private GameObject _goCamera;

    private bool _isnowBedRock;
    private int _floorMode = 0;


    // Start is called before the first frame update
    void Start()
    {
        muki = 0;
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        var gamepad = Gamepad.current;
        _spriteRPlayer = GetComponent<SpriteRenderer>();
        StartCoroutine(EffectAnim());
        pos = transform.position;
        Summon_Child();
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        //プレイヤーのグラフィックかちょっとでかいぶん銃の場所を下げる
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
            if (shotdown[i] != 0)
            {
                if (_gunMode!= i/2) shotdown[i] -= Time.deltaTime * 2;
                else shotdown[i] -= Time.deltaTime;
            }
        }

        //マウス
        //GunModeChange
        if(GameData.PlayerMoveAble>=5)_gunModeMouseValue += Input.GetAxis("Mouse ScrollWheel") * 5;
        //_gunMode = wh;
        if (_gunModeMouseValue <= 0.0f) _gunModeMouseValue = 4.0f-0.1f;
        if (_gunModeMouseValue >= 4.0f) _gunModeMouseValue = 0.1f;

        if (_gunMode != (int)_gunModeMouseValue&&!_isChanging)
        {
            GunModeChange((int)_gunModeMouseValue);
        }

        //if (wh < 0) wh = 15.0f;
        //if (wh > 15) wh = 0.0f;

        //Magic
        if (tpwdown > 0)
        {
            tpwdown -= Time.deltaTime;
        }

        //StarTime
        if (_invincibleTime != 0) _invincibleTime -= Time.deltaTime;

        //moveauto
        if (_isMoveL) MoveLeft(2);
        else if (_isMoveR) MoveRight(2);
        else MoveStop();
        if (Jumping) MoveJump();
        if (Downing) MoveDown();
        if (_isFireing) Fire();
        if (_isFireing2||_isFireingChange2) Fire2();

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

        if(GameData.PlayerMoveAble < 4)
        {
            _isFireing = false;
            _isFireing2 = false;
            _isFireingChange2 = false;
        }
        if (GameData.PlayerMoveAble < 3) Downing = false;
        if (GameData.PlayerMoveAble < 2) Jumping = false;

        //上限
        if (pos.y >= GameData.WindowSize.y - 20) transform.position = new Vector3(pos.x, GameData.WindowSize.y - 20, 0);

        //床下行ったらダメージ受けて一番上にワープ
        if (pos.y <= -16)
        {
            transform.position = new Vector3(pos.x, GameData.WindowSize.y - 20, 0);
            if (!GameData.StageMovingAction)
            {
                Damage(1+(int)GameData.HP / 2, 0);
            }

        }

        //炎上！
        if (_floorMode == 3) Damage(1, 0.3f);
        //刺傷！
        if (_floorMode == 5) Damage(2, 0.1f);
    }


    //Imput
    //十字移動
    public void OnMoveL(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 1)
        {
            if (context.started) _isMoveL = true;
            else if (context.canceled) _isMoveL = false;
        }
    }
    public void OnMoveR(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 1)
        {
            if (context.started) _isMoveR = true;
            else if (context.canceled) _isMoveR = false;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 2)
        {
            if (context.started) Jumping = true;
            else if (context.canceled) Jumping = false;
        }
    }
    public void OnDown(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 3)
        {
            if (context.started) Downing = true;
            else if (context.canceled) Downing = false;
        }
    }
    //Lスティック移動
    public void OnMoveLStick(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 1)
        {
            if (context.ReadValue<float>() > 0.5f)
            {
                _isMoveL = true;
            }
            else _isMoveL = false;
        }
    }
    public void OnMoveRStick(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 1)
        {
            if (context.ReadValue<float>() > 0.5f) {
                _isMoveR = true;
            }
            else _isMoveR = false;
        }
    }
    public void OnJumpStick(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 2)
        {
            if (context.ReadValue<float>() > 0.7f) Jumping = true;
            else Jumping = false;
        }
    }
    public void OnDownStick(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 3)
        {
            if (context.ReadValue<float>() > 0.7f) Downing = true;
            else Downing = false;
        }
    }
    //攻撃
    public void OnFire(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 4)
        {
            if (context.started) _isFireing = true;
            else if (context.canceled) _isFireing = false;
        }
    }
    public void OnFire2(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 4)
        {
            if (context.started) _isFireing2 = true;
            else if (context.canceled) _isFireing2 = false;
        }
    }
    //必殺
    public void OnSuperFire(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 6)
        {
            if (context.performed && tpwdown <= 0 && GameData.TP > 0) Magic();
        }
    }
    public void SP1(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 6 && !_isChanging)
        {
            if (context.started) SP1ing = true;
            else if (context.canceled) SP1ing = false;
        }
    }
    public void SP2(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 6 && !_isChanging)
        {
            if (context.started) SP2ing = true;
            else if (context.canceled) SP2ing = false;
        }
    }
    //マウス変形
    public void GunModeChangeUp(InputAction.CallbackContext context)
    {
        if (context.performed&&GameData.PlayerMoveAble>=5)
        {
            _gunMode++;
            _audioGO.PlayOneShot(ChangeS);
            _spriteRPlayer.sprite = _playerGurNormal[(int)_gunMode];
        }
    }
    public void GunModeChangeDown(InputAction.CallbackContext context)
    {
        if (context.performed && GameData.PlayerMoveAble >= 5)
        {
            _gunMode--;
            _audioGO.PlayOneShot(ChangeS);
            _spriteRPlayer.sprite = _playerGurNormal[(int)_gunMode];
        }
    }
    //NEWS変形
    public void GunModeChangeBeam(InputAction.CallbackContext context)
    {
        if (context.performed && GameData.PlayerMoveAble >= 5)
        {
            GunModeChange(0);
            if (context.started) _isFireingChange2 = true;
            else if (context.canceled) _isFireingChange2 = false;
        }
    }
    public void GunModeChangeBullet(InputAction.CallbackContext context)
    {
        if (context.performed && GameData.PlayerMoveAble >= 5)
        {
            GunModeChange(1);
            if (context.started) _isFireingChange2 = true;
            else if (context.canceled) _isFireingChange2 = false;
        }
        if (GameData.PlayerMoveAble >= 4)
        {

        }
    }
    public void GunModeChangeRocket(InputAction.CallbackContext context)
    {
        if (context.performed && GameData.PlayerMoveAble >= 5)
        {
            GunModeChange(2);
            if (context.started) _isFireingChange2 = true;
            else if (context.canceled) _isFireingChange2 = false;
        }
        if (GameData.PlayerMoveAble >= 4)
        {

        }
    }
    public void GunModeChangeMine(InputAction.CallbackContext context)
    {
        if (context.performed && GameData.PlayerMoveAble >= 5) {
            GunModeChange(3);
            if (context.started) _isFireingChange2 = true;
            else if (context.canceled) _isFireingChange2 = false;
        }
        if (GameData.PlayerMoveAble >= 4)
        {

        }
    }

    public void GunModeChangeBeamStick(InputAction.CallbackContext context)
    {
        bool isAction = context.ReadValue<float>() > 0.95f;
        if (isAction && GameData.PlayerMoveAble >= 5) GunModeChange(0);
        if (GameData.PlayerMoveAble >= 4)
        {
            if (isAction) _isFireingChange2 = true;
            else _isFireingChange2 = false;
        }
    }
    public void GunModeChangeBulletStick(InputAction.CallbackContext context)
    {
        bool isAction = context.ReadValue<float>() > 0.95f;
        if (isAction && GameData.PlayerMoveAble >= 5) GunModeChange(1);
        if (GameData.PlayerMoveAble >= 4)
        {
            if (isAction) _isFireingChange2 = true;
            else  _isFireingChange2 = false;
        }
    }
    public void GunModeChangeRocketStick(InputAction.CallbackContext context)
    {
        bool isAction = context.ReadValue<float>() > 0.95f;
        if (isAction && GameData.PlayerMoveAble >= 5) GunModeChange(2);
        if (GameData.PlayerMoveAble >= 4)
        {
            if (isAction) _isFireingChange2 = true;
            else _isFireingChange2 = false;
        }
    }
    public void GunModeChangeMineStick(InputAction.CallbackContext context)
    {
        bool isAction = context.ReadValue<float>() > 0.95f;
        if (isAction && GameData.PlayerMoveAble >= 5) GunModeChange(3);
        if (GameData.PlayerMoveAble >= 4)
        {
            if (isAction) _isFireingChange2 = true;
            else _isFireingChange2 = false;
        }
    }




    private void FixedUpdate()
    {
        //重力
        Ray2D playerFootRay = new Ray2D(transform.position - new Vector3(16, 20+1.0f, 0), new Vector2(32, 0));
        //Debug.DrawRay(playerFootRay.origin, playerFootRay.direction, Color.gray);
        if (_isJumping)
        {
            //重力をマイナスにし、重力がゼロ以下になったら_isJumpingを切る
            _isGround = false;
            if (_gravityNow >= 0) _isJumping = false;
        }
        else if (_isDowning)
        {
            //床から出たら降下中を切る。自分が乗ってる台だけすり抜ける
            _isGround = false;
            if (!Physics2D.Raycast(playerFootRay.origin, playerFootRay.direction, 32, 8)) _isDowning = false;
        }
        else
        {
            
            _hitPlayerToFloor = Physics2D.Raycast(playerFootRay.origin, playerFootRay.direction, 32, 8);
            //足元に地面がなければ空中
            _isGround = _hitPlayerToFloor;
        }

        //地面
        if (_isGround)
        {
            GameObject floor = _hitPlayerToFloor.collider.gameObject;
            _isnowBedRock = floor.GetComponent<FloorC>()._isBedRock;
            _floorMode = floor.GetComponent<FloorC>()._floorMode;
            pos = transform.position;

            transform.position = new Vector3(pos.x
                , floor.transform.position.y+(floor.GetComponent<BoxCollider2D>().size.y / 2) + ((GetComponent<BoxCollider2D>().size.y- GetComponent<BoxCollider2D>().offset.y) / 2), 0);

            //重力ゼロ
            _gravityNow = 0;
        }
        //空中
        else
        {
            //重力加速
            if (_gravityNow >= _gravityMax) _gravityNow = _gravityMax;
            else _gravityNow += _gravityDelta;
        }

        transform.position -= new Vector3(0, _gravityNow, 0);

        
        //MoveAction
        transform.localPosition += new Vector3(pfast, 0, 0);


        //TPEffect
        //if (GameData.TP >= 1) TPEffect();
        //else Gui.sprite = gn;

        //銃モード中は照準出す
        if (_gunMode == 1)
        {
            GameObject flontObj = GetFlontEnemy();
            if (flontObj != null)
            {
                Vector3 flontObjPos = GameData.FixPosition(flontObj.transform.position, 32, 32);
                Instantiate(_prfbTarget, flontObjPos, rot).EShot1(0, 0, 0.03f);
                /*
                for (int hoge = 0; hoge < 2; hoge++)
                {
                    _goChild[hoge].GetComponent<BulletChildC>().GOTarget = flontObjPos;
                }
                
            }
            else
            {
                for (int hoge = 0; hoge < 2; hoge++)
                {
                    _goChild[hoge].GetComponent<BulletChildC>().GOTarget = Vector3.zero;
                }
                */
            }
        }

        //ジャンプ中は風の影響を強く受けます。
        if (_isGround)transform.position += new Vector3(GameData.WindSpeed / 10, 0, 0);
        else transform.position += new Vector3(GameData.WindSpeed / 5, 0, 0);


        //graphic
        //ジャンプ中は歩きGur
        if (warktime < 4) warktime++;
        if (_isJumping)
        {
            _spriteRPlayer.sprite = _playerGurWark[_gunMode];
        }
        else if (warking == true)
        {
            if (warktime > 3)
            {
                if (_spriteRPlayer.sprite == _playerGurNormal[_gunMode]) _spriteRPlayer.sprite = _playerGurWark[_gunMode];
                else if (_spriteRPlayer.sprite == _playerGurWark[_gunMode]) _spriteRPlayer.sprite = _playerGurNormal[_gunMode];
                warktime = 0;
            }
        }
        else
        {
            _spriteRPlayer.sprite = _playerGurNormal[_gunMode];
        }
        if (_isChanging)
        {
            _spriteRPlayer.sprite = _playerChanging;
            _spriteREffect.sprite = _effectGur[16];
        }
        if (GameData.HP <= 0)
        {
            _spriteRPlayer.sprite = _playerDeath;
            _spriteREffect.sprite = _effectGur[16];
        }
        if (GameData.PlayerMoveAble < 4)
        {
            _isFireing = false;
            _isFireing2 = false;
            if (GameData.PlayerMoveAble < 3)
            {
                _isDowning = false;
                if (GameData.PlayerMoveAble < 2)
                {
                    _isJumping = false;
                    //if (GameData.PlayerMoveAble < 1)
                    //{
                    //    _isMoveL = false;
                    //    _isMoveR = false;
                    //}
                }
            }
        }

        /*
        //攻撃時
        else
        {
            if (tate == 1)
            {
                _spriteRPlayer.sprite =_playerGurWarkAttack[(int)_gunMode];
            }
            else if (warking == true)
            {
                if (warktime > 3)
                {
                    if (_spriteRPlayer.sprite ==_playerGurNormalAttack[(int)_gunMode])
                    {
                        _spriteRPlayer.sprite =_playerGurWarkAttack[(int)_gunMode];
                    }
                    else if (_spriteRPlayer.sprite ==_playerGurWarkAttack[(int)_gunMode])
                    {
                        _spriteRPlayer.sprite =_playerGurNormalAttack[(int)_gunMode];
                    }
                    warktime = 0;
                }

                warktime++;
            }
            else if (tate == 0)
            {
                _spriteRPlayer.sprite =_playerGurNormalAttack[(int)_gunMode];
            }
        }
        */

    }

    /// <summary>
    /// 帽子や薬莢などのアニメーションを管理
    /// </summary>
    /// <returns></returns>
    private IEnumerator EffectAnim()
    {
        
        for(int hoge = 0; hoge < 4; hoge++)
        {
            if (shotdown[_gunMode*2] <= 0/* &&shotdown[((int)_gunMode*2)+1] <= 0*/)
            {
                _spriteREffect.sprite = _effectGur[hoge + (_gunMode * 4)];
            }
            else _spriteREffect.sprite = _effectGur[16];

            yield return new WaitForSeconds(0.03f);
            if(shotdown[(_gunMode * 2) + 1] <= 0) yield return new WaitForSeconds(0.07f);
        }
        StartCoroutine(EffectAnim());
    }

    /// <summary>
    /// コントローラー振動管理
    /// </summary>
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

    /// <summary>
    /// 右移動
    /// </summary>
    /// <param name="mo"></param>
    private void MoveRight(float mo)
    {
        warking = true;
        muki = 1;
        _spriteREffect.flipX = true;
        _spriteRPlayer.flipX = true;
        pfast += mo;
        if (pfast >= _moveMax) pfast = _moveMax;
    }

    /// <summary>
    /// 左移動
    /// </summary>
    /// <param name="mo"></param>
    private void MoveLeft(float mo)
    {
        warking = true;
        muki = 0;
        _spriteREffect.flipX = false;
        _spriteRPlayer.flipX = false;
        pfast -= mo;
        if (pfast <= -_moveMax) pfast = -_moveMax;
    }

    /// <summary>
    /// 静止
    /// </summary>
    private void MoveStop()
    {
        if (_floorMode==1)
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

    /// <summary>
    /// 壁跳ね返り
    /// </summary>
    /// <param name="mpos"></param>
    private void MoveBound(Vector3 mpos)
    {
        transform.localPosition = mpos;
        if (_floorMode == 1)
        {
            pfast = -pfast;
        }
    }

    /// <summary>
    /// ジャンプ
    /// </summary>
    private void MoveJump()
    {
        if (_isGround)
        {
            _gravityNow = -30;
            _isJumping = true;
        }
    }

    /// <summary>
    /// 降下
    /// </summary>
    private void MoveDown()
    {
        if (_isGround&&!_isnowBedRock)
        {
            _isDowning = true;
            //_gravityNow = 11;
        }
    }

    /// <summary>
    /// 変形
    /// </summary>
    /// <param name="modevalue"></param>
    public void GunModeChange(int modevalue)
    {
        
        if (modevalue != _gunMode)
        {
            _isChanging = true;
            StartCoroutine(ChangingAnim(modevalue));
        }
    }

    /// <summary>
    /// 変形ディレイ
    /// </summary>
    /// <param name="modevalue"></param>
    /// <returns></returns>
    private IEnumerator ChangingAnim(int modevalue)
    {
        /*
        switch (_gunMode)
        {
            case 0:
                for (int hoge = 0; hoge < 3; hoge++) Destroy(_goChild[hoge]);
                break;
            case 1:
                for (int hoge = 0; hoge < 2; hoge++) Destroy(_goChild[hoge]);
                break;
            default:
                break;
        }
        */
        _audioGO.PlayOneShot(ChangeS);
        yield return new WaitForSeconds(0.2f);
        _gunMode = modevalue;
        if (_isLoaded[modevalue]) _audioGO.PlayOneShot(_loadS[_gunMode]);
        _spriteRPlayer.sprite = _playerGurNormal[_gunMode];
        DeleteChild();
        Summon_Child();
        _gunModeMouseValue = _gunMode + 0.5f;
        _isChanging = false;
    }

    private void Summon_Child()
    {
        switch (_gunMode)
        {
            case 0:
                Change_Beam();
                break;
            case 1:
                Change_Bullet();
                break;
            case 2:
                Change_Bomb();
                break;
            case 3:
                Change_Burn();
                break;
        }
    }

    /// <summary>
    /// ビームチャイルド呼び出し
    /// </summary>
    private void Change_Beam()
    {
        for (int hoge = 0; hoge < 3; hoge++)
        {
            _goChild[hoge] = Instantiate(_prhbBeamChild, pos, rot);
            //我が子にする
            //_goChild[hoge].transform.parent = transform;
            _goChild[hoge].GetComponent<BeamChildC>().SetOfset(hoge * 120);
        }
    }

    /// <summary>
    /// バレットチャイルド呼び出し
    /// </summary>
    private void Change_Bullet()
    {
        for (int hoge = 0; hoge < 2; hoge++)
        {
            _goChild[hoge] = Instantiate(_prhbBulletChild, pos, rot);
            //我が子にする
            //_goChild[hoge].transform.parent = transform;
            _goChild[hoge].GetComponent<BulletChildC>().SetOfset(new Vector3(0, hoge *32,0));
        }
    }

    /// <summary>
    /// ボムチャイルド呼び出し
    /// </summary>
    private void Change_Bomb()
    {
        for (int hoge = 0; hoge < 1; hoge++)
        {
            _goChild[hoge] = Instantiate(_prhbBombChild, pos, rot);
            //我が子にする
            //_goChild[hoge].transform.parent = transform;
            _goChild[hoge].GetComponent<BombChildC>().SetOfset(new Vector3(0, 48, 0));
        }
    }

    /// <summary>
    /// ボムチャイルド呼び出し
    /// </summary>
    private void Change_Burn()
    {
        for (int hoge = 0; hoge < 1; hoge++)
        {
            _goChild[hoge] = Instantiate(_prhbBurnChild, pos, rot);
            //我が子にする
            //_goChild[hoge].transform.parent = transform;
            _goChild[hoge].GetComponent<FireChildC>();
        }
    }

    /// <summary>
    /// 遠距離攻撃
    /// </summary>
    private void Fire()
    {
        if(!_isChanging)
        {
            if (_gunMode < 1 && shotdown[0] <= 0) Shot_Raser();
            else if (_gunMode >= 1 && _gunMode < 2 && shotdown[2] <= 0) Shot_Bullet();
            else if (_gunMode >= 2 && _gunMode < 3 && shotdown[4] <= 0) Shot_Drop();
            else if (_gunMode >= 3 && _gunMode < 4 && shotdown[6] <= 0) Shot_Rocket();
        }
    }

    /// <summary>
    /// 近距離攻撃
    /// </summary>
    private void Fire2()
    {
        if (!_isChanging)
        {
            if (_gunMode < 1 && shotdown[1] <= 0) Shot_Slash();
            else if (_gunMode >= 1 && _gunMode < 2 && shotdown[3] <= 0) Shot_ShotGun();
            else if (_gunMode >= 2 && _gunMode < 3 && shotdown[5] <= 0) Shot_MineSield();
            else if (_gunMode >= 3 && _gunMode < 4 && shotdown[7] <= 0) Shot_Fire();
        }
    }

    /// <summary>
    /// 回復
    /// </summary>
    /// <param name="dhp"></param>
    public bool Heal(int dhp)
    {
        if (GameData.HP < GameManagement.maxhp || GameData.StageMovingAction)
        {
            _audioGO.PlayOneShot(healS);
            GameData.HP += dhp;
            for (i = 0; i < 7; i++)
            {
                ExpC shot = Instantiate(HealEffectPrefab, _shotPos, rot);
                shot.EShot1(Random.Range(80, 100), Random.Range(10, 15), 0.3f);
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// 必殺チャージ
    /// </summary>
    /// <param name="dhp"></param>
    public bool MagicCharge()
    {
        if (GameData.TP < 5 || GameData.StageMovingAction)
        {
            _audioGO.PlayOneShot(magicgetS);
            if (GameData.TP < 5) GameData.TP += 1;
            return true;
        }
        return false;
    }



    /// <summary>
    /// 0Beam_Raser
    /// </summary>
    private void Shot_Raser()
    {
        
        for(int hoge = 0; hoge < 3; hoge++)
        {
            _goChild[hoge].GetComponent<BeamChildC>().DoAttackRaser();
        }
        _audioGO.PlayOneShot(shotS);
        _isLoaded[0] = false;
        shotdown[0] = _defaultCoolTime[0];
    }
    
    /// <summary>
    /// 1Beam_Slash
    /// </summary>
    private void Shot_Slash()
    {
        for (int hoge = 0; hoge < 3; hoge++)
        {
            _goChild[hoge].GetComponent<BeamChildC>().DoAttackSlash();

        }

        PExpC prefab = Instantiate(PSlashP, _shotPos, rot);
        prefab.EShot1(180 + (muki * 180), 0, 0.08f);
        prefab.transform.position += prefab.transform.up * 64;
        _audioGO.PlayOneShot(SlashS);
        shotdown[1] = _defaultCoolTime[1];
    }

    /// <summary>
    /// 2Bullet_Rifle
    /// </summary>
    private void Shot_Bullet()
    {
        
        for (int hoge = 0; hoge < 2; hoge++)
        {
            _goChild[hoge].GetComponent<BulletChildC>().DoAttackSniper();
        }
        _goCamera.GetComponent<CameraC>().StartShakeVertical(4, 5);
        //Instantiate(PBulletExpP, _shotPos, rot).EShot1(180 + (muki * 180) + Random.Range(-4, 4), 70, 0.1f);
        _audioGO.PlayOneShot(bulletS);
        shotdown[2] = _defaultCoolTime[2];
    }

    /*
    /// <summary>
    /// 2Bullet_bullet
    /// </summary>
    private void Shot_Bullet()
    {
        for (i = -4; i < 4; i++)
        {
            //Instantiate(PBulletP, _shotPos, rot).Shot(180 + (muki * 180), 100+(i*10), 0);
            PMissile prefab = Instantiate(PBulletP, _shotPos, rot);
            prefab.Shot(180 + (muki * 180) + (i * 3) + Random.Range(-1, 1), 70, 0);
            prefab.transform.position += prefab.transform.up * 16;
            //Instantiate(PBulletExpP, _shotPos, rot).EShot1(180 + (muki * 180) + (i * 5) + Random.Range(-1, 1), 70, 0.1f);
        }
        _audioGO.PlayOneShot(bulletS);
        _isLoaded[1] = false;
        shotdown[2] = _defaultCoolTime[2];
    }
    */

    /// <summary>
    /// 3Bullet_MachineGun
    /// </summary>
    private void Shot_ShotGun()
    {
        PMissile prefab = Instantiate(PBulletP, _shotPos, rot);
        prefab.Shot(180 + (muki * 180) + Random.Range(-4, 4), 70, 0);
        prefab.transform.position += prefab.transform.up * 16;
        _goCamera.GetComponent<CameraC>().StartShakeVertical(2, 4);
        BulletEffect();

        //Instantiate(PBulletExpP, _shotPos, rot).EShot1(180 + (muki * 180) + Random.Range(-4, 4), 70, 0.1f);
        _audioGO.PlayOneShot(bulletS);
        shotdown[3] = _defaultCoolTime[3];
    }

    /// <summary>
    /// 4Bomb_drop
    /// </summary>
    private void Shot_Drop()
    {
        for (int hoge = 0; hoge < 1; hoge++)
        {
            _goChild[hoge].GetComponent<BombChildC>().DoAttackDrop();
        }
        _audioGO.PlayOneShot(putS);
        _isLoaded[2] = false;
        shotdown[4] = _defaultCoolTime[4];
    }

    /// <summary>
    /// 5Bomb_Sield
    /// </summary>
    private void Shot_MineSield()
    {
        PBombC shot = Instantiate(PMinePutP, new Vector3(_shotPos.x-48+(96*muki), _shotPos.y, 0), rot);
        shot.EShot1(270, 0, 0, 100, 3, 0.5f);
        _audioGO.PlayOneShot(putS);
        shotdown[5] = _defaultCoolTime[5];
    }

    /// <summary>
    /// 6rocket
    /// </summary>
    private void Shot_Rocket()
    {
        for (int hoge = 0; hoge < 1; hoge++)
        {
            _goChild[hoge].GetComponent<FireChildC>().DoAttackRocket();
        }
        _audioGO.PlayOneShot(bulletS);
        _isLoaded[3] = false;
        shotdown[6] = _defaultCoolTime[6];
    }

    /// <summary>
    /// 7Fire
    /// </summary>
    private void Shot_Fire()
    {
        PExpC shot4 = Instantiate(PFireP, _shotPos, rot);
        shot4.EShot1(180 + (muki * 180) + Random.Range(-20, 20), Random.Range(2, 20), 0.2f);
        _audioGO.PlayOneShot(fireS);
        for (int hoge = 0; hoge < 1; hoge++)
        {
            _goChild[hoge].GetComponent<FireChildC>().DoAttackBress();
        }

        shotdown[7] = _defaultCoolTime[7];
    }

    /// <summary>
    /// 銃弾発射光エフェクト
    /// </summary>
    private void BulletEffect()
    {
        //発射エフェクト
        ExpC bulletEf = Instantiate(_prhbBulletShot, _shotPos, rot);
        bulletEf.transform.parent = transform;
        bulletEf.EShot1(180 + (muki * 180), 0, 0.06f);
        bulletEf.transform.position += bulletEf.transform.up * 24;
    }

    /// <summary>
    /// 必殺発動
    /// </summary>
    private void Magic()
    {
        tpwdown = 10;
        _audioGO.PlayOneShot(magicuseS);
        _audioGO.PlayOneShot(exprosionS);
        GameData.TP -= 1;
        if (_gunMode < 1 && shotdown[1] <= 0) Magic_SuperRaser();
        else if (_gunMode >= 1 && _gunMode < 2 && shotdown[3] <= 0) Magic_BackMachinegun();
        else if (_gunMode >= 2 && _gunMode < 3 && shotdown[5] <= 0) Magic_MineCristal();
        else if (_gunMode >= 3 && _gunMode < 4 && shotdown[7] <= 0) Magic_Meteor();
    }

    /// <summary>
    /// 必殺ビーム
    /// </summary>
    private void Magic_SuperRaser()
    {

        for (short i = 0; i < 2; i++)
        {

            /*BeamMC shot = */Instantiate(BeamMP, pos, rot).SetPos(i);
            //我が子にする
            //shot.transform.parent = transform;
            //shot.EShot1(i*120);
        }
    }

    /// <summary>
    /// 必殺マシンガン
    /// </summary>
    private void Magic_BackMachinegun()
    {
        PMachineGunC machine= Instantiate(PMachinegunP, _shotPos, rot);
        //我が子にする
        machine.transform.parent = transform;
    }

    /// <summary>
    /// 必殺ボム
    /// </summary>
    private void Magic_MineCristal()
    {
        Quaternion rot = transform.localRotation;

        for (i = 10; i < 640; i+=40)
        {
            PBombC shot = Instantiate(PMinecristalP, new Vector3(i,_shotPos.y,0), rot);
            shot.EShot1(Random.Range(0,360), 0, 0.001f, 300-(i/10), 10, 1.0f);
        }
        for (i = 10; i < 480; i += 40)
        {
            PBombC shot = Instantiate(PMinecristalP, new Vector3(_shotPos.x, i, 0), rot);
            shot.EShot1(Random.Range(0, 360), 0, 0.001f, 300 - (i / 10), 10, 1.0f);
        }
        for (i = 10; i < 360; i += 40)
        {
            PBombC shot = Instantiate(PMinecristalP, _shotPos + (new Vector3(Mathf.Sin(i* Mathf.Deg2Rad),Mathf.Cos(i * Mathf.Deg2Rad), 0)*100), rot);
            shot.EShot1(Random.Range(0, 360), 0, 0.001f,350, 10, 1.0f);
        }
    }

    /// <summary>
    /// 必殺ファイヤ
    /// </summary>
    private void Magic_Meteor()
    {

        
        for (int hoge = 0; hoge < 1; hoge++)
        {
            _goChild[hoge].GetComponent<FireChildC>().DoAttackSpecial();
            //Instantiate(PMeteorP, new Vector3(32+i*64, ((int)_shotPos.y / 90 * 90) + 64, 0), rot).EShot1(Random.Range(50,100+(i*15)));
        }
    }

    /// <summary>
    /// 統合ダメージ
    /// </summary>
    /// <param name="dhp"></param>
    public void Damage(int dhp,float addInvincible)
    {
        if (dhp > 0&&!GameData.Star)
        {
            if (_invincibleTime <= 0)
            {
                StartCoroutine("DamageControllerVibration");
                _goCamera.GetComponent<CameraC>().StartShakeBeside(5,10);
                GameData.HP -= dhp;
                if (GameData.HP < 0)
                {
                    GameData.HP = 0;
                }
                _audioGO.PlayOneShot(damageS);
                Instantiate(BDE, GameData.WindowSize/2, rot).EShot1(0, 0, 0.1f);
                for (i = 0; i < 10; i++)
                {
                    rot = transform.localRotation;
                    Instantiate(DamagePrefab, pos, rot).EShot1(Random.Range(0, 360), Random.Range(5, 20), 0.3f);
                }
                _invincibleTime = addInvincible;
            }
        }
        
    }

    /// <summary>
    /// チャイルド全消し
    /// </summary>
    /// <returns></returns>
    private void DeleteChild()
    {
        GameObject[] myObjects;
        myObjects = GameObject.FindGameObjectsWithTag("PChild");
        for(int j = 0; j < myObjects.Length; j++)
        {
            Destroy(myObjects[j]);
        }
    }

    /// <summary>
    /// 敵場所特定、自分とのアングルを求める
    /// </summary>
    /// <returns></returns>
    private float GetTagPosition(Vector3 pos)
    {
        GameObject[] myObjects;
        myObjects = GameObject.FindGameObjectsWithTag("Enemy");
        if (myObjects.Length > 0)
        {
            Vector3 enemyPos = GameData.FixPosition(myObjects[Random.Range(0, myObjects.Length)].transform.position, 32, 32);
            return GameData.GetAngle(pos, enemyPos);
        }
        else
        {
            if (GameData.Round == 0)
            {
                myObjects = GameObject.FindGameObjectsWithTag("Target");
                if (myObjects.Length > 0)
                {
                    Vector3 enemyPos = GameData.FixPosition(myObjects[Random.Range(0, myObjects.Length)].transform.position, 32, 32);
                    return GameData.GetAngle(pos, enemyPos);
                }
            }
            return 180 - (PlayerC.muki * 180);
        }
    }

    /// <summary>
    /// 前の敵と自分とのアングルを求める
    /// </summary>
    /// <returns></returns>
    private float GetTagPositionFlont()
    {
        GameObject flontEnemy;

        //前にいる敵がいれば、そいつへのアングルを返して終了
        flontEnemy = GetFlontEnemy();
        if (flontEnemy != null)
        {
            return GameData.GetAngle(pos, flontEnemy.transform.position);
        }

        //チュートリアル中であれば的探してアングル返して終了
        if (GameData.Round == 0)
        {
            GameObject[] myObjects;
            myObjects = GameObject.FindGameObjectsWithTag("Target");
            if (myObjects.Length > 0) return GameData.GetAngle(pos, myObjects[Random.Range(0, myObjects.Length)].transform.position);
        }
        return 180 - (PlayerC.muki * 180);
    }

    /// <summary>
    /// 前の敵場所特定
    /// </summary>
    /// <returns></returns>
    public GameObject GetFlontEnemy()
    {
        bool find = false;
        GameObject[] myObjects;
        myObjects = GameObject.FindGameObjectsWithTag("Enemy");

        for (int hoge = 0; hoge < myObjects.Length; hoge++)
        {

            if (muki == 0) find = myObjects[hoge].transform.position.x < pos.x;
            else find = myObjects[hoge].transform.position.x >= pos.x;

            if (find)
            {
                return myObjects[hoge];
            }

        }

        return null;
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

    /// <summary>
    /// ステージ移動アクション
    /// </summary>
    private IEnumerator StageMove()
    {
        GameData.TimerMoving = false;
        GameData.StageMovingAction = true;
        pfast = 0;
        while (!_isGround)
        {
            yield return new WaitForSeconds(0.03f);
        }
        GameData.PlayerMoveAble = 0;
        _isFireing = false;
        _isFireing2 = false;
        _isFireingChange2 = false;
        _isMoveR = false;
        _isMoveL = false;
        yield return new WaitForSeconds(2.0f);
        _isMoveR = true;
        while (transform.position.x <= 670)
        {
            yield return new WaitForSeconds(0.03f);
        }
        _isMoveR = false;
        transform.position = new Vector3(32, pos.y, 0);
        GameData.AllDeleteEnemy();
        GameData.AllDeleteEMissile();
        AllEnemyDelete();
        GameData.Star = false;
        GameData.Point += 100000;
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

    /// <summary>
    /// 中断
    /// </summary>
    /// <param name="context"></param>
    public void OnEnd(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GameData.Pouse)
            {
                GameData.PlayerMoveAble = 6;
                TimeManager.ChangeTimeValue(1.0f);
                GameData.Pouse = false;
            }

            
            if (GameData.Round <=0)
            {
                SceneManager.LoadScene("Title");
            }
            else
            {
                CrearC._isGiveUp = true;
                SceneManager.LoadScene("Clear");
            }
        }
    }

    private void AllEnemyDelete()
    {
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

    /// <summary>
    /// 特定の武器のクールタイムを返す
    /// </summary>
    public float CheckCoolTime(int weaponValue)
    {
        float cooltime = shotdown[weaponValue * 2]/ _defaultCoolTime[weaponValue * 2];
        return cooltime;
    }

    /// <summary>
    /// 現在の武器を返す
    /// </summary>
    public int CheckWeaponValue()
    {
        return (int)_gunMode;
    }

    public void ChangeWeapon(int mode)
    {
        GunModeChange(mode);
    }

}
