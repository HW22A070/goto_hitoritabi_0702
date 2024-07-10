using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControlC : MonoBehaviour
{
    private int mode = 0, check = 0;

    private AudioSource BGM;

    [SerializeField,Tooltip("‰¹Šy‚½‚¿")]
    private AudioClip A, B, C, D, E, F,EX,V1,V2,V3,_tuto;


    // Start is called before the first frame update
    void Start()
    {
        BGM = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.Difficulty != 3)
        {
            if (1 <= GameData.Round && GameData.Round <= 5) mode = 1;
            else if (6 <= GameData.Round && GameData.Round <= 10) mode = 2;
            else if (11 <= GameData.Round && GameData.Round <= 15) mode = 3;
            else if (16 <= GameData.Round && GameData.Round <= 20) mode = 4;
            else if (21 <= GameData.Round && GameData.Round <= 25) mode = 5;
            else if (26 <= GameData.Round && GameData.Round <= 30) mode = 6;
            else if (31 <= GameData.Round && GameData.Round <= 34) mode = 7;
            else if (GameData.Round == 35)
            {
                if (VirusC.VirusMode == 0) mode = 7;
                else if (VirusC.VirusMode == 1) mode = 102;
                else if (VirusC.VirusMode == 2) mode = 103;
                else mode = 100;
            }
            else if (GameData.Round == 0) mode = -1;

        }
        else
        {
            if (GameData.Boss == 0)
            {
                mode = 101;
            }
            else
            {
                mode = 102;
            }

        }

        if (check != mode)
        {
            if (mode == 1) BGM.clip = A;
            else if (mode == 2) BGM.clip = B;
            else if (mode == 3) BGM.clip = C;
            else if (mode == 4) BGM.clip = D;
            else if (mode == 5) BGM.clip = E;
            else if (mode == 6) BGM.clip = F;
            else if (mode == 7) BGM.clip = EX;
            else if (mode == 101) BGM.clip = V1;
            else if (mode == 102) BGM.clip = V2;
            else if (mode == 103) BGM.clip = V3;
            else if(mode==-1) BGM.clip = _tuto;
            else BGM.clip = null;
            BGM.Play();
            check = mode;
        }

        if (GameData.Boss == 0) BGM.volume = 0.5f;
        else BGM.volume = 0.7f;
    }
}
