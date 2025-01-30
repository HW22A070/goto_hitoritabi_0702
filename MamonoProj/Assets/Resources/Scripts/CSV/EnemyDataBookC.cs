using UnityEngine;

public class EnemyDataBookC : MonoBehaviour
{
    public EnemyDataC[] enemyDatas;

    // Start is called before the first frame update
    void FixedUpdate()
    {
        TextAsset textasset = new TextAsset();
        textasset = Resources.Load("EnemyData", typeof(TextAsset)) as TextAsset;
        enemyDatas = CSVSerializer.Deserialize<EnemyDataC>(textasset.text);
    }

    public EnemyDataC sendEnenyData(string name)
    {
        EnemyDataC enemyData=enemyDatas[0];
        for(int i = 0; i < enemyDatas.Length; i++)
        {
            if (enemyDatas[i].NAME == name)
            {
                enemyData= enemyDatas[i];
                break;
            }
        }
        return enemyData;
    }
}
