using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETypeDrawnC : MonoBehaviour
{
    protected int judge;

    [SerializeField]
    protected Vector2 move = new Vector2(0,-3);

    protected SpriteRenderer spriteRenderer;

    protected Vector3 pos;

    /// <summary>
    /// スピーカ
    /// </summary>
    protected AudioSource _audioGO;

    // Start is called before the first frame update
    protected void Start()
    {
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    protected void Update()
    {
        pos = transform.position;

        if (pos.y <= -50 || pos.x > 660 || pos.x < -20)
        {
            Destroy(gameObject);
        }
    }

    protected void FixedUpdate()
    {
        Vector3 moveValue = new Vector3(move.x, move.y, 0);
        transform.localPosition += moveValue;
    }
}
