using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirusTextsC : MonoBehaviour
{
    [SerializeField]
    private Text _textVirus;

    private List<string> _listText = new List<string> { };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 攻撃開始時や形態変化時
    /// </summary>
    /// <param name="text"></param>
    public void SetTextSystem(string text)
    {
        _listText.Add("<color=#ffbb00>"+text+"</color>");
        SetTextWindow();
    }

    /// <summary>
    /// 攻撃途中
    /// </summary>
    /// <param name="text"></param>
    public void SetTextDoingAction(string text)
    {
        _listText.Add("<color=#ffffff>" + text + "</color>");
        SetTextWindow();
    }

    /// <summary>
    /// ピンチ時
    /// </summary>
    /// <param name="text"></param>
    public void SetTextDamage(string text)
    {
        _listText.Add("<color=#ff0000>" + text + "</color>");
        SetTextWindow();
    }

    private void SetTextWindow()
    {
        if(_listText.Count>40) _listText.RemoveAt(0);
        string textWindow = "";
        foreach (string text in _listText)
        {
            textWindow += "\n" + text;
        }
        _textVirus.text = textWindow;
    }
}
