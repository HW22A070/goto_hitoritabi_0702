using UnityEngine;
using UnityEngine.UI;

public class PlayerUnderBarC : MonoBehaviour
{
    /// <summary>
    /// 自機HP用バー2
    /// </summary>
    [SerializeField]
    private Slider _playerHpBar;

    [SerializeField]
    private PlayerC _player;

    // Update is called once per frame
    void Update()
    {
        _playerHpBar.value = (float)_player.GetHP() / GameData.GetMaxHP();
    }
}
