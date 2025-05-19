using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDic.Enemy;

public class DicLogoC : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _sprites;

    private int _posX,_posY;
    
    public void SetSprite(KIND_ENEMY enemy,int posX,int posY)
    {
        GetComponent<SpriteRenderer>().sprite = _sprites[(int)enemy];
        _posX = posX;
        _posY = posY;
    }

    public void SetSprite(KIND_BOSS enemy, int posX, int posY)
    {
        GetComponent<SpriteRenderer>().sprite = _sprites[(int)enemy];
        _posX = posX;
        _posY = posY;
    }

    public bool CheckIsSelecting(int x, int y) => x == _posX && y == _posY;

    // Update is called once per frame
    void Update()
    {
        
    }
}
