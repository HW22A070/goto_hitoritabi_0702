using UnityEngine;

/// <summary>
/// ポイント上昇エネルギー弾
/// </summary>
public class PointEnergyC : MonoBehaviour
{
    private Ray _rayEnegry;

    private RaycastHit2D _hitEnergyToPointicon;

    private GameObject _iconPoint;

    private Vector3 _angle;

    // Start is called before the first frame update
    void Start()
    {
        _iconPoint = GameObject.Find("PointIcon");
        _angle = GameData.GetDirection(GameData.GetAngle(transform.position, _iconPoint.transform.position));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += _angle * 24;
        _hitEnergyToPointicon = Physics2D.Raycast(transform.position, Vector3.zero, 10,32);
        if (_hitEnergyToPointicon)
        {
            if (_hitEnergyToPointicon.collider.gameObject==_iconPoint)
            {
                GameData.Point++;
                Destroy(gameObject);
            }
        }
    }
}
