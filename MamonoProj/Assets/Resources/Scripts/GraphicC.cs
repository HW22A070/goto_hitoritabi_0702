using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicC : MonoBehaviour
{
    float timecount = 0;
    [SerializeField]
    private float changecount;
    int spritenumber = 0;
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    [SerializeField]
    private bool _random;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timecount += Time.deltaTime;
        if (_random)
        {
            if (timecount >= changecount)
            {
                spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
                timecount = 0;
            }
        }
        else
        {
            if (timecount >= changecount)
            {
                spriteRenderer.sprite = sprites[spritenumber];
                timecount = 0;
                spritenumber++;
                if (spritenumber >= sprites.Length)
                {
                    spritenumber = 0;
                }
            }
        }

    }
}
