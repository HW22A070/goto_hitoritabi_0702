using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingC : MonoBehaviour
{
    private void Start() => StartCoroutine(LoadSceneAsync());

    IEnumerator LoadSceneAsync()
    {
        GameData.Round = GameData.StartRound;
        GameData.LastCrearLound = 0;
        ClearC.BossBonus = 0;
        ClearC.DeathCount=0;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
