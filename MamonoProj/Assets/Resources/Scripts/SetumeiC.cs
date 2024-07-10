using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SetumeiC : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite a, b;

    private bool VA,VB,VC,VD;

    private void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (VA && VB && VC && VD)
        {
            GameData.Zuru = true;
            GameData.Round = 35;
            SceneManager.LoadScene("Game");
        }
    }

    public void OnStart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameData.Round = GameData.StartRound;
            GameData.LastCrearLound = 0;
            SceneManager.LoadScene("Game");
        }
    }

    public void OnEnd(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameData.Round = 1;
            GameData.HP = 20;
            GameData.Boss = 0;
            GameData.IceFloor = 0;
            GameData.Star = false;
            GameData.TP = 0;
            GameData.Score = 0;
            GameData.GameMode = 0;
            SceneManager.LoadScene("Title");
        }
    }

    public void OnLeft(InputAction.CallbackContext context)
    {
        if (context.performed) spriteRenderer.sprite = a;
    }

    public void OnRifht(InputAction.CallbackContext context)
    {
        if (context.performed) spriteRenderer.sprite = b;
    }

    public void OnBug1(InputAction.CallbackContext context)
    {
        if (context.started) VA = true;
        else if (context.canceled) VA = false;
    }

    public void OnBug2(InputAction.CallbackContext context)
    {
        if (context.started) VB = true;
        else if (context.canceled) VB = false;
    }

    public void OnBug3(InputAction.CallbackContext context)
    {
        if (context.started) VC = true;
        else if (context.canceled) VC = false;
    }

    public void OnBug4(InputAction.CallbackContext context)
    {
        if (context.started) VD = true;
        else if (context.canceled) VD = false;
    }
}
