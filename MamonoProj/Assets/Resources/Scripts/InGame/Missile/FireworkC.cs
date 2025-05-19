using UnityEngine;

public class FireworkC : HowitzerC
{
    private int i,j, hunj;

    [SerializeField]
    private bool up, down, right, left;

    protected override void DoDelete()
    {
        if (Random.Range(0, 2) == 0)
        {
            for (j = 1; j < 4; j++)
            {
                for (i = 0; i < 20; i++)
                {
                    Quaternion rot2 = transform.localRotation;
                    ExpC shot2 = Instantiate(ExpPrefab, _posOwn, rot2);
                    shot2.ShotEXP(i * 18, j * 3, _expCountTime);
                }
            }
        }
        else
        {
            for (i = 0; i < 36; i++)
            {
                Quaternion rot2 = transform.localRotation;
                ExpC shot2 = Instantiate(ExpPrefab, _posOwn, rot2);
                shot2.ShotEXP((i * 10) + Random.Range(-1, 2), 13, _expCountTime);
            }
        }
        _audioGO.PlayOneShot(expS);
        Destroy(gameObject);
    }

}
