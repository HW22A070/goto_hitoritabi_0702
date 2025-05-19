using UnityEngine;

public class DustC : MonoBehaviour
{
    Vector3 _posOwn;

    public void EShot1() => Destroy(gameObject, 0.5f);

    // Update is called once per frame
    void FixedUpdate()
    {
        _posOwn = transform.position;
        transform.localEulerAngles = new Vector3(0, 0, -GameData.WindSpeed);
        transform.position += new Vector3(GameData.WindSpeed / 5, 0, 0);
        if (_posOwn.y < 0 || _posOwn.y > 480 || _posOwn.x > 790 || _posOwn.x < -150)
        {
            Destroy(gameObject);
        }
    }
}
