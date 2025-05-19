using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PouseSummonerC : MonoBehaviour
{
    /// <summary>
    /// ’†’f
    /// </summary>
    /// <param name="context"></param>
    public void OnPouse(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameObject.Find("Pouse").GetComponent<PouseMenuC>().OnPouse();
        }
    }
}
