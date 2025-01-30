using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorC : MonoBehaviour
{
    
    public SpriteRenderer spriteRenderer;

    [SerializeField, Tooltip("A=Insect,B=UFO,C=Vane,D=Ice,E=Ifrirt,F=Zombie,G=Virus")]
    private Sprite[] normalTexture;
    [SerializeField]
    private Sprite ice,blue,red,bug1,_fire,_spike,_spike2;
    private int floorran;

    /// <summary>
    /// 0=通常
    /// 1=氷
    /// 2=可燃
    /// 3=炎上
    /// 4=ひょっこり
    /// 5=激やば理不尽棘
    /// </summary>
    public int _floorMode = 0;

    /// <summary>
    /// 最下層判定
    /// </summary>
    public bool _isBedRock;

    [SerializeField]
    private GameObject _goFire;
    

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
        _isBedRock = !Physics2D.Raycast(transform.position- new Vector3(0, 24, 0), new Vector3(0, -1, 0), GameData.WindowSize.y, 8);

        if (GameData.VirusBugEffectLevel == 0)
        {
            if (_floorMode ==1) spriteRenderer.sprite = ice;
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

        if (_floorMode >= 2&& _floorMode <= 3)
        {
            StartCoroutine(OnFire());
            spriteRenderer.sprite = _fire;
            if (_floorMode == 3)
            {

                _goFire.SetActive(true);
            }
        }
        else _goFire.SetActive(false);

        if (_floorMode >= 4 && _floorMode <= 5)
        {
            StartCoroutine(OnSpike());
            spriteRenderer.sprite = _spike;
            if (_floorMode == 5)
            {

                spriteRenderer.sprite = _spike2;
            }
        }
    }

    private IEnumerator OnFire()
    {
        yield return new WaitForSeconds(1.0f);
        if(_floorMode >= 2)
        {
            _floorMode = 3;
        }
    }

    private IEnumerator OnSpike()
    {
        yield return new WaitForSeconds(2.0f);
        if (_floorMode >= 4)
        {
            _floorMode = 5;
        }
    }
}
