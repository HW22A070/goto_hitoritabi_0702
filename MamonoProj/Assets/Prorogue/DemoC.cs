using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class DemoC : MonoBehaviour
{
    private bool _isSkiped;

    private float _time = 63.0f;

    [SerializeField]
    private VideoClip[] _demoV;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<VideoPlayer>().clip = _demoV[Random.Range(0, _demoV.Length)];
        GetComponent<VideoPlayer>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        _time -= Time.deltaTime;
        if (_time <= 0) _isSkiped=true;

        if (_isSkiped)
        {
            SceneManager.LoadScene("Title");
        }
    }

    //Imput
    public void OnSkip(InputAction.CallbackContext context)
    {
        if (context.performed && !_isSkiped)
        {
            _isSkiped = true;
        }
    }

    public void LoopPointReached(VideoPlayer vp)
    {
        _isSkiped = true;
    }
}
