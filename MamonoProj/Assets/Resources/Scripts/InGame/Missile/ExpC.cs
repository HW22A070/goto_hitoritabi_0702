using UnityEngine;

public class ExpC : EMissile1C
{
    [SerializeField, Tooltip("エフェクト扱い")]
    private bool _isEffect;

    public void ShotEXP(float angle, float speed,float delete)
    {
        ShotMissile(angle, speed, 0);
        Destroy(gameObject,delete);
    }
}
