using Move2DSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundMeterC : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] _srMeterTiles;

    [SerializeField]
    private GameObject _goPlayerFigure;

    private Vector3 _posFirstPlayerF;

    [SerializeField]
    private PlayersManagerC _scPlsM;

    [SerializeField]
    private Transform _tfIconParent;

    [SerializeField]
    private GameObject[] _goStageIcon;

    private List<GameObject> _listIcon=new List<GameObject> { };

    // Start is called before the first frame update
    void Start()
    {
        _posFirstPlayerF = _goPlayerFigure.transform.localPosition;

        for (int i = 0; i < _goStageIcon.Length; i++)
        {
            if ((GameData.StartRound - 1) / 5 <= i && i <= (GameData.GoalRound - 1) / 5)
            {
                GameObject icon = Instantiate(_goStageIcon[i], _tfIconParent.position + Vector3.right * _listIcon.Count * 13, Quaternion.Euler(0, 0, 0));
                icon.transform.parent = _tfIconParent;
                icon.GetComponent<SpriteRenderer>().color = new Color32(128, 128, 128, 255);
                _listIcon.Add(icon);
            }
        }

        SetTileSpritesByRound();
        if (GameData.Round == 0)
        {
            foreach (SpriteRenderer tile in _srMeterTiles)
            {
                tile.enabled = false;
            }
        }
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    public void SetTileSpritesByRound()
    {
        int stage = (GameData.Round - 1) % 5;
        int round = 0;
        foreach(SpriteRenderer tile in _srMeterTiles)
        {
            if (stage > round)tile.color = new Color32(200,180,0,255);
            else tile.color = new Color32(128, 128, 128, 255);
            round++;
        }
        if (GameData.StageMode == EnumDic.Stage.KIND_STAGE.Virus) SetVirusIcon();
        StartCoroutine(MoviePlayerFigure());
    }

    public void SetTileSpritesFull()
    {
        foreach (SpriteRenderer tile in _srMeterTiles)
        {
            tile.color = new Color32(200, 180, 0, 255);
        }
    }

    private void SetVirusIcon()
    {
        foreach(GameObject icon in _listIcon)
        {
            Destroy(icon);
        }
        _listIcon.Clear();
        _listIcon.Add(Instantiate(_goStageIcon[6], _tfIconParent.position + Vector3.right * _listIcon.Count * 13, Quaternion.Euler(0, 0, 0)));
    }

    private IEnumerator MoviePlayerFigure()
    {
        float distance= Moving2DSystems.GetDistance(_posFirstPlayerF + Vector3.right * ((GameData.Round - 1) % 5) * 38, _goPlayerFigure.transform.localPosition);

        if (GameData.Round%5==1)
        {
            _goPlayerFigure.transform.localPosition = _posFirstPlayerF;
            int index = ((GameData.Round - 1) / 5) - ((GameData.StartRound - 1) / 5);
            if (_listIcon.Count >index)
            {
                _listIcon[index].GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            }
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForFixedUpdate();
                _goPlayerFigure.transform.localPosition += Vector3.right * distance / 10;
            }
        }
    }
}
