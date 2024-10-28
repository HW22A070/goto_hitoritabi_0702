using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SetumeiC : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite a, b;

    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        GameData.Round = GameData.StartRound;
        GameData.LastCrearLound = 0;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");
        /*
        GameData.Round = 1;
        GameData.HP = 20;
        GameData.Boss = 0;
        FloorManagerC.StageIce(100) = 0;
        GameData.Star = false;
        GameData.TP = 0;
        GameData.Point = 0;
        GameData.GameMode = 0;
        SceneManager.LoadScene("Title");
        */

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
