using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerIconManagerC : MonoBehaviour
{
    [SerializeField]
    GameObject _prfbControllerIcon;

    List<GameObject> _listController = new List<GameObject> { };

    // Update is called once per frame
    void Update()
    {
        if(_listController.Count!= GetActiveControllerCount())
        {
            foreach(GameObject controller in _listController)
            {
                Destroy(controller);
                
            }
            _listController.Clear();

            Vector3 pos = transform.position;
            for(int i=0;i< GetActiveControllerCount(); i++)
            {
                _listController.Add(Instantiate(_prfbControllerIcon, pos + Vector3.right * 32*i, Quaternion.Euler(0, 0, 0)));
            }
        }
    }

    private int GetActiveControllerCount()
    {
        int count = 0;
        foreach(string name in Input.GetJoystickNames())
        {
            if (name != "") count++;
        }
        return count;
    }
}
