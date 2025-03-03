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

    /// <summary>
    /// 自機HP用バー2
    /// </summary>
    [SerializeField]
    private Slider _playerHpBar2;


    [SerializeField]
    [Header("自機HP表示")]
    private Text _hpText;

    [SerializeField, Header("Player")]
    private GameObject _playerGO;

    // Update is called once per frame
    void Update()
    {

        //HPメーター処理
        _playerHpBar.value = (float)_playerGO.GetComponent<PlayerC>().GetHP() / GameData.GetMaxHP();
        _playerHpBar2.value = (float)_playerGO.GetComponent<PlayerC>().GetHP() / GameData.GetMaxHP();

        //テキスト表示
        if (GameData.VirusBugEffectLevel ==EnumDic.Enemy.Virus.MODE_VIRUS.None)
        {
            _hpText.text = _playerGO.GetComponent<PlayerC>().GetHP().ToString() + " / " + GameData.GetMaxHP().ToString();
        }
    }
}
