using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealC : MonoBehaviour
{
    private Vector3 pos;
    private bool _isDestroy;

    //private MissileGravityC _scGravity;


    private GameObject playerGO;

    /// <summary>
    /// アイテムとプレイヤーの衝突判定
    /// </summary>
    private RaycastHit2D _hitItemToPlayer;

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
            GetComponent<MissileGravityC>().enabled=false;
            float angle = GameData.GetAngle(pos, playerGO.transform.position);
            var direction = GameData.GetDirection(angle);
            Vector3 velocity = direction * 20;
            transform.localPosition += velocity;
            _isDestroy = true;
        }
        else if (_isDestroy)
        {
            Destroy(gameObject);
        }

        _hitItemToPlayer = Physics2D.BoxCast(transform.position, GetComponent<BoxCollider2D>().size, transform.localEulerAngles.z, Vector2.zero, 0, 64);
        if (_hitItemToPlayer)
        {
            if (gameObject.tag == "Heal")
            {
                if(_hitItemToPlayer.collider.GetComponent<PlayerC>().Heal(3))Destroy(gameObject);
            }
            if (gameObject.tag == "Magic")
            {
                if(_hitItemToPlayer.collider.GetComponent<PlayerC>().MagicCharge())Destroy(gameObject);
            }
        }
    }

    public void EShot1()
    {

    }
}
