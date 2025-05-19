using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using EnumDic.Stage;
using EnumDic.Player;
using Move2DSystem;

public class PlayerC : MonoBehaviour
{
    /// <summary>
    /// プレイヤースプライト
    /// </summary>
    private SpriteRenderer _srOwn;

    [SerializeField, Tooltip("プレイヤー画像データ")]
    private Sprite[] _playerGurNormal, _playerGurWark;

    [SerializeField, Tooltip("プレイヤー死")]
    private Sprite _playerDeath;

    [SerializeField, Tooltip("プレイヤー変形中")]
    private Sprite _playerChanging;

    /// <summary>
    /// HP
    /// </summary>
    private int _hp = 100;

    private int _tp = 0;

    /// <summary>
    /// 行動の有無
    /// </summary>
    private bool _isMoveL, _isMoveR, _isFireing, _isFireing2, _isFireingChange2, Jumping, Downing, SP1ing, SP2ing;

    /// <summary>
    /// 右を向いているか
    /// </summary>
    private bool _isRight;

    /// <summary>
    /// プレイヤーの速さ
    /// </summary>
    private float _speedMove = 0;


    private bool _isWalking = false;
    private int _timeWarking = 0;

    private Vector3 _posOwn;

    private Quaternion _rotOwn;

    /// <summary>
    /// 必殺技発動時間
    /// </summary>
    private float _cooltimeSpecialAttack = 0;

    private float _timeInvincible = 0;

    /// <summary>
    /// 横移動最大値
    /// </summary>
    private float _moveMax = 8;

    [SerializeField, Tooltip("エフェクトアタッチ")]
    private ExpC DamagePrefab, HealEffectPrefab, BDE, ExpPrefab;

    [SerializeField, Tooltip("サウンド")]
    private AudioClip healS, damageS, magicgetS, magicuseS;

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
    /// 重力現在
    /// </summary>
    private int _gravityNow = 0;

    /// <summary>
    /// ジャンプ中か
    /// </summary>
    private bool _isJumping;

    /// <summary>
    /// 下降中か
    /// </summary>
    private bool _isDowning;


    /// <summary>
    /// プレイヤーと床の判定
    /// </summary>
    private RaycastHit2D _hitPlayerToFloor;

    private AudioSource _audioGO;

    [SerializeField, Header("Effect")]
    private SpriteRenderer _spriteREffect;

    /// <summary>
    /// カメラオブジェクト
    /// </summary>
    private GameObject _goCamera;

    /// <summary>
    /// 最下層か否か
    /// </summary>
    private bool _isnowBedRock;

    /// <summary>
    /// 現在立っている地面のモード
    /// </summary>
    private MODE_FLOOR _floorMode = MODE_FLOOR.Normal;


    private PlayerAttackC _scPAttack;

    private GameManagement _gameManager;

    /// <summary>
    /// 生きているか。死んだらfalse
    /// </summary>
    private bool _isAlive=true;

    /// <summary>
    /// ステージ名用タイル
    /// </summary>
    [SerializeField]
    private GameObject _prfbTextStage;
    private int _numberPlayer=1;


    // Start is called before the first frame update
    void Start()
    {
        SetGetComponents();

        _hp = GameData.GetMaxHP();
        _isRight = true;

        var gamepad = Gamepad.current;

        StartCoroutine(SetEffectAnim());
        _posOwn = transform.position;

        if (CheckIsLeader())
        {
            Instantiate(_prfbTextStage, GameData.WindowSize / 2 + Vector2.up * 84, Quaternion.Euler(0, 0, 0));
        }
    }

