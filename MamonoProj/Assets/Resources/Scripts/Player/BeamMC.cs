using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeamMC : MonoBehaviour
{
    private Vector3 velocity, pos, fpos;

    private float of,shotdown,sddelta=0.35f;
    private float sp = 0;

    public int geigeki = 0;

    public PMissile PBeamP,PRaserP;

    public AudioClip shotS;

    private GameObject Pl,_playerGO;

    private bool Fireing =true;

    // Start is called before the first frame update
    void Start()
    {
        _playerGO = GameObject.Find("Player");
    }

    void Update()
    {
        pos = _playerGO.transform.position;
        fpos = pos + new Vector3(Mathf.Sin((sp + of) * Mathf.Deg2Rad) * 32, Mathf.Cos((sp + of) * Mathf.Deg2Rad) * 32, 0);

        if (shotdown != 0) shotdown -= Time.deltaTime;
        if (Fireing && shotdown <= 0) Shot_Beam();
    }

    /*//input
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started) Fireing = true;
        else if (context.canceled) Fireing = false;
    }*/

    public Vector3 GetDirection(float angle)
    {
        Vector3 direction = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0);
        return direction;
    }

    public void EShot1(float offset)
    {
        of = offset;
        Destroy(gameObject,10);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localPosition = fpos;

        sp += 10;
        while (sp > 360)
        {
            sp -= 360;
        }
    }

    //beam
    private void Shot_Beam()
    {
        sddelta -= 0.01f;
        if (sddelta < 0.05f)
        {
            Quaternion rot = transform.localRotation;
            PMissile shot = Instantiate(PRaserP, fpos, rot);
            shot.Shot(GetTagPosition("Enemy1")/*180 + (PlayerC.muki * 180)*/, 320, 1000);
            sddelta = 0.03f;
        }
        else
        {
            Quaternion rot = transform.localRotation;
            PMissile shot = Instantiate(PRaserP, fpos, rot);
            shot.Shot(GetTagPosition("Enemy1")/*180 + (PlayerC.muki * 180)*/, 320, 1000);
        }
        if (of == 0)
        {
            GameObject.FindObjectOfType<AudioSource>().PlayOneShot(shotS);
        }

        shotdown = sddelta;
    }


    private float GetTagPosition(string tagName)
    {
        GameObject[] myObjects;
        myObjects = GameObject.FindGameObjectsWithTag("Enemy0");
        if (myObjects.Length > 0)return GameData.GetAngle(fpos,myObjects[Random.Range(0, myObjects.Length)].transform.position);
        else
        {
            myObjects = GameObject.FindGameObjectsWithTag("Enemy1");
            if (myObjects.Length > 0) return GameData.GetAngle(fpos, myObjects[Random.Range(0, myObjects.Length)].transform.position);
            else
            {
                myObjects = GameObject.FindGameObjectsWithTag("Enemy2");
                if (myObjects.Length > 0) return GameData.GetAngle(fpos, myObjects[Random.Range(0, myObjects.Length)].transform.position);
                else
                {
                    myObjects = GameObject.FindGameObjectsWithTag("Enemy3");
                    if (myObjects.Length > 0) return GameData.GetAngle(fpos, myObjects[Random.Range(0, myObjects.Length)].transform.position);
                    else
                    {
                        myObjects = GameObject.FindGameObjectsWithTag("Enemy4");
                        if (myObjects.Length > 0) return GameData.GetAngle(fpos, myObjects[Random.Range(0, myObjects.Length)].transform.position);
                        else
                        {
                            myObjects = GameObject.FindGameObjectsWithTag("Enemy6");
                            if (myObjects.Length > 0) return GameData.GetAngle(fpos, myObjects[Random.Range(0, myObjects.Length)].transform.position);
                            else
                            {
                                return Random.Range(0,360)/*180 + (PlayerC.muki * 180)*/;
                            }
                        }
                    }
                }
            }
        }
    }
}
