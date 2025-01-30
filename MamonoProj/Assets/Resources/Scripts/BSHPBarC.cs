using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BSHPBarC : MonoBehaviour
{
    private ECoreC _scBoss;

    private int _beforeHP;

    private byte _byteUnBlue = 0;

    private Slider _sliderHP;

    [SerializeField]
    private Image _imageHP;

    private bool _isBoss;

    private int _ofsetX;
    
    private void Start()
    {
        _scBoss = transform.parent.parent.gameObject.GetComponent<ECoreC>();
        _sliderHP = GetComponent<Slider>();
        if (_scBoss.IsBoss) _ofsetX = 100;
        else _ofsetX = 20;

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        int nowHP = _scBoss.TotalHp;
        if (_beforeHP> nowHP)
        {
            _byteUnBlue= 255;
        }
        else
        {
            if (_byteUnBlue > 0)
            {
                _byteUnBlue -= 17;
                if (_byteUnBlue < 0) _byteUnBlue = 0;
            }
        }
        _imageHP.color = new Color32(_byteUnBlue, _byteUnBlue, 255, 255);
        _beforeHP = nowHP;

        transform.eulerAngles = Vector3.zero;
        _sliderHP.value = (float)nowHP / (float)_scBoss.TotalHpFirst;
        transform.localPosition = Vector3.zero;
        if (transform.position.y < 10) transform.position = new Vector3(transform.position.x, 10,0);
        if (transform.position.y > GameData.WindowSize.x-10) transform.position = new Vector3(transform.position.x, GameData.WindowSize.x - 10, 0);
        if (transform.position.x < _ofsetX) transform.position = new Vector3(_ofsetX, transform.position.y, 0);
        if (transform.position.x > GameData.WindowSize.x - _ofsetX) transform.position = new Vector3(GameData.WindowSize.x - _ofsetX, transform.position.y, 0);
    }
}
