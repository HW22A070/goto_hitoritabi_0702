using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundC : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite windy,blue,bug1,bug2,bug3,bug4,bug5,bug6;
    int backran;

    [SerializeField, Tooltip("A=Insect,B=UFO,C=Vane,D=Ice,E=Ifrirt,F=Zombie,G=Virus")]
    private Sprite[] normalTexture;

    // Update is called once per frame
    void Start()
    {
    }

    void FixedUpdate()
    {
        if (GameData.VirusBugEffectLevel == 0)
        {

            if (GameData.WindSpeed >= 100 || GameData.WindSpeed <= -100) spriteRenderer.sprite = windy;

            else spriteRenderer.sprite = normalTexture[(GameData.Round - 1) / 5];

        }
        else if (GameData.VirusBugEffectLevel == 1)
        {
            backran = Random.Range(0, 30);
            if (backran == 2)
            {
                spriteRenderer.sprite = bug1;
            }
            else if (backran == 3)
            {
                spriteRenderer.sprite = bug2;
            }
            else
            {
                spriteRenderer.sprite = normalTexture[(GameData.Round - 1) / 5];
            }
        }
        else if (GameData.VirusBugEffectLevel == 2)
        {
            backran = Random.Range(0, 10);
            if (backran == 0)
            {
                spriteRenderer.sprite = normalTexture[(GameData.Round - 1) / 5];
            }
            else if (backran == 1)
            {
                spriteRenderer.sprite = normalTexture[Random.Range(0, 6)];
            }
            else if (backran == 2)
            {
                spriteRenderer.sprite = bug1;
            }
            else if (backran == 3)
            {
                spriteRenderer.sprite = bug2;
            }
        }
        else if (GameData.VirusBugEffectLevel == 3)
        {
            backran = Random.Range(0, 20);
            if (backran == 0)
            {
                spriteRenderer.sprite = normalTexture[(GameData.Round - 1) / 5];
            }
            else if (backran == 1)
            {
                spriteRenderer.sprite = normalTexture[Random.Range(0, 6)];
            }
            else if (backran == 2)
            {
                spriteRenderer.sprite = bug1;
            }
            else if (backran == 3)
            {
                spriteRenderer.sprite = bug2;
            }
            else if (backran == 4)
            {
                spriteRenderer.sprite = bug3;
            }
            else if (backran == 5)
            {
                spriteRenderer.sprite = bug4;
            }
            else if (backran == 6)
            {
                spriteRenderer.sprite = bug5;
            }
            else if (backran == 7)
            {
                spriteRenderer.sprite = bug6;
            }
        }
        else if (GameData.VirusBugEffectLevel == 100)
        {
            spriteRenderer.sprite = blue;
        }
        else if (GameData.VirusBugEffectLevel == 200)
        {
            backran = Random.Range(0, 10);
            if (backran == 0)
            {
                spriteRenderer.sprite = bug3;
            }
            else if (backran == 1)
            {
                spriteRenderer.sprite = bug4;
            }
            else if (backran == 2)
            {
                spriteRenderer.sprite = normalTexture[(GameData.Round - 1) / 5];
            }
        }
    }
}
