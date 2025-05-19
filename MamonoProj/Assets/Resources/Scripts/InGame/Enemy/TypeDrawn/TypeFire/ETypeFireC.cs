using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETypeFireC : ETypeDrawnC
{
    protected int mode = 0;

    [SerializeField]
    protected Vector2 _attackValue = new Vector2(5,0);

    private GameObject playerGO;

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();

        if (CheckIsTargetingAnyPlayers(16) && mode == 0)
        {
            mode = 1;
            if (_posOwn.x > _posPlayer.x)
            {
                move = -_attackValue;
                _srOwnBody.flipX = false;
            }
            else
            {
                move = _attackValue;
                _srOwnBody.flipX = true;
            }
        }
    }
}
