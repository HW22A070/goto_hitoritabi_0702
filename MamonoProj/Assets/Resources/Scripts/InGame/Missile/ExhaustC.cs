using UnityEngine;

public class ExhaustC : MonoBehaviour
{
    private Vector3 _posOwn;

    [SerializeField]
    private ExpC ExpPrefab;

    [SerializeField]
    [Tooltip("回転するか")]
    private bool _isKaitenLock;

    [SerializeField]
    private float _sizeX, _sizeY, _speed, _delete;

    private  float _angle;

    [SerializeField, Tooltip("1フレームにエフェクト出す回数")]
    private int _loop = 1;

    // Start is called before the first frame update
    void Start()
    {
        _posOwn = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _posOwn = transform.position;
    }

    void FixedUpdate()
    {
        for (int hoge = 0; hoge < _loop; hoge++)
        {
            _posOwn += transform.right* Random.Range(-_sizeX, _sizeX + 1);
            _posOwn += transform.up * Random.Range(-_sizeY, _sizeY + 1);
            if (_isKaitenLock) _angle = 90;
            else _angle = Random.Range(0, 360);
            Quaternion rot = transform.localRotation;
            ExpC shot = Instantiate(ExpPrefab, _posOwn, rot);
            shot.ShotEXP(_angle, _speed, _delete);
        }
    }
}
