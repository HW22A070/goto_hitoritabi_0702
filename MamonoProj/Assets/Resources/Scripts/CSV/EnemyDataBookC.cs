using UnityEngine;
using System.Collections.Generic;

public class EnemyDataBookC : MonoBehaviour
{
    public EnemyDatas[] enemyDatas;

    // Start is called before the first frame update
    void FixedUpdate()
    {
        TextAsset textasset = new TextAsset();
        textasset = Resources.Load("EnemyData", typeof(TextAsset)) as TextAsset;
        enemyDatas = CSVSerializer.Deserialize<EnemyDatas>(textasset.text);
    }

    public EnemyDatas sendEnenyData(string name)
    {
        foreach(EnemyDatas data in enemyDatas)
        {
            if (data.NAME == name)
            {
                return data;
            }
        }
        return enemyDatas[0];
    }

    public List<EnemyDatas> GetEnemysByScore(int score)
    {
        List<EnemyDatas> datas = new List<EnemyDatas> { };
        foreach(EnemyDatas data in enemyDatas)
        {
            if (data.SCORE == score) datas.Add(data);
        }
        return datas;
    }
}
