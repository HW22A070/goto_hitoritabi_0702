using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealC : MonoBehaviour
{
    Vector3 pos;
    private bool _isDestroy;


    private GameObject playerGO;

    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.Find("Player");

        pos = transform.position;
        if (pos.x < 16) transform.position = new Vector3(16, pos.y, 0);
        if (pos.x > 640-16) transform.position = new Vector3(640 - 16, pos.y, 0);
        if (pos.y < 16) transform.position = new Vector3(pos.x,32, 0);
        if (pos.y > 480) transform.position = new Vector3(pos.x, 392, 0);
    }

    void Update()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos = transform.position;
        if (GameData.StageMovingAction)
        {
            Vector3 muki = playerGO.transform.position - pos;
            float angle = GetAngle(muki);
            var direction = GetDirection(angle);
            Vector3 velocity = direction * 20;
            transform.localPosition += velocity;
            _isDestroy = true;
        }
        else if (_isDestroy)
        {
            Destroy(gameObject);
        }
    }

    public void EShot1()
    {

    }

    //Kakudo
    public float GetAngle(Vector2 direction)
    {
        float rad = Mathf.Atan2(direction.y, direction.x);
        return rad * Mathf.Rad2Deg;
    }

    //Muki
    public Vector3 GetDirection(float angle)
    {
        Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0);
        return direction;
    }
}
