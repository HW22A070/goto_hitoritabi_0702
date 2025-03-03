using UnityEngine;

public class ETypeDrawnC : ETypeCoreC
{
    protected int judge;

    [SerializeField]
    protected Vector2 move = new Vector2(0,-3);

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();

        if (_posOwn.y <= -50 || _posOwn.x > 660 || _posOwn.x < -20)
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
