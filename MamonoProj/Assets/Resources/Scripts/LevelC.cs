using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using EnumDic.System;

public class LevelC : MenuSystemC
{
    [SerializeField]
    private SpriteRenderer _background;

    [SerializeField]
    private Sprite[] _back;

    [SerializeField]
    private Text _textDisc,_textLevel;

    [SerializeField]
    private AudioSource _asBGM;

    [SerializeField]
    private AudioClip[] _acBGMs;

    /// <summary>
    /// 1=dif
    /// 2=lang
    /// </summary>
    private string[,] _disc = new string[4, 3]{
        {
            "安全性に重視を置いた　最新のマシン。\n攻撃力は低めだが　初心者でも簡単に扱えます。"
            ,"あんぜんじゅうしの　さいしんのマシン。\nこうげきりょくは　ひくめだが\nしょしんしゃでも\nかんたんに　あつかえます。"
            ,"A state-of-the-art machine that emphasizes safety.\nAlthough its attack power is low, even beginners can use it easily."},
        {
            "平均的な性能のマシン。\n慣れてきたらちょうど良い使いやすさです。"
            ,"へいきんてきな　せいのうの　マシン。\nなれてきたら　ちょうどよい　つかいやすさです。"
            ,"A machine with average performance.\nOnce you get used to it, it's just easy to use."},
        {
            "攻撃性能に特化した高火力マシン。\n攻撃力の分、耐久性や記憶力がかなり低いため\nこれでクリアできたら上級者！"
            ,"こうげきにとっかした　こうかりょくマシン。\nたいきゅうせいや\nきおくりょくがかなりひくく\nこれでクリアできたら　じょうきゅうしゃ！"
            ,"A high-powered machine specialized in attack performance.\nIts durability and memory are quite low to compensate for its attack power, so if you can clear it, you're an advanced player!"},
        {
            "耐久力と記憶力を捨て\n攻撃にすべてを捧げた凶悪戦闘狂マシン。\nこれでクリアできる想定はされていません。"
            ,"たいきゅうりょくと　きおくりょくをすて\nこうげきに　すべてをささげた\nせんとうきょうマシン。\nこれでクリアできる　そうていは\nされていません。"
            ,"A brutal battle-crazed machine that has abandoned durability and memory and dedicated everything to attack.\nIt is not expected that you will be able to clear the level with this."},
    };

    private string[] _maxhpTag = new string[3] { "最大HP: ", "さいだいHP: ", "Max HP: "};

    private string[] _continueTag = new string[3] { "やりなおし: \n", "やりなおし: \n", "Continue: \n" };

    private int[] _maxHP = new int[4] { 100, 50, 20, 1 };

    /// <summary>
    /// 1=dif
    /// 2=lang
    /// </summary>
    private string[,] _continueDisc = new string[4, 3]{
        {
            "倒れたラウンドから　再挑戦"
            ,"たおれたラウンドから　さいちょうせん"
            ,"Re-challenge from the round you fail"},
        {
            "倒れたラウンドから　再挑戦"
            ,"たおれたラウンドから　さいちょうせん"
            ,"Re-challenge from the round you fail"},
        {
            "倒れたステージから　再挑戦"
            ,"たおれたステージから　さいちょうせん"
            ,"Re-challenge from the stage you fail"},
        {
            "再挑戦不可。最初のラウンドからやり直し"
            ,"さいしょの　ラウンドから　やりなおし"
            ,"No second attempts allowed. Start over from the first round."},
    };


    private new void Start()
    {
        base.Start();
        _optionMax = 3;
        _background.sprite = _back[(int)(GameData.StartRound - 1) / 5];

        _asBGM.clip = _acBGMs[(GameData.StartRound - 1) / 5];
        _asBGM.Play();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        _textDisc.text = _disc[_titleMode,GameData.Language];
        _textLevel.text = _maxhpTag[GameData.Language] + _maxHP[_titleMode].ToString() + "\n\n" + _continueTag[GameData.Language] + _continueDisc[_titleMode, GameData.Language];
    }

    protected override void Option1() => StartCoroutine(DoOption());
    protected override void Option2() => StartCoroutine(DoOption());
    protected override void Option3() => StartCoroutine(DoOption());
    protected override void Option4() => StartCoroutine(DoOption());

    private IEnumerator DoOption()
    {
        yield return new WaitForSeconds(1.0f);
        GameData.Difficulty = (MODE_DIFFICULTY)_titleMode;
        ClearC.DeathCount = 0;
        SceneManager.LoadScene("Setumei");
    }

    public void OnEnd(InputAction.CallbackContext context)
    {
        if (context.performed && !_isStart)
        {
            SceneManager.LoadScene("Title");
        }
    }
    
}
