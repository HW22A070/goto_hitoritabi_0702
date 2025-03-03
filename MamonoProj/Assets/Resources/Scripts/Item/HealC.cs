using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealC : MonoBehaviour
{
    private Vector3 _posOwn;
    private bool _isDestroy;

    //private MissileGravityC _scGravity;


    private GameObject _goPlayer;

    /// <summary>
    /// アイテムとプレイヤーの衝突判定
    /// </summary>
    private RaycastHit2D _hitItemToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        _goPlayer = GameObject.Find("Player");

        _posOwn = transform.position;
        if (_posOwn.x < 16) transform.position = new Vector3(16, _posOwn.y, 0);
        if (_posOwn.x > 640-16) transform.position = new Vector3(640 - 16, _posOwn.y, 0);
        if (_posOwn.y < 16) transform.position = new Vector3(_posOwn.x,32, 0);
        if (_posOwn.y > 480) transform.position = new Vector3(_posOwn.x, 392, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _posOwn = transform.position;
        if (GameData.IsStageMovingAction)
        {
            if (GetComponent<MissileGravityC>())
            {
                GetComponent<MissileGravityC>().enabled = false;
            }
            float angle = GameData.GetAngle(_posOwn, _goPlayer.transform.position);
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
            switch (gameObject.tag)
            {
                case "Heal":
                    if (_hitItemToPlayer.collider.GetComponent<PlayerC>().SetHeal(3)) Destroy(gameObject);
                    break;

                case "Magic":
                    if (_hitItemToPlayer.collider.GetComponent<PlayerC>().SetAddMagic()) Destroy(gameObject);
                    break;

            }
        }
    }
}
