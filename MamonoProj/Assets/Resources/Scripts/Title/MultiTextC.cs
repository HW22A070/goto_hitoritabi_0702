using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiTextC : MonoBehaviour
{
    private Text _ownText;

    private string[,] _text = new string[2, 3]
    {
        {"ひとりで","ひとりで","1 Player"},
        {"ふたりで","ふたりで","2 Players"}
    };

    private void Start()
    {
        _ownText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _ownText.text = _text[GameData.MultiPlayerCount - 1, GameData.Language];
    }
}
