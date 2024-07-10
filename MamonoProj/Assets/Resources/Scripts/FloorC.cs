using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorC : MonoBehaviour
{
    
    public SpriteRenderer spriteRenderer;

    [SerializeField, Tooltip("A=Insect,B=UFO,C=Vane,D=Ice,E=Ifrirt,F=Zombie,G=Virus")]
    private Sprite[] normalTexture;
    [SerializeField]
    private Sprite ice,blue,red,bug1;
    int floorran;

    public int floornumber;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

        if (GameData.VirusBugEffectLevel == 0)
        {
            if (GameData.IceFloor == 1) spriteRenderer.sprite = ice;
            else spriteRenderer.sprite = normalTexture[(GameData.Round-1)/5];
        }
        else if (GameData.VirusBugEffectLevel == 1)
        {
            floorran = Random.Range(0, 20);
            if (floorran == 1)
            {
                spriteRenderer.sprite = bug1;
            }
            else
            {
                spriteRenderer.sprite = normalTexture[(GameData.Round - 1) / 5];
            }

        }
        else if (GameData.VirusBugEffectLevel == 2)
        {
            floorran = Random.Range(0, 10);
            if (floorran == 0)
            {
                spriteRenderer.sprite = normalTexture[(GameData.Round - 1) / 5];
            }
            else if (floorran == 1)
            {
                spriteRenderer.sprite = bug1;
            }
            else if (floorran == 2)
            {
                spriteRenderer.sprite = red;
            }

        }
        else if (GameData.VirusBugEffectLevel == 3)
        {
            floorran = Random.Range(0, 10);
            if (floorran == 0)
            {
                spriteRenderer.sprite = normalTexture[(GameData.Round - 1) / 5];
            }
            else if (floorran == 1)
            {
                spriteRenderer.sprite = bug1;
            }
            else if (floorran == 2)
            {
                spriteRenderer.sprite = red;
            }
            else if (floorran == 3)
            {
                spriteRenderer.sprite = blue;
            }

        }
        else if (GameData.VirusBugEffectLevel == 100)
        {
            spriteRenderer.sprite = blue;

        }

        else if (GameData.VirusBugEffectLevel == 200)
        {
            floorran = Random.Range(0, 20);
            if (floorran == 1)
            {
                spriteRenderer.sprite = bug1;
            }
            else
            {
                spriteRenderer.sprite = normalTexture[(GameData.Round - 1) / 5];
            }
        }
    }
}
