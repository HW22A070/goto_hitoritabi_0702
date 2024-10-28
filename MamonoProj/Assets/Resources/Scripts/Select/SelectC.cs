using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SelectC : MenuSystemC
{
    [SerializeField]
    private SpriteRenderer _backGroundSR,_floorSR;

    [SerializeField]
    private Sprite[] _backGronud,_floorSp,_textSp;

    private int[] _stageLevel= {1,1,2,2,3,4,5};

    private bool VA, VB, VC, VD;


    private new void Start()
    {
        base.Start();
        _optionMax = 5;
    }

    // Update is called once per frame
    void Update()
    {
        _backGroundSR.sprite = _backGronud[_titleMode];
        _floorSR.sprite = _floorSp[_titleMode];

        if (VA && VB && VC && VD)
        {
            _titleMode = 6;
            _audioSource.PlayOneShot(selectS);
            MoveFlash();
        }
    }

    protected override void Option1() { StartCoroutine(DoOption(0)); }
    protected override void Option2() { StartCoroutine(DoOption(1)); }
    protected override void Option3() { StartCoroutine(DoOption(2)); }
    protected override void Option4() { StartCoroutine(DoOption(3)); }
    protected override void Option5() { StartCoroutine(DoOption(4)); }
    protected override void Option6() { StartCoroutine(DoOption(5)); }
    protected override void Option7() { StartCoroutine(DoOption(6)); }

    private IEnumerator DoOption(int mode)
    {

        yield return new WaitForSeconds(1.0f);
        if (mode == 0)
        {
            GameData.GoalRound = 5;
            GameData.StartRound = 1;
            SceneManager.LoadScene("Level");
        }
        else if (mode == 1)
        {
            GameData.GoalRound = 10;
            GameData.StartRound = 6;
            SceneManager.LoadScene("Level");
        }
        else if (mode == 2)
        {
            GameData.GoalRound = 15;
            GameData.StartRound = 11;
            SceneManager.LoadScene("Level");
        }
        else if (mode == 3)
        {
            GameData.GoalRound = 20;
            GameData.StartRound = 16;
            SceneManager.LoadScene("Level");
        }
        else if (mode == 4)
        {
            GameData.GoalRound = 25;
            GameData.StartRound = 21;
            SceneManager.LoadScene("Level");
        }
        else if (mode == 5)
        {
            GameData.GoalRound = 30;
            GameData.StartRound = 26;
            SceneManager.LoadScene("Level");
        }
        else if (mode == 6)
        {
            GameData.StartRound = 31;
            GameData.GoalRound = 35;
            SceneManager.LoadScene("Level");
        }
    }


    public int StageLevelSender()
    {
        return _stageLevel[_titleMode];
    }

    public void OnBug1(InputAction.CallbackContext context)
    {
        if (context.started) VA = true;
        else if (context.canceled) VA = false;
    }

    public void OnBug2(InputAction.CallbackContext context)
    {
        if (context.started) VB = true;
        else if (context.canceled) VB = false;
    }

    public void OnBug3(InputAction.CallbackContext context)
    {
        if (context.started) VC = true;
        else if (context.canceled) VC = false;
    }

    public void OnBug4(InputAction.CallbackContext context)
    {
        if (context.started) VD = true;
        else if (context.canceled) VD = false;
    }

    public void OnEnd(InputAction.CallbackContext context)
    {
        if (context.performed && !start)
        {
            SceneManager.LoadScene("Title");
        }
    }
}
