using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager instance { get; private set; }

    private void Awake()
    {
        // �̱��� ���� : ���� �Ѱ��� Ŭ���� �ν��Ͻ��� ������ ����
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);


    }
    void Start()
    {

    }
    void Update()
    {

    }
}
