using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagC : MonoBehaviour
{
    private GameObject[] _tutorialGO;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _tutorialGO = GameObject.FindGameObjectsWithTag("Tutorial");
            for(int i = 0; i < _tutorialGO.Length; i++)_tutorialGO[i].GetComponent<TutorialC>().GoTutorial();
            Destroy(gameObject);
        }
    }
}