    /// <summary>
    /// Componentの取得
    /// </summary>
    private void SetGetComponents()
    {
        _scPAttack = GetComponent<PlayerAttackC>();
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        _srOwn = GetComponent<SpriteRenderer>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManagement>();
        _goCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        _posOwn = transform.position;
        _rotOwn = transform.localRotation;

        if (GameData.PlayerMoveAble >= 1)
        {
            //Bound
            if (_posOwn.x > 624)
            {
                MoveBound(new Vector3(624, _posOwn.y, 0));
            }
            if (_posOwn.x < 16)
            {
                MoveBound(new Vector3(16, _posOwn.y, 0));
            }
        }

        //必殺技状態でのクールタイムカウントダウン
        if (_cooltimeSpecialAttack > 0)
        {
            _cooltimeSpecialAttack -= Time.deltaTime;
        }
        else _scPAttack.SetIsSpecial(false);

        //StarTime
        if (_timeInvincible != 0) _timeInvincible -= Time.deltaTime;

        //moveauto
        if (_isMoveL) MoveLeft(2);
        else if (_isMoveR) MoveRight(2);
        else MoveStop();
        if (Jumping) MoveJump();
        if (Downing) MoveDown();
        if (_isFireing) _scPAttack.Fire();
        if (_isFireing2 || _isFireingChange2) _scPAttack.Fire2();

        if (SP2ing && SP2ing && _cooltimeSpecialAttack <= 0 && _tp > 0)
        {
            _cooltimeSpecialAttack = 20;
            _scPAttack.PlayMagic();
        }

        if (GameData.PlayerMoveAble < 4)
        {
            _isFireing = false;
            _isFireing2 = false;
            _isFireingChange2 = false;
        }
        if (GameData.PlayerMoveAble < 3) Downing = false;
        if (GameData.PlayerMoveAble < 2) Jumping = false;

        //上限
        if (_posOwn.y >= GameData.WindowSize.y - 20) transform.position = new Vector3(_posOwn.x, GameData.WindowSize.y - 20, 0);
        if (_hp > GameData.GetMaxHP()) _hp = GameData.GetMaxHP();

        //床下行ったらダメージ受けて一番上にワープ
        if (_posOwn.y <= -16)
        {
            transform.position = new Vector3(_posOwn.x, GameData.WindowSize.y - 20, 0);
            if (!GameData.IsStageMovingAction)
            {
                SetDamage(1 + _hp / 2, 0);
            }

        }

        //炎上！
        if (_floorMode == MODE_FLOOR.Burning) SetDamage(1, 0.3f);
        //刺傷！
        if (_floorMode == MODE_FLOOR.Needle) SetDamage(2, 0.1f);

        if(CheckIsLeader()&&!GameData.IsPouse)SetPouseControllerUnActive();
    }

    /// <summary>
    /// コントローラーの認識が切れたら自動でポーズメニュー
    /// </summary>
    /// <returns></returns>
    private void SetPouseControllerUnActive()
    {
        int count = 1;
        foreach (string name in Input.GetJoystickNames())
        {
            if (name != "") count++;
        }

        if (count < GameData.MultiPlayerCount)
        {
            DoPouse();
        }
    }

    /// <summary>
    /// プレイヤーナンバーを設定
    /// </summary>
    /// <param name="number"></param>
    public void SetPlayerNumber(int number)
    {
        _numberPlayer = number;
        GetComponent<PlayerFaceC>().SetFace(number);
    }

    #region Input
    //=======================================================
    //=========================Input=========================
    //=======================================================

