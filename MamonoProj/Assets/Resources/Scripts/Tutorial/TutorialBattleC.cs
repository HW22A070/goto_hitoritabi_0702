using EnumDic.Player;
using UnityEngine;

public class TutorialBattleC : MonoBehaviour
{
    [SerializeField, Header("0=移動\n1=攻撃1\n2=攻撃2\n3=変形")]
    private GameObject[] _levelPlate;

    private int _tutorialValue = -1;

    [SerializeField, Header("プレイヤー")]
    private GameObject _goPlayer;

    [SerializeField, Header("エネミーサモナー")]
    private GameObject _goEnemySummoner;

    private bool _isWeaponSetting;

    // Start is called before the first frame update
    void Start()
    {
        GameData.PlayerMoveAble = 3;
        _goEnemySummoner.SetActive(false);
        for (int i = 0; i < _levelPlate.Length; i++)
        {
            _levelPlate[i].SetActive(false);
        }
        GameData.IsTimerMoving = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < _levelPlate.Length; i++)
        {
            if (i == _tutorialValue) _levelPlate[i].SetActive(true);
            else _levelPlate[i].SetActive(false);
        }

        switch (_tutorialValue)
        {
            case -1:
                for (int i = 0; i < _levelPlate.Length; i++)
                {
                    _levelPlate[i].SetActive(false);
                }
                break;

            case 0:

                if (!_isWeaponSetting)
                {
                    switch ((GameData.Round - 1) / 5)
                    {
                        case 0:
                            _goPlayer.GetComponent<PlayerC>().ChangeWeapon(MODE_GUN.Physical);
                            break;
                        case 1:
                            _goPlayer.GetComponent<PlayerC>().ChangeWeapon(MODE_GUN.Physical);
                            break;
                        case 2:
                            _goPlayer.GetComponent<PlayerC>().ChangeWeapon(MODE_GUN.Shining);
                            break;
                        case 3:
                            _goPlayer.GetComponent<PlayerC>().ChangeWeapon(MODE_GUN.Heat);
                            break;
                        case 4:
                            _goPlayer.GetComponent<PlayerC>().ChangeWeapon(MODE_GUN.Crash);
                            break;
                        case 5:
                            _goPlayer.GetComponent<PlayerC>().ChangeWeapon(MODE_GUN.Crash);
                            break;
                    }
                    _isWeaponSetting = true;
                }

                if (_goPlayer.transform.position.y > 200) _tutorialValue = 1;
                break;

            case 1:
                if (GameData.StartRound == 1) GameData.PlayerMoveAble = 4;
                else GameData.PlayerMoveAble = 6;
                GameData.IsTimerMoving = true;
                _goEnemySummoner.SetActive(true);
                if (GameData.Round % 5 == 2) _tutorialValue = 2;
                break;

            case 2:
                if (GameData.Round % 5 == 3) _tutorialValue = 3;
                break;

            case 3:
                GameData.PlayerMoveAble = 6;
                if (GameData.Round % 5 == 0)
                {
                    GetComponent<Animator>().SetBool("IsActive", false);
                }
                break;


        }
    }

    private void InActive()
    {
        GetComponent<Animator>().SetBool("IsActive", true);
        _tutorialValue = 0;
    }

    private void DisActive()
    {
        gameObject.SetActive(false);
    }
}
