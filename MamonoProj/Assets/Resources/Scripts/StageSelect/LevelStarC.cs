using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStarC : MonoBehaviour
{
    [SerializeField]
    private int hpvalue;

    [SerializeField]
    private SpriteRenderer _spHP;

    [SerializeField]
    private Sprite _red, _void;

    
    private GameObject _gameobjectSelectP;

    private void Start()
    {
        _gameobjectSelectP = GameObject.Find("SelectManager");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_gameobjectSelectP.GetComponent<SelectC>().StageLevelSender() < hpvalue) _spHP.sprite = _void;
        else _spHP.sprite = _red;
    }
}