    /// <summary>
    /// 移動左
    /// </summary>
    /// <param name="context"></param>
    public void OnMoveL(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 1&&_isAlive)
        {
            if (context.started) _isMoveL = true;
            else if (context.canceled) _isMoveL = false;
        }
    }

    /// <summary>
    /// 移動右
    /// </summary>
    /// <param name="context"></param>
    public void OnMoveR(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 1 && _isAlive)
        {
            if (context.started) _isMoveR = true;
            else if (context.canceled) _isMoveR = false;
        }
    }

    /// <summary>
    /// 移動ジャンプ
    /// </summary>
    /// <param name="context"></param>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 2 && _isAlive)
        {
            if (context.started) Jumping = true;
            else if (context.canceled) Jumping = false;
        }
    }

    /// <summary>
    /// 移動降下
    /// </summary>
    /// <param name="context"></param>
    public void OnDown(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 3 && _isAlive)
        {
            if (context.started) Downing = true;
            else if (context.canceled) Downing = false;
        }
    }
    
    /// <summary>
    /// スティック移動
    /// </summary>
    /// <param name="context"></param>
    public void OnMoveLStick(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 1 && _isAlive)
        {
            if (context.ReadValue<float>() > 0.5f)
            {
                _isMoveL = true;
            }
            else _isMoveL = false;
        }
    }

    /// <summary>
    /// スティック移動
    /// </summary>
    /// <param name="context"></param>
    public void OnMoveRStick(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 1 && _isAlive)
        {
            if (context.ReadValue<float>() > 0.5f)
            {
                _isMoveR = true;
            }
            else _isMoveR = false;
        }
    }

    /// <summary>
    /// スティック移動
    /// </summary>
    /// <param name="context"></param>
    public void OnJumpStick(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 2 && _isAlive)
        {
            if (context.ReadValue<float>() > 0.7f) Jumping = true;
            else Jumping = false;
        }
    }

    /// <summary>
    /// スティック移動
    /// </summary>
    /// <param name="context"></param>
    public void OnDownStick(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 3 && _isAlive)
        {
            if (context.ReadValue<float>() > 0.7f) Downing = true;
            else Downing = false;
        }
    }
    
    /// <summary>
    /// 攻撃
    /// </summary>
    /// <param name="context"></param>
    public void OnFire(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 4 && _isAlive)
        {
            if (context.started) _isFireing = true;
            else if (context.canceled) _isFireing = false;
        }
    }
    public void OnFire2(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 4 && _isAlive)
        {
            if (context.started) _isFireing2 = true;
            else if (context.canceled) _isFireing2 = false;
        }
    }
    //必殺
    public void OnSuperFire(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 6 && _isAlive)
        {
            if (context.performed && _cooltimeSpecialAttack <= 0 && _tp > 0)
            {
                _cooltimeSpecialAttack = 20;
                _scPAttack.PlayMagic();
            }
        }
    }
    public void SP1(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 6 && !_scPAttack.CheckIsChanging() && _isAlive)
        {
            if (context.started) SP1ing = true;
            else if (context.canceled) SP1ing = false;
        }
    }
    public void SP2(InputAction.CallbackContext context)
    {
        if (GameData.PlayerMoveAble >= 6 && !_scPAttack.CheckIsChanging() && _isAlive)
        {
            if (context.started) SP2ing = true;
            else if (context.canceled) SP2ing = false;
        }
    }
    //マウス変形
    public void GunModeChange(InputAction.CallbackContext value)
    {
        float valueY = value.ReadValue<Vector2>().y;
        Debug.Log(valueY.ToString());
        if(GameData.PlayerMoveAble >= 5 && _isAlive)
        {
            if (valueY > 110)
            {
                int gun = ((int)_scPAttack.GetGunMode() + 1) % 4;
                if (gun < 0) gun += 4;
                else if(gun>3) gun -= 4;

                _scPAttack.SetGunModeChange((MODE_GUN)gun,0.09f);
            }
            else if (valueY < -110)
            {
                int gun = ((int)_scPAttack.GetGunMode() - 1) % 4;
                if (gun < 0) gun += 4;
                else if (gun > 3) gun -= 4;

                _scPAttack.SetGunModeChange((MODE_GUN)gun, 0.09f);
            }
        }

    }

    //NEWS変形
    public void GunModeChangeBeam(InputAction.CallbackContext context)
    {
        if (context.performed && GameData.PlayerMoveAble >= 5 && _isAlive)
        {
            _scPAttack.SetGunModeChange(MODE_GUN.Shining,0.21f);
            if (context.started) _isFireingChange2 = true;
            else if (context.canceled) _isFireingChange2 = false;
        }
    }
    public void GunModeChangeBullet(InputAction.CallbackContext context)
    {
        if (context.performed && GameData.PlayerMoveAble >= 5 && _isAlive)
        {
            _scPAttack.SetGunModeChange(MODE_GUN.Physical, 0.21f);
            if (context.started) _isFireingChange2 = true;
            else if (context.canceled) _isFireingChange2 = false;
        }
        if (GameData.PlayerMoveAble >= 4)
        {

        }
    }
    public void GunModeChangeRocket(InputAction.CallbackContext context)
    {
        if (context.performed && GameData.PlayerMoveAble >= 5 && _isAlive)
        {
            _scPAttack.SetGunModeChange(MODE_GUN.Crash, 0.21f);
            if (context.started) _isFireingChange2 = true;
            else if (context.canceled) _isFireingChange2 = false;
        }
        if (GameData.PlayerMoveAble >= 4)
        {

        }
    }
    public void GunModeChangeMine(InputAction.CallbackContext context)
    {
        if (context.performed && GameData.PlayerMoveAble >= 5 && _isAlive)
        {
            _scPAttack.SetGunModeChange(MODE_GUN.Heat, 0.21f);
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
        if (isAction && GameData.PlayerMoveAble >= 5) _scPAttack.SetGunModeChange(MODE_GUN.Shining, 0.21f);
        if (GameData.PlayerMoveAble >= 4 && _isAlive)
        {
            if (isAction) _isFireingChange2 = true;
            else _isFireingChange2 = false;
        }
    }
    public void GunModeChangeBulletStick(InputAction.CallbackContext context)
    {
        bool isAction = context.ReadValue<float>() > 0.95f;
        if (isAction && GameData.PlayerMoveAble >= 5) _scPAttack.SetGunModeChange(MODE_GUN.Physical, 0.21f);
        if (GameData.PlayerMoveAble >= 4 && _isAlive)
        {
            if (isAction) _isFireingChange2 = true;
            else _isFireingChange2 = false;
        }
    }
    public void GunModeChangeRocketStick(InputAction.CallbackContext context)
    {
        bool isAction = context.ReadValue<float>() > 0.95f;
        if (isAction && GameData.PlayerMoveAble >= 5) _scPAttack.SetGunModeChange(MODE_GUN.Crash, 0.21f);
        if (GameData.PlayerMoveAble >= 4 && _isAlive)
        {
            if (isAction) _isFireingChange2 = true;
            else _isFireingChange2 = false;
        }
    }
    public void GunModeChangeMineStick(InputAction.CallbackContext context)
    {
        bool isAction = context.ReadValue<float>() > 0.95f;
        if (isAction && GameData.PlayerMoveAble >= 5) _scPAttack.SetGunModeChange(MODE_GUN.Heat, 0.21f);
        if (GameData.PlayerMoveAble >= 4 && _isAlive)
        {
            if (isAction) _isFireingChange2 = true;
            else _isFireingChange2 = false;
        }
    }
    #endregion

    private void FixedUpdate()
    {
        MODE_GUN gunMode = _scPAttack.GetGunMode();

        //重力
        Ray2D playerFootRay = new Ray2D(transform.position - new Vector3(16, 20 + 1.0f, 0), new Vector2(32, 0));
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
            _floorMode = floor.GetComponent<FloorC>().GetFloorMode();
            _posOwn = transform.position;

            transform.position = new Vector3(_posOwn.x
                , floor.transform.position.y + (floor.GetComponent<BoxCollider2D>().size.y / 2) + ((GetComponent<BoxCollider2D>().size.y - GetComponent<BoxCollider2D>().offset.y) / 2), 0);

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
        transform.localPosition += new Vector3(_speedMove, 0, 0);


        //TPEffect
        //if (GameData.TP >= 1) TPEffect();
        //else Gui.sprite = gn;



        //ジャンプ中は風の影響を強く受けます。
        if (_isGround) transform.position += new Vector3(GameData.WindSpeed / 10, 0, 0);
        else transform.position += new Vector3(GameData.WindSpeed / 5, 0, 0);


        //graphic
        //ジャンプ中は歩きGur
        if (_timeWarking < 4) _timeWarking++;
        if (_isJumping)
        {
            _srOwn.sprite = _playerGurWark[(int)gunMode];
        }
        else if (_isWalking == true)
        {
            if (_timeWarking > 3)
            {
                if (_srOwn.sprite == _playerGurNormal[(int)gunMode]) _srOwn.sprite = _playerGurWark[(int)gunMode];
                else if (_srOwn.sprite == _playerGurWark[(int)gunMode]) _srOwn.sprite = _playerGurNormal[(int)gunMode];
                _timeWarking = 0;
            }
        }
        else
        {
            _srOwn.sprite = _playerGurNormal[(int)gunMode];
        }
        if (_scPAttack.CheckIsChanging())
        {
            _srOwn.sprite = _playerChanging;
            _spriteREffect.sprite = null;
        }
        if (_hp <= 0)
        {
            _srOwn.sprite = _playerDeath;
            _spriteREffect.sprite = null;
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
    }

    /// <summary>
    /// 武器エフェクトアニメーション
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetEffectAnim()
    {
        GunStates state = _scPAttack.GetWeaponState(_scPAttack.GetGunMode());

        Sprite[] sprite = state.spritesWeapon;

        for (int i = 0; i < sprite.Length; i++)
        {
            if (state.isLoaded)
            {
                _spriteREffect.sprite = sprite[i];
            }
            else _spriteREffect.sprite = null;

            yield return new WaitForFixedUpdate();
            if (state.cooltimeNow <= 0) yield return new WaitForSeconds(0.07f);
        }
        StartCoroutine(SetEffectAnim());
    }

    public void ResetPlayerAnim()
    {
        MODE_GUN gunMode = _scPAttack.GetGunMode();
        _srOwn.sprite = _playerGurNormal[(int)gunMode];
    }

    /// <summary>
    /// コントローラー振動管理
    /// </summary>
    public void VibrationCritical()
    {
        if (GameData.IsVibration) StartCoroutine(VibrationControllerCritical());
    }

    private IEnumerator VibrationControllerCritical()
    {
        var gamepad = Gamepad.current;

        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(1.0f, 1.0f);
            yield return new WaitForSeconds(0.1f);
            gamepad.SetMotorSpeeds(0.0f, 0.0f);
        }
    }

    private IEnumerator VibrationControllerDamage()
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
        if(_isAlive){
            _isWalking = true;
            _isRight = true;
            _spriteREffect.flipX = true;
            _srOwn.flipX = true;
            _speedMove += mo;
            if (_speedMove >= _moveMax) _speedMove = _moveMax;
        }
    }

    /// <summary>
    /// 左移動
    /// </summary>
    /// <param name="mo"></param>
    private void MoveLeft(float mo)
    {
        if (_isAlive)
        {
            _isWalking = true;
            _isRight = false;
            _spriteREffect.flipX = false;
            _srOwn.flipX = false;
            _speedMove -= mo;
            if (_speedMove <= -_moveMax) _speedMove = -_moveMax;
        }

    }

    /// <summary>
    /// 静止
    /// </summary>
    private void MoveStop()
    {
        if (_floorMode == MODE_FLOOR.IceFloor)
        {
            //IceFloor
            _isWalking = false;
            if (!_isRight && _speedMove != 0)
            {
                //pfast -= 2;
                if (_speedMove <= -12) _speedMove = -12;
            }
            else if (_isRight && _speedMove != 0)
            {
                //pfast += 2;
                if (_speedMove >= 12) _speedMove = 12;
            }
        }
        else
        {
            //Stop
            _isWalking = false;
            if (_speedMove > 1) _speedMove--;
            else if (_speedMove < -1) _speedMove++;
            else if (_speedMove <= 1 && _speedMove >= -1) _speedMove = 0;
        }
    }

    /// <summary>
    /// 壁跳ね返り
    /// </summary>
    /// <param name="mpos"></param>
    private void MoveBound(Vector3 mpos)
    {
        transform.localPosition = mpos;
        if (_floorMode == MODE_FLOOR.IceFloor)
        {
            _speedMove = -_speedMove;
        }
    }

    /// <summary>
    /// ジャンプ
    /// </summary>
    private void MoveJump()
    {
        if (_isAlive)
        {
            if (_isGround)
            {
                _gravityNow = -30;
                _isJumping = true;
            }
        }

    }

    /// <summary>
    /// 降下
    /// </summary>
    private void MoveDown()
    {
        if (_isAlive)
        {
            if (_isGround && !_isnowBedRock)
            {
                _isDowning = true;
                //_gravityNow = 11;
            }
        }
    }

    /// <summary>
    /// 回復
    /// </summary>
    /// <param name="heal"></param>
    public bool SetHeal(int heal)
    {
        if (_hp < GameData.GetMaxHP() || GameData.IsStageMovingAction)
        {
            _audioGO.PlayOneShot(healS);
            _hp += heal;
            for (int i = 0; i < 7; i++)
            {
                ExpC shot = Instantiate(HealEffectPrefab, _posOwn, _rotOwn);
                shot.ShotEXP(Random.Range(80, 100), Random.Range(10, 15), 0.3f);
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// 必殺チャージ
    /// </summary>
    /// <param name="dhp"></param>
    public bool SetAddTPPlus1()
    {
        if (_tp < 5 || GameData.IsStageMovingAction)
        {
            _audioGO.PlayOneShot(magicgetS);
            if (_tp < 5) _tp += 1;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 統合ダメージ
    /// </summary>
    /// <param name="powerAttack"></param>
    public void SetDamage(int powerAttack, float addInvincible)
    {
        if (powerAttack > 0 && !GameData.IsInvincible&&_isAlive)
        {
            if (_timeInvincible <= 0)
            {
                StartCoroutine(VibrationControllerDamage());
                _goCamera.GetComponent<CameraShakeC>().StartShakeBeside(5, 10);
                _hp -= powerAttack;
                if (_hp <= 0)
                {
                    _hp = 0;
                    if (_isAlive) DoDead();
                }
                _audioGO.PlayOneShot(damageS);
                Instantiate(BDE, GameData.WindowSize / 2, _rotOwn).ShotEXP(0, 0, 0.1f);
                for (int i = 0; i < 10; i++)
                {
                    _rotOwn = transform.localRotation;
                    Instantiate(DamagePrefab, _posOwn, _rotOwn).ShotEXP(Random.Range(0, 360), Random.Range(5, 20), 0.3f);
                }
                _timeInvincible = addInvincible;
            }
        }

    }

    /// <summary>
    /// 死ぬ
    /// </summary>
    private void DoDead()
    {
        _isAlive = false;
        _gameManager.SetDeadPlayer();
        _speedMove = 0;
        _isFireing = false;
        _isFireing2 = false;
    }


    /// <summary>
    /// 敵場所特定、自分とのアングルを求める
    /// </summary>
    /// <returns></returns>
    private float GetTagPosition(Vector3 _posOwn)
    {
        GameObject[] myObjects;
        myObjects = GameObject.FindGameObjectsWithTag("Enemy");
        if (myObjects.Length > 0)
        {
            Vector3 enemyPos = GameData.FixPosition(myObjects[Random.Range(0, myObjects.Length)].transform.position, 32, 32);
            return Moving2DSystems.GetAngle(_posOwn, enemyPos);
        }
        else
        {
            if (GameData.Round == 0)
            {
                myObjects = GameObject.FindGameObjectsWithTag("Target");
                if (myObjects.Length > 0)
                {
                    Vector3 enemyPos = GameData.FixPosition(myObjects[Random.Range(0, myObjects.Length)].transform.position, 32, 32);
                    return Moving2DSystems.GetAngle(_posOwn, enemyPos);
                }
            }
            return _isRight ? 0 : 180;
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
            return Moving2DSystems.GetAngle(_posOwn, flontEnemy.transform.position);
        }

        //チュートリアル中であれば的探してアングル返して終了
        if (GameData.Round == 0)
        {
            GameObject[] myObjects;
            myObjects = GameObject.FindGameObjectsWithTag("Target");
            if (myObjects.Length > 0) return Moving2DSystems.GetAngle(_posOwn, myObjects[Random.Range(0, myObjects.Length)].transform.position);
        }
        return _isRight ? 0 : 180;
    }

    /// <summary>
    /// 前方の敵場所特定
    /// </summary>
    /// <returns></returns>
    public GameObject GetFlontEnemy()
    {
        bool find = false;
        GameObject[] myObjects;
        myObjects = GameObject.FindGameObjectsWithTag("Enemy");

        for (int hoge = 0; hoge < myObjects.Length; hoge++)
        {

            if (!_isRight) find = myObjects[hoge].transform.position.x < _posOwn.x;
            else find = myObjects[hoge].transform.position.x >= _posOwn.x;

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
    public void DoAllPlayerStageMoveAction()
    {
        if (!_isAlive)
        {
            _isAlive = true;
            SetHeal(1);
            _scPAttack.Summon_Child();
        }

        if (!GameData.IsStageMovingAction)
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
        if (CheckIsLeader())
        {
            GameObject.Find("MeterBody").GetComponent<RoundMeterC>().SetTileSpritesFull();
        }
        _speedMove = 0;
        while (!_isGround)
        {
            yield return new WaitForFixedUpdate();
        }

        GameData.PlayerMoveAble = 0;
        _isFireing = false;
        _isFireing2 = false;
        _isFireingChange2 = false;
        _isMoveR = false;
        _isMoveL = false;
        yield return new WaitForSeconds(2.0f);

        //同時に着くように調節
        float moveEdge = GameData.WindowSize.x + 32;
        float moveMax = _moveMax;
        _moveMax = (moveEdge - _posOwn.x) / 33f;
        _speedMove = _moveMax;
        _isMoveR = true;

        yield return new WaitForSeconds(1.00f);
        
        _isMoveR = false;
        _moveMax = moveMax;

        transform.position = new Vector3(32, _posOwn.y, 0);
        if (CheckIsLeader())
        {
            GameData.DeleteAllEnemys();
            GameData.DeleteAllEMissiles();
            AllEnemyDelete();
            GameData.Point += 100000;
        }

        yield return new WaitForSeconds(1.0f);

        if (CheckIsLeader())
        {
            Instantiate(_prfbTextStage, GameData.WindowSize / 2 + Vector2.up * 84, Quaternion.Euler(0, 0, 0));
        }

        yield return new WaitForSeconds(1.0f);

        if (CheckIsLeader())
        {
            GameData.IsInvincible = false;
            GameData.PlayerMoveAble = 6;
            GameData.IsTimerMoving = true;
            GameData.IsStageMovingAction = false;
        }

    }

    /// <summary>
    /// プレイヤー番号がゼロ（唯一無二のリーダー）かを返す
    /// </summary>
    /// <returns></returns>
    public bool CheckIsLeader() => _numberPlayer == 0;

    private void AllEnemyDelete() => TagDelete("Effect");

    private void TagDelete(string tagName)
    {
        GameObject[] myObjects = GameObject.FindGameObjectsWithTag(tagName);
        for (int zz = 0; zz < myObjects.Length; zz++)
        {
            Destroy(myObjects[zz]);
        }
    }

    /// <summary>
    /// その武器のクールタイムを返す
    /// </summary>
    public float GetCoolTime(MODE_GUN mode) => _scPAttack.CheckEnergy(mode);

    /// <summary>
    /// 現在の武器を取得
    /// </summary>
    public MODE_GUN CheckWeaponValue() => _scPAttack.GetGunMode();

    /// <summary>
    /// その武器に強制変更
    /// </summary>
    /// <param name="mode"></param>
    public void ChangeWeapon(MODE_GUN mode) => _scPAttack.SetGunModeChange(mode, 0.03f);

    /// <summary>
    /// その武器の遠距離技が使えるか確認
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool CheckIsAbleChargeShot(MODE_GUN mode) => _scPAttack.CheckIsAbleChargeShot(mode);

    /// <summary>
    /// その武器が使用可能か確認
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool CheckIsLoaded(MODE_GUN mode) => _scPAttack.GetWeaponState(mode).isLoaded;

    /// <summary>
    /// HPを調べる
    /// </summary>
    /// <returns></returns>
    public int GetHP() => _hp;

    /// <summary>
    /// TPを調べる
    /// </summary>
    /// <returns></returns>
    public int GetTP() => _tp;

    /// <summary>
    /// TPの使用を試す。成功の有無を返す
    /// </summary>
    /// <returns></returns>
    public bool SetUseTP(int value)
    {
        if (_tp >= value)
        {
            _tp -= value;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 生きているかチェック
    /// </summary>
    /// <returns></returns>
    public bool CheckIsAlive() => _isAlive;

    /// <summary>
    /// HP強制変更
    /// </summary>
    /// <param name="value"></param>
    public void SetHP(int value) => _hp = value;

    /// <summary>
    /// 右を向いているか
    /// </summary>
    /// <returns></returns>
    public bool CheckPlayerAngleIsRight() => _isRight;

    /// <summary>
    /// Specialかどうか
    /// </summary>
    /// <returns></returns>
    public bool CheckIsSpecial() => _scPAttack.CheckIsSpecial();


    /// <summary>
    /// 中断
    /// </summary>
    /// <param name="context"></param>
    public void OnPouseEnd(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DoPouse();
        }

    }

    private void DoPouse()
    {
        if (!GameData.IsStageMovingAction)
        {
            GameObject.Find("Pouse").GetComponent<PouseMenuC>().OnPouse();
        }
    }

    //Imput
    public void OnPouseStart(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameObject.Find("Pouse").GetComponent<PouseMenuC>().OnStart();
        }
    }

    public void OnPouseUp(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameObject.Find("Pouse").GetComponent<PouseMenuC>().OnUp();
        }
    }

    public void OnPouseDown(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameObject.Find("Pouse").GetComponent<PouseMenuC>().OnDown();
        }
    }
}
