using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagC : MonoBehaviour
{
    private GameObject[] _tutorialGO;

    /// <summary>
    /// アイテムとプレイヤーの衝突判定
    /// </summary>
    private RaycastHit2D _hitFlagToPlayer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _hitFlagToPlayer = Physics2D.BoxCast(transform.position, GetComponent<BoxCollider2D>().size, transform.localEulerAngles.z, Vector2.zero, 0, 64);
        if (_hitFlagToPlayer)
        {
            _tutorialGO = GameObject.FindGameObjectsWithTag("Tutorial");
            for (int i = 0; i < _tutorialGO.Length; i++) _tutorialGO[i].GetComponent<TutorialC>().GoTutorial();
            Destroy(gameObject);
        }
    }
}
