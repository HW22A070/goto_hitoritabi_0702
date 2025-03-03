using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersManagerC : MonoBehaviour
{
    /*
    [SerializeField]
    private List<GameObject> _playerList;

    [SerializeField]
    private List<PlayerHPStatesC> _hpBars;

    [SerializeField]
    private GameObject _canvas;

    [SerializeField]
    private GameObject _coolDownUI;

    void Start()
    {
        //HPバーよりプレイヤーが多ければ上限を下げる
        if (GameData.MultiPlayerCount > _hpBars.Count)
        {
            GameData.MultiPlayerCount = _hpBars.Count;
        }

        //プレイヤーの数だけ生成、ステータス周りの配置、ナンバー振り分けを行う
        for (int i = 0; i < GameData.MultiPlayerCount; i++)
        {

            GameObject player = GetComponent<PlayerInputManager>().JoinPlayer().gameObject;
            player.transform.position = new Vector3(100 + ((GameData.WindowSize.x - 100) / GameData.MultiPlayerCount * (GameData.MultiPlayerCount - 1 - i)), 150, 0);
            _playerList.Add(player);
            _hpBars[i].gameObject.SetActive(true);
            _hpBars[i].SetHPBar(player);
            player.GetComponent<PlayerC>().SetPlayerNumber(i);
            //_playerHPstates.transform.parent = _canvas.transform;
        }

        //マルチなら場所確保のためクールダウンUIを消す
        if (GameData.MultiPlayerCount >= 2) _coolDownUI.SetActive(false);

    }

    /// <summary>
    /// ランダムなプレイヤーを与える
    /// </summary>
    /// <returns></returns>
    public GameObject GetRandomPlayer()
    {
        return _playerList[Random.Range(0, _playerList.Count)];
    }

    /// <summary>
    /// すべてのプレイヤー
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetAllPlayers()
    {
        return _playerList;
    }

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
            if (player.GetComponent<PlayerC>().GetAlive()) alivePlayers.Add(player);
        }

        return alivePlayers;
    }

    /// <summary>
    /// 生きてるプレイヤーの数
    /// </summary>
    /// <returns></returns>
    public int GetAlivePlayersCount()
    {
        return GetAlivePlayers().Count;
    }

    /// <summary>
    /// 生きてる中からランダムに選出
    /// </summary>
    /// <returns></returns>
    public GameObject GetRandomAlivePlayer()
    {
        List<GameObject> alives = GetAlivePlayers();
        return alives[Random.Range(0, alives.Count)];
    }
    */
}