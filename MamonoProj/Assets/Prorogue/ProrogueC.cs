using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ProrogueC : MonoBehaviour
{
    [SerializeField]
    private GameObject _goBackGround, _prfbMechazombie,_prfbMail,_prfbPlayer;

    [SerializeField]
    private ExpC _prfbVirusE, _prfbVirusE2;

    [SerializeField, Header("”wŒi")]
    private Sprite _metal;


    [SerializeField, Header("ƒ]ƒ“ƒr")]
    private Sprite _virused;

    private int _forhoge;

    private bool _isSkiped;

    private GameObject[] _prefabs;

    private Quaternion rot = Quaternion.Euler(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PrologureAnim());
    }

    // Update is called once per frame
    void Update()
    {
        if (_isSkiped)
        {
            SceneManager.LoadScene("Title");
        }
    }
    
    private IEnumerator PrologureAnim()
    {
        for (_forhoge = 200; _forhoge >0; _forhoge--)
        {
            _goBackGround.transform.position -= (transform.right * _forhoge / 35);
            yield return new WaitForSeconds(0.03f);
        }
        _goBackGround.transform.position = new Vector2(320, 265);
        _goBackGround.GetComponent<SpriteRenderer>().sprite = _metal;
        GameObject zombie = Instantiate(_prfbMechazombie, new Vector3(450,128,0), rot);
        yield return new WaitForSeconds(1.00f);
        GameObject mail = Instantiate(_prfbMail, new Vector3(470, 700, 0), rot);
        while (mail.transform.position.y>128)
        {
            mail.transform.position -= transform.up*2;
            yield return new WaitForSeconds(0.03f);
        }
        Destroy(mail);
        for (_forhoge = 0; _forhoge < 100; _forhoge++)
        {
            Instantiate(_prfbVirusE, GameData.GetRandomWindowPosition(), rot).EShot1(Random.Range(0, 360), 0.2f, 10);
        }
        for (_forhoge = 0; _forhoge < 30; _forhoge++)
        {
            Instantiate(_prfbVirusE2, zombie.transform.position+new Vector3(Random.Range(-64,64), Random.Range(-64, 64),0), rot)
                .EShot1(Random.Range(0, 360), 0.2f, 3);
        }
        zombie.GetComponent<SpriteRenderer>().sprite = _virused;
        yield return new WaitForSeconds(5.0f);
        while (zombie.transform.position.y < 700)
        {
            Instantiate(_prfbVirusE2, zombie.transform.position + new Vector3(Random.Range(-64, 64), -64), rot).EShot1(270, 0.5f, 1);
            zombie.transform.position += transform.up * 5;
            yield return new WaitForSeconds(0.03f);
        }
        GameObject player = Instantiate(_prfbPlayer, new Vector3(-32, 64 + 16, 0), rot);
        while (player.transform.position.x <680)
        {
            player.transform.position += transform.right * 5;
            yield return new WaitForSeconds(0.03f);
        }
        _isSkiped = true;
        
    }

    //Imput
    public void OnSkip(InputAction.CallbackContext context)
    {
        if (context.performed &&!_isSkiped)
        {
            _isSkiped = true;
        }
    }
}
