using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersManagerC : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _playerList;

    [SerializeField]
    private List<PlayerHPStatesC> _hpBars;

    [SerializeField, Header("1Pステータス")]
    private GameObject _ui1PStates;

    [SerializeField,Header("2Pステータス")]
    private GameObject _ui2PStates;

    [SerializeField, Header("1P武器アイコン")]
    private GameObject _ui1PCoolDown;

    [SerializeField, Header("タイマー")]
    private GameObject _uiTimer;

    void Start()
    {
        /*
        //HPバーよりプレイヤーが多ければ上限を下げる
        if (GameData.MultiPlayerCount > _hpBars.Count)
        {
            GameData.MultiPlayerCount = _hpBars.Count;
        }
        */

        //プレイヤーの数だけ生成、ステータス周りの配置、ナンバー振り分けを行う
        for (int i = 0; i < GameData.MultiPlayerCount; i++)
        {

            GameObject player = GetComponent<PlayerInputManager>().JoinPlayer().gameObject;
            player.transform.position = new Vector3(100 + ((GameData.WindowSize.x - 100) / GameData.MultiPlayerCount * (GameData.MultiPlayerCount - 1 - i)), 150, 0);
            _playerList.Add(player);
            //_hpBars[i].gameObject.SetActive(true);
            //_hpBars[i].SetHPBar(player);
            player.GetComponent<PlayerC>().SetPlayerNumber(i);
        }


        //2人以上なら場所確保のため1PクールダウンUIを消す
        if (GameData.MultiPlayerCount >= 2) _ui1PCoolDown.SetActive(false);

        //2人なら2Pステータスを表示
        if (GameData.MultiPlayerCount == 2) _ui2PStates.SetActive(true);

        //2人なら場所確保のためタイマーを隠す
        if (GameData.MultiPlayerCount == 2) _uiTimer.transform.position+=Vector3.up*2048;

        //3人以上なら場所確保のため1Pステータスを消す
        if (GameData.MultiPlayerCount >= 3) _ui1PStates.SetActive(false);
    }

    /// <summary>
    /// ランダムなプレイヤーを与える
    /// </summary>
    /// <returns></returns>
    public GameObject GetRandomPlayer() => _playerList[Random.Range(0, _playerList.Count)];

    /// <summary>
    /// すべてのプレイヤー
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetAllPlayers() => _playerList;

    /// <summary>
    /// 画面右に飛び出してないプレイヤー
    /// </summary>
    /// <returns></returns>
    public int GetDidMoveActionPlayers()
    {
        int movedCount = 0;
        foreach (GameObject player in _playerList)
        {
            if (player.transform.position.x < GameData.WindowSize.x + 32)
            {
                movedCount++;
            }
        }
        return movedCount;
    }

    /// <summary>
    /// 生きてるプレイヤー
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetAlivePlayers()
    {
        List<GameObject> alivePlayers = new List<GameObject> { };

        foreach (GameObject player in _playerList)
        {
            if (player.GetComponent<PlayerC>().CheckIsAlive()) alivePlayers.Add(player);
        }

        return alivePlayers;
    }

    /// <summary>
    /// 生きてるプレイヤーの数
    /// </summary>
    /// <returns></returns>
    public int GetAlivePlayersCount() => GetAlivePlayers().Count;

    /// <summary>
    /// 生きてる中からランダムに選出
    /// </summary>
    /// <returns></returns>
    public GameObject GetRandomAlivePlayer()
    {
        if (GetAlivePlayersCount() > 0)
        {
            List<GameObject> alives = GetAlivePlayers();
            return alives[Random.Range(0, alives.Count)];
        }
        return _playerList[0];
    }

    /// <summary>
    /// 指定プレイヤーを返す
    /// </summary>
    /// <returns></returns>
    public GameObject GetPlayerByNumber(int number) => _playerList[number];

}