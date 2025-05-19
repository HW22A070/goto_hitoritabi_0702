public class PExpC : PMissile
{
    public void ShotEXP(float angle, float speed,float delete)
    {
        ShotMissle(angle, speed, 0);
        Destroy(gameObject,delete);
    }
}
