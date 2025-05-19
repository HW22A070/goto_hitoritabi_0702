using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFaceC : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _srFace;

    [SerializeField]
    private Sprite[] _spFaces;

    private PlayerC _scPlayer;

    public void SetFace(int number)
    {
        _srFace.sprite = _spFaces[number];
        _scPlayer = GetComponent<PlayerC>();
    }

    private void Update()
    {
       _srFace.flipX= _scPlayer.CheckPlayerAngleIsRight();
    }
}
