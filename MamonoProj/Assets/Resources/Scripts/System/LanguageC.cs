using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 英語対応
/// </summary>
public class LanguageC : MonoBehaviour
{
    [SerializeField,Header("日本語")]
    private string _jp;

    [SerializeField, Header("にほんご")]
    private string _kids;

    [SerializeField, Header("English")]
    private string _en;

    private Text _ownText;

    // Start is called before the first frame update
    void Start()
    {
        _ownText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameData.Language)
        {
            case 0:
                _ownText.text = _jp;
                break;
            case 1:
                _ownText.text = _kids;
                break;
            case 2:
                _ownText.text = _en;
                break;
        }
    }
}
