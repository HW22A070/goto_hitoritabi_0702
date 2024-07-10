using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LevelC : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite easy, normal, hard, noooo;

    float time = 0;
    bool start = false;

    public AudioClip startS,selectS;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }


    // Update is called once per frame
    void Update()
    {

        switch (GameData.Difficulty)
        {
            case 0:
                spriteRenderer.sprite = easy;
                break;
            case 1:
                spriteRenderer.sprite = normal;
                break;
            case 2:
                spriteRenderer.sprite = hard;
                break;
            case 3:
                spriteRenderer.sprite = noooo;
                break;
        }
        
        if (start)
        {
            time += Time.deltaTime;
            if (time > 1.0f)
            {
                SceneManager.LoadScene("Setumei");
            }
        }
    }

    public void OnStart(InputAction.CallbackContext context)
    {
        if (context.performed && !start)
        {
            start = true;
            audioSource.PlayOneShot(startS);
        }
    }

    public void OnEnd(InputAction.CallbackContext context)
    {
        if (context.performed && !start)
        {
            SceneManager.LoadScene("Title");
        }
    }

    public void OnLeft(InputAction.CallbackContext context)
    {
        if (context.performed && !start)
        {
            GameData.Difficulty--;
            if (GameData.Difficulty < 0) GameData.Difficulty = 3;
            audioSource.PlayOneShot(selectS);
        }
    }

    public void OnRifht(InputAction.CallbackContext context)
    {
        if (context.performed && !start)
        {
            GameData.Difficulty++;
            if (GameData.Difficulty > 3) GameData.Difficulty = 0;
            audioSource.PlayOneShot(selectS);
        }
    }
}
