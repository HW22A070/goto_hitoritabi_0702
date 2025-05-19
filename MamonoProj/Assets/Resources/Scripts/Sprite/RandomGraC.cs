using UnityEngine;

public class RandomGraC : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _sprites;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = _sprites[Random.Range(0, _sprites.Length)];
    }
}
