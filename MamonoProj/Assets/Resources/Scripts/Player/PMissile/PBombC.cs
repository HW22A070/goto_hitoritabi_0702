using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PBombC : MonoBehaviour
{
    private Vector3 velocity, pos;

    private float sspeed, kkaso, aang, eexp, eexptim;
    private int i, hunj;

    [SerializeField]
    [Tooltip("�����g���K�[")]
    private bool up, down, right, left, sosai;

    /// <summary>
    /// 0=���ݒ���
    /// 1=�J�E���g�X�^�[�g
    /// 2=�N��
    /// </summary>
    private int _expMode;

    public PExpC ExpPrefab;

    public AudioClip expS;
    private int ddd;

    private Coroutine _movingCoroutine;

    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="angle">���ˌ���</param>
    /// <param name="speed">���ˑ��x</param>
    /// <param name="kasoku">���ˉ����x</param>
    /// <param name="exp">�@�N���J�E���g�_�E��</param>
    /// <param name="hunjin">�������o��</param>
    /// <param name="exptime">���o��������܂�</param>
    public void EShot1(float angle, float speed, float kasoku, float exp, int hunjin, float exptime)
    {
        Debug.Log(_expMode);
        var direction = GetDirection(angle);
        velocity = direction * speed;
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        sspeed = speed;
        kkaso = kasoku;
        aang = angle;
        eexp = exp;
        eexptim = exptime;
        hunj = hunjin;
        _expMode = 1;
    }

    private Vector3 GetDirection(float angle)
    {
        Vector3 direction = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0);
        return direction;
    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        if (_expMode == 1)
        {
            pos = transform.position;

            transform.localPosition += velocity;
            sspeed += kkaso;
            var direction = GetDirection(aang);
            velocity = direction * sspeed;

            //time_ex
            eexp--;
            if (eexp <= 0) EXPEffect(hunj);

            //down_ex
            if (pos.y <= 0) EXPEffect(hunj);

            //up_ex
            if (pos.y >= 480) EXPEffect(hunj);

            //left_ex
            if (pos.x <= 16) EXPEffect(hunj);

            //right_ex
            if (pos.x >= 640) EXPEffect(hunj);
        }
    }

    public void EXPEffect(int hun)
    {
        if (_expMode == 1)
        {
            _expMode = 2;
            StartCoroutine(Explosion(hun));
        }
    }

    private IEnumerator Explosion(int hunj)
    {
        pos = transform.position;
        GameObject.FindObjectOfType<AudioSource>().PlayOneShot(expS);
        for (int j = 0; j < hunj; j++)
        {
            for(int k = 0; k < 2; k++)
            {
                Vector3 direction2 = new Vector3(Random.Range(10, 630), Random.Range(10, 470), 0);
                Instantiate(ExpPrefab, pos, transform.localRotation).EShot1(Random.Range(0,360)/*((360 / hunj) * j)+(k*180)*/, 10, eexptim);
            }

            yield return new WaitForSeconds(0.03f);
        }
        Destroy(gameObject);
    }

        public void OnTriggerEnter2D(Collider2D collision)
    {
        if (_expMode == 1)
        {
            pos = transform.position;
            if (collision.gameObject.tag == "Enemy0"
                || collision.gameObject.tag == "Enemy1"
                || collision.gameObject.tag == "Enemy2"
                || collision.gameObject.tag == "Enemy3"
                || collision.gameObject.tag == "Enemy4"
                || collision.gameObject.tag == "Enemy6")
            {
                if (sosai)
                {
                    EXPEffect(hunj);
                }
            }
            if (collision.gameObject.tag == "Barrier")
            {
                EXPEffect(hunj);
            }
        }
    }
}
