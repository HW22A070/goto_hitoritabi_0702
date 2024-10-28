using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSpecialBeamC : MonoBehaviour
{
    private Vector3 _pos, _posSet;

    private float _angle, _angle2, _angle3, _time=3.0f;

    private bool _isAttack;

    private GameObject playerGO;

    [SerializeField, Tooltip("弾アタッチ")]
    private PMissile PRaserP;

    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        _pos = transform.position;
        _time -= Time.deltaTime;
        if (!_isAttack)
        {
            if (_time <= 0)
            {
                _isAttack = true;
            }
        }
        else
        {
            if(_time <= -4)
            {
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_isAttack)
        {
            transform.position += GameData.GetSneaking(_pos, _posSet, 10);
        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                PMissile shot = Instantiate(PRaserP, transform.position, transform.rotation);
                shot.Shot(_angle3, 0, 1000);
                shot.transform.position += shot.transform.up * 640;
                if (_time < -1)
                {
                    shot = Instantiate(PRaserP, transform.position, transform.rotation);
                    shot.Shot(_angle2, 0, 1000);
                    shot.transform.position += shot.transform.up * 640;
                }
                if (_time < -2)
                {
                    shot = Instantiate(PRaserP, transform.position, transform.rotation);
                    shot.Shot(_angle, 0, 1000);
                    shot.transform.position += shot.transform.up * 640;
                }
            }
        }
    }

    public void SetPos(short number)
    {
        switch (number)
        {
            case 0:
                _posSet = new Vector3(GameData.WindowSize.x - 32, GameData.WindowSize.y -64, 0);
                _angle3 = GameData.GetAngle(_posSet,new Vector3(32,32,0));
                _angle2 = GameData.GetAngle(_posSet, new Vector3(GameData.WindowSize.x/2, 32, 0));
                _angle = GameData.GetAngle(_posSet, new Vector3(GameData.WindowSize.x - 32, 32, 0));

                break;
            case 1:
                _posSet = new Vector3(32, GameData.WindowSize.y - 64, 0);
                _angle = GameData.GetAngle(_posSet, new Vector3(32, 32, 0));
                _angle2 = GameData.GetAngle(_posSet, new Vector3(GameData.WindowSize.x/2, 32, 0));
                _angle3 = GameData.GetAngle(_posSet, new Vector3(GameData.WindowSize.x - 32, 32, 0));
                break;
        }
    }
}
