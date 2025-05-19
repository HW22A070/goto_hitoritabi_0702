using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningWindowC : MonoBehaviour
{
    [SerializeField]
    private GameObject _goWondow;

    private bool _isOpen;

    private Coroutine _playing;

    public void DoOpen(float second)
    {
        if (!_isOpen)
        {
            _goWondow.SetActive(true);
            _isOpen = true;
            StartCoroutine(Opening(second));
        }
    }



    private IEnumerator Opening(float second)
    {
        yield return new WaitForSeconds(second);
        _isOpen = false;
        _goWondow.SetActive(false);
    }
}
