using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アニメーションシステム
/// </summary>
public class GraphicC : MonoBehaviour
{
    private float _countTime = 0;

    [SerializeField]
    private float _countChange;

    private int _spritenumber = 0;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private bool _random;

    // Update is called once per frame
    void Update()
    {
        _countTime += Time.deltaTime;
        if (_random)
        {
            if (_countTime >= _countChange)
            {
                spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
                _countTime = 0;
            }
        }
        else
        {
            if (_countTime >= _countChange)
            {
                spriteRenderer.sprite = sprites[_spritenumber];
                _countTime = 0;
                _spritenumber++;
                if (_spritenumber >= sprites.Length)
                {
                    _spritenumber = 0;
                }
            }
        }

    }
}
