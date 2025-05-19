using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーのHPバー管理
/// </summary>
public class PlayerHPStatesC : MonoBehaviour
{
    /// <summary>
    /// 自機HP用バー
    /// </summary>
    [SerializeField]
    private Slider _playerHpBar;

    [SerializeField]
    [Header("自機HP表示")]
    private Text _hpText;

    [SerializeField,Min(0)]
    private int _number=0;

    private GameObject _playerGO;

    [SerializeField, Header("Player")]
    private PlayersManagerC _scPlsM;

    // Update is called once per frame
    void Update()
    {
        _playerGO = _scPlsM.GetPlayerByNumber(_number);
        //HPメーター処理
        _playerHpBar.value = (float)_playerGO.GetComponent<PlayerC>().GetHP() / GameData.GetMaxHP();

        //テキスト表示
        if (GameData.VirusBugEffectLevel ==EnumDic.Enemy.Virus.MODE_VIRUS.None)
        {
            _hpText.text = _playerGO.GetComponent<PlayerC>().GetHP().ToString() + " / " + GameData.GetMaxHP().ToString();
        }
    }
}
