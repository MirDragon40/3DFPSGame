using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    void Start()
    {
        
    }

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
    void Update()
    {
        
    }
}
