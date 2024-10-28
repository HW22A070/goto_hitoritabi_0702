using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rose1C : MonoBehaviour
{

    float time = 0.5f;
    int texture = 0;

    Vector3 pos;
    public Rose2C Rose2Prefab;

    public SpriteRenderer spriteRenderer;
    public Sprite a, b, c;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void EShot1()
    {
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;

        if (time != 0)time -= Time.deltaTime;
        if (time <= 0)
        {
            time = 0.4f;
            texture++;
        }
        if (texture == 0) spriteRenderer.sprite = a;
        else if (texture == 1) spriteRenderer.sprite = b;
        else if (texture == 2) spriteRenderer.sprite = c;
        else if (texture >= 3)
        {
            Quaternion rot = transform.localRotation;
            Rose2C shot = Instantiate(Rose2Prefab, pos, rot);
            shot.EShot1();
            Destroy(gameObject);
        }

    }


}
