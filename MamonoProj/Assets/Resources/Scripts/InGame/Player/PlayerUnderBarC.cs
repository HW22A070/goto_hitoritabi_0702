using UnityEngine;
using UnityEngine.UI;

public class PlayerUnderBarC : MonoBehaviour
{
    /// <summary>
    /// ���@HP�p�o�[2
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
